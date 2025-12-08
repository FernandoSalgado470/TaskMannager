using GradesService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GradesService.Infrastructure.Data;

public class GradesDbContext : DbContext
{
    public GradesDbContext(DbContextOptions<GradesDbContext> options) : base(options)
    {
    }

    public DbSet<Grade> Grades { get; set; }
    public DbSet<GradeCategory> GradeCategories { get; set; }
    public DbSet<StudentGrade> StudentGrades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de GradeCategory
        modelBuilder.Entity<GradeCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.WeightPercentage).IsRequired().HasPrecision(5, 2);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configuración de Grade
        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StudentId).IsRequired();
            entity.Property(e => e.SubjectId).IsRequired();
            entity.Property(e => e.AcademicPeriodId).IsRequired();
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Score).IsRequired().HasPrecision(5, 2);
            entity.Property(e => e.MaxScore).IsRequired().HasPrecision(5, 2);
            entity.Property(e => e.GradeDate).IsRequired();
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.GradeCategory)
                  .WithMany(gc => gc.Grades)
                  .HasForeignKey(e => e.GradeCategoryId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => new { e.StudentId, e.SubjectId, e.AcademicPeriodId });
            entity.HasIndex(e => e.GradeDate);
        });

        // Configuración de StudentGrade
        modelBuilder.Entity<StudentGrade>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StudentId).IsRequired();
            entity.Property(e => e.SubjectId).IsRequired();
            entity.Property(e => e.AcademicPeriodId).IsRequired();
            entity.Property(e => e.FinalGrade).IsRequired().HasPrecision(5, 2);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.IsPassed).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => new { e.StudentId, e.SubjectId, e.AcademicPeriodId }).IsUnique();
        });

        // Datos semilla para categorías de calificaciones
        modelBuilder.Entity<GradeCategory>().HasData(
            new GradeCategory { Id = 1, Name = "Exámenes", Description = "Evaluaciones escritas", WeightPercentage = 40, IsActive = true, CreatedAt = DateTime.UtcNow },
            new GradeCategory { Id = 2, Name = "Tareas", Description = "Trabajos y tareas", WeightPercentage = 30, IsActive = true, CreatedAt = DateTime.UtcNow },
            new GradeCategory { Id = 3, Name = "Participación", Description = "Participación en clase", WeightPercentage = 20, IsActive = true, CreatedAt = DateTime.UtcNow },
            new GradeCategory { Id = 4, Name = "Proyecto Final", Description = "Proyecto final del curso", WeightPercentage = 10, IsActive = true, CreatedAt = DateTime.UtcNow }
        );
    }
}
