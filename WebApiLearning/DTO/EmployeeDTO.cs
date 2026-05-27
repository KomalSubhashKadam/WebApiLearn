using System.ComponentModel.DataAnnotations;

namespace WebApiLearning.DTO
{
    public class EmployeeDTO
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public DateOnly? DOB { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
        public string? EmailAddress { get; set; }
    }
}
