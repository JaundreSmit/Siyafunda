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
        }

        // Handle sign up button click event
        protected void SignUpButton_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;

            // Basic validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "All fields are required.";
                lblError.Visible = true;
                return;
            }

            // Query to insert a new user into the database
            string query = "INSERT INTO [dbo].[Users] (Username, Email, Password, Role_id) VALUES (@Username, @Email, @Password, 2)";

            try
            {
                using (Con)
                {
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    Con.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        // Redirect to login page or another page upon successful sign-up
                        Response.Redirect("frmLogin.aspx");
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
            finally
            {
                Con.Close();
            }
        }
    }
}
