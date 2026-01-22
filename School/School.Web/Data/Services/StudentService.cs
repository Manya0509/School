using Alfatraining.Ams.Common.DbRepository;
using Microsoft.EntityFrameworkCore;
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

        internal StudentItemViewModel Update(StudentItemViewModel student)
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
                return ConvertItem(updateItem);
            }
            return null;
        }

        private StudentItemViewModel ConvertItem(StudentModel x)
        {
            var item = new StudentItemViewModel(x);
            //item.Class = _context.ClassDbSet.FirstOrDefault(c => c.Id == student.ClassId);
            return item;
        }

        public StudentItemViewModel AddStudent(StudentItemViewModel student)
        {
            var entity = student.Item;
            var result = _repository.Create(entity);
            return ConvertItem(result);
            

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
            if (student != null)
            {
                return ConvertItem(student); 
            }
            return null;
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

        public async Task<bool> MarkAsDeletedAsync(int studentId)
        {
            var student = await _repository.FindByIdAsync(studentId);
            if (student == null) return false;
            student.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int studentId, bool isDeleted)
        {
            try
            {
                var student = await _repository.FindByIdAsync(studentId);
                if (student == null) return false;

                student.IsDeleted = isDeleted;

                var result = _repository.Update(student, student.RowVersion);
                return result != null; 
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> HasStudentsInClassAsync(int classId)
        {
            return await _context.StudentDbSet.AnyAsync(s => s.ClassId == classId && !s.IsDeleted);
        }
    }
}
