using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Data.OleDb;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;

namespace Dai_NetMessage_Logger_Service
{
    public partial class Service1 : ServiceBase
    {
        //Record the Logs and Exceptions
        FileStream fileStream;
        string path;
        StreamWriter streamWriter;

        //timers
        System.Timers.Timer timer1;//trigger to start the service, work after 1sec
        System.Timers.Timer timer2;//cyclically check if it is the time to clean and compact DB

        //Get values from config file and transfer to target data type
        static string sMDB = ConfigurationManager.AppSettings["sDatabase"];
        static int portID = Convert.ToInt32(ConfigurationManager.AppSettings["sPortID"]);
        static string exclude = ConfigurationManager.AppSettings["sexclude"];
        static bool DBsizeCtrl = Convert.ToBoolean(ConfigurationManager.AppSettings["sDBsizeCtrl"]);
        static int DBBuffersize = Convert.ToInt32(ConfigurationManager.AppSettings["sDBBuffersize"]);
        static DateTime targetDT = DateTime.Now.AddDays(-DBBuffersize);
        static DayOfWeek DBCmpDate = getDayOfWeek((ConfigurationManager.AppSettings["sDBCmpDate"]));
        // Connection String zusammenbauen
        static String myConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + sMDB;
        // Object erzeugen
        static OleDbConnection myConnection = new OleDbConnection(myConnectionString);

        string[] excludelist;
        //int counter = 1;
        string telegramm = null;
        //int zeilen = 0;
        int anzzeilen = 0;
        int anzzeilen2 = 0;
        int recieved;
        
        //Flag for compress the DB
        bool compressDB = false;

        /// <summary>
        /// get the string from config and convert the data to DayOfWeek
        /// </summary>
        /// <param name="source">source string to convert</param>
        /// <returns>finished convert, if not match any case, returns DayOfWeek.Sunday</returns>
        public static DayOfWeek getDayOfWeek(string source)
        {
            switch (source.ToLower())
            {
                case "monday":
                    return DayOfWeek.Monday;
                case "tuesday":
                    return DayOfWeek.Tuesday;
                case "wednesday":
                    return DayOfWeek.Wednesday;
                case "thursday":
                    return DayOfWeek.Thursday;
                case "friday":
                    return DayOfWeek.Friday;
                case "saturday":
                    return DayOfWeek.Saturday;
                case "sunday":
                    return DayOfWeek.Sunday;
                default:
                    return DayOfWeek.Sunday;
            }
        }
        
        public Service1()
        {
            InitializeComponent();

            path = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Dai_NetMessage_Logger_Service.log");
            fileStream = new FileStream(path, FileMode.Append, FileAccess.Write);
            streamWriter = new StreamWriter(fileStream);

            timer1 = new System.Timers.Timer(1000);
            timer1.AutoReset = false;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);

            timer2 = new System.Timers.Timer(86400000);// make it 24hours
            //timer2.AutoReset = false;//for test purpose
            timer2.Elapsed += new System.Timers.ElapsedEventHandler(timer2_Elapsed);
        }

        /// <summary>
        /// MBD compact method (c) 2004 Alexander Youmashev
        /// !!IMPORTANT!!
        /// !make sure there's no open connections
        ///    to your db before calling this method!
        /// !!IMPORTANT!!
        /// Modified by Peiren to change the temp storage path
        /// </summary>
        /// <param name="connectionString">connection string to your db</param>
        /// <param name="mdwfilename">FULL name
        ///     of an MDB file you want to compress.</param>
        public void CompactAccessDB(string connectionString, string mdwfilename)
        {
            //here is the path of the temp storage
            string temppath = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "temp.mdb");

            object[] oParams;

            //create an inctance of a Jet Replication Object
            object objJRO =
              Activator.CreateInstance(Type.GetTypeFromProgID("JRO.JetEngine"));

            //filling Parameters array
            //cnahge "Jet OLEDB:Engine Type=5" to an appropriate value
            // or leave it as is if you db is JET4X format (access 2000,2002)
            //(yes, jetengine5 is for JET4X, no misprint here)
            oParams = new object[] {
        connectionString,
        "Provider=Microsoft.Jet.OLEDB.4.0;Data" + 
        " Source=" + temppath +";Jet OLEDB:Engine Type=5"};

            //invoke a CompactDatabase method of a JRO object
            //pass Parameters array
            objJRO.GetType().InvokeMember("CompactDatabase",
                System.Reflection.BindingFlags.InvokeMethod,
                null,
                objJRO,
                oParams);

            //database is compacted now
            //to a new file temppath
            //let's copy it over an old one and delete it

            System.IO.File.Delete(mdwfilename);
            System.IO.File.Move(temppath, mdwfilename);

            //clean up (just in case)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objJRO);
            objJRO = null;
        }

        /// <summary>
        /// Attention: Only work with Access DB
        /// check if the condition is fulfilled and
        /// the DB need to be cleaned and compacted
        /// write to log if DB was cleaned and compacted
        /// </summary>
        private void checkCompactDB()
        {
            if (!this.compressDB)
                return;

            streamWriter.WriteLine("Start compress the database at " + DateTime.Now.ToString());
            streamWriter.Flush();
            //First check if it is necessary to clean up the DB, set up in the config file
            if (DBsizeCtrl)
            {

                string cleanup = "delete from KRMSGNET where [DateTime] < #" + targetDT.ToString("yyyy-MM-dd") + "#";
                OleDbCommand cmd = new OleDbCommand(cleanup, myConnection);
                int num = 0;//write to log how many records was cleaned up
                num = cmd.ExecuteNonQuery();
                streamWriter.WriteLine(num.ToString() + " records was deleted in this cleanup!");
            }
            //then compress db
            myConnection.Close();
            CompactAccessDB(myConnectionString, sMDB);
            myConnection.Open();
                        
            streamWriter.WriteLine("Finished compress the database at " + DateTime.Now.ToString());
            streamWriter.Flush();

            this.compressDB = false;
        }

        void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            streamWriter.WriteLine("Dai_NetMessage_Logger_Service Started at " + DateTime.Now.ToString());
            streamWriter.Flush();

            timer2.Start();

            byte[] data = new byte[1024];
            String[] datablockfeld;

            try
            {
                // Read Exclude List amount of lines to create the excludelist array
                myConnection.Open();
                StreamReader Reader = new StreamReader(exclude);

                do
                {
                    anzzeilen = anzzeilen + 1;
                    Reader.ReadLine();
                }
                while (Reader.Peek() != -1);

                Reader.Close();
                excludelist = new string[anzzeilen];

                //Read excludelist again and fill array
                StreamReader Reader2 = new StreamReader(exclude);
                do
                {

                    excludelist[anzzeilen2] = Reader2.ReadLine();
                    anzzeilen2 = anzzeilen2 + 1;
                }
                while (Reader2.Peek() != -1);
                Reader2.Close();
                //Console.Title = "DAI_NetMessage_Logger from Peter Schmitt V1.0";
                //Console.SetCursorPosition(0, 0);
                streamWriter.WriteLine("Programconfiguration: ");
                streamWriter.WriteLine("");
                streamWriter.WriteLine("Excludelistfile: " + exclude);
                streamWriter.WriteLine("");
                streamWriter.WriteLine(myConnectionString);
                streamWriter.WriteLine("");
                streamWriter.WriteLine("");
                streamWriter.WriteLine("Port ID for Listening is " + portID);
                streamWriter.WriteLine("");
                streamWriter.WriteLine("");
                streamWriter.Flush();
                System.Threading.Thread.Sleep(1000);

                //Prepare Listening on Port
                IPEndPoint ipadd = new IPEndPoint(IPAddress.Any, portID);

                Socket newsock = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

                newsock.Bind(ipadd);

                //Console.WriteLine("Waiting for Robots...");
                //???
                IPEndPoint ssender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint Remote = (EndPoint)(ssender);

                while (true)
                {
                    checkCompactDB();
                    
                    data = new byte[1024];
                    try
                    {
                        recieved = newsock.ReceiveFrom(data, ref Remote);
                        //put this inside the try catch module to let the program continue
                    }
                    catch (System.Net.Sockets.SocketException ee)
                    {
                        streamWriter.WriteLine(ee.Message);
                        streamWriter.WriteLine(System.DateTime.Now);
                        streamWriter.WriteLine("target IP is " + Remote.AddressFamily.ToString());
                        streamWriter.WriteLine(data.ToString());
                        streamWriter.Flush();
                        continue;
                    }
                    //Daten in Datei schreiben
                    telegramm = Encoding.UTF8.GetString(data, 0, recieved);

                    //Exclude filter do nothing
                    if (excludelist.Any(telegramm.Contains))
                    {
                        continue;
                    }


                    //check if content of telegramm is valid and create value for DB to add
                    else
                    {
                        datablockfeld = telegramm.Split(';');
                        if (datablockfeld.Length != 8)
                        {
                            continue;
                        }
                        if (datablockfeld[0].Length >= 26)
                        {
                            continue;
                        }

                        if (datablockfeld[1].Length >= 26)
                        {
                            continue;
                        }

                        if (datablockfeld[3].Length >= 36)
                        {
                            continue;
                        }

                        if (datablockfeld[6].Length >= 11)
                        {
                            continue;
                        }

                        if (datablockfeld[7].Length >= 11)
                        {
                            continue;
                        }

                        String modstr = datablockfeld[4].Replace("'", "");
                        if (modstr.Length >= 256)
                        {
                            continue;
                        }
                        string myInsert = "INSERT INTO KRMSGNET([Robotname], [Computername], [DateTime], [Message], [MessageNr], [Sender], [Messagetype], [MsgStatus])" + "VALUES('" + datablockfeld[1].Trim() + "','" + datablockfeld[0] + "',#" + datablockfeld[2] + "#,'" + modstr + "'," + datablockfeld[5] + ",'" + datablockfeld[3] + "','" + datablockfeld[6] + "','" + datablockfeld[7] + "')";
                        OleDbCommand cmd = new OleDbCommand(myInsert, myConnection);
                        int num = 0;
                        num = cmd.ExecuteNonQuery();
                        //here to check if the insert is successful or not
                        if (num != 1)
                        {
                            throw new Exception("Insert failed, please check Database!");
                        }
                    }
                }
            }
                
            catch (Exception ex)
            {
                streamWriter.WriteLine(ex.ToString() + " Dai_NetMessage_Logger_Service caused an exception at " + DateTime.Now.ToString());
                streamWriter.WriteLine("");
                streamWriter.Flush();
            }

            finally
            {
                myConnection.Close();
                fileStream.Close();
                this.Stop();//let the system know that the service stopped
            }
        }

        void timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Today.DayOfWeek == DBCmpDate)
            {
                this.compressDB = true;
            }
            refreshConfigrations();
        }

        private void refreshConfigrations()
        {
            sMDB = ConfigurationManager.AppSettings["sDatabase"];
            portID = Convert.ToInt32(ConfigurationManager.AppSettings["sPortID"]);
            exclude = ConfigurationManager.AppSettings["sexclude"];
            DBsizeCtrl = Convert.ToBoolean(ConfigurationManager.AppSettings["sDBsizeCtrl"]);
            DBBuffersize = Convert.ToInt32(ConfigurationManager.AppSettings["sDBBuffersize"]);
            targetDT = DateTime.Now.AddDays(-DBBuffersize);
            DBCmpDate = getDayOfWeek((ConfigurationManager.AppSettings["sDBCmpDate"]));
        }

        protected override void OnStart(string[] args)
        {
            timer1.Start();
        }

        protected override void OnStop()
        {
            streamWriter.WriteLine("Dai_NetMessage_Logger_Service Stopped at " + DateTime.Now.ToString());
            streamWriter.Flush();

            fileStream.Close();

            timer1.Stop();
            timer2.Stop();
        }
    }
}
