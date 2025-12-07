using AcademicService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcademicService.Infrastructure.Data
{
    public class AcademicDbContext : DbContext
    {
        public AcademicDbContext(DbContextOptions<AcademicDbContext> options) : base(options)
        {
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<AcademicPeriod> AcademicPeriods { get; set; }
        public DbSet<StudentGroup> StudentGroups { get; set; }
        public DbSet<ParentStudent> ParentStudents { get; set; }
        public DbSet<Student> Students { get; set; } 
        public DbSet<TeacherSubjectGroup> TeacherSubjectGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Subject Configuration
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ColorCode).HasMaxLength(7);
            });

            // Group Configuration
            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SchoolYear).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Level).IsRequired().HasMaxLength(50);
            });

            // AcademicPeriod Configuration
            modelBuilder.Entity<AcademicPeriod>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // TeacherSubjectGroup Configuration
            modelBuilder.Entity<TeacherSubjectGroup>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.Subject)
                    .WithMany(s => s.TeacherSubjectGroups)
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Group)
                    .WithMany(g => g.TeacherSubjectGroups)
                    .HasForeignKey(e => e.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.AcademicPeriod)
                    .WithMany(p => p.TeacherSubjectGroups)
                    .HasForeignKey(e => e.AcademicPeriodId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.TeacherId, e.SubjectId, e.GroupId, e.AcademicPeriodId })
                    .IsUnique();
            });

            // StudentGroup Configuration
            modelBuilder.Entity<StudentGroup>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Group)
                    .WithMany(g => g.StudentGroups)
                    .HasForeignKey(e => e.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.StudentId, e.GroupId }).IsUnique();
            });

            // ParentStudent Configuration
            modelBuilder.Entity<ParentStudent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Relationship).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => new { e.ParentId, e.StudentId }).IsUnique();
            });
        }
    }
}
