using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmUploadFiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindModules();
            }
        }

        private void BindModules()
        {
            using (SqlConnection conn = new SqlConnection("your-connection-string"))
            {
                string query = "SELECT module_id, title FROM [dbo].[Modules]";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                ModuleDropDown.DataSource = reader;
                ModuleDropDown.DataBind();

                // add a default item
                ModuleDropDown.Items.Insert(0, new ListItem("Select Module", "0"));

                conn.Close();
            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                //Get the selected module_id from the dropdown
                int moduleId = int.Parse(ModuleDropDown.SelectedValue);
                if (moduleId == 0)
                {
                    Response.Write("Please select a valid module.");
                    return; // Exit if no valid module is selected
                }

                //Create the path to the UploadedFiles directory for the specific module
                string moduleDirectory = Server.MapPath($"~/UploadedFiles/{moduleId}/");

                // Ensure the module directory exists
                if (!Directory.Exists(moduleDirectory))
                {
                    Directory.CreateDirectory(moduleDirectory);
                }

                // Combine the directory path with the uploaded file name
                string filePath = Path.Combine(moduleDirectory, FileUploadControl.FileName);
                string fileType = Path.GetExtension(FileUploadControl.FileName);
                int fileSize = FileUploadControl.PostedFile.ContentLength;

                try
                {
                    // Save the uploaded file to the specified path
                    FileUploadControl.SaveAs(filePath);

                    // Sync file upload with DB
                    using (SqlConnection conn = new SqlConnection("your-connection-string"))
                    {
                        string query = @"INSERT INTO [dbo].[Files] (resource_id, file_path, file_type, file_size)
                                         VALUES (@resource_id, @file_path, @file_type, @file_size)";

                        SqlCommand cmd = new SqlCommand(query, conn);

                        // Assuming you get resource_id from another source
                        int resourceId;
                        if (!int.TryParse(Request.QueryString["resource_id"], out resourceId))
                        {
                            Response.Write("Invalid resource ID.");
                            return; // Exit if resource_id is invalid
                        }

                        cmd.Parameters.AddWithValue("@resource_id", resourceId);
                        cmd.Parameters.AddWithValue("@file_path", filePath);
                        cmd.Parameters.AddWithValue("@file_type", fileType);
                        cmd.Parameters.AddWithValue("@file_size", fileSize);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    // Success
                    Response.Write("File uploaded successfully! Path: " + filePath);
                }
                catch (Exception ex)
                {
                    Response.Write("Error occurred: " + ex.Message);
                }
            }
            else
            {
                // No file selected
                Response.Write("No file selected.");
            }
        }
    }
}
