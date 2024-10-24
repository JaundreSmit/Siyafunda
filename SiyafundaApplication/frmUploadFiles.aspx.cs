using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmUploadFiles : System.Web.UI.Page
    {
        private async Task<string> GetConnectionStringAsync()
        {
            return await SiyafundaFunctions.GetConnectionStringAsync();
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || Convert.ToInt32(Session["RoleID"]) >= 7)
            {
                SiyafundaFunctions.SafeRedirect("frmDashboard.aspx");
                return;
            }

            if (!IsPostBack)
            {
                await BindModulesAsync();
            }
        }

        private async Task BindModulesAsync()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            using (var con = new SqlConnection(await GetConnectionStringAsync()))
            {
                string query = Convert.ToInt32(Session["RoleID"]) < 4
                    ? "SELECT module_id, title FROM [dbo].[Modules]"
                    : "SELECT module_id, title FROM [dbo].[Modules] WHERE educator_id = @UserID";

                using (var cmd = new SqlCommand(query, con))
                {
                    if (Convert.ToInt32(Session["RoleID"]) >= 4)
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                    }

                    await con.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            ModuleDropDown.DataSource = reader;
                            ModuleDropDown.DataTextField = "title";
                            ModuleDropDown.DataValueField = "module_id";
                            ModuleDropDown.DataBind();
                            ModuleDropDown.Items.Insert(0, new ListItem("Select Module", "0"));
                        }
                    }
                }
            }
        }

        protected async void UploadButton_Click(object sender, EventArgs e)
        {
            await UploadFileAsync();
        }

        private async Task UploadFileAsync()
        {
            if (!FileUploadControl.HasFile || ModuleDropDown.SelectedValue == "0")
            {
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            int moduleId = int.Parse(ModuleDropDown.SelectedValue);
            string fileName = Path.GetFileName(FileUploadControl.FileName);
            string filePath = Path.Combine(Server.MapPath($"~/UploadedFiles/{moduleId}/"), fileName);
            string relativePath = $"UploadedFiles/{moduleId}/{fileName}";
            string fileType = Path.GetExtension(fileName).ToLower();
            int fileSize = FileUploadControl.PostedFile.ContentLength;

            using (var con = new SqlConnection(await GetConnectionStringAsync()))
            {
                const string resourceQuery = @"
                    INSERT INTO [dbo].[Resources] (user_id, module_id, title, description, upload_date)
                    VALUES (@UserId, @ModuleId, @Title, @Description, @UploadDate);
                    SELECT SCOPE_IDENTITY();";

                using (var cmd = new SqlCommand(resourceQuery, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ModuleId", moduleId);
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDesc.Text.Trim());
                    cmd.Parameters.AddWithValue("@UploadDate", DateTime.Now);

                    await con.OpenAsync();
                    int resourceId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                    // Save the file
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    FileUploadControl.SaveAs(filePath);

                    /*if (fileType == ".jpg" || fileType == ".jpeg" || fileType == ".png")
                    {
                        // Perform watermarking if needed
                    }*/

                    // Insert the file details into the Files table
                    const string fileQuery = @"
                        INSERT INTO [dbo].[Files] (resource_id, file_path, file_type, file_size)
                        VALUES (@ResourceId, @FilePath, @FileType, @FileSize)";

                    cmd.CommandText = fileQuery;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ResourceId", resourceId);
                    cmd.Parameters.AddWithValue("@FilePath", relativePath);
                    cmd.Parameters.AddWithValue("@FileType", fileType);
                    cmd.Parameters.AddWithValue("@FileSize", fileSize);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            SiyafundaFunctions.SafeRedirect("frmEducator.aspx");
        }
    }
}