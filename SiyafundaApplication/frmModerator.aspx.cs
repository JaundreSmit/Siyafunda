using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmModerator : System.Web.UI.Page
    {
        // Define connection string method
        protected string getConnectionString()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SiyafundaDB.mdf;Integrated Security=True";
        }

        // Declare connection variable
        private SqlConnection Con;

        private int UserID = 0;
        private int SelectedResourceID = 0;
        private int ApproveReject = 3;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                UserID = Convert.ToInt32(Session["UserID"]);
                if (Convert.ToInt32(Session["RoleID"]) > 5) //Not atleast moderator
                {
                    Response.Redirect("frmDashboard.aspx");
                }
            }
            else
            {
                Response.Redirect("frmLandingPage.aspx");
            }

            Con = new SqlConnection(getConnectionString());
            lblProgressErrors.Visible = false;

            //Hide in progress controls:
            dgvInProgress.Visible = false;
            txtFeedback.Visible = false;
            txtSearchProgress.Visible = false;
            btnApprove.Visible = false;
            btnReject.Visible = false;
            btnInProgressSubmit.Visible = false;
            LoadData();
        }

        private void LoadData(string filter = null)
        {
            try
            {
                // Open the connection
                Con.Open();

                // Define the base query with resource_id
                string query = @"
        SELECT r.resource_id,
               m.title AS ModuleTitle,
               r.title AS ResourceTitle,
               r.description,
               r.upload_date,
               f.file_type,
               f.file_size
        FROM [Res_to_status] rs
        INNER JOIN [Resources] r ON rs.resource_id = r.resource_id
        INNER JOIN [Modules] m ON r.module_id = m.module_id
        INNER JOIN [Files] f ON r.resource_id = f.resource_id
        WHERE rs.status_id = 3";  // Only get resources with status_id of 3 aka in progress

                // Add filter for module title or resource title if provided
                if (!string.IsNullOrEmpty(filter))
                {
                    query += " AND (m.title LIKE @Filter OR r.title LIKE @Filter)";
                }

                // Create SQL command
                SqlCommand cmd = new SqlCommand(query, Con);

                // Add filter parameter if applicable
                if (!string.IsNullOrEmpty(filter))
                {
                    cmd.Parameters.AddWithValue("@Filter", "%" + filter + "%");
                }

                // Execute the query
                SqlDataReader reader = cmd.ExecuteReader();

                // Check if there are rows to read
                if (reader.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);  // Load data into DataTable

                    // Bind the DataTable to the DataGridView
                    dgvInProgress.DataSource = dt;
                    dgvInProgress.DataBind();
                }
                else
                {
                    lblProgressErrors.Text = "No files in progress found.";
                    lblProgressErrors.Visible = true;
                }

                reader.Close(); // Close the reader

                // Show controls:
                dgvInProgress.Visible = true;
                txtSearchProgress.Visible = true;
                txtFeedback.Visible = false;
                btnApprove.Visible = false;
                btnReject.Visible = false;
                btnInProgressSubmit.Visible = false;
            }
            catch (Exception ex)
            {
                lblProgressErrors.Text = "Error loading data: " + ex.Message;
                lblProgressErrors.Visible = true;
            }
            finally
            {
                // Ensure the connection is closed
                Con.Close();
            }
        }

        private void UpdateResource(int statusID)
        {
            try
            {
                Con.Open();

                // Define the query to update resource status and feedback
                string query = @"
                UPDATE [Res_to_status]
                SET status_id = @StatusID,
                user_id = @UserID,
                feedback = @Feedback
                WHERE resource_id = @ResourceID";

                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@StatusID", statusID);
                cmd.Parameters.AddWithValue("@UserID", UserID); // The moderator's ID
                cmd.Parameters.AddWithValue("@Feedback", txtFeedback.Text.Trim());
                cmd.Parameters.AddWithValue("@ResourceID", SelectedResourceID);

                // Execute the query
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    lblProgressErrors.Text = "Resource status updated successfully.";
                    lblProgressErrors.Visible = true;
                }
                else
                {
                    lblProgressErrors.Text = "Failed to update resource status.";
                    lblProgressErrors.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblProgressErrors.Text = "Error updating resource: " + ex.Message;
                lblProgressErrors.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        protected void txtSearchProgress_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchProgress.Text.Length > 0)
            {
                // Get the search term from the TextBox
                string searchTerm = txtSearchProgress.Text.Trim();

                // Call the LoadData method with the search term to filter results
                LoadData(searchTerm);
            }
        }

        protected void dgvInProgress_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make feedback controls visible
            txtFeedback.Visible = true;
            btnApprove.Visible = true;
            btnReject.Visible = true;
            btnInProgressSubmit.Visible = true;

            // Retrieve the selected resource ID from the GridView
            if (dgvInProgress.SelectedDataKey != null)
            {
                int selectedResourceID = Convert.ToInt32(dgvInProgress.SelectedDataKey.Value);

                // Assign the selected Resource ID to a field or use it as needed
                SelectedResourceID = selectedResourceID;
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            ApproveReject = 1; // 1 = Rejected
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            ApproveReject = 2; // 2 = Approved
        }

        protected void btnInProgressSubmit_Click(object sender, EventArgs e)
        {
            if (txtFeedback.Text.Length > 0)
            {
                UpdateResource(ApproveReject);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmDashboard.aspx");
        }
    }
}