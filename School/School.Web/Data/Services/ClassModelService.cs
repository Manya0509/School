using School.Db.Models;

namespace School.Web.Data.Services
{
    public class ClassModelService
    {     
        private readonly StudentService _studentService;

        public ClassModelService(StudentService studentService)
        {
            _studentService = studentService;
        }
        public List<ClassModel> GetClasses()
        {
            var students = _studentService.GetStudents();

            return new List<ClassModel>
            {
                new ClassModel
                {
                    Id = 1,
                    Number = 1,
                    Students = students.Where(s => s.ClassId == 1).ToList()
                },
                new ClassModel
                {
                    Id = 2,
                    Number = 2,
                    Students = students.Where(s => s.ClassId == 2).ToList()
                },
                new ClassModel
                {
                    Id = 3,
                    Number = 3,
                    Students = students.Where(s => s.ClassId == 3).ToList()
                },
                new ClassModel
                {
                    Id = 4,
                    Number = 4,
                    Students = students.Where(s => s.ClassId == 4).ToList()
                }
            };
        }
    }
}

