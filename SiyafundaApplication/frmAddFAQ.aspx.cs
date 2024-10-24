using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.UI;

namespace SiyafundaApplication
{
    public partial class frmAddFAQ : System.Web.UI.Page
    {
        // Declare the connection variable
        private SqlConnection Con;

        // Page load event
        protected async void Page_Load(object sender, EventArgs e)
        {
            // Retrieve the connection string asynchronously from Azure Key Vault
            string connectionString = await SiyafundaFunctions.GetConnectionStringAsync();
            Con = new SqlConnection(connectionString);

            // Redirect if user is not logged in or does not have the Educator role (RoleID = 6)
            if (Session["UserID"] == null || Convert.ToInt32(Session["RoleID"]) != 6)
            {
                SiyafundaFunctions.SafeRedirect("frmEducator.aspx");
                return;
            }

            if (!IsPostBack)
            {
                await LoadModuleAsync(); // Load module asynchronously
            }

            lblErrors.Visible = false;
        }

        // Asynchronously load the module for the educator
        private async Task LoadModuleAsync()
        {
            try
            {
                string query = @"
                    SELECT title
                    FROM Modules
                    WHERE educator_id = @EducatorId";

                using (SqlCommand cmd = new SqlCommand(query, Con))
                {
                    cmd.Parameters.AddWithValue("@EducatorId", Convert.ToInt32(Session["UserID"]));
                    Con.Open();
                    var result = await cmd.ExecuteScalarAsync();

                    if (result != null)
                    {
                        lblModuleName.Text = result.ToString(); // Display the module title
                    }
                    else
                    {
                        lblErrors.Text = "No module found for this educator.";
                        lblErrors.Visible = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                lblErrors.Text = "An error occurred while loading the module: " + ex.Message;
                lblErrors.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        // Asynchronously retrieve the ModuleID based on the module title
        private async Task<int> GetModuleIdAsync(string moduleTitle)
        {
            int moduleId = 0;
            try
            {
                string query = @"
                    SELECT module_id
                    FROM Modules
                    WHERE title = @Title";

                using (SqlCommand cmd = new SqlCommand(query, Con))
                {
                    cmd.Parameters.AddWithValue("@Title", moduleTitle);
                    Con.Open();
                    var result = await cmd.ExecuteScalarAsync();

                    if (result != null)
                    {
                        moduleId = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException ex)
            {
                lblErrors.Text = "An error occurred while retrieving the module ID: " + ex.Message;
                lblErrors.Visible = true;
            }
            finally
            {
                Con.Close();
            }

            return moduleId;
        }

        // Handle Add FAQ button click
        protected async void btnAddFAQ_Click(object sender, EventArgs e)
        {
            // Check if question and answer fields are filled
            if (string.IsNullOrWhiteSpace(txtQuestion.Text) || string.IsNullOrWhiteSpace(txtAnswer.Text))
            {
                lblErrors.Text = "Please fill in both the question and the answer.";
                lblErrors.Visible = true;
                return;
            }

            int userID = Convert.ToInt32(Session["UserID"]);
            int module_id = await GetModuleIdAsync(lblModuleName.Text);
            string question = txtQuestion.Text.Trim();
            string answer = txtAnswer.Text.Trim();
            DateTime dateTime = DateTime.Now;

            try
            {
                string insertQuery = @"
                    INSERT INTO FAQs (question, answer, user_id, module_id, created_at)
                    VALUES (@Question, @Answer, @UserId, @ModuleId, @created_at)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, Con))
                {
                    cmd.Parameters.AddWithValue("@Question", question);
                    cmd.Parameters.AddWithValue("@Answer", answer);
                    cmd.Parameters.AddWithValue("@UserId", userID);
                    cmd.Parameters.AddWithValue("@ModuleId", module_id);
                    cmd.Parameters.AddWithValue("@created_at", dateTime);

                    Con.Open();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        lblErrors.Text = "FAQ added successfully.";
                        lblErrors.Visible = true;
                        // Optionally clear the fields after successful insert
                        txtQuestion.Text = string.Empty;
                        txtAnswer.Text = string.Empty;
                    }
                    else
                    {
                        lblErrors.Text = "Failed to add the FAQ.";
                        lblErrors.Visible = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                lblErrors.Text = "An error occurred while adding the FAQ: " + ex.Message;
                lblErrors.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        // Handle back button click
        protected void Back_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmEducator.aspx");
        }
    }
}