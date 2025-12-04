namespace AcademicService.Application.DTOs
{
    // Usado para la creación (POST)
    public class CreateStudentDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
    }
}
