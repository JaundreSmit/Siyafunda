using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmAddAnnouncement : System.Web.UI.Page
    {
        private async Task<string> GetConnectionStringAsync()
        {
            return await SiyafundaFunctions.GetConnectionStringAsync();
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || Convert.ToInt32(Session["RoleID"]) != 6)
            {
                SiyafundaFunctions.SafeRedirect("frmEducator.aspx");
                return;
            }

            if (!IsPostBack)
            {
                await LoadModuleAsync();
            }
        }

        private async Task LoadModuleAsync()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            using (var con = new SqlConnection(await GetConnectionStringAsync()))
            {
                const string query = "SELECT title FROM Modules WHERE educator_id = @EducatorId";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EducatorId", userId);
                    await con.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    if (result != null)
                    {
                        lblModuleName.Text = result.ToString();
                    }
                }
            }
        }

        protected async void btnAddAnnouncement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text) || string.IsNullOrEmpty(txtContent.Text))
            {
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            int moduleId = await GetModuleIdAsync(lblModuleName.Text);
            DateTime created_at = DateTime.Now;

            using (var con = new SqlConnection(await GetConnectionStringAsync()))
            {
                const string query = @"
                    INSERT INTO Announcements (title, content, created_at, user_id, module_id)
                    VALUES (@Title, @Content, @CreatedAt, @UserId, @ModuleId)";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@Content", txtContent.Text.Trim());
                    cmd.Parameters.AddWithValue("@CreatedAt", created_at);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ModuleId", moduleId);
                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task<int> GetModuleIdAsync(string moduleTitle)
        {
            using (var con = new SqlConnection(await GetConnectionStringAsync()))
            {
                const string query = "SELECT module_id FROM Modules WHERE title = @Title";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Title", moduleTitle);
                    await con.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmEducator.aspx");
        }
    }
}
