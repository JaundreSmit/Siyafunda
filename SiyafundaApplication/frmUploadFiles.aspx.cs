using System;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
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
            UploadButton.Enabled = true;

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
                        UploadButton.Enabled = false;
                        ModuleDropDown.Items.Clear();
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

        // Watermark service class to call the API
        private async Task<string> AddWatermark(string mainImageUrl, string watermarkImageUrl, double markRatio)
        {
            // Create the JSON payload
            var payload = new
            {
                mainImageUrl,
                markImageUrl = watermarkImageUrl,
                markRatio
            };

            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://quickchart.io/watermark", content);

                if (response.IsSuccessStatusCode)
                {
                    string watermarkedImageUrl = await response.Content.ReadAsStringAsync();
                    return watermarkedImageUrl; // The URL of the watermarked image
                }
                else
                {
                    throw new Exception("Failed to apply watermark: " + response.ReasonPhrase);
                }
            }
        }

        // Handle file upload process
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
                            string mainImageUrl = $"{Request.Url.GetLeftPart(UriPartial.Path)}/{relativeFilePath}";
                            string watermarkUrl = $"{Request.Url.GetLeftPart(UriPartial.Path)}/Assets/SiyafundaLogo.png"; //logo path

                            // Ensure that both image URLs are valid
                            if (await IsImageAccessible(mainImageUrl) && await IsImageAccessible(watermarkUrl))
                            {
                                string watermarkedImageUrl = await AddWatermark(mainImageUrl, watermarkUrl, 0.25);

                                // Download and save the watermarked image
                                using (HttpClient client = new HttpClient())
                                {
                                    HttpResponseMessage response = await client.GetAsync(watermarkedImageUrl);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        byte[] watermarkedImageBytes = await response.Content.ReadAsByteArrayAsync();
                                        File.WriteAllBytes(filePath, watermarkedImageBytes); // Overwrite the original file
                                        lblError.Text = "File uploaded and watermarked successfully!";
                                    }
                                    else
                                    {
                                        throw new Exception("Failed to download the watermarked image: " + response.ReasonPhrase);
                                    }
                                }
                            }
                            else
                            {
                                lblError.Text = "Both image files must be accessible.";
                            }
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

                        // Execute the command and check if successful
                        int rowsAffected = fileCmd.ExecuteNonQuery();
                        if (rowsAffected > 0) // Check if any rows were affected
                        {
                            lblError.Text = "File successfully loaded.";
                        }
                        else
                        {
                            lblError.Text = "File load failed.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                }
            }
            else
            {
                lblError.Text = "No file selected.";
            }
        }

        // Helper method to check if an image URL is accessible
        private async Task<bool> IsImageAccessible(string imageUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(imageUrl);
                    return response.IsSuccessStatusCode;
                }
                catch
                {
                    return false; // Image is not accessible
                }
            }
        }

        protected void UploadButton_Click1(object sender, EventArgs e)
        {
            UploadFileAsync().GetAwaiter().GetResult();
        }
    }
}