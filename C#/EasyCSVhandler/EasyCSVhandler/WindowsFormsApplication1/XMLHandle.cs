using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace EasyCSVHandler
{
    static class XMLHandle
    {
        public static string defaultDBpath = System.Windows.Forms.Application.StartupPath + @"\KRMSGNET.mdb";
        public static string xmlpath = System.Windows.Forms.Application.StartupPath + @"\Config.xml";
        public static string DefaultStorePath = System.Windows.Forms.Application.StartupPath + @"\";
        public static void createConfig()
        {
            
            try
            {
                if (testPath())
                {
                   if(MessageBox.Show("File already exist, do you want to overwrite it?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                       return;
                }
                XmlDocument myXmlDoc = new XmlDocument();
                XmlElement rootElement = myXmlDoc.CreateElement("PathConfig");
                myXmlDoc.AppendChild(rootElement);
                XmlElement firstLevelElement1 = myXmlDoc.CreateElement("DBConfig");
                firstLevelElement1.SetAttribute("ID", "1");
                firstLevelElement1.SetAttribute("Description", "Access DB Path");
                rootElement.AppendChild(firstLevelElement1);
                XmlElement secondLevelElement11 = myXmlDoc.CreateElement("DBPath");
                secondLevelElement11.InnerText = defaultDBpath;
                XmlElement secondLevelElement12 = myXmlDoc.CreateElement("DefaultStorePath");
                secondLevelElement12.InnerText = DefaultStorePath;
                firstLevelElement1.AppendChild(secondLevelElement11);
                firstLevelElement1.AppendChild(secondLevelElement12);
                myXmlDoc.Save(xmlpath);
                MessageBox.Show("File created successful!", "Info!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static string readPath()
        {
            string path;
            try
            {
                XmlDocument myXmlDoc = new XmlDocument();
                myXmlDoc.Load(xmlpath);
                XmlNode rootNode = myXmlDoc.SelectSingleNode("PathConfig");
                XmlNodeList firstLevelNodeList = rootNode.ChildNodes;
                path = firstLevelNodeList[0].ChildNodes[0].InnerText;
                return path;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null; 
        }
        public static string readStorePath()
        {
            string path;
            try
            {
                XmlDocument myXmlDoc = new XmlDocument();
                myXmlDoc.Load(xmlpath);
                XmlNode rootNode = myXmlDoc.SelectSingleNode("PathConfig");
                XmlNodeList firstLevelNodeList = rootNode.ChildNodes;
                path = firstLevelNodeList[0].ChildNodes[1].InnerText;
                return path;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
        public static void writePath(string path)
        {
            try
            {
                XmlDocument myXmlDoc = new XmlDocument();
                myXmlDoc.Load(xmlpath);
                XmlNode rootNode = myXmlDoc.FirstChild;
                XmlNodeList firstLevelNodeList = rootNode.ChildNodes;
                firstLevelNodeList[0].ChildNodes[0].InnerText = path;
                myXmlDoc.Save(xmlpath);
                MessageBox.Show("File changed successful!", "Info!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public static void writeStorePath(string path)
        {
            try
            {
                XmlDocument myXmlDoc = new XmlDocument();
                myXmlDoc.Load(xmlpath);
                XmlNode rootNode = myXmlDoc.FirstChild;
                XmlNodeList firstLevelNodeList = rootNode.ChildNodes;
                firstLevelNodeList[0].ChildNodes[1].InnerText = path;
                myXmlDoc.Save(xmlpath);
                MessageBox.Show("Default path changed successful!", "Info!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public static bool testPath()
        {
            if (File.Exists(xmlpath))
            {
                return true;
            }
            return false;
        }

    }
}
