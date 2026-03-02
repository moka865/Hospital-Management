using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Hospital_Management
{
    public partial class Medicine : Form
    {
        SqlConnection Con = new SqlConnection(
            @"Data Source=MOKA\SQLEXPRESS;Initial Catalog=hospital_management_system;Integrated Security=True;TrustServerCertificate=True");

        private readonly string[] AllowedTypes = new[]
        {
            "Tablet", "Syrup", "Injection", "Capsule", "Ointment", "Drops"
        };

        public Medicine()
        {
            InitializeComponent();
            this.Load += Medicine_Load;
            dataGridView1.CellClick += dataGridView1_CellContentClick;
        }

        void populate()
        {
            try
            {
                Con.Open();
                string query = @"SELECT M_ID, M_Name, M_Type, M_Price FROM Medicine";
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

        // زر الإضافة
        private void Insert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textName.Text) ||
                string.IsNullOrWhiteSpace(textType.Text) ||
                string.IsNullOrWhiteSpace(textPrice.Text))
            {
                MessageBox.Show("Missing Information! Please fill Name, Type, and Price.");
                return;
            }

            var typeVal = textType.Text.Trim();
            if (!AllowedTypes.Contains(typeVal))
            {
                MessageBox.Show("Type must be one of: Tablet, Syrup, Injection, Capsule, Ointment, Drops.");
                return;
            }

            if (!int.TryParse(textPrice.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int price))
            {
                MessageBox.Show("Price must be a number");
                return;
            }

            try
            {
                Con.Open();
                string query = @"INSERT INTO Medicine (M_Name, M_Type, M_Price)
                                 VALUES (@Name, @Type, @Price)";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Name", textName.Text.Trim());
                cmd.Parameters.AddWithValue("@Type", typeVal);
                cmd.Parameters.AddWithValue("@Price", price);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Medicine Successfully Added");
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

        // زر التعديل
        private void Update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textID.Text))
            {
                MessageBox.Show("Select a Medicine to Update");
                return;
            }

            if (!int.TryParse(textID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int id))
            {
                MessageBox.Show("ID must be a number");
                return;
            }

            if (string.IsNullOrWhiteSpace(textName.Text) ||
                string.IsNullOrWhiteSpace(textType.Text) ||
                string.IsNullOrWhiteSpace(textPrice.Text))
            {
                MessageBox.Show("Missing Information! Please fill Name, Type, and Price.");
                return;
            }

            var typeVal = textType.Text.Trim();
            if (!AllowedTypes.Contains(typeVal))
            {
                MessageBox.Show("Type must be one of: Tablet, Syrup, Injection, Capsule, Ointment, Drops.");
                return;
            }

            if (!int.TryParse(textPrice.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int price))
            {
                MessageBox.Show("Price must be a number");
                return;
            }

            try
            {
                Con.Open();
                string query = @"UPDATE Medicine
                                 SET M_Name=@Name, M_Type=@Type, M_Price=@Price
                                 WHERE M_ID=@Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", textName.Text.Trim());
                cmd.Parameters.AddWithValue("@Type", typeVal);
                cmd.Parameters.AddWithValue("@Price", price);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Medicine Successfully Updated");
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

        // زر الحذف
        private void Delete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textID.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int id))
            {
                MessageBox.Show("Enter medicine ID to delete");
                return;
            }

            try
            {
                Con.Open();
                string query = "DELETE FROM Medicine WHERE M_ID = @Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", id);
                int rowsAffected = cmd.ExecuteNonQuery();

                MessageBox.Show(rowsAffected > 0 ? "Medicine Successfully Deleted" : "Medicine Not Found");
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textID.Text = row.Cells["M_ID"].Value?.ToString();
                textName.Text = row.Cells["M_Name"].Value?.ToString();
                textType.Text = row.Cells["M_Type"].Value?.ToString();
                textPrice.Text = row.Cells["M_Price"].Value?.ToString();
            }
        }

        private void ClearFields()
        {
            textID.Text = "";
            textName.Text = "";
            textType.Text = "";
            textPrice.Text = "";
            dataGridView1.ClearSelection();
        }

        private void Medicine_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void textID_TextChanged(object sender, EventArgs e) { }
        private void textName_TextChanged(object sender, EventArgs e) { }
        private void textType_TextChanged(object sender, EventArgs e) { }
        private void textPrice_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
    }
}