namespace AcademicService.Domain.Entities
{
    public class Student 
    {
        public int Id { get; set; }
        public required string FirstName { get; set; } // Añadir required
        public required string LastName { get; set; }  // Añadir required
        public required string Email { get; set; }    // Añadir required
        public bool IsActive { get; set; }
    }
}