namespace MonitorPorts
{
    partial class FilterForm
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
            this.GroupBox_Protocol = new System.Windows.Forms.GroupBox();
            this.Unit2 = new System.Windows.Forms.CheckBox();
            this.Unit1 = new System.Windows.Forms.CheckBox();
            this.txtPcapLength = new System.Windows.Forms.TextBox();
            this.GroupBox_Source = new System.Windows.Forms.GroupBox();
            this.DestSelectAll = new System.Windows.Forms.CheckBox();
            this.SourceSelectAll = new System.Windows.Forms.CheckBox();
            this.DestListBox2 = new System.Windows.Forms.CheckedListBox();
            this.SourceListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label_SourceIP = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtATSToVOBC = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtVOBCToATS = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCIToVOBC = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtVOBCToCI = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtZCToVOBC = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVOBCToZC = new System.Windows.Forms.TextBox();
            this.GroupBox_Protocol.SuspendLayout();
            this.GroupBox_Source.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox_Protocol
            // 
            this.GroupBox_Protocol.Controls.Add(this.Unit2);
            this.GroupBox_Protocol.Controls.Add(this.Unit1);
            this.GroupBox_Protocol.Controls.Add(this.txtPcapLength);
            this.GroupBox_Protocol.Dock = System.Windows.Forms.DockStyle.Top;
            this.GroupBox_Protocol.Location = new System.Drawing.Point(0, 0);
            this.GroupBox_Protocol.Name = "GroupBox_Protocol";
            this.GroupBox_Protocol.Size = new System.Drawing.Size(279, 45);
            this.GroupBox_Protocol.TabIndex = 4;
            this.GroupBox_Protocol.TabStop = false;
            this.GroupBox_Protocol.Text = "保存pcap文件大小";
            // 
            // Unit2
            // 
            this.Unit2.AutoSize = true;
            this.Unit2.Location = new System.Drawing.Point(189, 18);
            this.Unit2.Name = "Unit2";
            this.Unit2.Size = new System.Drawing.Size(36, 16);
            this.Unit2.TabIndex = 2;
            this.Unit2.Text = "GB";
            this.Unit2.UseVisualStyleBackColor = true;
            this.Unit2.CheckedChanged += new System.EventHandler(this.Unit2_CheckedChanged);
            // 
            // Unit1
            // 
            this.Unit1.AutoSize = true;
            this.Unit1.Checked = true;
            this.Unit1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Unit1.Location = new System.Drawing.Point(147, 18);
            this.Unit1.Name = "Unit1";
            this.Unit1.Size = new System.Drawing.Size(36, 16);
            this.Unit1.TabIndex = 1;
            this.Unit1.Text = "MB";
            this.Unit1.UseVisualStyleBackColor = true;
            this.Unit1.CheckedChanged += new System.EventHandler(this.Unit1_CheckedChanged);
            // 
            // txtPcapLength
            // 
            this.txtPcapLength.Location = new System.Drawing.Point(51, 16);
            this.txtPcapLength.Name = "txtPcapLength";
            this.txtPcapLength.Size = new System.Drawing.Size(75, 21);
            this.txtPcapLength.TabIndex = 0;
            this.txtPcapLength.Text = "10";
            // 
            // GroupBox_Source
            // 
            this.GroupBox_Source.Controls.Add(this.DestSelectAll);
            this.GroupBox_Source.Controls.Add(this.SourceSelectAll);
            this.GroupBox_Source.Controls.Add(this.DestListBox2);
            this.GroupBox_Source.Controls.Add(this.SourceListBox1);
            this.GroupBox_Source.Controls.Add(this.label1);
            this.GroupBox_Source.Controls.Add(this.label_SourceIP);
            this.GroupBox_Source.Dock = System.Windows.Forms.DockStyle.Top;
            this.GroupBox_Source.Location = new System.Drawing.Point(0, 45);
            this.GroupBox_Source.Name = "GroupBox_Source";
            this.GroupBox_Source.Size = new System.Drawing.Size(279, 188);
            this.GroupBox_Source.TabIndex = 5;
            this.GroupBox_Source.TabStop = false;
            this.GroupBox_Source.Text = "IP地址筛选";
            // 
            // DestSelectAll
            // 
            this.DestSelectAll.AutoSize = true;
            this.DestSelectAll.Location = new System.Drawing.Point(150, 42);
            this.DestSelectAll.Name = "DestSelectAll";
            this.DestSelectAll.Size = new System.Drawing.Size(48, 16);
            this.DestSelectAll.TabIndex = 7;
            this.DestSelectAll.Text = "全选";
            this.DestSelectAll.UseVisualStyleBackColor = true;
            this.DestSelectAll.CheckedChanged += new System.EventHandler(this.DestSelectAll_CheckedChanged);
            // 
            // SourceSelectAll
            // 
            this.SourceSelectAll.AutoSize = true;
            this.SourceSelectAll.Location = new System.Drawing.Point(17, 42);
            this.SourceSelectAll.Name = "SourceSelectAll";
            this.SourceSelectAll.Size = new System.Drawing.Size(48, 16);
            this.SourceSelectAll.TabIndex = 6;
            this.SourceSelectAll.Text = "全选";
            this.SourceSelectAll.UseVisualStyleBackColor = true;
            this.SourceSelectAll.CheckedChanged += new System.EventHandler(this.SourceSelectAll_CheckedChanged);
            // 
            // DestListBox2
            // 
            this.DestListBox2.FormattingEnabled = true;
            this.DestListBox2.Location = new System.Drawing.Point(147, 61);
            this.DestListBox2.Name = "DestListBox2";
            this.DestListBox2.Size = new System.Drawing.Size(123, 116);
            this.DestListBox2.TabIndex = 5;
            // 
            // SourceListBox1
            // 
            this.SourceListBox1.FormattingEnabled = true;
            this.SourceListBox1.Location = new System.Drawing.Point(14, 61);
            this.SourceListBox1.Name = "SourceListBox1";
            this.SourceListBox1.Size = new System.Drawing.Size(123, 116);
            this.SourceListBox1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(150, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "目的IP：";
            // 
            // label_SourceIP
            // 
            this.label_SourceIP.AutoSize = true;
            this.label_SourceIP.Location = new System.Drawing.Point(14, 23);
            this.label_SourceIP.Name = "label_SourceIP";
            this.label_SourceIP.Size = new System.Drawing.Size(41, 12);
            this.label_SourceIP.TabIndex = 0;
            this.label_SourceIP.Text = "源IP：";
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(116, 417);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(74, 26);
            this.button_OK.TabIndex = 7;
            this.button_OK.Text = "确定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(196, 417);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(74, 26);
            this.button_Cancel.TabIndex = 8;
            this.button_Cancel.Text = "取消";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtATSToVOBC);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtVOBCToATS);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtCIToVOBC);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtVOBCToCI);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtZCToVOBC);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtVOBCToZC);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 233);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 181);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "告警门限";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 152);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 12);
            this.label10.TabIndex = 10;
            this.label10.Text = "ATS发送给VOBC";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(161, 152);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 11;
            this.label11.Text = "单位（ms）";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 126);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 12);
            this.label12.TabIndex = 12;
            this.label12.Text = "VOBC发送给ATS";
            // 
            // txtATSToVOBC
            // 
            this.txtATSToVOBC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtATSToVOBC.Location = new System.Drawing.Point(99, 148);
            this.txtATSToVOBC.Name = "txtATSToVOBC";
            this.txtATSToVOBC.Size = new System.Drawing.Size(56, 21);
            this.txtATSToVOBC.TabIndex = 8;
            this.txtATSToVOBC.Text = "2400";
            this.txtATSToVOBC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(161, 126);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 13;
            this.label13.Text = "单位（ms）";
            // 
            // txtVOBCToATS
            // 
            this.txtVOBCToATS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVOBCToATS.Location = new System.Drawing.Point(99, 122);
            this.txtVOBCToATS.Name = "txtVOBCToATS";
            this.txtVOBCToATS.Size = new System.Drawing.Size(56, 21);
            this.txtVOBCToATS.TabIndex = 9;
            this.txtVOBCToATS.Text = "2400";
            this.txtVOBCToATS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "CI发送给VOBC";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(161, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "单位（ms）";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "VOBC发送给CI";
            // 
            // txtCIToVOBC
            // 
            this.txtCIToVOBC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCIToVOBC.Location = new System.Drawing.Point(99, 97);
            this.txtCIToVOBC.Name = "txtCIToVOBC";
            this.txtCIToVOBC.Size = new System.Drawing.Size(56, 21);
            this.txtCIToVOBC.TabIndex = 2;
            this.txtCIToVOBC.Text = "2400";
            this.txtCIToVOBC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(161, 75);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 7;
            this.label9.Text = "单位（ms）";
            // 
            // txtVOBCToCI
            // 
            this.txtVOBCToCI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVOBCToCI.Location = new System.Drawing.Point(99, 71);
            this.txtVOBCToCI.Name = "txtVOBCToCI";
            this.txtVOBCToCI.Size = new System.Drawing.Size(56, 21);
            this.txtVOBCToCI.TabIndex = 3;
            this.txtVOBCToCI.Text = "2400";
            this.txtVOBCToCI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "ZC发送给VOBC";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(161, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "单位（ms）";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "VOBC发送给ZC";
            // 
            // txtZCToVOBC
            // 
            this.txtZCToVOBC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtZCToVOBC.Location = new System.Drawing.Point(99, 45);
            this.txtZCToVOBC.Name = "txtZCToVOBC";
            this.txtZCToVOBC.Size = new System.Drawing.Size(56, 21);
            this.txtZCToVOBC.TabIndex = 0;
            this.txtZCToVOBC.Text = "2400";
            this.txtZCToVOBC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "单位（ms）";
            // 
            // txtVOBCToZC
            // 
            this.txtVOBCToZC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVOBCToZC.Location = new System.Drawing.Point(99, 19);
            this.txtVOBCToZC.Name = "txtVOBCToZC";
            this.txtVOBCToZC.Size = new System.Drawing.Size(56, 21);
            this.txtVOBCToZC.TabIndex = 0;
            this.txtVOBCToZC.Text = "2400";
            this.txtVOBCToZC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // FilterForm
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(279, 447);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.GroupBox_Source);
            this.Controls.Add(this.GroupBox_Protocol);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FilterForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新建设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FilterForm_FormClosed);
            this.GroupBox_Protocol.ResumeLayout(false);
            this.GroupBox_Protocol.PerformLayout();
            this.GroupBox_Source.ResumeLayout(false);
            this.GroupBox_Source.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBox_Protocol;
        private System.Windows.Forms.GroupBox GroupBox_Source;
        private System.Windows.Forms.Label label_SourceIP;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.CheckedListBox DestListBox2;
        private System.Windows.Forms.CheckedListBox SourceListBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtVOBCToZC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtZCToVOBC;
        private System.Windows.Forms.CheckBox Unit2;
        private System.Windows.Forms.CheckBox Unit1;
        private System.Windows.Forms.TextBox txtPcapLength;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtATSToVOBC;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtVOBCToATS;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCIToVOBC;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtVOBCToCI;
        private System.Windows.Forms.CheckBox DestSelectAll;
        private System.Windows.Forms.CheckBox SourceSelectAll;
    }
}