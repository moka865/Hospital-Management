using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace Hospital_Management
{
    public partial class Doctor : Form
    {
        SqlConnection Con = new SqlConnection(
            @"Data Source=MOKA\SQLEXPRESS;Initial Catalog=hospital_management_system;Integrated Security=True;TrustServerCertificate=True");

        public Doctor()
        {
            InitializeComponent();
        }

        void populate()
        {
            try
            {
                Con.Open();
                string query = @"SELECT D_ID, D_Name, D_Salary, D_phone, D_Gender, D_Specialization, DEF_ID FROM Doctor";
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

        private void Doctor_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void Insert_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textName.Text) ||
                string.IsNullOrWhiteSpace(textSalary.Text) ||
                string.IsNullOrWhiteSpace(textPhone.Text) ||
                string.IsNullOrWhiteSpace(textGender.Text))
            {
                MessageBox.Show("Missing Information! Please fill Name, Salary, Phone, and Gender.");
                return;
            }

            if (!int.TryParse(textSalary.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int salary))
            {
                MessageBox.Show("Salary must be a number");
                return;
            }

            int? depId = null;
            if (!string.IsNullOrWhiteSpace(textDepartmentID.Text))
            {
                if (!int.TryParse(textDepartmentID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int d))
                {
                    MessageBox.Show($"Department ID must be numeric. Current value: \"{textDepartmentID.Text}\"");
                    return;
                }
                depId = d;
            }

            try
            {
                Con.Open();
                string query = @"INSERT INTO Doctor (D_Name, D_Salary, D_phone, D_Gender, D_Specialization, DEF_ID)
                                 VALUES (@Name, @Salary, @Phone, @Gender, @Spec, @Dep)";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Name", textName.Text.Trim());
                cmd.Parameters.AddWithValue("@Salary", salary);
                cmd.Parameters.AddWithValue("@Phone", textPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", textGender.Text.Trim());
                cmd.Parameters.AddWithValue("@Spec", textSpecialization.Text.Trim());
                cmd.Parameters.AddWithValue("@Dep", (object)depId ?? DBNull.Value);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Doctor Successfully Added");
                populate();
                ClearFields();
            }
            catch (SqlException sqlEx)
            {
                var depVal = textDepartmentID.Text.Trim();
                MessageBox.Show(
                    $"SQL Error ({sqlEx.Number}): {sqlEx.Message}\nDepartmentID sent: \"{depVal}\"",
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

        private void Update_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textID.Text))
            {
                MessageBox.Show("Select a Doctor to Update");
                return;
            }

            if (!int.TryParse(textID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int id))
            {
                MessageBox.Show("ID must be a number");
                return;
            }
            if (!int.TryParse(textSalary.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int salary))
            {
                MessageBox.Show("Salary must be a number");
                return;
            }

            int? depId = null;
            if (!string.IsNullOrWhiteSpace(textDepartmentID.Text))
            {
                if (!int.TryParse(textDepartmentID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int d))
                {
                    MessageBox.Show($"Department ID must be numeric. Current value: \"{textDepartmentID.Text}\"");
                    return;
                }
                depId = d;
            }

            try
            {
                Con.Open();
                string query = @"UPDATE Doctor
                                 SET D_Name=@Name, D_Salary=@Salary, D_phone=@Phone, D_Gender=@Gender,
                                     D_Specialization=@Spec, DEF_ID=@Dep
                                 WHERE D_ID=@Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", textName.Text.Trim());
                cmd.Parameters.AddWithValue("@Salary", salary);
                cmd.Parameters.AddWithValue("@Phone", textPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", textGender.Text.Trim());
                cmd.Parameters.AddWithValue("@Spec", textSpecialization.Text.Trim());
                cmd.Parameters.AddWithValue("@Dep", (object)depId ?? DBNull.Value);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Doctor Successfully Updated");
                populate();
                ClearFields();
            }
            catch (SqlException sqlEx)
            {
                var depVal = textDepartmentID.Text.Trim();
                MessageBox.Show(
                    $"SQL Error ({sqlEx.Number}): {sqlEx.Message}\nDepartmentID sent: \"{depVal}\"",
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

        private void Delete_Click_1(object sender, EventArgs e)
        {
            if (!int.TryParse(textID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int id))
            {
                MessageBox.Show("Enter doctor ID to delete");
                return;
            }

            try
            {
                Con.Open();
                string query = "DELETE FROM Doctor WHERE D_ID = @Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", id);
                int rowsAffected = cmd.ExecuteNonQuery();

                MessageBox.Show(rowsAffected > 0 ? "Doctor Successfully Deleted" : "Doctor Not Found");
                populate();
                ClearFields();
            }
            catch (SqlException sqlEx)
            {
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                textID.Text = row.Cells["D_ID"].Value?.ToString();
                textName.Text = row.Cells["D_Name"].Value?.ToString();
                textSalary.Text = row.Cells["D_Salary"].Value?.ToString();
                textPhone.Text = row.Cells["D_phone"].Value?.ToString();
                textGender.Text = row.Cells["D_Gender"].Value?.ToString();
                textSpecialization.Text = row.Cells["D_Specialization"].Value?.ToString();
                textDepartmentID.Text = row.Cells["DEF_ID"].Value == DBNull.Value ? "" : row.Cells["DEF_ID"].Value?.ToString();
            }
        }

        private void ClearFields()
        {
            textID.Text = "";
            textName.Text = "";
            textSalary.Text = "";
            textPhone.Text = "";
            textGender.Text = "";
            textSpecialization.Text = "";
            textDepartmentID.Text = "";
        }

        private void textGender_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textSalary_TextChanged(object sender, EventArgs e) { }
        private void textID_TextChanged(object sender, EventArgs e) { }
        private void textName_TextChanged(object sender, EventArgs e) { }
        private void textSpecialization_TextChanged(object sender, EventArgs e) { }
        private void textPhone_TextChanged(object sender, EventArgs e) { }
        private void textDepartmentID_TextChanged(object sender, EventArgs e) { }

        private void Doctor_Load_1(object sender, EventArgs e)
        {
            populate();

        }
    }
}