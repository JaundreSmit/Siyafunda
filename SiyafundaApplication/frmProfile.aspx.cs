using System;
using System.Data.SqlClient;
using System.Data;
using System.Xml;

namespace SiyafundaApplication  //TODO User_id (ex. row 27) needs to contain logged-in user's ID
{
    public partial class Profile : System.Web.UI.Page
    {
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        private int userId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrors.Visible = false;
            // Check if the session contains the user ID
            if (Session["UserID"] != null && int.TryParse(Session["UserID"].ToString(), out userId))
            {
                // Check if the user ID is within a valid range
                if (userId < 2 || userId > 7)
                {
                    Response.Redirect("frmLandingPage.aspx");
                    return;
                }
            }
            else
            {
                // Redirect if no valid session exists
                Response.Redirect("frmLandingPage.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadUserProfile();
                LoadUserModules();
            }
        }

        private void LoadUserProfile()
        {
            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                string query = @"SELECT u.Name, u.Surname, u.Email, u.Password, r.role_name
                                 FROM [dbo].[Users] u
                                 INNER JOIN [dbo].[Roles] r ON u.Role_id = r.role_id
                                 WHERE u.user_id = @user_id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    RoleLabel.Text = reader["role_name"].ToString();
                    // Pre-fill textboxes for editing
                    NameTextBox.Text = reader["Name"].ToString();
                    SurnameTextBox.Text = reader["Surname"].ToString();
                    EmailTextBox.Text = reader["Email"].ToString();
                    PasswordTextBox.Text = reader["Password"].ToString();
                }
                reader.Close();
            }
        }

        private void LoadUserModules()
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
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
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
                cmd.Parameters.AddWithValue("@user_id", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Reload the profile after saving
            LoadUserProfile();
            lblErrors.Text = "Profile updated successfully.";
            lblErrors.Visible = true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmDashboard.aspx");
        }
    }
}