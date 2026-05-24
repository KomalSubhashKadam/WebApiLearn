using System.ComponentModel.DataAnnotations;
namespace WebApiLearning.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
}
