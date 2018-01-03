using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MonitorPorts
{
    public partial class FilterForm : Form
    {
        public static double PcapLengthNum;
        public double MaxVOBCToZC;
        public double MaxVOBCToCI;
        public double MaxVOBCToATS;
        public double MaxATSToVOBC;
        public double MaxCIToVOBC;
        public double MaxZCToVOBC;
        private List<string> BackUps = new List<string>();

        public FilterForm()
        {
            InitializeComponent();
            MaxVOBCToATS = MaxVOBCToZC = MaxVOBCToCI = MaxATSToVOBC = MaxCIToVOBC = MaxZCToVOBC = 2.4;
            IPList ReadIP = new IPList();
            LoadIpConfiguration();
            LoadHistoryConfig();
        }

        private void Unit1_CheckedChanged(object sender, EventArgs e)
        {
            if (Unit1.Checked)
            {
                Unit2.Checked = false;
                Unit1.Checked = true;
            }
        }

        private void Unit2_CheckedChanged(object sender, EventArgs e)
        {
            if (Unit2.Checked)
            {
                Unit1.Checked = false;
                Unit2.Checked = true;
            }
        }

        public void LoadIpConfiguration()
        {
            for (int i = 1; i < IPList.ConfigProperties.Count; i++)
            {
                SourceListBox1.Items.Add(IPList.ConfigProperties[i].Name);
                DestListBox2.Items.Add(IPList.ConfigProperties[i].Name);
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            GetAlarmInterval();
            SetPcapLength();
            SaveSetting();
            SaveToTxt();
            this.Hide();
        }

        private void SaveToTxt()
        {
            FileStream fs = new FileStream("BackUps.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine(this.txtPcapLength.Text);
            sw.WriteLine(this.Unit1.Checked ? "MB" : "GB");

            string Source = "";
            string Dest = "";
            for (int i = 0; i < SourceListBox1.Items.Count; i++)
            {
                if (SourceListBox1.GetItemChecked(i))
                {
                    Source += "#" + SourceListBox1.Items[i].ToString();
                }
            }
            for (int i = 0; i < DestListBox2.Items.Count; i++)
            {
                if (DestListBox2.GetItemChecked(i))
                {
                    Dest += "#" + DestListBox2.Items[i].ToString();
                }
            }
            sw.WriteLine(Source);
            sw.WriteLine(Dest);

            sw.WriteLine(MaxVOBCToZC * 1000);
            sw.WriteLine(MaxZCToVOBC * 1000);

            sw.WriteLine(MaxVOBCToCI * 1000);
            sw.WriteLine(MaxCIToVOBC * 1000);

            sw.WriteLine(MaxVOBCToATS * 1000);
            sw.WriteLine(MaxATSToVOBC * 1000);

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        private void GetAlarmInterval()
        {
            if (txtVOBCToZC.Text != "" && txtVOBCToZC.Text != null)
            {
                MaxVOBCToZC = Convert.ToDouble(this.txtVOBCToZC.Text) / 1000;
            }
            if (txtVOBCToCI.Text != "" && txtVOBCToCI.Text != null)
            {
                MaxVOBCToCI = Convert.ToDouble(this.txtVOBCToCI.Text) / 1000;
            }
            if (txtVOBCToATS.Text != "" && txtVOBCToATS.Text != null)
            {
                MaxVOBCToATS = Convert.ToDouble(this.txtVOBCToATS.Text) / 1000;
            }
            if (txtZCToVOBC.Text != "" && txtZCToVOBC.Text != null)
            {
                MaxZCToVOBC = Convert.ToDouble(this.txtZCToVOBC.Text) / 1000;
            }
            if (txtCIToVOBC.Text != "" && txtCIToVOBC.Text != null)
            {
                MaxCIToVOBC = Convert.ToDouble(this.txtCIToVOBC.Text) / 1000;
            }
            if (txtATSToVOBC.Text != "" && txtATSToVOBC.Text != null)
            {
                MaxATSToVOBC = Convert.ToDouble(this.txtATSToVOBC.Text) / 1000;
            }
        }

        private void SetPcapLength()
        {
            int PcapLength = Convert.ToInt16(this.txtPcapLength.Text);
            string PcapLengthUnit;
            if (this.Unit1.Checked)
            {
                PcapLengthUnit = "MB";
            }
            else
            {
                PcapLengthUnit = "GB";
            }
            GetPcapLengthNum(PcapLength, PcapLengthUnit);
        }

        private void GetPcapLengthNum(int PcapLength,string PcapLengthUnit)
        {
            if (PcapLengthUnit == "MB")
            {
                PcapLengthNum = PcapLength * 1024 * 1024;
            }
            if (PcapLengthUnit == "GB")
            {
                PcapLengthNum = PcapLength * 1024 * 1024 * 1024;
            }
        }

        private void SaveSetting()
        {
            for (int i = 0; i < SourceListBox1.Items.Count; i++)
            {
                foreach (var item in IPList.ConfigProperties)
                {
                    if (item.Name == SourceListBox1.Items[i].ToString())
                    {
                        if (SourceListBox1.GetItemChecked(i))
                        {
                            item.IsSourceChoose = true;
                        }
                        else
                        {
                            item.IsSourceChoose = false;
                        }
                    }
                }
            }
            for (int i = 0; i < DestListBox2.Items.Count; i++)
            {
                foreach (var item in IPList.ConfigProperties)
                {
                    if (item.Name == DestListBox2.Items[i].ToString())
                    {
                        if (DestListBox2.GetItemChecked(i))
                        {
                            item.IsDestChoose = true;
                        }
                        else
                        {
                            item.IsDestChoose = false;
                        }
                    }
                }
            }
        }

        private void FilterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
        }
        
        private void LoadHistoryConfig()
        {
            StreamReader sr = new StreamReader("BackUps.txt", Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                BackUps.Add(line);
            }
            sr.Close();
            if (BackUps.Count != 0)
            {
                LoadHistory(BackUps);
            }
        }

        private void LoadHistory(List<string> backUps)
        {
            if (backUps[0]!="" && backUps[0]!=null)
            {
                this.txtPcapLength.Text = backUps[0];
            }
            if (backUps[1] == "GB")
            {
                this.Unit2.Checked = true;
                this.Unit1.Checked = false;
            }
            if (backUps[1] == "MB")
            {
                this.Unit1.Checked = true;
                this.Unit2.Checked = false;
            }
            this.txtVOBCToZC.Text = backUps[4];
            this.txtZCToVOBC.Text = backUps[5];
            this.txtVOBCToCI.Text = backUps[6];
            this.txtCIToVOBC.Text = backUps[7];
            this.txtVOBCToATS.Text = backUps[8];
            this.txtATSToVOBC.Text = backUps[9];
        }

        private void SelectAll(CheckedListBox ListBox, CheckState state)
        {
            for (int i = 0; i < ListBox.Items.Count; i++)
            {
                ListBox.SetItemCheckState(i, state);
            }
        }

        private void SourceSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.SourceSelectAll.Checked)
            {
                SelectAll(SourceListBox1, CheckState.Checked);
            }
            else
            {
                SelectAll(SourceListBox1, CheckState.Unchecked);
            }
        }

        private void DestSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.DestSelectAll.Checked)
            {
                SelectAll(DestListBox2, CheckState.Checked);
            }
            else
            {
                SelectAll(DestListBox2, CheckState.Unchecked);
            }
        }
    }
}
