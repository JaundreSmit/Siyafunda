using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Web.UI;

namespace SiyafundaApplication
{
    public partial class frmLogin : System.Web.UI.Page
    {
        // Declare connection variable
        private SqlConnection Con;

        // Method to retrieve the connection string from Azure Key Vault
        protected async Task<string> GetConnectionStringFromKeyVault()
        {
            string keyVaultName = System.Configuration.ConfigurationManager.AppSettings["KeyVaultName"];
            string kvUri = "https://SiyafundVault.vault.azure.net/";

            // Create a secret client to connect to Azure Key Vault
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            // Retrieve the connection string secret from Key Vault
            KeyVaultSecret secret = await client.GetSecretAsync("ConnectionStrings--SiyafundaDB");
            return secret.Value;
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;

            // Get the connection string from Azure Key Vault
            string connectionString = await GetConnectionStringFromKeyVault();
            Con = new SqlConnection(connectionString);
        }

        // Handle login button click event
        protected async void LoginButton_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Please enter both username/email and password.";
                lblError.Visible = true;
                return;
            }

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

                        Session["UserID"] = userId;
                        Session["RoleID"] = roleId;

                        // Redirect to the dashboard without ending the response
                        Response.Redirect("frmDashboard.aspx", false);
                        Context.ApplicationInstance.CompleteRequest(); // Avoid ThreadAbortException
                    }
                    else
                    {
                        lblError.Text = "Invalid username or password.";
                        lblError.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
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
            // Redirect to the landing page without ending the response
            Response.Redirect("frmLandingPage.aspx", false);
            Context.ApplicationInstance.CompleteRequest(); // Avoid ThreadAbortException
        }
    }
}