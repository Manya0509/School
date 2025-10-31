using School.Db;
using School.Db.Models;

namespace School.Web.Data.Services
{
    public class StudentService
    {
        private SchoolDbContext _context;

        public StudentService(SchoolDbContext schoolDbContext)
        {
            _context = schoolDbContext;
        }

        public List<StudentItemViewModel> GetStudents()
        {
            var list = _context.StudentDbSet.ToList();
            return list.ConvertAll(x => ConvertItem(x));

            //return new List<StudentModel>
            //{
            //    new StudentModel {Id = 1, FirstName = "Кристина", MiddleName = "Федоровна", LastName = "Липина", Age = 7, ClassId = 1 },
            //    new StudentModel {Id = 2, FirstName = "Валерий", MiddleName = "Иванович", LastName = "Архипов", Age = 7, ClassId = 1 }
            //};
        }

        internal void Update(StudentItemViewModel student)
        {
            //var item = _context.StudentDbSet.FirstOrDefault(x => x.Id == student.Id);
            //if (item != null)
            //{
            //    item.FirstName = student.FirstName;
            //    item.LastName = student.LastName;
            //    item.MiddleName = student.MiddleName;
            //    item.Age = student.Age;
            //    item.ClassId = student.ClassId;

                var updateItem = _context.UpdateStudent(student.Item);
            //}
        }

        private StudentItemViewModel ConvertItem(StudentModel x)
        {
            var item = new StudentItemViewModel(x);
            //item.Class = _context.ClassDbSet.FirstOrDefault(c => c.Id == student.ClassId);
            return item;
        }

        public void AddStudent(StudentItemViewModel student)
        {
            var entity = student.Item;
            _context.StudentDbSet.Add(entity);
            _context.SaveChanges();
        }
    }
}
