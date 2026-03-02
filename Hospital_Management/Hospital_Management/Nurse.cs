using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Hospital_Management
{
    public partial class Nurse : Form
    {
        SqlConnection Con = new SqlConnection(
            @"Data Source=MOKA\SQLEXPRESS;Initial Catalog=hospital_management_system;Integrated Security=True;TrustServerCertificate=True");

        private readonly string[] AllowedGenders = new[]
        {
            "male",
            "female",
            "It is best not to answer."
        };

        public Nurse()
        {
            InitializeComponent();
        }

        void populate()
        {
            try
            {
                Con.Open();
                string query = @"SELECT N_ID, N_Name, N_Phone, N_Gender FROM Nurse";
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


        // زر الإضافة
        private void Insert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textName.Text) ||
                string.IsNullOrWhiteSpace(textPhone.Text) ||
                string.IsNullOrWhiteSpace(textGender.Text))
            {
                MessageBox.Show("Missing Information! Please fill Name, Phone, and Gender.");
                return;
            }

            var genderVal = textGender.Text.Trim();
            if (!AllowedGenders.Contains(genderVal))
            {
                MessageBox.Show("Gender must be one of: male, female, It is best not to answer.");
                return;
            }

            try
            {
                Con.Open();
                string query = @"INSERT INTO Nurse (N_Name, N_Phone, N_Gender)
                                 VALUES (@Name, @Phone, @Gender)";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Name", textName.Text.Trim());
                cmd.Parameters.AddWithValue("@Phone", textPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", genderVal);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Nurse Successfully Added");
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

        private void Update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textID.Text))
            {
                MessageBox.Show("Select a Nurse to Update");
                return;
            }

            if (!int.TryParse(textID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int id))
            {
                MessageBox.Show("ID must be a number");
                return;
            }

            if (string.IsNullOrWhiteSpace(textName.Text) ||
                string.IsNullOrWhiteSpace(textPhone.Text) ||
                string.IsNullOrWhiteSpace(textGender.Text))
            {
                MessageBox.Show("Missing Information! Please fill Name, Phone, and Gender.");
                return;
            }

            var genderVal = textGender.Text.Trim();
            if (!AllowedGenders.Contains(genderVal))
            {
                MessageBox.Show("Gender must be one of: male, female, It is best not to answer.");
                return;
            }

            try
            {
                Con.Open();
                string query = @"UPDATE Nurse
                                 SET N_Name=@Name, N_Phone=@Phone, N_Gender=@Gender
                                 WHERE N_ID=@Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", textName.Text.Trim());
                cmd.Parameters.AddWithValue("@Phone", textPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", genderVal);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Nurse Successfully Updated");
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

        private void Delete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int id))
            {
                MessageBox.Show("Enter nurse ID to delete");
                return;
            }

            try
            {
                Con.Open();
                string query = "DELETE FROM Nurse WHERE N_ID = @Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", id);
                int rowsAffected = cmd.ExecuteNonQuery();

                MessageBox.Show(rowsAffected > 0 ? "Nurse Successfully Deleted" : "Nurse Not Found");
                populate();
                ClearFields();
            }
            catch (SqlException sqlEx)
            {
                // قد يفشل FK إذا كانت الممرضة مرتبطة بمرضى
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                textID.Text = row.Cells["N_ID"].Value?.ToString();
                textName.Text = row.Cells["N_Name"].Value?.ToString();
                textPhone.Text = row.Cells["N_Phone"].Value?.ToString();
                textGender.Text = row.Cells["N_Gender"].Value?.ToString();
            }
        }

        private void ClearFields()
        {
            textID.Text = "";
            textName.Text = "";
            textPhone.Text = "";
            textGender.Text = "";
        }

        // أحداث فارغة (يمكن حذفها أو استخدامها حسب الحاجة)
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textID_TextChanged(object sender, EventArgs e) { }
        private void textName_TextChanged(object sender, EventArgs e) { }
        private void textPhone_TextChanged(object sender, EventArgs e) { }
        private void textGender_SelectedIndexChanged(object sender, EventArgs e) { }

        private void Nurse_Load_1(object sender, EventArgs e)
        {
            populate();

        }
    }
}