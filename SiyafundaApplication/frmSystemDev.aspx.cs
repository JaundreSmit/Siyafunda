using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmSystemDev : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            lblAddModErrors.Visible = false;
            lblEditModErrors.Visible = false;

            if (!IsPostBack)
            {
                await LoadEducatorsAsync();
                await LoadModulesAsync();
            }
        }

        private async Task LoadEducatorsAsync()
        {
            ddlAddEducator.Items.Clear();
            ddlEditEducator.Items.Clear();

            try
            {
                using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
                {
                    // Select users with Role_id corresponding to educators (assuming Role_id 6 for educators)
                    string query = "SELECT user_id, CONCAT(Name, ' ', Surname) AS FullName FROM [dbo].[Users] WHERE Role_id = 6";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    await conn.OpenAsync();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        ListItem item = new ListItem(reader["FullName"].ToString(), reader["user_id"].ToString());
                        ddlAddEducator.Items.Add(item);
                        ddlEditEducator.Items.Add(item);
                    }

                    reader.Close();
                    ddlAddEducator.Items.Insert(0, new ListItem("--Select Educator--", "0"));
                    ddlEditEducator.Items.Insert(0, new ListItem("--Select Educator--", "0"));
                }
            }
            catch (Exception ex)
            {
                lblAddModErrors.Text = $"Error loading educators: {ex.Message}";
                lblAddModErrors.Visible = true;
            }
        }

        private async Task LoadModulesAsync()
        {
            ddlEditModSelect.Items.Clear();

            try
            {
                using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
                {
                    string query = "SELECT module_id, title FROM [dbo].[Modules]";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    await conn.OpenAsync();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        ListItem item = new ListItem(reader["title"].ToString(), reader["module_id"].ToString());
                        ddlEditModSelect.Items.Add(item);
                    }

                    reader.Close();
                    ddlEditModSelect.Items.Insert(0, new ListItem("--Select Module--", "0"));
                }
            }
            catch (Exception ex)
            {
                lblEditModErrors.Text = $"Error loading modules: {ex.Message}";
                lblEditModErrors.Visible = true;
            }
        }

        protected async void btnAddMod_Click(object sender, EventArgs e)
        {
            string title = txtAddModName.Text;
            string description = txtAddModDesc.Text;
            int educatorId = int.Parse(ddlAddEducator.SelectedValue);

            try
            {
                using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
                {
                    string query = "INSERT INTO [dbo].[Modules] (title, description, educator_id, created_at) VALUES (@title, @description, @educator_id, GETDATE())";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@educator_id", educatorId);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    lblAddModErrors.Text = "Module added successfully!";
                    lblAddModErrors.Visible = true;
                    await LoadModulesAsync();
                }
            }
            catch (Exception ex)
            {
                lblAddModErrors.Text = $"Error adding module: {ex.Message}";
                lblAddModErrors.Visible = true;
            }
        }

        protected async void ddlEditModSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedModuleId = int.Parse(ddlEditModSelect.SelectedValue);

            try
            {
                using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
                {
                    string query = "SELECT title, description, educator_id FROM [dbo].[Modules] WHERE module_id = @module_id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@module_id", selectedModuleId);

                    await conn.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        txtEditModTitle.Text = reader["title"].ToString();
                        txtEditModDesc.Text = reader["description"].ToString();
                        ddlEditEducator.SelectedValue = reader["educator_id"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                lblEditModErrors.Text = $"Error loading module details: {ex.Message}";
                lblEditModErrors.Visible = true;
            }
        }

        protected async void btnEditMod_Click(object sender, EventArgs e)
        {
            int moduleId = int.Parse(ddlEditModSelect.SelectedValue);
            string title = txtEditModTitle.Text;
            string description = txtEditModDesc.Text;
            int educatorId = int.Parse(ddlEditEducator.SelectedValue);

            try
            {
                using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
                {
                    string query = "UPDATE [dbo].[Modules] SET title = @title, description = @description, educator_id = @educator_id WHERE module_id = @module_id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@educator_id", educatorId);
                    cmd.Parameters.AddWithValue("@module_id", moduleId);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    lblEditModErrors.Text = "Module updated successfully!";
                    lblEditModErrors.Visible = true;
                    await LoadModulesAsync();
                }
            }
            catch (Exception ex)
            {
                lblEditModErrors.Text = $"Error updating module: {ex.Message}";
                lblEditModErrors.Visible = true;
            }
        }

        protected async void btnDeleteModule_Click(object sender, EventArgs e)
        {
            int moduleId = int.Parse(ddlEditModSelect.SelectedValue);

            try
            {
                using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
                {
                    string query = "DELETE FROM [dbo].[Modules] WHERE module_id = @module_id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@module_id", moduleId);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    lblEditModErrors.Text = "Module deleted successfully!";
                    lblEditModErrors.Visible = true;
                    await LoadModulesAsync();
                }
            }
            catch (Exception ex)
            {
                lblEditModErrors.Text = $"Error deleting module: {ex.Message}";
                lblEditModErrors.Visible = true;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmDashboard.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
