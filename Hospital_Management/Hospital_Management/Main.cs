using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital_Management
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Patient P = new Patient();
            P.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Doctor d = new Doctor();
            d.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Nurse n = new Nurse();
            n.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MedicalRecord mr = new MedicalRecord();
            mr.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Medicine m = new Medicine();
            m.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
        }
    }
}
