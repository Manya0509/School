using Microsoft.EntityFrameworkCore;
using School.Db.Models;

namespace School.Db
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
            
        }

        public DbSet<StudentModel> StudentDbSet { get; set; }
        public DbSet<TeacherModel> TeacherDbSet { get; set; }
        public DbSet<ManagementModel> ManagementDbSet { get; set; }
        public DbSet<ClassModel> ClassDbSet { get; set; }

    }
}
