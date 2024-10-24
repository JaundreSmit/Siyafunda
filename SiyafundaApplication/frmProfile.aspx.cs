using System;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace SiyafundaApplication
{
    public partial class Profile : System.Web.UI.Page
    {
        private int userId = 0;
        private int userRole = 0;

        protected async void Page_Load(object sender, EventArgs e)
        {
            lblErrors.Visible = false;

            if (Session["UserID"] != null && int.TryParse(Session["UserID"].ToString(), out userId))
            {
                userRole = Convert.ToInt32(Session["RoleID"]);

                if (userRole < 2 || userRole > 7)
                {
                    Response.Redirect("frmLandingPage.aspx");
                    return;
                }
            }
            else
            {
                Response.Redirect("frmLandingPage.aspx");
                return;
            }

            if (!IsPostBack)
            {
                await LoadUserProfileAsync();
                await LoadUserModulesAsync();
            }
        }

        private async Task LoadUserProfileAsync()
        {
            using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
            {
                string query = @"
                    SELECT u.Name, u.Surname, u.Email, u.Password, r.role_name
                    FROM [dbo].[Users] u
                    INNER JOIN [dbo].[Roles] r ON u.Role_id = r.role_id
                    WHERE u.user_id = @user_id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);

                await conn.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.Read())
                {
                    RoleLabel.Text = reader["role_name"].ToString();
                    NameTextBox.Text = reader["Name"].ToString();
                    SurnameTextBox.Text = reader["Surname"].ToString();
                    EmailTextBox.Text = reader["Email"].ToString();
                    PasswordTextBox.Text = reader["Password"].ToString();
                }
                reader.Close();
                // Close the reader
            }
        }

        private async Task LoadUserModulesAsync()
        {
            using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
            {
                string query = @"
                    SELECT m.title, m.description
                    FROM [dbo].[Modules] m
                    INNER JOIN [dbo].[Stu_To_Module] stm ON m.module_id = stm.module_id
                    WHERE stm.user_id = @user_id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);

                await conn.OpenAsync();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                await Task.Run(() => da.Fill(dt)); // Run Fill in a task for async
                ModulesGridView.DataSource = dt;
                ModulesGridView.DataBind();
            }
        }

        protected async void SaveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(await SiyafundaFunctions.GetConnectionStringAsync()))
            {
                string query = @"
                    UPDATE [dbo].[Users]
                    SET Name = @Name, Surname = @Surname, Email = @Email, Password = @Password
                    WHERE user_id = @user_id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", NameTextBox.Text);
                cmd.Parameters.AddWithValue("@Surname", SurnameTextBox.Text);
                cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                cmd.Parameters.AddWithValue("@Password", PasswordTextBox.Text);
                cmd.Parameters.AddWithValue("@user_id", userId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            // Reload the profile after saving
            await LoadUserProfileAsync();
            lblErrors.Text = "Profile updated successfully.";
            lblErrors.Visible = true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmDashboard.aspx");
        }
    }
}