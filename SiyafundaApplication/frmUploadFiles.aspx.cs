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
        // Define connection string method
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        private int UserID = 0;

        // Declare connection variable
        private SqlConnection Con;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize connection using getConnectionString method
            Con = new SqlConnection(getConnectionString());
            lblError.Visible = false;
            UploadButton.Enabled = true;

            if (Session["UserID"] != null && Convert.ToInt32(Session["RoleID"]) < 7) //Only users with higher than student status can  upload files
            {
                UserID = Convert.ToInt32(Session["UserID"]);
            }

            if (!IsPostBack)
            {
                BindModules();
            }
        }

        // Populate the module dropdown list
        private void BindModules()
        {
            // Use existing connection
            using (Con)
            {
                try
                {
                    string query;

                    // Check the user's role and adjust the query accordingly
                    if (Convert.ToInt32(Session["RoleID"]) < 4)
                    {
                        // For roles less than 4, show all modules
                        query = "SELECT module_id, title FROM [dbo].[Modules]";
                    }
                    else
                    {
                        // For roles 4 and above, filter modules based on the UserID
                        query = "SELECT module_id, title FROM [dbo].[Modules] WHERE educator_id = @UserID";
                    }

                    SqlCommand cmd = new SqlCommand(query, Con);

                    // Add parameter only if the role is 4 or higher
                    if (Convert.ToInt32(Session["RoleID"]) >= 4)
                    {
                        cmd.Parameters.AddWithValue("@UserID", UserID); // Use parameterized query to prevent SQL injection
                    }

                    Con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    // Check if the reader has rows
                    if (reader.HasRows)
                    {
                        ModuleDropDown.DataSource = reader;
                        ModuleDropDown.DataTextField = "title";   // Display module title
                        ModuleDropDown.DataValueField = "module_id"; // Store module_id as value
                        ModuleDropDown.DataBind();

                        // Add a default item
                        ModuleDropDown.Items.Insert(0, new ListItem("Select Module", "0"));
                    }
                    else
                    {
                        // No modules found; display an error message
                        lblError.Text = "No modules found for the specified user.";
                        lblError.Visible = true;
                        UploadButton.Enabled = false;

                        // Optionally, you can clear the dropdown
                        ModuleDropDown.Items.Clear();
                    }
                }
                catch (Exception ex)
                {
                    // Error handling
                    lblError.Text = "Error retrieving modules: " + ex.Message;
                    lblError.Visible = true;
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        // Handle file upload process
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                // Get the selected module_id from the dropdown
                int moduleId = int.Parse(ModuleDropDown.SelectedValue);
                if (moduleId == 0)
                {
                    Response.Write("Please select a valid module.");
                    return; // Exit if no valid module is selected
                }

                // Create the path to the UploadedFiles directory for the specific module
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

                    // Sync file upload with DB using the existing connection
                    using (Con)
                    {
                        string query = @"INSERT INTO [dbo].[Files] (resource_id, file_path, file_type, file_size)
                                         VALUES (@resource_id, @file_path, @file_type, @file_size)";

                        SqlCommand cmd = new SqlCommand(query, Con);

                        // Assuming you get resource_id from another source (e.g., query string)
                        int resourceId;
                        if (!int.TryParse(Request.QueryString["resource_id"], out resourceId))
                        {
                            lblError.Text = "Invalid resource ID.";
                            lblError.Visible = true;
                            return; // Exit if resource_id is invalid
                        }

                        // Add parameters
                        cmd.Parameters.AddWithValue("@resource_id", resourceId);
                        cmd.Parameters.AddWithValue("@file_path", filePath);
                        cmd.Parameters.AddWithValue("@file_type", fileType);
                        cmd.Parameters.AddWithValue("@file_size", fileSize);

                        // Open the connection, execute query, and close the connection
                        Con.Open();
                        cmd.ExecuteNonQuery();
                        Con.Close();
                    }

                    // Success
                    Response.Write("File uploaded successfully! Path: " + filePath);
                }
                catch (Exception ex)
                {
                    // Error handling
                    lblError.Text = ex.Message;
                    lblError.Visible = true;
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