using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class Admin : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            // Check if user is an admin
            if (Convert.ToInt32(Session["RoleID"]) != 2) // 2 = Admin role id
            {
                Response.Redirect("frmLandingPage.aspx"); // Redirect if not admin
            }

            if (!IsPostBack)
            {
                await LoadUsersAsync();
            }
        }

        // Asynchronously load users into GridView
        private async Task LoadUsersAsync()
        {
            using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
            {
                string query = @"SELECT u.user_id, u.Name, u.Surname, u.Email, r.role_name
                                 FROM [dbo].[Users] u
                                 INNER JOIN [dbo].[Roles] r ON u.Role_id = r.role_id";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                await Task.Run(() => da.Fill(dt)); // Fill the DataTable asynchronously

                UsersGridView.DataSource = dt;
                UsersGridView.DataBind();
            }
        }

        // Handle row command (delete user)
        protected async void UsersGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteUser")
            {
                if (int.TryParse(e.CommandArgument.ToString(), out int userId))
                {
                    await DeleteUserAsync(userId);
                }
            }
        }

        // Asynchronously delete user from the database
        private async Task DeleteUserAsync(int userId)
        {
            using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
            {
                string query = @"DELETE FROM [dbo].[Users] WHERE user_id = @user_id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await LoadUsersAsync(); // Reload users after deletion
            }
        }

        // Handle role change
        protected async void RoleDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;

            int userId = Convert.ToInt32(row.Cells[0].Text);
            int roleId = Convert.ToInt32(ddl.SelectedValue);

            await UpdateUserRoleAsync(userId, roleId);
        }

        // Asynchronously update user role in the database
        private async Task UpdateUserRoleAsync(int userId, int roleId)
        {
            using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
            {
                string query = @"UPDATE [dbo].[Users] SET Role_id = @role_id WHERE user_id = @user_id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@role_id", roleId);
                cmd.Parameters.AddWithValue("@user_id", userId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await LoadUsersAsync(); // Reload users after updating role
            }
        }

        protected async void btnPurge_Click(object sender, EventArgs e)
        {
            string uploadPath = Server.MapPath("~/UploadedFiles");
            lblResults.Visible = false; // Hide the label initially
            lblResults.Text = string.Empty; // Clear any previous messages

            try
            {
                // Step 1: Get all existing module_ids from the database
                var existingModuleIds = new HashSet<int>();
                using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
                {
                    string query = @"SELECT module_id FROM [dbo].[Modules]";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    await conn.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        existingModuleIds.Add(reader.GetInt32(0));
                    }
                    conn.Close();
                }

                // Step 2: Check folders in the upload directory
                var directories = System.IO.Directory.GetDirectories(uploadPath);
                foreach (var dir in directories)
                {
                    int moduleId = int.Parse(System.IO.Path.GetFileName(dir));

                    // Step 3: If the module_id does not exist, delete the folder and its contents
                    if (!existingModuleIds.Contains(moduleId))
                    {
                        System.IO.Directory.Delete(dir, true); // Delete the folder and all its contents
                    }
                }

                // Step 4: Check remaining files in the Files table
                using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
                {
                    string query = @"SELECT file_id, file_path FROM [dbo].[Files]";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    await conn.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<int> filesToDelete = new List<int>();

                    while (await reader.ReadAsync())
                    {
                        int fileId = reader.GetInt32(0);
                        string filePath = reader.GetString(1);
                        string fullPath = Server.MapPath("~/" + filePath);

                        // Step 5: Check if the file exists
                        if (!System.IO.File.Exists(fullPath))
                        {
                            filesToDelete.Add(fileId); // Mark for deletion
                        }
                    }
                    conn.Close();

                    // Step 6: Delete marked files from the database and cascade deletions
                    foreach (var fileId in filesToDelete)
                    {
                        await DeleteFileAndReferencesAsync(fileId);
                    }
                }

                // Step 7: Delete Resources that do not have corresponding files
                using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
                {
                    string deleteOrphanedResourcesQuery = @"
                DELETE FROM [dbo].[Resources]
                WHERE resource_id NOT IN (SELECT DISTINCT resource_id FROM [dbo].[Files])";

                    SqlCommand cmd = new SqlCommand(deleteOrphanedResourcesQuery, conn);
                    await conn.OpenAsync();
                    int orphanedResourcesCount = await cmd.ExecuteNonQueryAsync();
                    conn.Close();

                    // Inform about deleted orphaned resources
                    if (orphanedResourcesCount > 0)
                    {
                        lblResults.Text += $"{orphanedResourcesCount} orphaned resources deleted from the Resources table.<br/>";
                    }
                }

                // Step 8: Ensure all resource_ids in Resources exist in Res_to_status
                using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
                {
                    string query = @"
        SELECT r.resource_id, r.user_id
        FROM [dbo].[Resources] r
        WHERE r.resource_id NOT IN (SELECT resource_id FROM [dbo].[Res_to_status])";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    await conn.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<(int resourceId, int userId)> missingResources = new List<(int, int)>();

                    // Collect resource_ids and corresponding user_ids that need to be inserted
                    while (await reader.ReadAsync())
                    {
                        missingResources.Add((reader.GetInt32(0), reader.GetInt32(1))); // (resource_id, user_id)
                    }
                    conn.Close();

                    // Insert missing resource_ids into Res_to_status with the correct user_id
                    foreach (var (resourceId, userId) in missingResources)
                    {
                        using (SqlConnection insertConn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
                        {
                            string insertQuery = @"
                INSERT INTO [dbo].[Res_to_status] (resource_id, user_id, status_id, feedback)
                VALUES (@resource_id, @user_id, 3, 'Waiting for moderator approval')";

                            SqlCommand insertCmd = new SqlCommand(insertQuery, insertConn);
                            insertCmd.Parameters.AddWithValue("@resource_id", resourceId);
                            insertCmd.Parameters.AddWithValue("@user_id", userId);
                            await insertConn.OpenAsync();
                            await insertCmd.ExecuteNonQueryAsync();
                            insertConn.Close();
                        }
                    }

                    // Inform about added resources
                    if (missingResources.Count > 0)
                    {
                        lblResults.Text += $"{missingResources.Count} new entries added to Res_to_status.<br/>";
                    }
                }

                // Display success message
                lblResults.Text += "Purge operation completed successfully.";
            }
            catch (Exception ex)
            {
                // Display error message
                lblResults.Text = "An error occurred: " + ex.Message;
            }
            finally
            {
                lblResults.Visible = true; // Make the label visible
            }
        }

        // Asynchronously delete a file and its references
        private async Task DeleteFileAndReferencesAsync(int fileId)
        {
            using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
            {
                string deleteFileQuery = @"DELETE FROM [dbo].[Files] WHERE file_id = @file_id";
                string deleteReferencesQuery = @"DELETE FROM [dbo].[QuizResponses] WHERE file_id = @file_id;

                                               DELETE FROM [dbo].[LFQuestions] WHERE file_id = @file_id;

                                               DELETE FROM [dbo].[FBQuestions] WHERE file_id = @file_id";

                // Delete the file first
                SqlCommand deleteCmd = new SqlCommand(deleteFileQuery, conn);
                deleteCmd.Parameters.AddWithValue("@file_id", fileId);
                await conn.OpenAsync();
                await deleteCmd.ExecuteNonQueryAsync();

                // Delete references in other tables
                SqlCommand deleteRefsCmd = new SqlCommand(deleteReferencesQuery, conn);
                deleteRefsCmd.Parameters.AddWithValue("@file_id", fileId);
                await deleteRefsCmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }
    }
}