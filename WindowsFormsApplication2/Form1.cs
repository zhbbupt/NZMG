using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public double[] result1 = new double[2];
        public double[] result2 = new double[2];
        public double[] result3 = new double[2];
        public double test;
        //XYtoBL convert = new XYtoBL();
        LB_TO_XY nz = new LB_TO_XY();
        XY_TO_LB nz2 = new XY_TO_LB();
        XY_TO_LB_2 nz3 = new XY_TO_LB_2();
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            result1 = nz.NZMG_LB_TO_XY(41.0, 173.0, 47.0, 168.0, 6378388.0);
            textBox1.Text = result1[0].ToString();
            textBox2.Text = result1[1].ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            result1 = nz.NZMG_LB_TO_XY(41.0, 173.0, 43.0, 178.0, 6378388.0);
            result2 = nz2.NZMG_XY_TO_BL(41.0, 173.0, result1[0], result1[1], 6378388.0);
            textBox3.Text = result2[0].ToString();
            textBox4.Text = result2[1].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            result1 = nz.NZMG_LB_TO_XY(41.0, 173.0, 47.0, 168.0, 6378388.0);
            result3 = nz3.NZMG_XY_TO_BL(41.0, 173.0, result1[0], result1[1], 6378388.0);
            textBox5.Text = result3[0].ToString();
            textBox6.Text = result3[1].ToString();
        }
    }
}
