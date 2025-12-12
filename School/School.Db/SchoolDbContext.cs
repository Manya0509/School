using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using School.Db.Models;
using School.Db.Views;

namespace School.Db
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<FilterModel>(eb => { eb.HasNoKey(); });
        }

        public DbSet<StudentModel> StudentDbSet { get; set; }
        public DbSet<TeacherModel> TeacherDbSet { get; set; }
        public DbSet<ManagementModel> ManagementDbSet { get; set; }
        public DbSet<ClassModel> ClassDbSet { get; set; }
        public DbSet<CabinetModel> CabinetDbSet { get; set; }
        public DbSet<FilterModel> FilterModels { get; set; }

        public object UpdateCabinet(CabinetModel cabinet)
        {
            var sql = @"Update Cabinets
                Set Number = @pNumber
                Where Id = @pId";

            var list = new List<SqlParameter> {
                new SqlParameter("@pNumber", cabinet.Number),
                new SqlParameter("@pId", cabinet.Id)};

            this.Database.ExecuteSqlRaw(sql, list);
            var sqlSelect = "Select * from Cabinets where Id = " + cabinet.Id;
            var result = CabinetDbSet.FromSqlRaw(sqlSelect).FirstOrDefault();
            return result;
        }

        public object UpdateManagement(ManagementModel management)
        {
            var sql = @"Update Managements
                Set Position  = @pPosition ,
                FirstName = @pFirstName,
                LastName = @pLastName,
                MiddleName = @pMiddleName,
                Age = @pAge
                Where Id = @pId";

            var list = new List<SqlParameter> {
                new SqlParameter("@pPosition", management.Position),
                new SqlParameter("@pFirstName", management.FirstName),
                new SqlParameter("@pLastName", management.LastName),
                new SqlParameter("@pMiddleName", management.MiddleName),
                new SqlParameter("@pAge", management.Age),
                new SqlParameter("@pId", management.Id) };

            this.Database.ExecuteSqlRaw(sql, list);
            var sqlSelect = "Select * from Managements where Id = " + management.Id;
            var result = ManagementDbSet.FromSqlRaw(sqlSelect).FirstOrDefault();
            return result;
        }
        
        public StudentModel UpdateStudent(StudentModel student)
        {
            var sql = @"Update Students
                SET FirstName = @pFirstName,
                LastName = @pLastName,
                MiddleName = @pMiddleName,
                Age = @pAge,
                ClassId = @pClassId
                Where Id = @pId";

            var list = new List<SqlParameter> {
                new SqlParameter("@pFirstName", student.FirstName),
                new SqlParameter("@pLastName", student.LastName),
                new SqlParameter("@pMiddleName", student.MiddleName),
                new SqlParameter("@pAge", student.Age),
                new SqlParameter("@pClassId", student.ClassId),
                new SqlParameter("@pId", student.Id) };

            this.Database.ExecuteSqlRaw(sql, list);
            var sqlSelect = "Select * from Students where Id = " + student.Id;
            var result = StudentDbSet.FromSqlRaw(sqlSelect).FirstOrDefault();
            return result;
        }

        public object UpdateTeacher(TeacherModel teacher)
        {
            var sql = @"Update Teachers
                Set FirstName = @pFirstName,
                LastName = @pLastName,
                MiddleName = @pMiddleName,
                Age = @pAge,
                SubjectName = @pSubjectName
                Where Id = @pId";

            var list = new List<SqlParameter> {
                new SqlParameter("@pFirstName", teacher.FirstName),
                new SqlParameter("@pLastName", teacher.LastName),
                new SqlParameter("@pMiddleName", teacher.MiddleName),
                new SqlParameter("@pAge", teacher.Age),
                new SqlParameter("@pSubjectName", teacher.SubjectName),
                new SqlParameter("@pId", teacher.Id) };

            this.Database.ExecuteSqlRaw(sql,list);
            var sqlSelect = "Select * from Teachers where Id = " + teacher.Id;
            var result = TeacherDbSet.FromSqlRaw(sqlSelect).FirstOrDefault();
            return result;
        }



        //public void DeleteTeacher(TeacherModel teacher)
        //{
        //    var sql = @"DELETE FROM Teachers 
        //        WHERE Id = @pId";

        //    var parameters = new List<SqlParameter> {
        //    new SqlParameter("@pId", teacher.Id) };

        //    this.Database.ExecuteSqlRaw(sql, parameters);
        //}
    }
}
