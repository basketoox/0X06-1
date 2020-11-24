using desay.ProductData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace desay.View
{
    public partial class WhiteBoardPower : Form
    {
        private SerialPort SP;
        public WhiteBoardPower(SerialPort sp)
        {
            InitializeComponent();
            SP = sp;
        }
        public WhiteBoardPower(SerialPort sp, out bool success)
        {
            InitializeComponent();
            SP = sp;
            if (SP != null)
            {
                success = true;
            }
            else
            {
                success = false;
            }
        }

        private void WhiteBoardPower_Load(object sender, EventArgs e)
        {
            // this.numericUpDown1.Value = Position.Instance.
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("是否立即生效", "是否立即生效", MessageBoxButtons.YesNo))
                {

                    if (this.SP.IsOpen)
                    {
                        SP.Write("SYST:REM" + Environment.NewLine);
                        Thread.Sleep(50);
                        SP.Write("INST CH1" + Environment.NewLine);
                        SP.Write("VOLT " + Position.Instance.Voltage[0].ToString() + Environment.NewLine);
                        Thread.Sleep(50);
                        SP.Write("CURR " + Position.Instance.Current[0].ToString() + Environment.NewLine);
                        Thread.Sleep(50);

                        SP.Write("INST CH2" + Environment.NewLine);
                        Thread.Sleep(50);
                        SP.Write("VOLT " + Position.Instance.Voltage[1].ToString() + Environment.NewLine);
                        Thread.Sleep(50);
                        SP.Write("CURR " + Position.Instance.Current[1].ToString() + Environment.NewLine);
                        Thread.Sleep(50);


                        SP.Write("INST CH3" + Environment.NewLine);
                        Thread.Sleep(50);
                        SP.Write("VOLT " + Position.Instance.Voltage[2].ToString() + Environment.NewLine);
                        Thread.Sleep(50);
                        SP.Write("CURR " + Position.Instance.Current[2].ToString() + Environment.NewLine);
                        Thread.Sleep(50);

                        this.SP.Write("OUTP 1" + Environment.NewLine);

                    }
                    else
                    {
                        MessageBox.Show("端口已断开");
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("立即生效异常:" + ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (SP.IsOpen)
            {
                SP.Write("OUPT 0\r\n");
                MessageBox.Show("完成");
            }
            else
            {
                MessageBox.Show("端口已断开，设置失败！");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("是否保存", "是否保存", MessageBoxButtons.YesNo))
                {
                    string SelectedString = comboBox1.Text;
                    switch (SelectedString)
                    {
                        case "通道1":
                            Position.Instance.Current[0] = (double)numericUpDown1.Value;
                            Position.Instance.Voltage[0] = (double)numericUpDown2.Value;
                            break;
                        case "通道2":
                            Position.Instance.Current[1] = (double)numericUpDown1.Value;
                            Position.Instance.Voltage[1] = (double)numericUpDown2.Value;
                            break;
                        case "通道3":
                            Position.Instance.Current[2] = (double)numericUpDown1.Value;
                            Position.Instance.Voltage[2] = (double)numericUpDown2.Value;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存异常:" + ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string SelectedString = comboBox1.Text;
            switch (SelectedString)
            {
                case "通道1":
                    numericUpDown1.Value = (decimal)Position.Instance.Current[0];
                    numericUpDown2.Value = (decimal)Position.Instance.Voltage[0];
                    break;
                case "通道2":
                    numericUpDown1.Value = (decimal)Position.Instance.Current[1];
                    numericUpDown2.Value = (decimal)Position.Instance.Voltage[1];
                    break;
                case "通道3":
                    numericUpDown1.Value = (decimal)Position.Instance.Current[2];
                    numericUpDown2.Value = (decimal)Position.Instance.Voltage[2];
                    break;
                default:
                    break;
            }

        }

    }
}

