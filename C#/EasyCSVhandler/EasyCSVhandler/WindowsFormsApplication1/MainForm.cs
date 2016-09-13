using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;
using System.IO;

namespace EasyCSVHandler
{
    public partial class MainForm : Form
    {

        private DataSet ds = null;
        private DataSet tech = null;
        //private DataTable[] dt = new DataTable[5];
        private DataView tempdv = new DataView();
        //private DataView techdv = new DataView();
        private DataTable targetrobots = new DataTable();
        private int TotalQuantity;
        //private int doublecontrol = 1;
        //private Button assistant = new Button();
        public MainForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            BindFirstcheckedlistbox();
            if (!XMLHandle.testPath())
            {
                MessageBox.Show("No config file found, please create config file first!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            building_databind();
            zarea_databind();
            cellname_databind();
            robname_databind();
            techversion_databind();
            openEvent();
            checkedallHook();
            addoncheck();
            updatePanel();
            textBox1.Text = XMLHandle.readStorePath();
            //assistant.Click += new EventHandler(assistant_Click);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.groupBox2.Enabled == true)
            {
                this.groupBox2.Enabled = false;
                this.groupBox1.Enabled = true;
                this.button4.Text = "Active TechFilter";
                building_databind();
                zarea_databind();
                cellname_databind();
                robname_databind();
            }
            else
            {
                this.groupBox2.Enabled = true;
                this.groupBox1.Enabled = false;
                this.button4.Text = "Active LocationFilter";
                techversion_databind();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GC.Collect();
            Application.Exit();
        }

        private void createConfigFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XMLHandle.createConfig();
            textBox1.Text = XMLHandle.readStorePath();
        }

        private void changeDBPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!XMLHandle.testPath())
            {
                MessageBox.Show("No config file found, please create config file first!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Input ip = new Input();
            if(ip.ShowDialog() == DialogResult.OK)
                XMLHandle.writePath(ip.path);

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void all_project_CheckedChanged(object sender, EventArgs e)
        {
            if (all_project.Checked)
            {
                for (int i = 0; i < project.Items.Count; i++)
                {
                    this.project.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < project.Items.Count; i++)
                {
                    this.project.SetItemChecked(i, false);
                }
            }
        }

        private void all_buliding_CheckedChanged(object sender, EventArgs e)
        {
            if (all_buliding.Checked)
            {
                for (int i = 0; i < building.Items.Count; i++)
                {
                    this.building.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < building.Items.Count; i++)
                {
                    this.building.SetItemChecked(i, false);
                }
            }
        }

        private void all_zarea_CheckedChanged(object sender, EventArgs e)
        {
            if (all_zarea.Checked)
            {
                for (int i = 0; i < zarea.Items.Count; i++)
                {
                    this.zarea.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < zarea.Items.Count; i++)
                {
                    this.zarea.SetItemChecked(i, false);
                }
            }
        }

        private void all_cellname_CheckedChanged(object sender, EventArgs e)
        {
            if (all_cellname.Checked)
            {
                for (int i = 0; i < cellname.Items.Count; i++)
                {
                    this.cellname.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < cellname.Items.Count; i++)
                {
                    this.cellname.SetItemChecked(i, false);
                }
            }
        }

        private void all_robname_CheckedChanged(object sender, EventArgs e)
        {
            if (all_robname.Checked)
            {
                for (int i = 0; i < robname.Items.Count; i++)
                {
                    this.robname.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < robname.Items.Count; i++)
                {
                    this.robname.SetItemChecked(i, false);
                }
            }
        }

        private void all_techname_CheckedChanged(object sender, EventArgs e)
        {
            if (all_techname.Checked)
            {
                for (int i = 0; i < techname.Items.Count; i++)
                {
                    this.techname.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < techname.Items.Count; i++)
                {
                    this.techname.SetItemChecked(i, false);
                }
            }
        }

        private void all_techversion_CheckedChanged(object sender, EventArgs e)
        {
            if (all_techversion.Checked)
            {
                for (int i = 0; i < techversion.Items.Count; i++)
                {
                    this.techversion.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < techversion.Items.Count; i++)
                {
                    this.techversion.SetItemChecked(i, false);
                }
            }
        }

        private void BindFirstcheckedlistbox()
        {
            try
            {
                string sql = "SELECT IIf(IsNull([ProjectIPRelation].Project_Name),'Assembly',[ProjectIPRelation].Project_Name) AS project, Mid([All].cellname,1,3) AS building, Mid([All].cellname,7,2) AS zarea, Mid([All].cellname,10,4) AS cellname, [All].robotname as robname, Techpakete.*"
                    + "FROM ([All] LEFT JOIN ProjectIPRelation ON MID([All].IP_adress,4,3)=ProjectIPRelation.IP_Range) LEFT JOIN Techpakete ON Techpakete.Robotername = [All].robotname;";
                Db_operation.con.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(sql, Db_operation.con);
                this.ds = new DataSet();
                //ds.Tables[0] = new DataTable();
                da.Fill(ds);
                sql = "select TName from TechName";
                da = new OleDbDataAdapter(sql, Db_operation.con);
                this.tech = new DataSet();
                da.Fill(tech);


                targetrobots = ds.Tables[0].Copy();
                TotalQuantity = targetrobots.Rows.Count;
                //tempdv = ds.Tables[0].AsDataView();
                tempdv = targetrobots.AsDataView();
                tempdv.Sort = "project ASC";
                
                project.DataSource = tempdv.ToTable(true, "project");// SelectDistinct("Table1", ds.Tables[0], "project");
                project.ValueMember = "project";
                project.DisplayMember = "project";

                techname.DataSource = tech.Tables[0];
                techname.ValueMember = "TName";
                techname.DisplayMember = "TName";

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Db_operation.con.Close();
            }
        }

        private void building_databind()
        {
            tempdv.RowFilter = "project = '" + this.project.SelectedValue.ToString() + "'";
            tempdv.Sort = "building ASC";
            if (tempdv.Count == 0)
            {
                building.Visible = false;
                return;
            }
            removecheck();
            building.DataSource = tempdv.ToTable(true, "building");
            building.ValueMember = "building";
            building.DisplayMember = "building";
            
            for (int i = 0; i < building.Items.Count; i++)
            {
                this.building.SetItemChecked(i, true);
            }
            addoncheck();
        }

        private void project_SelectedIndexChanged(object sender, EventArgs e)
        {
            closeEvent();
            building_databind();
            zarea_databind();
            cellname_databind();
            robname_databind();
            openEvent(); 
        }
        private void zarea_databind()
        {
            if (this.building.SelectedValue != null)
            {
                tempdv.RowFilter = "project = '" + this.project.SelectedValue.ToString() + "'" + " and building = '" + this.building.SelectedValue.ToString() + "'";
            }
            tempdv.Sort = "zarea ASC";
            if (tempdv.Count == 0)
            {
                zarea.Visible = false;
                return;
            }
            removecheck();
            zarea.DataSource = tempdv.ToTable(true, "zarea");
            zarea.ValueMember = "zarea";
            zarea.DisplayMember = "zarea";
            
            for (int i = 0; i < zarea.Items.Count; i++)
            {
                this.zarea.SetItemChecked(i, true);
            }
            addoncheck();
        }
        private void building_SelectedIndexChanged(object sender, EventArgs e)
        {
            closeEvent();
            zarea_databind();
            cellname_databind();
            robname_databind();
            openEvent(); 
        }
        private void cellname_databind()
        {

            if (this.building.SelectedValue != null && this.zarea.SelectedValue != null)
            {
                tempdv.RowFilter = "project = '" + this.project.SelectedValue.ToString() + "'" + " and building = '" + this.building.SelectedValue.ToString() + "'"
                    + " and zarea = '" + this.zarea.SelectedValue.ToString() + "'";
            }
            tempdv.Sort = "cellname ASC";
            if (tempdv.Count == 0)
            {
                cellname.Visible = false;
                return;
            }
            removecheck();
            cellname.DataSource = tempdv.ToTable(true, "cellname");
            cellname.ValueMember = "cellname";
            cellname.DisplayMember = "cellname";
            
            for (int i = 0; i < cellname.Items.Count; i++)
            {
                this.cellname.SetItemChecked(i, true);
            }
            addoncheck();
        }
        private void zarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            closeEvent();
            cellname_databind();
            robname_databind();
            openEvent(); 
        }
        private void robname_databind()
        {
            if (this.building.SelectedValue != null && this.zarea.SelectedValue != null && this.cellname.SelectedValue != null)
            {
                tempdv.RowFilter = "project = '" + this.project.SelectedValue.ToString() + "'" + " and building = '" + this.building.SelectedValue.ToString() + "'"
                    + " and zarea = '" + this.zarea.SelectedValue.ToString() + "'" + " and cellname = '" + this.cellname.SelectedValue.ToString() + "'";
            }
            tempdv.Sort = "robname ASC";
            if (tempdv.Count == 0)
            {
                robname.Visible = false;
                return;
            }
            else
                showallcheckbox();
            removecheck();
            robname.DataSource = tempdv.ToTable(true, "robname");
            robname.ValueMember = "robname";
            robname.DisplayMember = "robname";
            
            for (int i = 0; i < robname.Items.Count; i++)
            {
                this.robname.SetItemChecked(i, true);
            }
            addoncheck();
        }
        private void cellname_SelectedIndexChanged(object sender, EventArgs e)
        {
            closeEvent();
            robname_databind();
            openEvent();            
        }

        private void closeEvent()
        {
            this.project.SelectedIndexChanged -= new EventHandler(project_SelectedIndexChanged);
            this.building.SelectedIndexChanged -= new System.EventHandler(this.building_SelectedIndexChanged);
            this.zarea.SelectedIndexChanged -= new EventHandler(zarea_SelectedIndexChanged);
            this.cellname.SelectedIndexChanged -= new EventHandler(cellname_SelectedIndexChanged);
            this.techname.SelectedIndexChanged -= new EventHandler(techname_SelectedIndexChanged);
            
        }

        private void openEvent()
        {
            this.project.SelectedIndexChanged += new EventHandler(project_SelectedIndexChanged);
            this.building.SelectedIndexChanged += new System.EventHandler(this.building_SelectedIndexChanged);
            this.zarea.SelectedIndexChanged += new EventHandler(zarea_SelectedIndexChanged);
            this.cellname.SelectedIndexChanged += new EventHandler(cellname_SelectedIndexChanged);
            this.techname.SelectedIndexChanged += new EventHandler(techname_SelectedIndexChanged);
            
        }

        private void checkedallHook()
        {
            all_project.Checked = true;
            all_buliding.Checked = true;
            all_zarea.Checked = true;
            all_cellname.Checked = true;
            all_robname.Checked = true;

            all_techname.Checked = true;
            all_techversion.Checked = true;
        }

        private void showAllLabel_Click(object sender, EventArgs e)
        {
            targetrobots = ds.Tables[0].Copy();
            updatePanel();
            tempdv = targetrobots.AsDataView();
            tempdv.Sort = "project ASC";
            closeEvent();
            removecheck();
            project.DataSource = tempdv.ToTable(true, "project");// SelectDistinct("Table1", ds.Tables[0], "project");
            project.ValueMember = "project";
            project.DisplayMember = "project";
            building_databind();
            zarea_databind();
            cellname_databind();
            robname_databind();
            techname_databind();
            techversion_databind();
            for (int i = 0; i < project.Items.Count; i++)
            {
                this.project.SetItemChecked(i, true);
            }
            addoncheck();
            openEvent();
            showallcheckbox();
            //DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(dataGridView1);
        }

        private void addoncheck()
        {
            this.project.ItemCheck += new ItemCheckEventHandler(projectHandle_ItemUnchecked);
            this.building.ItemCheck += new ItemCheckEventHandler(buildingHandle_ItemUnchecked);
            this.zarea.ItemCheck += new ItemCheckEventHandler(zareaHandle_ItemUnchecked);
            this.cellname.ItemCheck += new ItemCheckEventHandler(cellnameHandle_ItemUnchecked);
            this.robname.ItemCheck += new ItemCheckEventHandler(robnameHandle_ItemUnchecked);
            this.techname.ItemCheck += new ItemCheckEventHandler(techname_ItemCheck);
            this.techversion.ItemCheck += new ItemCheckEventHandler(techversion_ItemCheck);
        }

        private void removecheck()
        {
            this.project.ItemCheck -= new ItemCheckEventHandler(projectHandle_ItemUnchecked);
            this.building.ItemCheck -= new ItemCheckEventHandler(buildingHandle_ItemUnchecked);
            this.zarea.ItemCheck -= new ItemCheckEventHandler(zareaHandle_ItemUnchecked);
            this.cellname.ItemCheck -= new ItemCheckEventHandler(cellnameHandle_ItemUnchecked);
            this.robname.ItemCheck -= new ItemCheckEventHandler(robnameHandle_ItemUnchecked);
            this.techname.ItemCheck -= new ItemCheckEventHandler(techname_ItemCheck);
            this.techversion.ItemCheck -= new ItemCheckEventHandler(techversion_ItemCheck);
        }
        private void projectHandle_ItemUnchecked(object sender, ItemCheckEventArgs e)
        {

            if (e.NewValue == CheckState.Unchecked)
            {
                foreach (DataRow r in targetrobots.Select(((ListBox)sender).Name + " = " + "'" + ((ListBox)sender).SelectedValue.ToString() + "'"))
                {
                    r.Delete();
                }
                targetrobots.AcceptChanges();
                //closeEvent();
                //project.SetItemCheckState(project.SelectedIndex, CheckState.Indeterminate);
                //closeEvent();
                //removecheck();
                //tempdv.RowFilter = "";
                //project.DataSource = tempdv.ToTable(true, "project");
                //if(sender == null)
                //    sender = project.Items[0];
                //project.ValueMember = "project";
                //project.DisplayMember = "project";
                //openEvent();
                updatePanel();
                e.NewValue = CheckState.Indeterminate;
                //addoncheck();
                //openEvent();
            }
        }
        private void cellnameHandle_ItemUnchecked(object sender, ItemCheckEventArgs e)
        {


            if (e.NewValue == CheckState.Unchecked)
            {
                foreach (DataRow r in targetrobots.Select(((ListBox)sender).Name + " = " + "'" + ((ListBox)sender).SelectedValue.ToString() + "'"))
                {
                    r.Delete();
                }
                targetrobots.AcceptChanges();
                //closeEvent();
                if (cellname.Items.Count == 1)
                {
                    zarea.SetItemCheckState(zarea.SelectedIndex, CheckState.Indeterminate);
                    if (zarea.Items.Count == 1)
                    {
                        building.SetItemCheckState(building.SelectedIndex, CheckState.Indeterminate);
                        if (building.Items.Count == 1)
                            project.SetItemCheckState(project.SelectedIndex, CheckState.Indeterminate);
                    }
                }
                //cellname_databind();
                //openEvent();
                updatePanel();
                e.NewValue = CheckState.Indeterminate;

            }
        }
        private void robnameHandle_ItemUnchecked(object sender, ItemCheckEventArgs e)
        {
            
            if (e.NewValue == CheckState.Unchecked)
            {
                foreach (DataRow r in targetrobots.Select(((ListBox)sender).Name + " = " + "'" + ((ListBox)sender).SelectedValue.ToString() + "'"))
                {
                    r.Delete();
                }
                targetrobots.AcceptChanges();
                //closeEvent();
                if (robname.Items.Count == 1)
                {
                    cellname.SetItemCheckState(cellname.SelectedIndex, CheckState.Indeterminate);
                    if (cellname.Items.Count == 1)
                    {
                        zarea.SetItemCheckState(zarea.SelectedIndex, CheckState.Indeterminate);
                        if (zarea.Items.Count == 1)
                        {
                            building.SetItemCheckState(building.SelectedIndex, CheckState.Indeterminate);
                            if (building.Items.Count == 1)
                                project.SetItemCheckState(project.SelectedIndex, CheckState.Indeterminate);
                        }
                    }
                }
                //robname_databind();
                //openEvent();
                updatePanel();
                e.NewValue = CheckState.Indeterminate;

            }
        }

        private void buildingHandle_ItemUnchecked(object sender, ItemCheckEventArgs e)
        {
            
            if (e.NewValue == CheckState.Unchecked)
            {
                string selectstr = "project = '" + this.project.SelectedValue.ToString() + "'" + " and building = '" + ((ListBox)sender).SelectedValue.ToString() + "'";
                foreach (DataRow r in targetrobots.Select(selectstr))
                {
                    r.Delete();
                }
                targetrobots.AcceptChanges();
                //closeEvent();
                if (building.Items.Count == 1)
                {
                    project.SetItemCheckState(project.SelectedIndex, CheckState.Indeterminate);
                }
                //building_databind();
                //openEvent();
                updatePanel();
                e.NewValue = CheckState.Indeterminate;

            }
        }

        private void zareaHandle_ItemUnchecked(object sender, ItemCheckEventArgs e)
        {

            if (e.NewValue == CheckState.Unchecked)
            {
                string selectstr = "project = '" + this.project.SelectedValue.ToString() + "'" + " and building = '" + this.building.SelectedValue.ToString()
                    + "' and zarea = '" + ((ListBox)sender).SelectedValue.ToString() + "'";
                foreach (DataRow r in targetrobots.Select(selectstr))
                {
                    r.Delete();
                }
                targetrobots.AcceptChanges();
                //closeEvent();
                if (zarea.Items.Count == 1)
                {
                    building.SetItemCheckState(building.SelectedIndex, CheckState.Indeterminate);
                    if(building.Items.Count == 1)
                        project.SetItemCheckState(project.SelectedIndex, CheckState.Indeterminate);
                }
                //zarea_databind();
                //openEvent();
                updatePanel();
                e.NewValue = CheckState.Indeterminate;

            }
        }
        private void techversion_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Unchecked)
            {
                string selectstr;
                if (this.techversion.SelectedValue.ToString() == "")
                    selectstr = "ISNULL(" + this.techname.SelectedValue.ToString() + ",'') = ''";
                else selectstr = this.techname.SelectedValue.ToString() + " = '"+ this.techversion.SelectedValue.ToString() +"'";
                foreach (DataRow r in targetrobots.Select(selectstr))
                {
                    r.Delete();
                }
                targetrobots.AcceptChanges();
                updatePanel();
                e.NewValue = CheckState.Indeterminate;
            }
        }

        private void techname_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Unchecked)
            {
                string selectstr = this.techname.SelectedValue.ToString() + " <> 'Not installed'";
                foreach (DataRow r in targetrobots.Select(selectstr))
                {
                    r.Delete();
                }
                targetrobots.AcceptChanges();
                updatePanel();
                e.NewValue = CheckState.Indeterminate;
            }
        }
        private void updatePanel()
        {
            toolStripStatusLabel1.Text = targetrobots.Rows.Count.ToString() + " of "
                + TotalQuantity.ToString() + " robots selected.";
        }

        private void showallcheckbox()
        {
            this.project.Visible = true;
            this.building.Visible = true;
            this.zarea.Visible = true;
            this.cellname.Visible = true;
            this.robname.Visible = true;
        }

        private void techname_databind()
        {
            //tempdv.RowFilter = "techname = '" + this.project.SelectedValue.ToString() + "'";
            //tempdv.Sort = "building ASC";

            removecheck();
            techname.DataSource = tech.Tables[0];
            techname.ValueMember = "TName";
            techname.DisplayMember = "TName";

            for (int i = 0; i < techname.Items.Count; i++)
            {
                this.techname.SetItemChecked(i, true);
            }
            addoncheck();
        }

        private void techname_SelectedIndexChanged(object sender, EventArgs e)
        {
            closeEvent();
            techversion_databind();
            openEvent();
        }

        private void techversion_databind()
        {
            tempdv.RowFilter = "";
            //tempdv.RowFilter = this.techname.SelectedValue.ToString() + " = '" + this.techname.SelectedValue.ToString() + "'";
            tempdv.Sort = this.techname.SelectedValue.ToString() + " ASC";
            if (tempdv.Count == 0)
            {
                techversion.Visible = false;
                return;
            }
            techversion.Visible = true;
            removecheck();
            techversion.DataSource = tempdv.ToTable(true, this.techname.SelectedValue.ToString());
            techversion.ValueMember = this.techname.SelectedValue.ToString();
            techversion.DisplayMember = this.techname.SelectedValue.ToString();

            for (int i = 0; i < techversion.Items.Count; i++)
            {
                this.techversion.SetItemChecked(i, true);
            }
            addoncheck();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                XMLHandle.writeStorePath(this.folderBrowserDialog1.SelectedPath + @"\");
            }
            textBox1.Text = XMLHandle.readStorePath();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (targetrobots.Rows.Count == 0)
                {
                    throw new Exception("No robot selected, please select robot first!");
                }
                saveFileDialog1.InitialDirectory = textBox1.Text;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.Filter = "CSV File|*.csv";
                if ((saveFileDialog1.ShowDialog()) != DialogResult.OK)
                    return;
                string robots = null;
                foreach (DataRow dr in targetrobots.Rows)
                {
                    robots += "'" + dr["robname"] + "',";
                }
                robots = robots.Substring(0, robots.Length - 1);
                string sql = "SELECT * from [All] where robotname in ("+ robots +");";
                Db_operation.con.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(sql, Db_operation.con);
                DataSet ouput = new DataSet();
                da.Fill(ouput);
                exportCSV(ouput.Tables[0], saveFileDialog1.FileName);
                Db_operation.con.Close();

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exportCSV(DataTable dt, string filepath)
        {
            //string filename = saveFileDialog1.FileName;
            //string filepath = saveFileDialog1.FileName;
            FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, new System.Text.UnicodeEncoding());

            //write the first head line
            for(int i = 0; i <dt.Columns.Count - 1; i++)
            {
                sw.Write(dt.Columns[i].ColumnName + ";");
            }
            sw.Write(dt.Columns[dt.Columns.Count - 1].ColumnName);
            sw.WriteLine("");
            //loop to write the content
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count -1; j++)
                {
                    sw.Write(dt.Rows[i][j].ToString());
                    sw.Write(";");
                }
                sw.Write(dt.Rows[i][dt.Columns.Count - 1].ToString());
                sw.WriteLine("");
            }
            sw.Close();

            MessageBox.Show("Export to " + filepath +" successful!","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
