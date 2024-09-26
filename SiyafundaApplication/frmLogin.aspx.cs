using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace SiyafundaApplication
{
    public partial class frmLogin : System.Web.UI.Page
    {
        // Define connection string method
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        // Declare connection variable
        private SqlConnection Con;

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new SqlConnection(getConnectionString());
            lblError.Visible = false;
        }

        // Handle login button click event
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Text;

            // Basic validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Please enter both username/email and password.";
                lblError.Visible = true;
                return;
            }

            // Query to validate user credentials
            string query = "SELECT user_id, Role_id FROM [dbo].[Users] WHERE (Username = @Username OR Email = @Username) AND Password = @Password";

            try
            {
                using (Con)
                {
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    Con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        int userId = Convert.ToInt32(reader["user_id"]);
                        int roleId = Convert.ToInt32(reader["Role_id"]);

                        // Store UserID and RoleID in session
                        Session["UserID"] = userId;
                        Session["RoleID"] = roleId;

                        // Redirect to another page upon successful login (e.g., dashboard)
                        Response.Redirect("frmDashboard.aspx");
                    }
                    else
                    {
                        // Invalid login attempt
                        lblError.Text = "Invalid username or password.";
                        lblError.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Error handling
                lblError.Text = "Error: " + ex.Message;
                lblError.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmLandingPage.aspx");
        }
    }
}
