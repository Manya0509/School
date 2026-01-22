using Alfatraining.Ams.Common.DbRepository;
using Alfatraining.Ams.Common.DbRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using School.Db;
using School.Db.Models;
using School.Db.Views;
using School.Web.PageModels.Classes;
using School.Web.PageModels.Students;

namespace School.Web.Data.Services
{
    public class ClassModelService
    {
        private SchoolDbContext _context;
        private readonly StudentService _studentService;
        private EFCoreRepository<ClassModel> _repository;

        public ClassModelService(SchoolDbContext schoolDbContext, StudentService studentService)
        {
            _context = schoolDbContext;
            _studentService = studentService;
            _repository = new EFCoreRepository<ClassModel>(_context, "user123");
        }

        public List<ClassItemViewModel> GetClasses()
        {
            var classes = _repository.Get().ToList();
            return classes.ConvertAll(classModel => ConvertItem(classModel));
        }

        public List<ClassModel> GetClassesModel()
        {
            var classes = _context.ClassDbSet.ToList();
            return classes;
        }

        private ClassItemViewModel ConvertItem(ClassModel classModel/* List<StudentModel> students*/)
        {
            var item = new ClassItemViewModel(classModel);

            item.IsStudent = _context.StudentDbSet.Any(x => x.ClassId == item.Id);

            //if (students != null && students.Any())
            //{
            //    item.Students = students
            //        .Select(s => new StudentItemViewModel(s))
            //        .ToList();
            //}

            return item;
        }

        public ClassItemViewModel AddClass(ClassItemViewModel classItem)
        {
            var entity = classItem.Item;
            var result = _repository.Create(entity);
            return ConvertItem(result);
        }

        internal ClassItemViewModel Update(ClassItemViewModel classItem)
        {
            var item = _repository.FindByIdForReload(classItem.Id);
            if (item != null)
            {
                item.Number = classItem.Number;
                item.IsDeleted = classItem.IsDeleted;

                var updateItem = _repository.Update(item, classItem.Item.RowVersion, "update");
                return ConvertItem(updateItem);
            }
            return null;
        }
        public List<FilterModel> GetFilterModels()
        {
            var list = from s in _context.ClassDbSet
                       select new FilterModel()
                       {
                           Id = s.Id,
                           Name = s.Number.ToString()
                       };
            return list.ToList();
        }

        public List<ClassItemViewModel> GetFilterClasses(string number)
        {
            var query = _repository.Get();  

            if (!string.IsNullOrWhiteSpace(number))
            {
                query = query.Where(c => c.Number.ToString().Contains(number));
            }

            var classes = query.ToList();
            return classes.ConvertAll(classModel => ConvertItem(classModel));
        }

        public ClassItemViewModel GetClass(int id)
        {
            var classModel = _repository.FindById(id);
            if (classModel != null)
            {
                return ConvertItem(classModel);
            }
            return null;
        }

        public void DeleteClass(ClassItemViewModel classItem)
        {
            if (classItem.Item != null)
            {
                var entity = _repository.FindByIdForReload(classItem.Id);
                if (entity != null)
                {
                    _repository.Remove(entity);
                }
            }
        }

        public async Task<bool> RestoreAsync(int classId, bool isDeleted)
        {
            try
            {
                var classItem = await _repository.FindByIdAsync(classId);
                if (classItem == null) return false;

                classItem.IsDeleted = isDeleted;

                var result = _repository.Update(classItem, classItem.RowVersion);
                return result != null; 
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}


