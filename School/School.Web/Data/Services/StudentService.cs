using Alfatraining.Ams.Common.DbRepository;
using School.Db;
using School.Db.Models;
using School.Web.PageModels.Managements;
using School.Web.PageModels.Students;

namespace School.Web.Data.Services
{
    public class StudentService
    {
        private SchoolDbContext _context;
        private EFCoreRepository<StudentModel> _repository;

        public StudentService(SchoolDbContext schoolDbContext)
        {
            _context = schoolDbContext;
            _repository = new EFCoreRepository<StudentModel>(_context, "user123");
        }

        public List<StudentItemViewModel> GetStudents()
        {
            var list = _repository.Get().ToList(); /*_context.StudentDbSet.ToList();*/
            return list.ConvertAll(x => ConvertItem(x));

            //return new List<StudentModel>
            //{
            //    new StudentModel {Id = 1, FirstName = "Кристина", MiddleName = "Федоровна", LastName = "Липина", Age = 7, ClassId = 1 },
            //    new StudentModel {Id = 2, FirstName = "Валерий", MiddleName = "Иванович", LastName = "Архипов", Age = 7, ClassId = 1 }
            //};
        }

        internal void Update(StudentItemViewModel student)
        {
            var item = _repository.FindByIdForReload(student.Id); /*_context.StudentDbSet.FirstOrDefault(x => x.Id == student.Id);*/
            if (item != null)
            {
                item.FirstName = student.FirstName;
                item.LastName = student.LastName;
                item.MiddleName = student.MiddleName;
                item.Age = student.Age;
                item.ClassId = student.ClassId;

                var updateItem = _repository.Update(item, student.Item.RowVersion, "update"); /*_context.UpdateStudent(student.Item);*/
            }
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
            _repository.Create(entity);
            //_context.StudentDbSet.Add(entity);
            //_context.SaveChanges();
        }

        public void DeleteStudent(StudentItemViewModel student)
        {
            if (student.Item != null)
            {
                var entity = _repository.FindByIdForReload(student.Id); /*_context.StudentDbSet.FirstOrDefault(s => s.Id == student.Id);*/
                if (entity != null)
                {
                    _repository.Remove(entity);
                    //_context.StudentDbSet.Remove(entity);
                    //_context.SaveChanges();
                }
            }
        }

        public StudentItemViewModel GetStudent(int id)
        {
            var student = _repository.FindById(id);
            var result = new StudentItemViewModel(student);
            return result;
        }

        public List<StudentItemViewModel> GetStudentsFilter(string firstName, string lastName, int classId)
        {
            var list = _repository.GetQueryable().Where(x =>
                (string.IsNullOrEmpty(firstName) || 
                x.FirstName.ToLower().StartsWith(firstName.ToLower())) &&
                (string.IsNullOrEmpty(lastName) || 
                x.LastName.ToLower().StartsWith(lastName.ToLower())) &&
                (classId == 0 || x.ClassId == classId)).ToList();
            return list.ConvertAll(x => ConvertItem(x));
        }
    }
}
