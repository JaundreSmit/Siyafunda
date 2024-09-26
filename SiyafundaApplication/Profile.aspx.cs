using System;
using System.Data.SqlClient;
using System.Data;
using System.Xml;

namespace SiyafundaApplication  //TODO User_id (ex. row 27) needs to contain logged in user's ID
{
    public partial class Profile : System.Web.UI.Page
    {
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserProfile();
                LoadUserModules();
            }
        }

        private void LoadUserProfile()
        {
            int userId;
            if (int.TryParse(Session["user_id"]?.ToString(), out userId))
            {
                using (SqlConnection conn = new SqlConnection(getConnectionString()))
                {
                    string query = @"SELECT u.Name, u.Surname, u.Email, r.role_name
                                     FROM [dbo].[Users] u
                                     INNER JOIN [dbo].[Roles] r ON u.Role_id = r.role_id
                                     WHERE u.user_id = @user_id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        RoleLabel.Text = "Role: " + reader["role_name"].ToString();
                        NameLabel.Text = "Name: " + reader["Name"].ToString();
                        SurnameLabel.Text = "Surname: " + reader["Surname"].ToString();
                        EmailLabel.Text = "Email: " + reader["Email"].ToString();

                        // Pre-fill textboxes for editing
                        NameTextBox.Text = reader["Name"].ToString();
                        SurnameTextBox.Text = reader["Surname"].ToString();
                        EmailTextBox.Text = reader["Email"].ToString();
                    }
                    conn.Close();
                }
            }
        }

        private void LoadUserModules()
        {
            int userId;
            if (int.TryParse(Session["user_id"]?.ToString(), out userId))
            {
                using (SqlConnection conn = new SqlConnection(getConnectionString()))
                {
                    string query = @"SELECT m.title, m.description 
                                     FROM [dbo].[Modules] m
                                     INNER JOIN [dbo].[Stu_To_Module] stm ON m.module_id = stm.module_id
                                     WHERE stm.user_id = @user_id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ModulesGridView.DataSource = dt;
                    ModulesGridView.DataBind();
                    conn.Close();
                }
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            int userId;
            if (int.TryParse(Session["user_id"]?.ToString(), out userId))
            {
                using (SqlConnection conn = new SqlConnection(getConnectionString()))
                {
                    string query = @"UPDATE [dbo].[Users]
                                     SET Name = @Name, Surname = @Surname, Email = @Email, Password = @Password
                                     WHERE user_id = @user_id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", NameTextBox.Text);
                    cmd.Parameters.AddWithValue("@Surname", SurnameTextBox.Text);
                    cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                    cmd.Parameters.AddWithValue("@Password", PasswordTextBox.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                // Reload the profile after saving
                LoadUserProfile();
                Response.Write("Profile updated successfully.");
            }
        }
    }
}

