using Alfatraining.Ams.Common.DbRepository;
using School.Db;
using School.Db.Models;
using School.Web.PageModels.Students;
using School.Web.PageModels.Teachers;

namespace School.Web.Data.Services
{
    public class TeacherService
    {
        private SchoolDbContext _context;
        private EFCoreRepository<TeacherModel> _repository;

        public TeacherService(SchoolDbContext schoolDbContext)
        {
            _context = schoolDbContext;
            _repository = new EFCoreRepository<TeacherModel>(_context, "user123");
        }

        public List<TeacherItemViewModel> GetTeachers()
        {
            var list = _repository.Get().ToList();
            return list.ConvertAll(x => ConvertItem(x));
        }

        internal void Update(TeacherItemViewModel teacher)
        {
            var item = _repository.FindByIdForReload(teacher.Id);

            if (item != null)
            {
                item.FirstName = teacher.FirstName;
                item.MiddleName = teacher.MiddleName;
                item.LastName = teacher.LastName;
                item.Age = teacher.Age;
                item.SubjectName = teacher.SubjectName;

                var updateItem = _repository.Update(item, teacher.Item.RowVersion, "update");
            }
        }

        private TeacherItemViewModel ConvertItem(TeacherModel x)
        {
            var item = new TeacherItemViewModel(x);
            return item;
        }

        public void AddTeacher(TeacherItemViewModel teacher)
        { 
            var entity = teacher.Item;
            _repository.Create(entity);
            //_context.TeacherDbSet.Add(entity);
            //_context.SaveChanges(); 
        }

        public void DeleteTeacher(TeacherItemViewModel teacher)
        {
            if (teacher.Item != null)
            {
                var entity = _repository.FindByIdForReload(teacher.Id);
                if (entity != null)
                { 
                    _repository.Remove(entity);
                    //_context.TeacherDbSet.Remove(entity);
                    //_context.SaveChanges();
                }
            }
        }

        public TeacherItemViewModel GetTeacher(int id)
        {
            var teacher = _repository.FindById(id);
            var result = new TeacherItemViewModel(teacher);
            return result;
        }
    }
}

