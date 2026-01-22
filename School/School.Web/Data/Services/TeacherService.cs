using Alfatraining.Ams.Common.DbRepository;
using School.Db;
using School.Db.Models;
using School.Db.Views;
using School.Web.PageModels.Students;
using School.Web.PageModels.Teachers;
using School.Web.Pages.Teacher;

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

        internal TeacherItemViewModel Update(TeacherItemViewModel teacher)
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
                return ConvertItem(updateItem);
            }
            return null;
        }

        private TeacherItemViewModel ConvertItem(TeacherModel x)
        {
            var item = new TeacherItemViewModel(x);
            return item;
        }

        public TeacherItemViewModel AddTeacher(TeacherItemViewModel teacher)
        { 
            var entity = teacher.Item;
            var result = _repository.Create(entity);
            return ConvertItem(result);
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
            if (teacher != null)
            {
                return ConvertItem(teacher);
            }
            return null;
        }

        public List<TeacherItemViewModel> GetTeachersFilter(string firstName, string lastName, string subjectName)
        {
            var list = _repository.GetQueryable().Where(x =>
                (string.IsNullOrEmpty(firstName) ||
                x.FirstName.ToLower().StartsWith(firstName.ToLower())) &&
                (string.IsNullOrEmpty(lastName) ||
                x.LastName.ToLower().StartsWith(lastName.ToLower())) &&
                (string.IsNullOrEmpty(subjectName) ||
                x.SubjectName.ToLower().StartsWith(subjectName.ToLower()))).ToList();
            return list.ConvertAll(x => ConvertItem(x));
        }

        public List<FilterModel> GetFilterModels()
        {
            var list = from t in _context.TeacherDbSet
                       select new FilterModel()
                       {
                           Id = t.Id,
                           Name = $"{t.LastName} {t.FirstName[0]}.{t.MiddleName[0]}."
                       };
            return list.ToList();
        }

        public async Task<bool> RestoreAsync(int teacherId, bool isDeleted)
        {
            try
            {
                var teacher = await _repository.FindByIdAsync(teacherId);
                if (teacher == null) return false;

                teacher.IsDeleted = isDeleted;

                var result = _repository.Update(teacher, teacher.RowVersion);
                return result != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

