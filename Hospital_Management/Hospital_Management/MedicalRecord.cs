using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace Hospital_Management
{
    public partial class MedicalRecord : Form
    {
        SqlConnection Con = new SqlConnection(
            @"Data Source=MOKA\SQLEXPRESS;Initial Catalog=hospital_management_system;Integrated Security=True;TrustServerCertificate=True");

        public MedicalRecord()
        {
            InitializeComponent();
            this.Load += MedicalRecord_Load;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
        }

        void populate()
        {
            try
            {
                Con.Open();
                string query = @"SELECT R_ID, Diagnosis, Date, P_ID FROM MedicalRecord";
                SqlDataAdapter da = new SqlDataAdapter(query, Con);
                var ds = new DataSet();
                da.Fill(ds);
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open) Con.Close();
            }
        }

        private void MedicalRecord_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void Insert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textPatientID.Text))
            {
                MessageBox.Show("Patient ID is required.");
                return;
            }

            if (!int.TryParse(textPatientID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int pid))
            {
                MessageBox.Show("Patient ID must be a number");
                return;
            }

            string diagnosis = textDiagnosis.Text.Trim();
            DateTime date = dateTimePicker.Value.Date;

            try
            {
                Con.Open();
                string query = @"INSERT INTO MedicalRecord (Diagnosis, Date, P_ID)
                                 VALUES (@Diag, @Date, @Pid)";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Diag", string.IsNullOrWhiteSpace(diagnosis) ? (object)DBNull.Value : diagnosis);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@Pid", pid);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Successfully Added");
                populate();
                ClearFields();
            }
            catch (SqlException sqlEx)
            {
                var pidVal = textPatientID.Text.Trim();
                MessageBox.Show(
                    $"SQL Error ({sqlEx.Number}): {sqlEx.Message}\nPatientID sent: \"{pidVal}\"",
                    "SQL Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open) Con.Close();
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textID.Text))
            {
                MessageBox.Show("Select a record to update");
                return;
            }

            if (!int.TryParse(textID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int id))
            {
                MessageBox.Show("Record ID must be a number");
                return;
            }

            if (string.IsNullOrWhiteSpace(textPatientID.Text) ||
                !int.TryParse(textPatientID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int pid))
            {
                MessageBox.Show("Patient ID must be a number");
                return;
            }

            string diagnosis = textDiagnosis.Text.Trim();
            DateTime date = dateTimePicker.Value.Date;

            try
            {
                Con.Open();
                string query = @"UPDATE MedicalRecord
                                 SET Diagnosis=@Diag, Date=@Date, P_ID=@Pid
                                 WHERE R_ID=@Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Diag", string.IsNullOrWhiteSpace(diagnosis) ? (object)DBNull.Value : diagnosis);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@Pid", pid);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Successfully Updated");
                populate();
                ClearFields();
            }
            catch (SqlException sqlEx)
            {
                var pidVal = textPatientID.Text.Trim();
                MessageBox.Show(
                    $"SQL Error ({sqlEx.Number}): {sqlEx.Message}\nPatientID sent: \"{pidVal}\"",
                    "SQL Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open) Con.Close();
            }
        }

        // زر الحذف
        private void Delete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int id))
            {
                MessageBox.Show("Enter record ID to delete");
                return;
            }

            try
            {
                Con.Open();
                string query = "DELETE FROM MedicalRecord WHERE R_ID = @Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", id);
                int rowsAffected = cmd.ExecuteNonQuery();

                MessageBox.Show(rowsAffected > 0 ? "Record Successfully Deleted" : "Record Not Found");
                populate();
                ClearFields();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Error ({sqlEx.Number}): {sqlEx.Message}", "SQL Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open) Con.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                textID.Text = row.Cells["R_ID"].Value?.ToString();
                textDiagnosis.Text = row.Cells["Diagnosis"].Value?.ToString();
                textPatientID.Text = row.Cells["P_ID"].Value?.ToString();

                if (row.Cells["Date"].Value != null && DateTime.TryParse(row.Cells["Date"].Value.ToString(), out DateTime d))
                    dateTimePicker.Value = d;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);
        }

        private void ClearFields()
        {
            textID.Text = "";
            textPatientID.Text = "";
            textDiagnosis.Text = "";
            dateTimePicker.Value = DateTime.Today;
            dataGridView1.ClearSelection();
        }

        private void textID_TextChanged(object sender, EventArgs e) { }
        private void textPatientID_TextChanged(object sender, EventArgs e) { }
        private void textDiagnosis_TextChanged(object sender, EventArgs e) { }
        private void dateTimePicker_ValueChanged(object sender, EventArgs e) { }
        private void MedicalRecord_Load_1(object sender, EventArgs e) { }
    }
}