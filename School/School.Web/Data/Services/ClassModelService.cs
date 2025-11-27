using Alfatraining.Ams.Common.DbRepository;
using School.Db;
using School.Db.Models;
using School.Web.PageModels.Classes;
using School.Web.PageModels.Students;

namespace School.Web.Data.Services
{
    public class ClassModelService
    {
        private SchoolDbContext _context;
        private readonly StudentService _studentService;

        public ClassModelService(SchoolDbContext schoolDbContext, StudentService studentService)
        {
            _context = schoolDbContext;
            _studentService = studentService;
        }

        public List<ClassItemViewModel> GetClasses()
        {
            var classes = _context.ClassDbSet.ToList();
            var students = _context.StudentDbSet.ToList();

            var result = classes.ConvertAll(classModel =>
            {
                var classStudents = students.Where(s => s.ClassId == classModel.Id).ToList();
                return ConvertItem(classModel, classStudents);
            });
            return result;
        }

        public List<ClassModel> GetClassesModel()
        {
            var classes = _context.ClassDbSet.ToList();
            return classes;
        }

        private ClassItemViewModel ConvertItem(ClassModel classModel, List<StudentModel> students)
        {
            var item = new ClassItemViewModel(classModel);

            if (students != null && students.Any())
            {
                item.Students = students
                    .Select(s => new StudentItemViewModel(s))
                    .ToList();
            }

            return item;
        }
    }
    
}

