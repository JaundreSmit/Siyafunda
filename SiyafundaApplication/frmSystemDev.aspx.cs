using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;

namespace SiyafundaApplication
{
    public partial class frmSystemDev : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            lblAddModErrors.Visible = false;
            lblEditModErrors.Visible = false;

            if (Session["UserID"] == null || Convert.ToInt32(Session["RoleID"]) > 3)
            {
                // Invalid permission level!
                SiyafundaFunctions.SafeRedirect("frmLandingPage.aspx");
            }

            if (!IsPostBack)
            {
                await LoadAddModulesAsync(); // Load educators into the dropdown on initial page load
                await LoadEditModulesAsync(); // Load current modules to edit
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmDashboard.aspx");
        }

        // Add Modules section:
        private async Task LoadAddModulesAsync()
        {
            try
            {
                string connectionString = await SiyafundaFunctions.GetConnectionStringAsync();
                string query = @"
                SELECT user_id, Name, Surname
                FROM Users
                WHERE Role_id = 6 AND user_id NOT IN (SELECT educator_id FROM Modules)";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dt.Columns.Add("FullName", typeof(string), "Name + ' ' + Surname"); // Create a new column for full name

                        ddlAddEducator.DataSource = dt;
                        ddlAddEducator.DataTextField = "FullName"; // Use the new FullName column
                        ddlAddEducator.DataValueField = "user_id";   // Store user_id as value
                        ddlAddEducator.DataBind();

                        // Add a default item
                        ddlAddEducator.Items.Insert(0, new ListItem("Select Educator", "0"));
                    }
                    else
                    {
                        lblAddModErrors.Text = "No educators available for selection.";
                        lblAddModErrors.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblAddModErrors.Text = "Error loading educators: " + ex.Message;
                lblAddModErrors.Visible = true;
            }
        }

        protected async void btnAddMod_Click(object sender, EventArgs e)
        {
            string title = txtAddModName.Text.Trim();
            string description = txtAddModDesc.Text.Trim();
            DateTime created = DateTime.Now.Date; // Only the date
            int educatorId = ddlAddEducator.SelectedIndex > 0 ? Convert.ToInt32(ddlAddEducator.SelectedValue) : 0;

            if (educatorId == 0)
            {
                lblAddModErrors.Text = "Please select an educator.";
                lblAddModErrors.Visible = true;
                return;
            }

            string connectionString = await SiyafundaFunctions.GetConnectionStringAsync();

            try
            {
                // Check if the title already exists
                string checkQuery = "SELECT COUNT(*) FROM Modules WHERE title = @Title";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@Title", title);
                        int count = (int)await checkCmd.ExecuteScalarAsync();
                        if (count > 0)
                        {
                            lblAddModErrors.Text = "A module with this title already exists.";
                            lblAddModErrors.Visible = true;
                            return;
                        }
                    }

                    // Insert the new module
                    string insertQuery = @"
                    INSERT INTO Modules (title, description, created_at, educator_id)
                    VALUES (@Title, @Description, @Created, @EducatorId)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                    {
                        insertCmd.Parameters.AddWithValue("@Title", title);
                        insertCmd.Parameters.AddWithValue("@Description", description);
                        insertCmd.Parameters.AddWithValue("@Created", created);
                        insertCmd.Parameters.AddWithValue("@EducatorId", educatorId);

                        await insertCmd.ExecuteNonQueryAsync();
                        lblAddModErrors.Text = "Module added successfully.";
                        lblAddModErrors.Visible = true; // Show success message
                        await LoadAddModulesAsync(); // Refresh the educator dropdown
                    }
                }
            }
            catch (Exception ex)
            {
                lblAddModErrors.Text = "Error adding module: " + ex.Message;
                lblAddModErrors.Visible = true;
            }
        }

        // Edit Modules section:
        private async Task LoadEditModulesAsync()
        {
            string connectionString = await SiyafundaFunctions.GetConnectionStringAsync();
            try
            {
                string query = @"
                SELECT module_id, title
                FROM Modules";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        ddlEditModSelect.DataSource = reader;
                        ddlEditModSelect.DataTextField = "title";   // Display module title
                        ddlEditModSelect.DataValueField = "module_id"; // Store module_id as value
                        ddlEditModSelect.DataBind();

                        // Add a default item
                        ddlEditModSelect.Items.Insert(0, new ListItem("Select Module", "0"));
                    }
                    else
                    {
                        lblEditModErrors.Text = "No modules available for editing.";
                        lblEditModErrors.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblEditModErrors.Text = "Error loading modules: " + ex.Message;
                lblEditModErrors.Visible = true;
            }
        }

        private async Task LoadModuleDetailsAsync(int moduleId)
        {
            string connectionString = await SiyafundaFunctions.GetConnectionStringAsync();
            try
            {
                // Query to get module details including current educator's name
                string moduleQuery = @"
                SELECT m.title, m.description, m.created_at, m.educator_id,
                       CONCAT(u.Name, ' ', u.Surname) AS EducatorName
                FROM Modules m
                JOIN Users u ON m.educator_id = u.user_id
                WHERE m.module_id = @ModuleId";

                // Query to get all unassigned educators (users with Role_id = 6)
                string educatorQuery = @"
                SELECT user_id, CONCAT(Name, ' ', Surname) AS EducatorName
                FROM Users
                WHERE Role_id = 6 AND user_id NOT IN (SELECT educator_id FROM Modules)";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();

                    // Execute the module details query
                    SqlCommand moduleCmd = new SqlCommand(moduleQuery, con);
                    moduleCmd.Parameters.AddWithValue("@ModuleId", moduleId);
                    SqlDataReader moduleReader = await moduleCmd.ExecuteReaderAsync();

                    if (moduleReader.Read())
                    {
                        txtEditModTitle.Text = moduleReader["title"].ToString();
                        txtEditModDesc.Text = moduleReader["description"].ToString();

                        // Get the educator ID and name
                        string educatorId = moduleReader["educator_id"].ToString();
                        string educatorName = moduleReader["EducatorName"].ToString();

                        // Close the moduleReader to allow for a new reader
                        moduleReader.Close();

                        // Now execute the educator query
                        SqlCommand educatorCmd = new SqlCommand(educatorQuery, con);
                        SqlDataReader educatorReader = await educatorCmd.ExecuteReaderAsync();

                        // Clear previous items before adding new ones
                        ddlEditEducator.Items.Clear();

                        // Add the current educator as the selected item
                        ddlEditEducator.Items.Add(new ListItem(educatorName, educatorId));

                        // Populate dropdown with other unassigned educators
                        if (educatorReader.HasRows)
                        {
                            while (educatorReader.Read())
                            {
                                // Add each unassigned educator to the dropdown
                                ddlEditEducator.Items.Add(new ListItem(educatorReader["EducatorName"].ToString(), educatorReader["user_id"].ToString()));
                            }
                        }
                        // Close the educator reader after reading
                        educatorReader.Close();

                        // Add a default option
                        ddlEditEducator.Items.Insert(0, new ListItem("Select Educator", "0"));
                    }
                    else
                    {
                        lblEditModErrors.Text = "Module details not found.";
                        lblEditModErrors.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblEditModErrors.Text = "Error loading module details: " + ex.Message;
                lblEditModErrors.Visible = true;
            }
        }

        protected async void btnEditMod_Click(object sender, EventArgs e)
        {
            int moduleId = Convert.ToInt32(ddlEditModSelect.SelectedValue);
            string title = txtEditModTitle.Text.Trim();
            string description = txtEditModDesc.Text.Trim();
            int educatorId = ddlEditEducator.SelectedIndex > 0 ? Convert.ToInt32(ddlEditEducator.SelectedValue) : 0;

            if (educatorId == 0)
            {
                lblEditModErrors.Text = "Please select an educator.";
                lblEditModErrors.Visible = true;
                return;
            }

            string connectionString = await SiyafundaFunctions.GetConnectionStringAsync();

            try
            {
                string updateQuery = @"
                UPDATE Modules
                SET title = @Title, description = @Description, educator_id = @EducatorId
                WHERE module_id = @ModuleId";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();

                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                    {
                        updateCmd.Parameters.AddWithValue("@Title", title);
                        updateCmd.Parameters.AddWithValue("@Description", description);
                        updateCmd.Parameters.AddWithValue("@EducatorId", educatorId);
                        updateCmd.Parameters.AddWithValue("@ModuleId", moduleId);

                        await updateCmd.ExecuteNonQueryAsync();
                        lblEditModErrors.Text = "Module updated successfully.";
                        lblEditModErrors.Visible = true; // Show success message
                        await LoadEditModulesAsync(); // Refresh the modules list
                    }
                }
            }
            catch (Exception ex)
            {
                lblEditModErrors.Text = "Error updating module: " + ex.Message;
                lblEditModErrors.Visible = true;
            }
        }

        protected async void ddlEditModSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEditModSelect.SelectedIndex > 0)
            {
                int moduleId = Convert.ToInt32(ddlEditModSelect.SelectedValue);
                await LoadModuleDetailsAsync(moduleId);
            }
        }

        protected async void btnDeleteModule_Click(object sender, EventArgs e)
        {
            int moduleId = 0;

            // Get the selected module ID from the dropdown
            if (ddlEditModSelect.SelectedIndex > 0)
            {
                moduleId = Convert.ToInt32(ddlEditModSelect.SelectedValue);
            }
            else
            {
                lblEditModErrors.Text = "Please select a module to delete.";
                lblEditModErrors.Visible = true;
                return;
            }

            string connectionString = await SiyafundaFunctions.GetConnectionStringAsync();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();

                    // Check if the module exists
                    string checkQuery = "SELECT COUNT(*) FROM Modules WHERE module_id = @ModuleId";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@ModuleId", moduleId);
                        int count = (int)await checkCmd.ExecuteScalarAsync();
                        if (count == 0)
                        {
                            lblEditModErrors.Text = "Module not found.";
                            lblEditModErrors.Visible = true;
                            return;
                        }
                    }

                    // Delete related records from all dependent tables
                    string deleteQuery = @"
                DELETE FROM QuizResponses WHERE question_id IN (SELECT question_id FROM QuizQuestions WHERE quiz_id IN (SELECT quiz_id FROM Quizzes WHERE module_id = @ModuleId));
                DELETE FROM QuizQuestions WHERE quiz_id IN (SELECT quiz_id FROM Quizzes WHERE module_id = @ModuleId);
                DELETE FROM Stu_To_Module WHERE module_id = @ModuleId;
                DELETE FROM TimeTable WHERE module_id = @ModuleId;
                DELETE FROM Resources WHERE module_id = @ModuleId;
                DELETE FROM Announcements WHERE module_id = @ModuleId;
                DELETE FROM FAQs WHERE module_id = @ModuleId;
                DELETE FROM Reviews WHERE resource_id IN (SELECT resource_id FROM Resources WHERE module_id = @ModuleId);
                DELETE FROM Gradebook WHERE module_id = @ModuleId;
                DELETE FROM Quizzes WHERE module_id = @ModuleId;
                DELETE FROM Modules WHERE module_id = @ModuleId;";

                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, con))
                    {
                        deleteCmd.Parameters.AddWithValue("@ModuleId", moduleId);
                        int rowsAffected = await deleteCmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            lblEditModErrors.Text = "Module and all related records deleted successfully.";
                            lblEditModErrors.Visible = true;
                        }
                        else
                        {
                            lblEditModErrors.Text = "Error deleting module or related records.";
                            lblEditModErrors.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblEditModErrors.Text = "Error deleting module: " + ex.Message;
                lblEditModErrors.Visible = true;
            }
            finally
            {
                ClearEditControls();
                await LoadEditModulesAsync(); // Reload modules asynchronously
            }
        }

        private void ClearEditControls()
        {
            txtEditModTitle.Text = string.Empty;
            txtEditModDesc.Text = string.Empty;
            ddlEditEducator.Items.Clear();
        }
    }
}