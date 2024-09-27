using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new SqlConnection(getConnectionString());
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

            lblError.Visible = false;
            btnEducators.Visible = false;
            btnModerators.Visible = false;
            btnSystemDevs.Visible = false;
            btnSystemAdmins.Visible = false;
            lblError.Visible = true;
            // Determine visibility of buttons based on user role
            if (Session["RoleID"] != null)
            {
                lblError.Visible = true;
                switch (Convert.ToInt32(Session["RoleID"]))
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
                        lblError.Text = "No user role found";
                        lblError.Visible = true;
                        return;
                }
            }

            ShowAllResources();
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
            string query;

            // Determine the user's role and construct the appropriate SQL query
            if (Convert.ToInt32(Session["RoleID"]) < 6)
            {
                // For roles below 6, show all approved resources
                query = @"
            SELECT
                m.title AS ModuleName,
                r.title AS ResourceTitle,
                r.description AS ResourceDescription,
                r.upload_date AS UploadDate,
                f.file_size AS FileSize,
                ds.status_name AS StatusName,
                AVG(rv.rating) AS AvgRating
            FROM
                Resources r
            INNER JOIN
                Modules m ON r.module_id = m.module_id
            INNER JOIN
                Files f ON r.resource_id = f.resource_id
            INNER JOIN
                Res_to_status rst ON r.resource_id = rst.resource_id
            INNER JOIN
                DocStatuses ds ON rst.status_id = ds.status_id
            LEFT JOIN
                Reviews rv ON r.resource_id = rv.resource_id
            WHERE
                ds.status_name = 'Approved'";

                // Add GROUP BY for all non-aggregated columns
                query += @"
            GROUP BY
                m.title, r.title, r.description, r.upload_date, f.file_size, ds.status_name";
            }
            else
            {
                // For Educators and Students: Only show resources for modules they are part of
                query = @"
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
                Files f ON r.resource_id = f.resource_id
            INNER JOIN
                Res_to_status rst ON r.resource_id = rst.resource_id
            INNER JOIN
                DocStatuses ds ON rst.status_id = ds.status_id
            INNER JOIN
                Stu_To_Module stm ON m.module_id = stm.module_id
            WHERE
                ds.status_name = 'Approved' AND (stm.user_id = @UserId OR m.educator_id = @UserId)";
            }

            // If a filter is provided, add it to the query
            if (!string.IsNullOrEmpty(filter))
            {
                query += " AND r.title LIKE @Filter";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add parameters for user filtering, if applicable
                        if (Convert.ToInt32(Session["RoleID"]) >= 6)
                        {
                            cmd.Parameters.AddWithValue("@UserId", UserID);
                        }

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
        }

        protected void btnModerators_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmModerator.aspx");
        }

        protected void btnEducators_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmEducator.aspx");
        }
    }
}