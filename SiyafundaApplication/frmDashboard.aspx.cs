using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.IO;

namespace SiyafundaApplication
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private SqlConnection Con;
        private int UserID = 0;
        private int RoleID = 0;

        // Method to retrieve the connection string from Azure Key Vault
        protected async Task<string> GetConnectionStringFromKeyVault()
        {
            return await SiyafundaFunctions.GetConnectionStringAsync();
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            btnEducators.Visible = false;
            btnModerators.Visible = false;
            btnSystemDevs.Visible = false;
            btnSystemAdmins.Visible = false;
            lblError.Visible = false;
            lblRole.Visible = false;
            lblWelcome.Visible = false;
            btnProfile.Visible = false;
            lblDate.Visible = true;
            lblTimeTableError.Visible = false;

            lblDate.Text = "Today is " + DateTime.Now.ToString("dddd, MMMM dd yyyy, hh:mm tt");

            if (Session["UserID"] != null)
            {
                UserID = Convert.ToInt32(Session["UserID"]);
                await LoadUserData(UserID);
            }
            else
            {
                lblError.Text = "Session has expired. Please log in again.";
                lblError.Visible = true;
                return;
            }

            // Determine visibility of buttons based on user role
            if (Session["RoleID"] != null)
            {
                RoleID = Convert.ToInt32(Session["RoleID"]);
                lblError.Visible = true;
                switch (RoleID)
                {
                    case 2: // System Admin
                        ShowAllButtons();
                        break;

                    case 3: // System Dev
                        ShowSystemDevButtons();
                        break;

                    case 4: // Moderator
                        ShowModeratorButtons();
                        break;

                    case 6: // Educator
                        btnEducators.Visible = true;
                        break;

                    case 7: // Student
                        // No additional buttons for Student
                        break;

                    default:
                        Response.Redirect("frmLandingPage.aspx");
                        return;
                }
            }

            await ShowAllResources();
            await ShowAnnouncements();
            await LoadTimeTable();
        }

        private async Task LoadUserData(int userId)
        {
            string connectionString = await GetConnectionStringFromKeyVault();
            using (Con = new SqlConnection(connectionString))
            {
                string query = "SELECT Name, Surname, r.role_name FROM Users u INNER JOIN Roles r ON u.Role_id = r.role_id WHERE u.user_id = @UserId";

                using (SqlCommand cmd = new SqlCommand(query, Con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    Con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string name = reader["Name"].ToString();
                        string surname = reader["Surname"].ToString();
                        string roleName = reader["role_name"].ToString();

                        lblWelcome.Text = $"Welcome, {name} {surname}";
                        lblRole.Text = $"Signed in as: {roleName}";
                        lblWelcome.Visible = true;
                        lblRole.Visible = true;
                        btnProfile.Visible = true;
                    }
                    else
                    {
                        lblError.Text = "User not found.";
                        lblError.Visible = true;
                    }
                    reader.Close();
                }
            }
        }

        private async Task ShowAllResources(string filter = null)
        {
            string connectionString = await GetConnectionStringFromKeyVault();
            string query = @"
                SELECT
                    r.resource_id AS ResourceId,
                    m.title AS ModuleName,
                    r.title AS ResourceTitle,
                    r.description AS ResourceDescription,
                    r.upload_date AS UploadDate,
                    f.file_size AS FileSize
                FROM
                    Resources r
                INNER JOIN
                    Modules m ON r.module_id = m.module_id
                INNER JOIN
                    Files f ON r.resource_id = f.resource_id";

            // If a filter is provided, add it to the query
            if (!string.IsNullOrEmpty(filter))
            {
                query += " WHERE r.title LIKE @Filter"; // Added WHERE clause for filtering
            }

            try
            {
                using (Con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        // Add the filter parameter if a filter is provided
                        if (!string.IsNullOrEmpty(filter))
                        {
                            cmd.Parameters.AddWithValue("@Filter", "%" + filter + "%");
                        }

                        Con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        if (dt.Rows.Count == 0)
                        {
                            lblError.Text = "No resources found.";
                            lblError.Visible = true;
                        }
                        else
                        {
                            lblError.Visible = false; // Hide error label if resources are found
                            dgvAvailableFiles.DataSource = dt;
                            dgvAvailableFiles.DataBind(); // Bind the data to the control
                        }

                        reader.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                lblError.Text = "An error occurred while retrieving resources: " + ex.Message;
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                lblError.Text = "An unexpected error occurred: " + ex.Message;
                lblError.Visible = true;
            }
        }

        private async Task ShowAnnouncements()
        {
            string connectionString = await GetConnectionStringFromKeyVault();
            string query = @"
                            SELECT
                                m.title AS ModuleName,
                                a.created_at AS AnnouncementDate,
                                a.title AS AnnouncementTitle,
                                a.content AS AnnouncementContent
                            FROM
                                Announcements a
                            INNER JOIN
                                Modules m ON a.module_id = m.module_id
                            INNER JOIN
                                Stu_To_Module stm ON m.module_id = stm.module_id
                            WHERE
                                stm.user_id = @UserId OR m.educator_id = @UserId
                            ORDER BY
                                a.created_at DESC";  // Order by the announcement creation date

            try
            {
                using (Con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserID);  // Using the current UserID

                        Con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        if (dt.Rows.Count == 0)
                        {
                            lblAnnouncementsError.Text = "No announcements found.";
                            lblAnnouncementsError.Visible = true;
                        }
                        else
                        {
                            lblAnnouncementsError.Visible = false;  // Hide error label if announcements are found
                            dgvAnnouncements.DataSource = dt;
                            dgvAnnouncements.DataBind();  // Bind the data to the control
                        }

                        reader.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                lblAnnouncementsError.Text = "An error occurred while retrieving announcements: " + ex.Message;
                lblAnnouncementsError.Visible = true;
            }
            catch (Exception ex)
            {
                lblAnnouncementsError.Text = "An unexpected error occurred: " + ex.Message;
                lblAnnouncementsError.Visible = true;
            }
        }

        private async Task LoadTimeTable()
        {
            string connectionString = await GetConnectionStringFromKeyVault();
            // Ensure a connection to the database is established
            using (Con = new SqlConnection(connectionString))
            {
                try
                {
                    Con.Open(); // Open the database connection

                    // SQL query to select the timetable for the current user
                    string query = @"SELECT TT.time_table_id, M.title AS module_title, D.day_name,
                            TT.class_start_time, TT.class_end_time
                     FROM TimeTable TT
                     JOIN Modules M ON TT.module_id = M.module_id
                     JOIN DaysOfTheWeek D ON TT.day_id = D.day_id
                     WHERE TT.user_id = @UserID";

                    // Create the SQL command and parameterize the query
                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        cmd.Parameters.AddWithValue("@UserID", UserID); // Pass the current user's ID

                        // Execute the query and load the result into a DataTable
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Bind the result to the GridView (dgvTimeTable)
                            dgvTimeTable.DataSource = dt;
                            dgvTimeTable.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (log the error, show message, etc.)
                    lblTimeTableError.Visible = true;
                    lblTimeTableError.Text = "Error loading timetable: " + ex.Message;
                }
                finally
                {
                    Con.Close(); // Ensure the connection is closed
                }
            }
        }

        private async Task<int> GetResourceId(string resourceTitle)
        {
            string connectionString = await GetConnectionStringFromKeyVault();
            using (Con = new SqlConnection(connectionString))
            {
                string query = "SELECT resource_id FROM Resources WHERE title = @ResourceTitle";

                using (SqlCommand cmd = new SqlCommand(query, Con))
                {
                    cmd.Parameters.AddWithValue("@ResourceTitle", resourceTitle);
                    Con.Open();
                    object result = await cmd.ExecuteScalarAsync(); // Execute asynchronously

                    return result != null ? Convert.ToInt32(result) : -1; // Return resource ID or -1 if not found
                }
            }
        }

        private async Task<string> GetFilePath(int resourceId)
        {
            string connectionString = await GetConnectionStringFromKeyVault();
            using (Con = new SqlConnection(connectionString))
            {
                string query = "SELECT file_path FROM Files WHERE resource_id = @ResourceId";

                using (SqlCommand cmd = new SqlCommand(query, Con))
                {
                    cmd.Parameters.AddWithValue("@ResourceId", resourceId);
                    Con.Open();
                    object result = await cmd.ExecuteScalarAsync(); // Execute asynchronously

                    return result != null ? result.ToString() : string.Empty; // Return file path or empty string if not found
                }
            }
        }

        private async Task<int> GetModuleId(string moduleTitle)
        {
            string connectionString = await GetConnectionStringFromKeyVault();
            using (Con = new SqlConnection(connectionString))
            {
                string query = "SELECT module_id FROM Modules WHERE title = @ModuleTitle";

                using (SqlCommand cmd = new SqlCommand(query, Con))
                {
                    cmd.Parameters.AddWithValue("@ModuleTitle", moduleTitle);
                    Con.Open();
                    object result = await cmd.ExecuteScalarAsync(); // Execute asynchronously

                    return result != null ? Convert.ToInt32(result) : -1; // Return module ID or -1 if not found
                }
            }
        }

        protected async void dgvAvailableFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int resourceId = Convert.ToInt32(e.CommandArgument);
                lblError.Text = $"Selected Resource ID: {resourceId}";
                lblError.Visible = true;
            }
            else if (e.CommandName == "Download")
            {
                int resourceId = Convert.ToInt32(e.CommandArgument);

                try
                {
                    // Get the file path from the database
                    string filePath = await GetFilePath(resourceId);

                    // Ensure the file path is not empty
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        string fullPath = Server.MapPath("~/" + filePath);

                        // Check if the file exists on the server
                        if (File.Exists(fullPath))
                        {
                            // Serve the file to the client for download
                            Response.ContentType = "application/octet-stream";
                            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fullPath));
                            Response.TransmitFile(fullPath);
                            Response.End(); // End the response to prevent further output
                        }
                        else
                        {
                            lblError.Text = "File not found.";
                            lblError.Visible = true;
                        }
                    }
                    else
                    {
                        lblError.Text = "Invalid file path.";
                        lblError.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "An error occurred while downloading the file: " + ex.Message;
                    lblError.Visible = true;
                }
            }
        }



        private void ShowAllButtons()
        {
            btnEducators.Visible = true;
            btnModerators.Visible = true;
            btnSystemDevs.Visible = true;
            btnSystemAdmins.Visible = true;
        }

        private void ShowSystemDevButtons()
        {
            btnSystemDevs.Visible = true;
        }

        private void ShowModeratorButtons()
        {
            btnEducators.Visible = true;
            btnModerators.Visible = true;
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            if (Session != null)
            {
                Session.Clear();
                Session.Abandon(); // Completely abandon the session to remove all session data.
            }
            Response.Redirect("frmLogin.aspx", false);
            Context.ApplicationInstance.CompleteRequest();

        }

        protected void btnEditTimeTable_Click(object sender, EventArgs e)
        {
            if (RoleID == 7)
            {
                // Students can edit time tables here
                Response.Redirect("frmTimeTableEdit.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnEducators_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmEducator.aspx");
        }

        protected void btnModerators_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmModerator.aspx");
        }

        protected void btnSystemDevs_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmSystemDev.aspx");
        }

        protected void btnSystemAdmins_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmSystemAdmin.aspx");
        }

        protected void btnProfile_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmProfile.aspx");
        }

    }
}