using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmAddFAQ : System.Web.UI.Page
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

        protected async void btnAddFAQ_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuestion.Text) || string.IsNullOrWhiteSpace(txtAnswer.Text))
            {
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            int moduleId = await GetModuleIdAsync(lblModuleName.Text);
            DateTime dateTime = DateTime.Now;

            using (var con = new SqlConnection(await GetConnectionStringAsync()))
            {
                const string query = @"
                    INSERT INTO FAQs (question, answer, user_id, module_id, created_at)
                    VALUES (@Question, @Answer, @UserId, @ModuleId, @created_at)";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Question", txtQuestion.Text.Trim());
                    cmd.Parameters.AddWithValue("@Answer", txtAnswer.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ModuleId", moduleId);
                    cmd.Parameters.AddWithValue("@created_at", dateTime);
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
