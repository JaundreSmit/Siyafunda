using System.ComponentModel.DataAnnotations;

namespace SiyafundaApplication.Models
{
    public class LeadEntity
    {
        //Users
        [Key]
        public int UserID { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
