using System;
namespace AcademicService.Application.DTOs
{
    // Este DTO es el objeto que el API DEBE devolver para que el frontend obtenga el ID.
    public class StudentDto
    {
        public int Id { get; set; } // <--- ¡ESTE CAMPO ES VITAL!
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}