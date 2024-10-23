using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        private SqlConnection Con;
        private int UserID = 0;
        private int RoleID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new SqlConnection(getConnectionString());
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
                LoadUserData(UserID);
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

            ShowAllResources();
            ShowAnnouncements();
            loadTimeTable();
        }

        private void LoadUserData(int userId)
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

            Con.Close();
        }

        private void ShowAllResources(string filter = null)
        {
            string query = @"
                SELECT
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
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add the filter parameter if a filter is provided
                        if (!string.IsNullOrEmpty(filter))
                        {
                            cmd.Parameters.AddWithValue("@Filter", "%" + filter + "%");
                        }

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        // Assuming dgvAvailableFiles is a DataGridView or similar control
                        dgvAvailableFiles.DataSource = null; // Clear existing data
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

        private void ShowAnnouncements()
        {
            // Query to retrieve announcements based on user ID and module enrollment
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
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserID);  // Using the current UserID

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        if (dt.Rows.Count == 0)
                        {
                            lblAnnoucementsError.Text = "No announcements found.";
                            lblAnnoucementsError.Visible = true;
                        }
                        else
                        {
                            lblAnnoucementsError.Visible = false;  // Hide error label if announcements are found

                            // Assuming dgvAnnouncements is a GridView, DataGridView, or similar control
                            dgvAnnouncements.DataSource = dt;
                            dgvAnnouncements.DataBind();  // Bind the data to the control
                        }

                        reader.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                lblAnnoucementsError.Text = "An error occurred while retrieving announcements: " + ex.Message;
                lblAnnoucementsError.Visible = true;
            }
            catch (Exception ex)
            {
                lblAnnoucementsError.Text = "An unexpected error occurred: " + ex.Message;
                lblAnnoucementsError.Visible = true;
            }
        }

        private void loadTimeTable()
        {
            // Ensure a connection to the database is established
            using (Con = new SqlConnection(getConnectionString()))
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
                            System.Data.DataTable dt = new System.Data.DataTable();
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

        private void ShowAllButtons()
        {
            btnEducators.Visible = true;
            btnModerators.Visible = true;
            btnSystemDevs.Visible = true;
            btnSystemAdmins.Visible = true;
        }

        private void ShowSystemDevButtons()
        {
            btnEducators.Visible = true;
            btnModerators.Visible = true;
            btnSystemDevs.Visible = true;
        }

        private void ShowModeratorButtons()
        {
            btnEducators.Visible = true;
            btnModerators.Visible = true;
        }

        protected void btnSystemAdmins_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmSystemAdmin.aspx");
        }

        protected void btnSystemDevs_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmSystemDev.aspx");
            lblDate.Text = Convert.ToInt32(Session["UserID"]).ToString();
        }

        protected void btnModerators_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmModerator.aspx");
        }

        protected void btnEducators_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmEducator.aspx");
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmLandingPage.aspx");
        }

        protected void btnProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmProfile.aspx");
        }

        protected void btnEditTimeTable_Click(object sender, EventArgs e)
        {
            if (RoleID == 7)
            {
                // Students can edit time tables here
                Response.Redirect("frmTimeTableEdit.aspx");
            }
        }

        protected void dgvAvailableFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Get the row index of the clicked item
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                // Get the selected row
                GridViewRow selectedRow = dgvAvailableFiles.Rows[rowIndex];

                // Retrieve the module name and resource title from the selected row
                string moduleName = selectedRow.Cells[0].Text; // Assuming the Module Name is in the first column
                string resourceTitle = ((LinkButton)selectedRow.FindControl("lnkResourceTitle")).Text; // Get the text of the LinkButton

                int moduleId = 0;
                int resourceId = 0;
                string filePath = string.Empty;

                // Get the module_id from the Modules table
                string moduleQuery = "SELECT module_id FROM Modules WHERE title = @ModuleName";
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(moduleQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@ModuleName", moduleName);
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            moduleId = Convert.ToInt32(result);
                        }
                    }
                }

                // Get the resource_id and file path from Resources and Files tables
                string resourceQuery = "SELECT r.resource_id, f.file_path FROM Resources r " +
                                       "INNER JOIN Files f ON r.resource_id = f.resource_id " +
                                       "WHERE r.title = @ResourceTitle AND r.module_id = @ModuleId";
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(resourceQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@ResourceTitle", resourceTitle);
                        cmd.Parameters.AddWithValue("@ModuleId", moduleId);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                resourceId = reader.GetInt32(0); // resource_id
                                filePath = reader.GetString(1); // file_path
                            }
                        }
                    }
                }

                // Check if filePath is not empty and trigger download
                if (!string.IsNullOrEmpty(filePath))
                {
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", $"attachment; filename={System.IO.Path.GetFileName(filePath)}");
                    Response.TransmitFile(Server.MapPath(filePath));
                    Response.End();
                }
                else
                {
                    lblError.Text = "File not found.";
                    lblError.Visible = true;
                }
            }
        }

        private int GetModuleId(string moduleName)
        {
            int moduleId = 0;
            string query = "SELECT module_id FROM Modules WHERE title = @ModuleName";

            using (SqlConnection con = new SqlConnection(getConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ModuleName", moduleName);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        moduleId = Convert.ToInt32(result);
                    }
                }
            }

            return moduleId;
        }

        private int GetResourceId(string resourceTitle, int moduleId)
        {
            int resourceId = 0;
            string query = "SELECT resource_id FROM Resources WHERE title = @ResourceTitle AND module_id = @ModuleId";

            using (SqlConnection con = new SqlConnection(getConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ResourceTitle", resourceTitle);
                    cmd.Parameters.AddWithValue("@ModuleId", moduleId);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        resourceId = Convert.ToInt32(result);
                    }
                }
            }

            return resourceId;
        }

        private string GetFilePath(int resourceId)
        {
            string filePath = string.Empty;
            string query = "SELECT file_path FROM Files WHERE resource_id = @ResourceId";

            using (SqlConnection con = new SqlConnection(getConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ResourceId", resourceId);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        filePath = result.ToString();
                    }
                }
            }

            return filePath;
        }
    }
}