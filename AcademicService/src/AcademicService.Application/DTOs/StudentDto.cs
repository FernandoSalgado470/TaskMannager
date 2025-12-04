namespace AcademicService.Application.DTOs
{
    // Usado para la respuesta (GET)
    public class StudentDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; }
    }
}