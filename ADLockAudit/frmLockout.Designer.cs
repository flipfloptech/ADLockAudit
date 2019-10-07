namespace ADLockAudit
{
    partial class frmLockout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.viewMachines = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrentDomain = new System.Windows.Forms.LinkLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnRunAnalysis = new System.Windows.Forms.Button();
            this.primaryStatusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstEvents = new System.Windows.Forms.ListView();
            this.clmDateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmMachine = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmEventType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmEventReason = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimeEnd = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimeStart = new System.Windows.Forms.DateTimePicker();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblLastPasswordSet = new System.Windows.Forms.Label();
            this.lblLastLogon = new System.Windows.Forms.Label();
            this.lblLastBadPassword = new System.Windows.Forms.Label();
            this.lblExpirationDate = new System.Windows.Forms.Label();
            this.lblBadLogonCount = new System.Windows.Forms.Label();
            this.lblLockoutTime = new System.Windows.Forms.Label();
            this.lblLocked = new System.Windows.Forms.Label();
            this.lblEnabled = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lstSignedOn = new System.Windows.Forms.ListView();
            this.clmSignOnMachine = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmSignOnTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmSignOnSessionID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmSignOnState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.lstServices = new System.Windows.Forms.ListView();
            this.clmSvcMachine = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmServiceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmServiceID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmServiceState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.primaryStatusStrip.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.viewMachines);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblCurrentDomain);
            this.groupBox1.Location = new System.Drawing.Point(7, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 257);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Active Directory Information";
            // 
            // viewMachines
            // 
            this.viewMachines.CheckBoxes = true;
            this.viewMachines.FullRowSelect = true;
            this.viewMachines.Location = new System.Drawing.Point(8, 48);
            this.viewMachines.Name = "viewMachines";
            this.viewMachines.Size = new System.Drawing.Size(229, 201);
            this.viewMachines.TabIndex = 2;
            this.viewMachines.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.viewMachines_AfterCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current Domain:";
            // 
            // lblCurrentDomain
            // 
            this.lblCurrentDomain.AutoSize = true;
            this.lblCurrentDomain.Location = new System.Drawing.Point(96, 20);
            this.lblCurrentDomain.Name = "lblCurrentDomain";
            this.lblCurrentDomain.Size = new System.Drawing.Size(87, 13);
            this.lblCurrentDomain.TabIndex = 0;
            this.lblCurrentDomain.TabStop = true;
            this.lblCurrentDomain.Text = "placeholder.local";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtUsername);
            this.groupBox2.Location = new System.Drawing.Point(7, 369);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 56);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User Information";
            // 
            // txtUsername
            // 
            this.txtUsername.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtUsername.Location = new System.Drawing.Point(10, 20);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(228, 20);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.Text = "DOMAIN\\Username, username@domain.local, username";
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            this.txtUsername.Enter += new System.EventHandler(this.txtUsername_Enter);
            this.txtUsername.Leave += new System.EventHandler(this.txtUsername_Leave);
            // 
            // btnRunAnalysis
            // 
            this.btnRunAnalysis.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRunAnalysis.Enabled = false;
            this.btnRunAnalysis.Location = new System.Drawing.Point(337, 427);
            this.btnRunAnalysis.Name = "btnRunAnalysis";
            this.btnRunAnalysis.Size = new System.Drawing.Size(188, 23);
            this.btnRunAnalysis.TabIndex = 1;
            this.btnRunAnalysis.Text = "Run Analysis";
            this.btnRunAnalysis.UseVisualStyleBackColor = true;
            this.btnRunAnalysis.Click += new System.EventHandler(this.btnRunAnalysis_Click);
            // 
            // primaryStatusStrip
            // 
            this.primaryStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.progressBar});
            this.primaryStatusStrip.Location = new System.Drawing.Point(0, 452);
            this.primaryStatusStrip.Name = "primaryStatusStrip";
            this.primaryStatusStrip.Size = new System.Drawing.Size(862, 22);
            this.primaryStatusStrip.TabIndex = 3;
            this.primaryStatusStrip.Text = "primaryStatusStrip";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(745, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "lblStatus";
            // 
            // progressBar
            // 
            this.progressBar.AutoSize = false;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstEvents);
            this.groupBox3.Location = new System.Drawing.Point(258, 110);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(592, 155);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Account History";
            // 
            // lstEvents
            // 
            this.lstEvents.AutoArrange = false;
            this.lstEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmDateTime,
            this.clmMachine,
            this.clmEventType,
            this.clmEventReason});
            this.lstEvents.FullRowSelect = true;
            this.lstEvents.HideSelection = false;
            this.lstEvents.Location = new System.Drawing.Point(6, 20);
            this.lstEvents.MultiSelect = false;
            this.lstEvents.Name = "lstEvents";
            this.lstEvents.ShowItemToolTips = true;
            this.lstEvents.Size = new System.Drawing.Size(580, 129);
            this.lstEvents.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.lstEvents.TabIndex = 0;
            this.lstEvents.UseCompatibleStateImageBehavior = false;
            this.lstEvents.View = System.Windows.Forms.View.Details;
            this.lstEvents.SelectedIndexChanged += new System.EventHandler(this.lstEvents_SelectedIndexChanged);
            this.lstEvents.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstEvents_MouseDoubleClick);
            // 
            // clmDateTime
            // 
            this.clmDateTime.Text = "Date and Time";
            this.clmDateTime.Width = 120;
            // 
            // clmMachine
            // 
            this.clmMachine.Text = "Machine";
            // 
            // clmEventType
            // 
            this.clmEventType.Text = "Type";
            // 
            // clmEventReason
            // 
            this.clmEventReason.Text = "Reason";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.dateTimeEnd);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.dateTimeStart);
            this.groupBox4.Location = new System.Drawing.Point(7, 267);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(244, 100);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Time Frame Information";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(93, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "End Date:";
            // 
            // dateTimeEnd
            // 
            this.dateTimeEnd.Location = new System.Drawing.Point(8, 72);
            this.dateTimeEnd.Name = "dateTimeEnd";
            this.dateTimeEnd.Size = new System.Drawing.Size(229, 20);
            this.dateTimeEnd.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Start Date:";
            // 
            // dateTimeStart
            // 
            this.dateTimeStart.Location = new System.Drawing.Point(8, 34);
            this.dateTimeStart.Name = "dateTimeStart";
            this.dateTimeStart.Size = new System.Drawing.Size(229, 20);
            this.dateTimeStart.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblLastPasswordSet);
            this.groupBox5.Controls.Add(this.lblLastLogon);
            this.groupBox5.Controls.Add(this.lblLastBadPassword);
            this.groupBox5.Controls.Add(this.lblExpirationDate);
            this.groupBox5.Controls.Add(this.lblBadLogonCount);
            this.groupBox5.Controls.Add(this.lblLockoutTime);
            this.groupBox5.Controls.Add(this.lblLocked);
            this.groupBox5.Controls.Add(this.lblEnabled);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Location = new System.Drawing.Point(258, 8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(592, 100);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Account Information";
            // 
            // lblLastPasswordSet
            // 
            this.lblLastPasswordSet.AutoSize = true;
            this.lblLastPasswordSet.Location = new System.Drawing.Point(440, 84);
            this.lblLastPasswordSet.Name = "lblLastPasswordSet";
            this.lblLastPasswordSet.Size = new System.Drawing.Size(129, 13);
            this.lblLastPasswordSet.TabIndex = 15;
            this.lblLastPasswordSet.Text = "05/20/2019 07:01:13 PM";
            // 
            // lblLastLogon
            // 
            this.lblLastLogon.AutoSize = true;
            this.lblLastLogon.Location = new System.Drawing.Point(440, 62);
            this.lblLastLogon.Name = "lblLastLogon";
            this.lblLastLogon.Size = new System.Drawing.Size(129, 13);
            this.lblLastLogon.TabIndex = 14;
            this.lblLastLogon.Text = "05/20/2019 07:01:13 PM";
            // 
            // lblLastBadPassword
            // 
            this.lblLastBadPassword.AutoSize = true;
            this.lblLastBadPassword.Location = new System.Drawing.Point(440, 41);
            this.lblLastBadPassword.Name = "lblLastBadPassword";
            this.lblLastBadPassword.Size = new System.Drawing.Size(129, 13);
            this.lblLastBadPassword.TabIndex = 13;
            this.lblLastBadPassword.Text = "05/20/2019 07:01:13 PM";
            // 
            // lblExpirationDate
            // 
            this.lblExpirationDate.AutoSize = true;
            this.lblExpirationDate.Location = new System.Drawing.Point(440, 20);
            this.lblExpirationDate.Name = "lblExpirationDate";
            this.lblExpirationDate.Size = new System.Drawing.Size(129, 13);
            this.lblExpirationDate.TabIndex = 12;
            this.lblExpirationDate.Text = "05/20/2019 07:01:13 PM";
            // 
            // lblBadLogonCount
            // 
            this.lblBadLogonCount.AutoSize = true;
            this.lblBadLogonCount.Location = new System.Drawing.Point(110, 83);
            this.lblBadLogonCount.Name = "lblBadLogonCount";
            this.lblBadLogonCount.Size = new System.Drawing.Size(13, 13);
            this.lblBadLogonCount.TabIndex = 11;
            this.lblBadLogonCount.Text = "3";
            // 
            // lblLockoutTime
            // 
            this.lblLockoutTime.AutoSize = true;
            this.lblLockoutTime.Location = new System.Drawing.Point(110, 62);
            this.lblLockoutTime.Name = "lblLockoutTime";
            this.lblLockoutTime.Size = new System.Drawing.Size(129, 13);
            this.lblLockoutTime.TabIndex = 10;
            this.lblLockoutTime.Text = "05/20/2019 07:01:13 PM";
            // 
            // lblLocked
            // 
            this.lblLocked.AutoSize = true;
            this.lblLocked.Location = new System.Drawing.Point(110, 41);
            this.lblLocked.Name = "lblLocked";
            this.lblLocked.Size = new System.Drawing.Size(32, 13);
            this.lblLocked.TabIndex = 9;
            this.lblLocked.Text = "False";
            this.lblLocked.TextChanged += new System.EventHandler(this.lblLocked_TextChanged);
            // 
            // lblEnabled
            // 
            this.lblEnabled.AutoSize = true;
            this.lblEnabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblEnabled.Location = new System.Drawing.Point(110, 20);
            this.lblEnabled.Name = "lblEnabled";
            this.lblEnabled.Size = new System.Drawing.Size(29, 13);
            this.lblEnabled.TabIndex = 8;
            this.lblEnabled.Text = "True";
            this.lblEnabled.TextChanged += new System.EventHandler(this.lblEnabled_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(67, 41);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Locked:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(38, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Lockout Time:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(345, 83);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Last Password Set:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(380, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Last Logon:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(342, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Last Bad Password:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Bad Logon Count:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(361, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Expiration Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(64, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Enabled:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lstSignedOn);
            this.groupBox6.Location = new System.Drawing.Point(258, 267);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(297, 158);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Signed On Machines";
            // 
            // lstSignedOn
            // 
            this.lstSignedOn.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmSignOnMachine,
            this.clmSignOnTime,
            this.clmSignOnSessionID,
            this.clmSignOnState});
            this.lstSignedOn.FullRowSelect = true;
            this.lstSignedOn.HideSelection = false;
            this.lstSignedOn.Location = new System.Drawing.Point(6, 20);
            this.lstSignedOn.Name = "lstSignedOn";
            this.lstSignedOn.Size = new System.Drawing.Size(283, 127);
            this.lstSignedOn.TabIndex = 0;
            this.lstSignedOn.UseCompatibleStateImageBehavior = false;
            this.lstSignedOn.View = System.Windows.Forms.View.Details;
            // 
            // clmSignOnMachine
            // 
            this.clmSignOnMachine.Text = "Machine";
            // 
            // clmSignOnTime
            // 
            this.clmSignOnTime.Text = "Signed On At";
            this.clmSignOnTime.Width = 95;
            // 
            // clmSignOnSessionID
            // 
            this.clmSignOnSessionID.Text = "ID";
            // 
            // clmSignOnState
            // 
            this.clmSignOnState.Text = "State";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.lstServices);
            this.groupBox7.Location = new System.Drawing.Point(553, 267);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(297, 158);
            this.groupBox7.TabIndex = 8;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Services";
            // 
            // lstServices
            // 
            this.lstServices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmSvcMachine,
            this.clmServiceName,
            this.clmServiceID,
            this.clmServiceState});
            this.lstServices.FullRowSelect = true;
            this.lstServices.HideSelection = false;
            this.lstServices.Location = new System.Drawing.Point(7, 20);
            this.lstServices.Name = "lstServices";
            this.lstServices.Size = new System.Drawing.Size(283, 127);
            this.lstServices.TabIndex = 1;
            this.lstServices.UseCompatibleStateImageBehavior = false;
            this.lstServices.View = System.Windows.Forms.View.Details;
            // 
            // clmSvcMachine
            // 
            this.clmSvcMachine.Text = "Machine";
            // 
            // clmServiceName
            // 
            this.clmServiceName.Text = "Name";
            this.clmServiceName.Width = 95;
            // 
            // clmServiceID
            // 
            this.clmServiceID.Text = "ID";
            // 
            // clmServiceState
            // 
            this.clmServiceState.Text = "State";
            this.clmServiceState.Width = 88;
            // 
            // frmLockout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 474);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.primaryStatusStrip);
            this.Controls.Add(this.btnRunAnalysis);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmLockout";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "AD Lock Out Analyzer";
            this.Load += new System.EventHandler(this.frmLockout_Load);
            this.Shown += new System.EventHandler(this.frmLockout_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.primaryStatusStrip.ResumeLayout(false);
            this.primaryStatusStrip.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lblCurrentDomain;
        private System.Windows.Forms.TreeView viewMachines;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnRunAnalysis;
        private System.Windows.Forms.StatusStrip primaryStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView lstEvents;
        private System.Windows.Forms.ColumnHeader clmDateTime;
        private System.Windows.Forms.ColumnHeader clmMachine;
        private System.Windows.Forms.ColumnHeader clmEventType;
        private System.Windows.Forms.ColumnHeader clmEventReason;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimeEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimeStart;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblEnabled;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblLocked;
        private System.Windows.Forms.Label lblBadLogonCount;
        private System.Windows.Forms.Label lblLockoutTime;
        private System.Windows.Forms.Label lblLastPasswordSet;
        private System.Windows.Forms.Label lblLastLogon;
        private System.Windows.Forms.Label lblLastBadPassword;
        private System.Windows.Forms.Label lblExpirationDate;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ListView lstSignedOn;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ListView lstServices;
        private System.Windows.Forms.ColumnHeader clmServiceName;
        private System.Windows.Forms.ColumnHeader clmServiceState;
        private System.Windows.Forms.ColumnHeader clmSignOnMachine;
        private System.Windows.Forms.ColumnHeader clmSignOnTime;
        private System.Windows.Forms.ColumnHeader clmSignOnSessionID;
        private System.Windows.Forms.ColumnHeader clmSignOnState;
        private System.Windows.Forms.ColumnHeader clmSvcMachine;
        private System.Windows.Forms.ColumnHeader clmServiceID;
    }
}

