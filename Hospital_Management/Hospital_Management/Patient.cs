using System;
using System.IO;

using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace Hospital_Management
{
    public partial class Patient : Form
    {
        SqlConnection Con = new SqlConnection(
            @"Data Source=MOKA\SQLEXPRESS;Initial Catalog=hospital_management_system;Integrated Security=True;TrustServerCertificate=True");

        public Patient()
        {
            InitializeComponent();
        }

        void populate()
        {
            try
            {
                Con.Open();
                string query = @"SELECT * FROM Patient"; 
                SqlDataAdapter da = new SqlDataAdapter(query, Con);
                var ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
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


        private void Patient_Load_1(object sender, EventArgs e)
        {
            populate();
        }

        // زر الإضافة
        private void Insert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textName.Text) ||
                string.IsNullOrWhiteSpace(textAge.Text) ||
                string.IsNullOrWhiteSpace(textGender.Text))
            {
                MessageBox.Show("Missing Information! Please fill Name, Age, and Gender.");
                return;
            }

            if (!int.TryParse(textAge.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int age))
            {
                MessageBox.Show("Age must be a number");
                return;
            }

            int? roomId = int.TryParse(textRoomID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int r) ? r : (int?)null;
            int? nurseId = int.TryParse(textNurseID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int n) ? n : (int?)null;

            try
            {
                Con.Open();
                string query = @"INSERT INTO Patient (P_Name, P_Gender, P_Phone, P_Age, P_Address, Room_ID, N_ID)
                                 VALUES (@Name, @Gender, @Phone, @Age, @Address, @Room, @Nurse)";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Name", textName.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", textGender.Text.Trim());
                cmd.Parameters.AddWithValue("@Phone", textPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Address", textAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@Room", (object)roomId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nurse", (object)nurseId ?? DBNull.Value);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Patient Successfully Added");
                ClearFields();
                populate();
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 547)
                    MessageBox.Show("Error: The Room ID or Nurse ID you entered does not exist.");
                else
                    MessageBox.Show("SQL Error: " + sqlEx.Message);
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
                MessageBox.Show("Select a Patient to Update");
                return;
            }

            if (!int.TryParse(textID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int id))
            {
                MessageBox.Show("ID must be a number");
                return;
            }

            if (!int.TryParse(textAge.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int age))
            {
                MessageBox.Show("Age must be a number");
                return;
            }

            int? roomId = int.TryParse(textRoomID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int r) ? r : (int?)null;
            int? nurseId = int.TryParse(textNurseID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int n) ? n : (int?)null;

            try
            {
                Con.Open();
                string query = @"UPDATE Patient
                                 SET P_Name=@Name, P_Gender=@Gender, P_Phone=@Phone, P_Age=@Age, P_Address=@Address,
                                     Room_ID=@Room, N_ID=@Nurse
                                 WHERE P_ID=@Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", textName.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", textGender.Text.Trim());
                cmd.Parameters.AddWithValue("@Phone", textPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Address", textAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@Room", (object)roomId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nurse", (object)nurseId ?? DBNull.Value);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Patient Successfully Updated");
                ClearFields();
                populate();
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 547)
                    MessageBox.Show("Error: The Room ID or Nurse ID does not exist.");
                else
                    MessageBox.Show("SQL Error: " + sqlEx.Message);
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

        private void Delete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int id))
            {
                MessageBox.Show("Enter patient ID to delete");
                return;
            }

            try
            {
                Con.Open();
                string query = "DELETE FROM Patient WHERE P_ID = @Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", id);
                int rowsAffected = cmd.ExecuteNonQuery();

                MessageBox.Show(rowsAffected > 0 ? "Patient Successfully Deleted" : "Patient Not Found");
                ClearFields();
                populate();
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 547)
                    MessageBox.Show("Cannot delete this patient. They have related records (appointments/medical records/medicines).");
                else
                    MessageBox.Show("SQL Error: " + sqlEx.Message);
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

        // اختيار صف من الجدول
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                textID.Text = row.Cells["P_ID"].Value?.ToString();
                textName.Text = row.Cells["P_Name"].Value?.ToString();
                textPhone.Text = row.Cells["P_Phone"].Value?.ToString();
                textGender.Text = row.Cells["P_Gender"].Value?.ToString();
                textAge.Text = row.Cells["P_Age"].Value?.ToString();
                textAddress.Text = row.Cells["P_Address"].Value?.ToString();
                textRoomID.Text = row.Cells["Room_ID"].Value == DBNull.Value ? "" : row.Cells["Room_ID"].Value?.ToString();
                textNurseID.Text = row.Cells["N_ID"].Value == DBNull.Value ? "" : row.Cells["N_ID"].Value?.ToString();
            }
        }

        private void ClearFields()
        {
            textID.Clear();
            textName.Clear();
            textPhone.Clear();
            textAge.Clear();
            textAddress.Clear();
            textRoomID.Clear();
            textNurseID.Clear();
            textGender.SelectedIndex = -1;
            textGender.Text = "";
        }

        // أحداث فارغة
        private void textID_TextChanged(object sender, EventArgs e) { }
        private void textAge_TextChanged(object sender, EventArgs e) { }
        private void textName_TextChanged(object sender, EventArgs e) { }
        private void TextGender_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textAddress_TextChanged(object sender, EventArgs e) { }
        private void textGender_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void textPhone_TextChanged(object sender, EventArgs e) { }
        private void textRoomID_TextChanged(object sender, EventArgs e) { }
        private void textNurseID_TextChanged(object sender, EventArgs e) { }
    }
}