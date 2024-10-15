using System;
using System.Data;
using System.Data.SqlClient;
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

        private SqlConnection Con;
        private int UserID = 0;
        private int SelectedResourceID = 0;
        private int ApproveReject = 3; // Default to 'In Progress' state

        protected void Page_Load(object sender, EventArgs e)
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

            Con = new SqlConnection(getConnectionString());
            lblProgressErrors.Visible = false;

            // Hide moderation controls initially
            dgvInProgress.Visible = false;
            txtFeedback.Visible = false;
            txtSearchProgress.Visible = false;
            btnApprove.Visible = false;
            btnReject.Visible = false;
            btnInProgressSubmit.Visible = false;

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData(string filter = null)
        {
            try
            {
                Con.Open();

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
                WHERE rs.status_id = 3";  // Only get resources with status_id of 3 (In Progress)

                if (!string.IsNullOrEmpty(filter))
                {
                    query += " AND (m.title LIKE @Filter OR r.title LIKE @Filter)";
                }

                SqlCommand cmd = new SqlCommand(query, Con);

                if (!string.IsNullOrEmpty(filter))
                {
                    cmd.Parameters.AddWithValue("@Filter", "%" + filter + "%");
                }

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dgvInProgress.DataSource = dt;
                    dgvInProgress.DataBind();
                }
                else
                {
                    lblProgressErrors.Text = "No files in progress found.";
                    lblProgressErrors.Visible = true;
                }

                reader.Close();
                dgvInProgress.Visible = true;
                txtSearchProgress.Visible = true;
            }
            catch (Exception ex)
            {
                lblProgressErrors.Text = "Error loading data: " + ex.Message;
                lblProgressErrors.Visible = true;
            }
            finally
            {
                Con.Close();
            }
        }

        private void UpdateResource(int statusID)
        {
            try
            {
                Con.Open();

                string query = @"
                UPDATE [Res_to_status]
                SET status_id = @StatusID,
                    user_id = @UserID,
                    feedback = @Feedback
                WHERE resource_id = @ResourceID";

                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@StatusID", statusID);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@Feedback", txtFeedback.Text.Trim());
                cmd.Parameters.AddWithValue("@ResourceID", SelectedResourceID);

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
            string searchTerm = txtSearchProgress.Text.Trim();
            LoadData(searchTerm);
        }

        protected void dgvInProgress_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFeedback.Visible = true;
            btnApprove.Visible = true;
            btnReject.Visible = true;
            btnInProgressSubmit.Visible = true;

            SelectedResourceID = Convert.ToInt32(dgvInProgress.SelectedDataKey.Value);
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
            if (!string.IsNullOrEmpty(txtFeedback.Text))
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
