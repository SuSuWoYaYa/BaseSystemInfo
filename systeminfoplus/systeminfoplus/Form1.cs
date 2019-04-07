using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using IOEx;

namespace systeminfoplus
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            ShowInfo();
           

        }

        private void ShowInfo()
        {
            Computer computer = Computer.Instance();
            textBox1.Text = computer.OperatingSystem;
            textBox2.Text = computer.InstallDate;
            textBox3.Text = computer.HardDiskID;
            textBox4.Text = computer.IpAddress;
            textBox5.Text = computer.MacAddress;
            textBox6.Text = computer.PCsn;

            //textBox7.Text = computer.GetHdId();
            //try
            //{
            //    HardDiskInfo hdd = AtapiDevice.GetHddInfo(0); // 第一个硬盘
            //    textBox7.Text = hdd.SerialNumber;
            //}catch (Exception ex)
            //{
            //    textBox7.Text = ex.Message;
            //}

            try
            {
                HardDisk[] disk = new HardDisk().GetDevice();
                textBox7.Text = disk[0].SerialNumber;
            }
            catch (Exception ex)
            {
                textBox7.Text = ex.Message ;
            }

            textBox8.Text = computer.HardDiskSerialNumber;


            try
            {
                textBox9.Text = computer.GetHardDiskSerialNumber3();
            }
            catch (Exception ex)
            {
                textBox9.Text = ex.Message;
            } 
            textBox10.Text = HardDiskSN.SerialNumber;
            // DriveListEx  m_list = new DriveListEx();
             //textBox7.Text =m_list.s;
            // char* sn = stackalloc char[100];

             
            //
            //DiskInfo& di = DiskInfo::GetDiskInfo();
			//di.LoadDiskInfo();
			//SerialNumber = di.GetSerialNumber(cnt);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowInfo();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                textBox1.SelectionStart = 0;
                textBox1.SelectionLength = textBox1.Text.Length;
            }
            string copy = this.textBox1.SelectedText;
            if (copy != "") 
                Clipboard.SetDataObject(copy); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                textBox2.SelectionStart = 0;
                textBox2.SelectionLength = textBox2.Text.Length;
            }

            string copy = this.textBox2.SelectedText;
            if (copy != "") 
                Clipboard.SetDataObject(copy); 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length > 0)
            {
                textBox3.SelectionStart = 0;
                textBox3.SelectionLength = textBox3.Text.Length;
            }

            string copy = this.textBox3.SelectedText;
            if (copy != "") 
                Clipboard.SetDataObject(copy); 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Length > 0)
            {
                textBox4.SelectionStart = 0;
                textBox4.SelectionLength = textBox4.Text.Length;
            }
            string copy = this.textBox4.SelectedText;
            if (copy != "") 
                Clipboard.SetDataObject(copy); 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox5.Text.Length > 0)
            {
                textBox5.SelectionStart = 0;
                textBox5.SelectionLength = textBox5.Text.Length;
            }
            string copy = this.textBox5.SelectedText;
            if (copy != "") 
                Clipboard.SetDataObject(copy); 
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox6.Text.Length > 0)
            {
                textBox6.SelectionStart = 0;
                textBox6.SelectionLength = textBox6.Text.Length;
            }
            string copy = this.textBox6.SelectedText;
            if (copy != "") 
                Clipboard.SetDataObject(copy); 
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox7.Text.Length > 0)
            {
                textBox7.SelectionStart = 0;
                textBox7.SelectionLength = textBox7.Text.Length;
            }
            string copy = this.textBox7.SelectedText;
            if (copy != "")
                Clipboard.SetDataObject(copy); 
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox8.Text.Length > 0)
            {
                textBox8.SelectionStart = 0;
                textBox8.SelectionLength = textBox8.Text.Length;
            }
            string copy = this.textBox8.SelectedText;
            if (copy != "")
                Clipboard.SetDataObject(copy); 
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (textBox9.Text.Length > 0)
            {
                textBox9.SelectionStart = 0;
                textBox9.SelectionLength = textBox9.Text.Length;
            }
            string copy = this.textBox9.SelectedText;
            if (copy != "")
                Clipboard.SetDataObject(copy); 
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (textBox10.Text.Length > 0)
            {
                textBox10.SelectionStart = 0;
                textBox10.SelectionLength = textBox10.Text.Length;
            }
            string copy = this.textBox10.SelectedText;
            if (copy != "")
                Clipboard.SetDataObject(copy); 
        }
 

        
 

       

        
        

       
    }

 
       
}
