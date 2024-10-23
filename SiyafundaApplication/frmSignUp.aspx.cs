using System;
using System.Data.SqlClient;

namespace SiyafundaApplication
{
    public partial class frmSignUp : System.Web.UI.Page
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
            Master.FindControl("footerControl").Visible = false;
        }

        // Handle sign-up button click event
        protected void SignUpButton_Click(object sender, EventArgs e)
        {
            string Name = txtName.Text;
            string Surname = txtSurname.Text;
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;

            // Basic validation
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "All fields are required.";
                lblError.Visible = true;
                return;
            }

            // Retrieve the Role_id for the role "Student"
            int roleId = GetRoleId("Student");

            if (roleId == 0)
            {
                lblError.Text = "Failed to retrieve role information.";
                lblError.Visible = true;
                return;
            }

            // Query to insert a new user into the database
            string query = @"INSERT INTO [dbo].[Users] (Name, Surname, Username, Email, Password, Role_id) 
                             VALUES (@Name, @Surname, @Username, @Email, @Password, @RoleID);
                             SELECT SCOPE_IDENTITY();"; // Added to get the newly inserted user's ID

            try
            {
                using (SqlConnection Con = new SqlConnection(getConnectionString())) // Create a new connection instance
                {
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Surname", Surname);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@RoleID", roleId); // Use retrieved Role_id

                    Con.Open();
                    object result = cmd.ExecuteScalar(); // Get the newly inserted user's ID

                    if (result != null)
                    {
                        int userId = Convert.ToInt32(result); // Get the user ID

                        // Store UserID and RoleID in session
                        Session["UserID"] = userId;
                        Session["RoleID"] = roleId;

                        // Redirect to login page or another page upon successful sign-up
                        Response.Redirect("frmDashboard.aspx");
                    }
                    else
                    {
                        lblError.Text = "Sign up failed. Please try again.";
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
        }

        // Helper method to get Role_id based on the role name
        private int GetRoleId(string roleName)
        {
            int roleId = 0;
            string roleQuery = "SELECT Role_id FROM [dbo].[Roles] WHERE Role_name = @RoleName";

            try
            {
                // Open a new connection in the GetRoleId method
                using (SqlConnection roleCon = new SqlConnection(getConnectionString()))
                {
                    SqlCommand roleCmd = new SqlCommand(roleQuery, roleCon);
                    roleCmd.Parameters.AddWithValue("@RoleName", roleName);

                    roleCon.Open();
                    object result = roleCmd.ExecuteScalar();

                    if (result != null)
                    {
                        roleId = Convert.ToInt32(result);
                    }
                    roleCon.Close();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error appropriately
                lblError.Text = "Error retrieving role ID: " + ex.Message;
                lblError.Visible = true;
            }

            return roleId;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmLandingPage.aspx");
        }
    }
}
