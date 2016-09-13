namespace EasyCSVHandler
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.all_robname = new System.Windows.Forms.CheckBox();
            this.robname = new System.Windows.Forms.CheckedListBox();
            this.all_cellname = new System.Windows.Forms.CheckBox();
            this.cellname = new System.Windows.Forms.CheckedListBox();
            this.all_zarea = new System.Windows.Forms.CheckBox();
            this.zarea = new System.Windows.Forms.CheckedListBox();
            this.all_buliding = new System.Windows.Forms.CheckBox();
            this.building = new System.Windows.Forms.CheckedListBox();
            this.all_project = new System.Windows.Forms.CheckBox();
            this.project = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.all_techversion = new System.Windows.Forms.CheckBox();
            this.all_techname = new System.Windows.Forms.CheckBox();
            this.techversion = new System.Windows.Forms.CheckedListBox();
            this.techname = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configurationSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createConfigFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDBPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.all_robname);
            this.groupBox1.Controls.Add(this.robname);
            this.groupBox1.Controls.Add(this.all_cellname);
            this.groupBox1.Controls.Add(this.cellname);
            this.groupBox1.Controls.Add(this.all_zarea);
            this.groupBox1.Controls.Add(this.zarea);
            this.groupBox1.Controls.Add(this.all_buliding);
            this.groupBox1.Controls.Add(this.building);
            this.groupBox1.Controls.Add(this.all_project);
            this.groupBox1.Controls.Add(this.project);
            this.groupBox1.Location = new System.Drawing.Point(14, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(681, 261);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Robot Location Filter";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(539, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "RobotName";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(428, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "CellName";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(295, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Z Area";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(174, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Building";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(48, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Project";
            // 
            // all_robname
            // 
            this.all_robname.AutoSize = true;
            this.all_robname.Location = new System.Drawing.Point(529, 67);
            this.all_robname.Name = "all_robname";
            this.all_robname.Size = new System.Drawing.Size(82, 17);
            this.all_robname.TabIndex = 23;
            this.all_robname.Text = "Unselect All";
            this.all_robname.UseVisualStyleBackColor = true;
            this.all_robname.CheckedChanged += new System.EventHandler(this.all_robname_CheckedChanged);
            // 
            // robname
            // 
            this.robname.FormattingEnabled = true;
            this.robname.Location = new System.Drawing.Point(526, 84);
            this.robname.Name = "robname";
            this.robname.Size = new System.Drawing.Size(141, 154);
            this.robname.TabIndex = 22;
            // 
            // all_cellname
            // 
            this.all_cellname.AutoSize = true;
            this.all_cellname.Location = new System.Drawing.Point(403, 67);
            this.all_cellname.Name = "all_cellname";
            this.all_cellname.Size = new System.Drawing.Size(82, 17);
            this.all_cellname.TabIndex = 21;
            this.all_cellname.Text = "Unselect All";
            this.all_cellname.UseVisualStyleBackColor = true;
            this.all_cellname.CheckedChanged += new System.EventHandler(this.all_cellname_CheckedChanged);
            // 
            // cellname
            // 
            this.cellname.FormattingEnabled = true;
            this.cellname.Location = new System.Drawing.Point(400, 84);
            this.cellname.Name = "cellname";
            this.cellname.Size = new System.Drawing.Size(120, 154);
            this.cellname.TabIndex = 20;
            // 
            // all_zarea
            // 
            this.all_zarea.AutoSize = true;
            this.all_zarea.Location = new System.Drawing.Point(277, 67);
            this.all_zarea.Name = "all_zarea";
            this.all_zarea.Size = new System.Drawing.Size(82, 17);
            this.all_zarea.TabIndex = 19;
            this.all_zarea.Text = "Unselect All";
            this.all_zarea.UseVisualStyleBackColor = true;
            this.all_zarea.CheckedChanged += new System.EventHandler(this.all_zarea_CheckedChanged);
            // 
            // zarea
            // 
            this.zarea.FormattingEnabled = true;
            this.zarea.Location = new System.Drawing.Point(274, 84);
            this.zarea.Name = "zarea";
            this.zarea.Size = new System.Drawing.Size(120, 154);
            this.zarea.TabIndex = 18;
            // 
            // all_buliding
            // 
            this.all_buliding.AutoSize = true;
            this.all_buliding.Location = new System.Drawing.Point(151, 67);
            this.all_buliding.Name = "all_buliding";
            this.all_buliding.Size = new System.Drawing.Size(82, 17);
            this.all_buliding.TabIndex = 17;
            this.all_buliding.Text = "Unselect All";
            this.all_buliding.UseVisualStyleBackColor = true;
            this.all_buliding.CheckedChanged += new System.EventHandler(this.all_buliding_CheckedChanged);
            // 
            // building
            // 
            this.building.FormattingEnabled = true;
            this.building.Location = new System.Drawing.Point(148, 84);
            this.building.Name = "building";
            this.building.Size = new System.Drawing.Size(120, 154);
            this.building.TabIndex = 16;
            // 
            // all_project
            // 
            this.all_project.AutoSize = true;
            this.all_project.Location = new System.Drawing.Point(25, 67);
            this.all_project.Name = "all_project";
            this.all_project.Size = new System.Drawing.Size(82, 17);
            this.all_project.TabIndex = 15;
            this.all_project.Text = "Unselect All";
            this.all_project.UseVisualStyleBackColor = true;
            this.all_project.CheckedChanged += new System.EventHandler(this.all_project_CheckedChanged);
            // 
            // project
            // 
            this.project.FormattingEnabled = true;
            this.project.Location = new System.Drawing.Point(22, 84);
            this.project.Name = "project";
            this.project.Size = new System.Drawing.Size(120, 154);
            this.project.TabIndex = 14;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.all_techversion);
            this.groupBox2.Controls.Add(this.all_techname);
            this.groupBox2.Controls.Add(this.techversion);
            this.groupBox2.Controls.Add(this.techname);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(12, 312);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(397, 219);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Technology Filter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(231, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Technology Version";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(64, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Technology Name";
            // 
            // all_techversion
            // 
            this.all_techversion.AutoSize = true;
            this.all_techversion.Location = new System.Drawing.Point(200, 44);
            this.all_techversion.Name = "all_techversion";
            this.all_techversion.Size = new System.Drawing.Size(82, 17);
            this.all_techversion.TabIndex = 14;
            this.all_techversion.Text = "Unselect All";
            this.all_techversion.UseVisualStyleBackColor = true;
            this.all_techversion.CheckedChanged += new System.EventHandler(this.all_techversion_CheckedChanged);
            // 
            // all_techname
            // 
            this.all_techname.AutoSize = true;
            this.all_techname.Location = new System.Drawing.Point(31, 44);
            this.all_techname.Name = "all_techname";
            this.all_techname.Size = new System.Drawing.Size(82, 17);
            this.all_techname.TabIndex = 13;
            this.all_techname.Text = "Unselect All";
            this.all_techname.UseVisualStyleBackColor = true;
            this.all_techname.CheckedChanged += new System.EventHandler(this.all_techname_CheckedChanged);
            // 
            // techversion
            // 
            this.techversion.FormattingEnabled = true;
            this.techversion.Location = new System.Drawing.Point(198, 61);
            this.techversion.Name = "techversion";
            this.techversion.Size = new System.Drawing.Size(171, 154);
            this.techversion.TabIndex = 12;
            // 
            // techname
            // 
            this.techname.FormattingEnabled = true;
            this.techname.Location = new System.Drawing.Point(28, 61);
            this.techname.Name = "techname";
            this.techname.Size = new System.Drawing.Size(163, 154);
            this.techname.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(477, 484);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 43);
            this.button1.TabIndex = 14;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(606, 484);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 43);
            this.button2.TabIndex = 15;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(422, 377);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "File Save Path:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(422, 403);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(255, 20);
            this.textBox1.TabIndex = 17;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(602, 429);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 18;
            this.button3.Text = "Relocate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(417, 322);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(135, 31);
            this.button4.TabIndex = 19;
            this.button4.Text = "Active TechFilter";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationSettingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(703, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // configurationSettingsToolStripMenuItem
            // 
            this.configurationSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createConfigFileToolStripMenuItem,
            this.changeDBPathToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.configurationSettingsToolStripMenuItem.Name = "configurationSettingsToolStripMenuItem";
            this.configurationSettingsToolStripMenuItem.Size = new System.Drawing.Size(138, 20);
            this.configurationSettingsToolStripMenuItem.Text = "Configuration Settings";
            // 
            // createConfigFileToolStripMenuItem
            // 
            this.createConfigFileToolStripMenuItem.Name = "createConfigFileToolStripMenuItem";
            this.createConfigFileToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.createConfigFileToolStripMenuItem.Text = "Create Config File";
            this.createConfigFileToolStripMenuItem.Click += new System.EventHandler(this.createConfigFileToolStripMenuItem_Click);
            // 
            // changeDBPathToolStripMenuItem
            // 
            this.changeDBPathToolStripMenuItem.Name = "changeDBPathToolStripMenuItem";
            this.changeDBPathToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.changeDBPathToolStripMenuItem.Text = "Change DB Path";
            this.changeDBPathToolStripMenuItem.Click += new System.EventHandler(this.changeDBPathToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 534);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(703, 22);
            this.statusStrip1.TabIndex = 21;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.IsLink = true;
            this.toolStripStatusLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(126, 17);
            this.toolStripStatusLabel2.Text = "Remove Selected Filter";
            this.toolStripStatusLabel2.Click += new System.EventHandler(this.showAllLabel_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 556);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Easy CSV Handler Programmed by Peiren Yang";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox all_techversion;
        private System.Windows.Forms.CheckBox all_techname;
        private System.Windows.Forms.CheckedListBox techversion;
        private System.Windows.Forms.CheckedListBox techname;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox all_robname;
        private System.Windows.Forms.CheckedListBox robname;
        private System.Windows.Forms.CheckBox all_cellname;
        private System.Windows.Forms.CheckedListBox cellname;
        private System.Windows.Forms.CheckBox all_zarea;
        private System.Windows.Forms.CheckedListBox zarea;
        private System.Windows.Forms.CheckBox all_buliding;
        private System.Windows.Forms.CheckedListBox building;
        private System.Windows.Forms.CheckBox all_project;
        private System.Windows.Forms.CheckedListBox project;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configurationSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createConfigFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeDBPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

