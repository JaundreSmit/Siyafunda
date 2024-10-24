﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmEducator : System.Web.UI.Page
    {
        // Declare connection variable
        private SqlConnection Con;

        private int UserID = 0;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                UserID = Convert.ToInt32(Session["UserID"]);
                if (Convert.ToInt32(Session["RoleID"]) == 7) // Not at least educator
                {
                    SiyafundaFunctions.SafeRedirect("frmDashboard.aspx");
                }
            }
            else
            {
                SiyafundaFunctions.SafeRedirect("frmLandingPage.aspx");
            }

            if (!IsPostBack)
            {
                // Populate the RadioButtonList asynchronously
                await PopulateStudentFilterAsync();
            }
        }

        private async Task<string> GetConnectionStringAsync()
        {
            return await SiyafundaFunctions.GetConnectionStringAsync();
        }

        private async Task PopulateStudentFilterAsync()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(await GetConnectionStringAsync()))
                {
                    await con.OpenAsync();
                    string moduleQuery;

                    // Query to get module titles, depending on the role
                    if (Convert.ToInt32(Session["RoleID"]) == 6)
                    {
                        // For educators, get their specific modules
                        moduleQuery = @"
                                SELECT m.title
                                FROM Modules m
                                WHERE m.educator_id = @EducatorId";
                    }
                    else // For higher-level roles (Admins, etc) to access all modules
                    {
                        moduleQuery = @"
                                SELECT m.title
                                FROM Modules m";
                    }

                    using (SqlCommand cmd = new SqlCommand(moduleQuery, con))
                    {
                        // Add the EducatorId parameter for educators
                        if (Convert.ToInt32(Session["RoleID"]) == 6)
                        {
                            cmd.Parameters.AddWithValue("@EducatorId", UserID);
                        }

                        // Execute the query and get the module title(s)
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            // Clear the RadioButtonList before populating it
                            rblStudentFilter.Items.Clear();

                            if (reader.HasRows)
                            {
                                // Add option for unassigned students
                                rblStudentFilter.Items.Add(new ListItem("Show unassigned students", "Unassigned"));

                                // Add options for each module this educator or admin has access to
                                while (await reader.ReadAsync())
                                {
                                    string moduleTitle = reader["title"].ToString();
                                    rblStudentFilter.Items.Add(new ListItem("Show students for " + moduleTitle, "Assigned"));
                                }

                                rblStudentFilter.SelectedIndex = -1;
                            }
                            else
                            {
                                // No module found
                                lblStudentErrors.Text = "No modules found.";
                                lblStudentErrors.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                lblStudentErrors.Text = "An error occurred while retrieving modules: " + ex.Message;
                lblStudentErrors.Visible = true;
            }
            catch (Exception ex)
            {
                lblStudentErrors.Text = "An unexpected error occurred: " + ex.Message;
                lblStudentErrors.Visible = true;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmDashboard.aspx");
        }

        protected void btnUploadResources_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmUploadFiles.aspx");
        }

        protected async void rblStudentFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check which option is selected and load students accordingly
            string selectedValue = rblStudentFilter.SelectedValue;

            if (selectedValue == "Unassigned")
            {
                await LoadUnassignedStudentsAsync();
                btnAssignStudent.Enabled = true;
                btnRemoveStudent.Enabled = false;
            }
            else if (selectedValue == "Assigned")
            {
                // Get the selected module title to filter students
                string selectedModuleTitle = rblStudentFilter.SelectedItem.Text.Replace("Show students for ", "");
                await LoadStudentsForModuleAsync(selectedModuleTitle);
                btnAssignStudent.Enabled = false;
                btnRemoveStudent.Enabled = true;
            }
        }

        private async Task LoadUnassignedStudentsAsync()
        {
            try
            {
                string query = @"
                        SELECT
                            (u.name + ' ' + u.surname) AS FullName,
                            u.username,
                            u.email
                        FROM
                            Users u
                        WHERE
                            u.user_id NOT IN (SELECT user_id FROM Stu_To_Module)
                            AND u.role_id = 7"; // Only show students

                using (SqlConnection con = new SqlConnection(await GetConnectionStringAsync()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        await con.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                DataTable dt = new DataTable();
                                dt.Load(reader);
                                dgvStudents.DataSource = dt;
                                dgvStudents.DataBind();
                            }
                            else
                            {
                                lblStudentErrors.Text = "No unassigned students found.";
                                lblStudentErrors.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                lblStudentErrors.Text = "An error occurred while retrieving unassigned students: " + ex.Message;
                lblStudentErrors.Visible = true;
            }
            catch (Exception ex)
            {
                lblStudentErrors.Text = "An unexpected error occurred: " + ex.Message;
                lblStudentErrors.Visible = true;
            }
        }

        private async Task LoadStudentsForModuleAsync(string moduleTitle, string filter = null)
        {
            // Clear the existing data
            dgvStudents.DataSource = null; // Clear existing data source
            dgvStudents.DataBind(); // Rebind to refresh the display
            try
            {
                // Include filter condition if provided
                string filterCondition = string.IsNullOrEmpty(filter) ? "" : " AND (u.name LIKE @Filter OR u.surname LIKE @Filter)";

                string query = $@"
                                SELECT
                                    (u.name + ' ' + u.surname) AS FullName,
                                    u.username,
                                    u.email,
                                    u.user_id,
                                    stm.module_id
                                FROM
                                    Stu_To_Module stm
                                INNER JOIN
                                    Users u ON stm.user_id = u.user_id
                                INNER JOIN
                                    Modules m ON stm.module_id = m.module_id
                                WHERE
                                    m.title = @ModuleTitle
                                    AND u.role_id = 7 {filterCondition}";

                using (SqlConnection con = new SqlConnection(await GetConnectionStringAsync()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ModuleTitle", moduleTitle);

                        // Add filter parameter if applicable
                        if (!string.IsNullOrEmpty(filter))
                        {
                            cmd.Parameters.AddWithValue("@Filter", "%" + filter + "%");
                        }

                        await con.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                DataTable dt = new DataTable();
                                dt.Load(reader);
                                dgvStudents.DataSource = dt;
                                dgvStudents.DataBind();
                            }
                            else
                            {
                                lblStudentErrors.Text = "No students found for the selected module.";
                                lblStudentErrors.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                lblStudentErrors.Text = "An error occurred while retrieving students for the module: " + ex.Message;
                lblStudentErrors.Visible = true;
            }
            catch (Exception ex)
            {
                lblStudentErrors.Text = "An unexpected error occurred: " + ex.Message;
                lblStudentErrors.Visible = true;
            }
        }

        protected async void txtSearchStudents_TextChanged(object sender, EventArgs e)
        {
            // Get the search term from the TextBox
            string searchTerm = txtSearchStudents.Text.Trim();
            if (searchTerm.Length > 0)
            {
                // Get the selected module title to filter students
                string selectedModuleTitle = rblStudentFilter.SelectedItem.Text.Replace("Show students for ", "");
                await LoadStudentsForModuleAsync(selectedModuleTitle, searchTerm);
            }
        }

        private int SelectedRow = -1;

        protected async void btnRemoveStudent_Click(object sender, EventArgs e)
        {
            // Ensure a student is selected in the GridView (dgvStudents)
            if (SelectedRow > -1)
            {
                // Get the student ID from the selected row (assuming it's in the fourth column, which you might need to adjust)
                int studentId = Convert.ToInt32(dgvStudents.SelectedRow.Cells[3].Text); // Adjust index if needed

                // Get the module ID from the selected row (assuming it's in the fifth column)
                int moduleId = Convert.ToInt32(dgvStudents.SelectedRow.Cells[4].Text); // Adjust index if needed

                // Call the method to remove the student
                await RemoveStudentFromModuleAsync(studentId, moduleId);
            }
            else
            {
                lblStudentErrors.Text = "Please select a student to remove.";
                lblStudentErrors.Visible = true;
            }
        }

        private async Task RemoveStudentFromModuleAsync(int studentId, int moduleId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(await GetConnectionStringAsync()))
                {
                    await con.OpenAsync();
                    // Define the query to remove the student from the specific module
                    string query = @"
                                    DELETE FROM Stu_To_Module
                                    WHERE user_id = @StudentId AND module_id = @ModuleId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Set parameters for the SQL command
                        cmd.Parameters.AddWithValue("@StudentId", studentId);
                        cmd.Parameters.AddWithValue("@ModuleId", moduleId);

                        // Execute the deletion
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            lblStudentErrors.Text = "Student removed successfully.";
                            lblStudentErrors.Visible = true;

                            // Optionally reload the student list after removal
                            await LoadStudentsForModuleAsync(rblStudentFilter.SelectedItem.Text.Replace("Show students for ", ""));
                        }
                        else
                        {
                            lblStudentErrors.Text = "No student was found to remove.";
                            lblStudentErrors.Visible = true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                lblStudentErrors.Text = "An error occurred while removing the student: " + ex.Message;
                lblStudentErrors.Visible = true;
            }
            catch (Exception ex)
            {
                lblStudentErrors.Text = "An unexpected error occurred: " + ex.Message;
                lblStudentErrors.Visible = true;
            }
        }

        protected void btnAddAnnouncement_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmAddAnnouncement.aspx");
        }

        protected void dgvStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedRow = dgvStudents.SelectedRow.RowIndex;
        }

        protected void btnAddFaq_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmAddFAQ.aspx");
        }

        protected void btnAssignStudent_Click(object sender, EventArgs e)
        {
            //TODO
        }

        protected void btnAddStudent_Click(object sender, EventArgs e)
        {
            //TODO
        }
    }
}