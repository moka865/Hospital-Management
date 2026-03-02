using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hospital_Management
{
    public partial class Dashboard : Form
    {
        SqlConnection Con = new SqlConnection(
            @"Data Source=MOKA\SQLEXPRESS;Initial Catalog=hospital_management_system;Integrated Security=True;TrustServerCertificate=True");

        public Dashboard()
        {
            InitializeComponent();
            this.Load += Dashboard_Load;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            LoadCounts();
        }

        private void LoadCounts()
        {
            try
            {
                Con.Open();

                Patient.Text = GetCount("Patient").ToString();
                Doctor.Text = GetCount("Doctor").ToString();
                Nurse.Text = GetCount("Nurse").ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading counts: " + ex.Message);
            }
            finally
            {
                if (Con.State == System.Data.ConnectionState.Open) Con.Close();
            }
        }

        private int GetCount(string tableName)
        {
            using (var cmd = new SqlCommand($"SELECT COUNT(*) FROM {tableName}", Con))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void label7_Click(object sender, EventArgs e) { }
        private void Patient_Click(object sender, EventArgs e) { }
        private void Doctor_Click(object sender, EventArgs e) { }
        private void Nurse_Click(object sender, EventArgs e) { }
    }
}