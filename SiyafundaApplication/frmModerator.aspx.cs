using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiyafundaApplication
{
    public partial class frmModerator : System.Web.UI.Page
    {
        // Define connection string method
        private async Task<string> GetConnectionStringAsync()
        {
            return await SiyafundaFunctions.GetConnectionStringAsync();
        }

        // Declare connection variable
        private SqlConnection Con;

        private int UserID = 0;
        private int SelectedResourceID = 0;
        private int ApproveReject = 3;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                UserID = Convert.ToInt32(Session["UserID"]);
                if (Convert.ToInt32(Session["RoleID"]) > 5) // Not at least moderator
                {
                    Response.Redirect("frmDashboard.aspx");
                }
            }
            else
            {
                Response.Redirect("frmLandingPage.aspx");
            }

            lblProgressErrors.Visible = false;

            // Hide in progress controls:
            pnlInProgress.Visible = true;
            pnlRejectApprove.Visible = false;

            if (!IsPostBack)
            {
                await LoadDataAsync();
            }
        }

        private async Task LoadDataAsync(string filter = null)
        {
            try
            {
                string connectionString = await GetConnectionStringAsync();
                using (Con = new SqlConnection(connectionString))
                {
                    await Con.OpenAsync();

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

                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        // Add filter parameter if applicable
                        if (!string.IsNullOrEmpty(filter))
                        {
                            cmd.Parameters.AddWithValue("@Filter", "%" + filter + "%");
                        }

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
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
                        }
                    }
                }
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
        }

        private async Task UpdateResourceAsync(int statusID)
        {
            try
            {
                string connectionString = await GetConnectionStringAsync();
                using (Con = new SqlConnection(connectionString))
                {
                    await Con.OpenAsync();

                    // Define the query to update resource status and feedback
                    string query = @"
                    UPDATE [Res_to_status]
                    SET status_id = @StatusID,
                        feedback = @Feedback
                    WHERE resource_id = @ResourceID";

                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        cmd.Parameters.AddWithValue("@StatusID", statusID);
                        cmd.Parameters.AddWithValue("@Feedback", txtFeedback.Text.Trim());
                        cmd.Parameters.AddWithValue("@ResourceID", Convert.ToInt32(ViewState["SelectedResourceID"]));

                        // Execute the query
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            lblProgressErrors.Text = "Resource status updated successfully.";
                        }
                        else
                        {
                            lblProgressErrors.Text = "Failed to update resource status.";
                        }
                        lblProgressErrors.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblProgressErrors.Text = "Error updating resource: " + ex.Message;
                lblProgressErrors.Visible = true;
            }
        }

        protected async void txtSearchProgress_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearchProgress.Text))
            {
                // Get the search term from the TextBox
                string searchTerm = txtSearchProgress.Text.Trim();

                // Call the LoadDataAsync method with the search term to filter results
                await LoadDataAsync(searchTerm);
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
                SelectedResourceID = Convert.ToInt32(dgvInProgress.SelectedValue);
                pnlRejectApprove.Visible = true;
                ViewState["SelectedResourceID"] = SelectedResourceID; // Store in view state
            }
        }

        protected async void btnInProgressSubmit_Click(object sender, EventArgs e)
        {
            ApproveReject = Convert.ToInt32(rbDecision.SelectedValue);
            if (!string.IsNullOrWhiteSpace(txtFeedback.Text))
            {
                await UpdateResourceAsync(ApproveReject);
            }
            else
            {
                lblProgressErrors.Text = "Please provide feedback before submitting.";
                lblProgressErrors.Visible = true;
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

        protected void rbDecision_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Additional logic can be added here if needed
        }
    }
}