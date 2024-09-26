using System;
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
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                //Path to the UploadedFiles directory
                string uploadDir = Server.MapPath("~/UploadedFiles/");

                //Makes sure the directory exists
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Combines the directory path with the uploaded file name
                string filePath = Path.Combine(uploadDir, FileUploadControl.FileName);

                try
                {
                    //TODO: sync file upload with DB, make sure path is correct to each module and implement moderation of files
                    //Save the uploaded file to the specified path
                    FileUploadControl.SaveAs(filePath);

                    //Success
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
