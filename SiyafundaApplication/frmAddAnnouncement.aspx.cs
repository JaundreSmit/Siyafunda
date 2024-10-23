using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.UI;

namespace SiyafundaApplication
{
    public partial class frmAddAnnouncement : System.Web.UI.Page
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
                        lblModuleName.Text = result.ToString(); // Assuming lblModuleName is the label to display the module title
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

        // Handle Add Announcement button click
        protected async void btnAddAnnouncement_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string content = txtContent.Text.Trim();
            DateTime created_at = DateTime.Now;
            int User_ID = Convert.ToInt32(Session["UserID"]);

            // Retrieve the ModuleID using the loaded module title
            int ModuleID = await GetModuleIdAsync(lblModuleName.Text);

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                lblErrors.Text = "Title and content cannot be empty.";
                lblErrors.Visible = true;
                return;
            }

            try
            {
                // Check if the title already exists for the module
                string checkQuery = @"
                    SELECT COUNT(*)
                    FROM Announcements
                    WHERE title = @Title AND module_id = @ModuleID";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                {
                    checkCmd.Parameters.AddWithValue("@Title", title);
                    checkCmd.Parameters.AddWithValue("@ModuleID", ModuleID);
                    Con.Open();
                    int count = (int)await checkCmd.ExecuteScalarAsync();

                    if (count > 0)
                    {
                        lblErrors.Text = "An announcement with this title already exists for the selected module.";
                        lblErrors.Visible = true;
                        return;
                    }
                }

                // Insert the new announcement into the database
                string insertQuery = @"
                    INSERT INTO Announcements (title, content, created_at, user_id, module_id)
                    VALUES (@Title, @Content, @CreatedAt, @UserId, @ModuleId)";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, Con))
                {
                    insertCmd.Parameters.AddWithValue("@Title", title);
                    insertCmd.Parameters.AddWithValue("@Content", content);
                    insertCmd.Parameters.AddWithValue("@CreatedAt", created_at);
                    insertCmd.Parameters.AddWithValue("@UserId", User_ID);
                    insertCmd.Parameters.AddWithValue("@ModuleId", ModuleID);

                    await insertCmd.ExecuteNonQueryAsync();
                    lblErrors.Text = "Announcement added successfully.";
                    lblErrors.Visible = true;
                }
            }
            catch (SqlException ex)
            {
                lblErrors.Text = "An error occurred while adding the announcement: " + ex.Message;
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

        // Handle back button click
        protected void Back_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmEducator.aspx");
        }
    }
}