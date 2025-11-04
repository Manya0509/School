
using School.Db;
using School.Db.Models;
using School.Web.PageModels.Teachers;

namespace School.Web.Data.Services
{
    public class TeacherService
    {
        private SchoolDbContext _context;

        public TeacherService(SchoolDbContext schoolDbContext)
        {
            _context = schoolDbContext;
        }

        public List<TeacherItemViewModel> GetTeachers()
        {
            var list = _context.TeacherDbSet.ToList();
            return list.ConvertAll(x => ConvertItem(x));
        }

        internal void Update(TeacherItemViewModel teacher)
        {
            //var item = _context.TeacherDbSet.FirstOrDefault(x => x.Id == teacher.Id);

            //if (item == null)
            //{
                //item.FirstName = teacher.FirstName;
                //item.MiddleName = teacher.MiddleName;
                //item.LastName = teacher.LastName;
                //item.Age = teacher.Age;
                //item.SubjectName = teacher.SubjectName;

                var updateItem = _context.UpdateTeacher(teacher.Item);
            //}
        }

        private TeacherItemViewModel ConvertItem(TeacherModel x)
        {
            var item = new TeacherItemViewModel(x);
            return item;
        }

        public void AddTeacher(TeacherItemViewModel teacher)
        { 
            var entity = teacher.Item;
            _context.TeacherDbSet.Add(entity);
            _context.SaveChanges(); 
        }
    }
}
