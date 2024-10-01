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
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        private int UserID = 0;
        private SqlConnection Con;

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new SqlConnection(getConnectionString());
            lblError.Visible = false;

            if (Session["UserID"] != null && Convert.ToInt32(Session["RoleID"]) < 7)
            {
                UserID = Convert.ToInt32(Session["UserID"]);
            }
            else
            {
                Response.Redirect("frmDashboard.aspx");
            }

            if (!IsPostBack)
            {
                BindModules();
            }
        }

        private void BindModules()
        {
            using (Con)
            {
                try
                {
                    string query;
                    if (Convert.ToInt32(Session["RoleID"]) < 4)
                    {
                        query = "SELECT module_id, title FROM [dbo].[Modules]";
                    }
                    else
                    {
                        query = "SELECT module_id, title FROM [dbo].[Modules] WHERE educator_id = @UserID";
                    }

                    SqlCommand cmd = new SqlCommand(query, Con);

                    if (Convert.ToInt32(Session["RoleID"]) >= 4)
                    {
                        cmd.Parameters.AddWithValue("@UserID", UserID);
                    }

                    Con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        ModuleDropDown.DataSource = reader;
                        ModuleDropDown.DataTextField = "title";
                        ModuleDropDown.DataValueField = "module_id";
                        ModuleDropDown.DataBind();
                        ModuleDropDown.Items.Insert(0, new ListItem("Select Module", "0"));
                    }
                    else
                    {
                        lblError.Text = "No modules found for the specified user.";
                        lblError.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error retrieving modules: " + ex.Message;
                    lblError.Visible = true;
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        private string AddWatermark(string mainImagePath, string watermarkImagePath, double markRatio)
        {
            using (var mainImage = System.Drawing.Image.FromFile(mainImagePath))
            using (var watermarkImage = System.Drawing.Image.FromFile(watermarkImagePath))
            {
                using (var bitmap = new System.Drawing.Bitmap(mainImage.Width, mainImage.Height))
                {
                    using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        graphics.DrawImage(mainImage, 0, 0);
                        var watermarkWidth = (int)(mainImage.Width * markRatio);
                        var watermarkHeight = (int)(watermarkImage.Height * ((double)watermarkWidth / watermarkImage.Width));
                        var watermarkPositionX = mainImage.Width - watermarkWidth - 10; // 10px from right
                        var watermarkPositionY = mainImage.Height - watermarkHeight - 10; // 10px from bottom

                        graphics.DrawImage(watermarkImage, watermarkPositionX, watermarkPositionY, watermarkWidth, watermarkHeight);
                    }
                    var watermarkedPath = Path.Combine(Path.GetDirectoryName(mainImagePath), "watermarked_" + Path.GetFileName(mainImagePath));
                    bitmap.Save(watermarkedPath);
                    return watermarkedPath;
                }
            }
        }

        private async Task UploadFileAsync()
        {
            if (FileUploadControl.HasFile)
            {
                int moduleId = int.Parse(ModuleDropDown.SelectedValue);
                if (moduleId == 0)
                {
                    lblError.Text = "Please select a valid module.";
                    lblError.Visible = true;
                    return;
                }

                lblError.Visible = true;
                string moduleDirectory = Server.MapPath($"~/UploadedFiles/{moduleId}/");
                if (!Directory.Exists(moduleDirectory))
                {
                    Directory.CreateDirectory(moduleDirectory);
                }

                string fileName = FileUploadControl.FileName;
                string filePath = Path.Combine(moduleDirectory, fileName);
                string relativeFilePath = $"UploadedFiles/{moduleId}/{fileName}";
                string fileType = Path.GetExtension(fileName).ToLower();
                int fileSize = FileUploadControl.PostedFile.ContentLength;

                try
                {
                    FileUploadControl.SaveAs(filePath); // Save the uploaded file

                    if (fileType == ".jpg" || fileType == ".jpeg" || fileType == ".png" || fileType == ".gif")
                    {
                        if (fileSize < 1048576) // Check if file size is less than 1 MB
                        {
                            string watermarkImagePath = Server.MapPath("~/Assets/SiyafundaLogo.png"); // logo path
                            string watermarkedImagePath = AddWatermark(filePath, watermarkImagePath, 0.25);

                            // Overwrite the original file with the watermarked image
                            File.Copy(watermarkedImagePath, filePath, true);

                            lblError.Text = "File uploaded and watermarked successfully!";
                        }
                        else
                        {
                            lblError.Text = "Uploaded file is too large";
                        }
                    }

                    using (Con)
                    {
                        string resourceQuery = @"INSERT INTO [dbo].[Resources] (user_id, module_id, title, description, upload_date)
                                         VALUES (@user_id, @module_id, @title, @description, @upload_date);
                                         SELECT SCOPE_IDENTITY();";

                        SqlCommand resourceCmd = new SqlCommand(resourceQuery, Con);
                        resourceCmd.Parameters.AddWithValue("@user_id", UserID);
                        resourceCmd.Parameters.AddWithValue("@module_id", moduleId);
                        resourceCmd.Parameters.AddWithValue("@title", txtTitle.Text);
                        resourceCmd.Parameters.AddWithValue("@description", txtDesc.Text);
                        resourceCmd.Parameters.AddWithValue("@upload_date", DateTime.Now);

                        Con.Open();
                        int resourceId = Convert.ToInt32(resourceCmd.ExecuteScalar());

                        string fileQuery = @"INSERT INTO [dbo].[Files] (resource_id, file_path, file_type, file_size)
                                     VALUES (@resource_id, @file_path, @file_type, @file_size)";

                        SqlCommand fileCmd = new SqlCommand(fileQuery, Con);
                        fileCmd.Parameters.AddWithValue("@resource_id", resourceId);
                        fileCmd.Parameters.AddWithValue("@file_path", relativeFilePath);
                        fileCmd.Parameters.AddWithValue("@file_type", fileType);
                        fileCmd.Parameters.AddWithValue("@file_size", fileSize);
                        fileCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "File upload failed: " + ex.Message;
                }
            }
            else
            {
                lblError.Text = "Please select a file.";
            }
        }

        protected async void UploadButton_Click(object sender, EventArgs e)
        {
            await UploadFileAsync();
        }
    }
}








