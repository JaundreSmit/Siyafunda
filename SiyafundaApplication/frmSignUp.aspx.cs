using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SiyafundaApplication
{
    public partial class frmSignUp : System.Web.UI.Page
    {
        // Declare the connection variable
        private SqlConnection Con;

        // Page load event
        protected async void Page_Load(object sender, EventArgs e)
        {
            // Retrieve the connection string asynchronously from Azure Key Vault
            string connectionString = await SiyafundaFunctions.GetConnectionStringAsync();
            Con = new SqlConnection(connectionString);

            lblError.Visible = false;
            Master.FindControl("footerControl").Visible = false;
        }

        // Handle sign-up button click event
        protected async void SignUpButton_Click(object sender, EventArgs e)
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
            int roleId = await GetRoleIdAsync("Student");

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
                using (SqlConnection Con = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync())) // Create a new connection instance
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
                        SiyafundaFunctions.SafeRedirect("frmDashboard.aspx");
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
        private async Task<int> GetRoleIdAsync(string roleName)
        {
            int roleId = 0;
            string roleQuery = "SELECT Role_id FROM [dbo].[Roles] WHERE Role_name = @RoleName";

            try
            {
                // Open a new connection in the GetRoleIdAsync method
                using (SqlConnection roleCon = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
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

        // Handle back button click event
        protected void btnBack_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmLandingPage.aspx");
        }
    }
}