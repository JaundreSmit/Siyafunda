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
            pnlInProgress.Visible = true;
            pnlRejectApprove.Visible = false;

            if (!IsPostBack)
            {
                LoadData();
            }
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
                feedback = @Feedback
                WHERE resource_id = @ResourceID";

                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@StatusID", statusID);
                cmd.Parameters.AddWithValue("@Feedback", txtFeedback.Text.Trim());
                cmd.Parameters.AddWithValue("@ResourceID", Convert.ToInt32(ViewState["SelectedResourceID"]));

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
            btnInProgressSubmit.Visible = true;

            // Retrieve the selected resource ID from the GridView
            if (dgvInProgress.SelectedValue != null)
            {
                int selectedResourceID = Convert.ToInt32(dgvInProgress.SelectedValue);

                // Assign the selected Resource ID to a field or use it as needed
                SelectedResourceID = selectedResourceID;
                pnlRejectApprove.Visible = true;
                ViewState["SelectedResourceID"] = SelectedResourceID; //the view state is needed to ensure that the value is passed correctly
            }
        }

        protected void btnInProgressSubmit_Click(object sender, EventArgs e)
        {
            ApproveReject = Convert.ToInt32(rbDecision.SelectedValue);
            if (txtFeedback.Text.Length > 0)
            {
                UpdateResource(ApproveReject);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmDashboard.aspx");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // This method is required to avoid the 'Invalid postback or callback argument' error.
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlRejectApprove.Visible = false;
            txtSearchProgress.Focus();
        }
    }
}