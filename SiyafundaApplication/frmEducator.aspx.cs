using System;
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
                    string moduleQuery = Convert.ToInt32(Session["RoleID"]) == 6
                        ? @"SELECT m.module_id, m.title FROM Modules m WHERE m.educator_id = @EducatorId"
                        : "SELECT m.module_id, m.title FROM Modules m";

                    using (SqlCommand cmd = new SqlCommand(moduleQuery, con))
                    {
                        if (Convert.ToInt32(Session["RoleID"]) == 6)
                        {
                            cmd.Parameters.AddWithValue("@EducatorId", UserID);
                        }

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            rblStudentFilter.Items.Clear();

                            if (reader.HasRows)
                            {
                                rblStudentFilter.Items.Add(new ListItem("Show unassigned students", "Unassigned"));

                                while (await reader.ReadAsync())
                                {
                                    string moduleTitle = reader["title"].ToString();
                                    string moduleId = reader["module_id"].ToString();
                                    rblStudentFilter.Items.Add(new ListItem($"Show students for {moduleTitle}", moduleId));
                                }

                                rblStudentFilter.SelectedIndex = -1;
                            }
                            else
                            {
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
            Response.Redirect("frmDashboard.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnUploadResources_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmUploadFiles.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected async void rblStudentFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = rblStudentFilter.SelectedValue;

            if (selectedValue == "Unassigned")
            {
                await LoadUnassignedStudentsAsync();
                btnAssignStudent.Enabled = true;
                btnRemoveStudent.Enabled = false;
            }
            else if (int.TryParse(selectedValue, out int selectedModuleId))
            {
                await LoadStudentsForModuleAsync(selectedModuleId.ToString());
                btnAssignStudent.Enabled = false;
                btnRemoveStudent.Enabled = true;
            }
            else
            {
                lblStudentErrors.Text = "Invalid module selection. Please select a valid module.";
                lblStudentErrors.Visible = true;
            }
        }

        private async Task LoadUnassignedStudentsAsync()
        {
            try
            {
                string query = @"
                    SELECT (u.name + ' ' + u.surname) AS FullName, u.username, u.email
                    FROM Users u
                    WHERE u.user_id NOT IN (SELECT user_id FROM Stu_To_Module) AND u.role_id = 7";

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

        private async Task LoadStudentsForModuleAsync(string moduleId, string filter = null)
        {
            dgvStudents.DataSource = null;
            dgvStudents.DataBind();

            try
            {
                string filterCondition = string.IsNullOrEmpty(filter) ? "" : " AND (u.name LIKE @Filter OR u.surname LIKE @Filter)";
                string query = $@"
                    SELECT (u.name + ' ' + u.surname) AS FullName, u.username, u.email, u.user_id, stm.module_id
                    FROM Stu_To_Module stm
                    INNER JOIN Users u ON stm.user_id = u.user_id
                    WHERE stm.module_id = @ModuleId AND u.role_id = 7 {filterCondition}";

                using (SqlConnection con = new SqlConnection(await GetConnectionStringAsync()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ModuleId", moduleId);

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
            string searchTerm = txtSearchStudents.Text.Trim();
            if (searchTerm.Length > 0)
            {
                string selectedModuleTitle = rblStudentFilter.SelectedItem.Text.Replace("Show students for ", "");
                await LoadStudentsForModuleAsync(selectedModuleTitle, searchTerm);
            }
        }

        private int SelectedRow = -1;

        protected async void btnRemoveStudent_Click(object sender, EventArgs e)
        {
            if (SelectedRow > -1)
            {
                int studentId = Convert.ToInt32(dgvStudents.SelectedRow.Cells[3].Text);
                int moduleId = Convert.ToInt32(dgvStudents.SelectedRow.Cells[4].Text);

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
                    string query = "DELETE FROM Stu_To_Module WHERE user_id = @StudentId AND module_id = @ModuleId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", studentId);
                        cmd.Parameters.AddWithValue("@ModuleId", moduleId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            lblStudentErrors.Text = "Student removed successfully.";
                            lblStudentErrors.Visible = true;
                            await LoadStudentsForModuleAsync(rblStudentFilter.SelectedItem.Value);
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
            Response.Redirect("frmAddAnnouncement.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void dgvStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedRow = dgvStudents.SelectedRow.RowIndex;
        }

        protected void btnAddFaq_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAddFAQ.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
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
