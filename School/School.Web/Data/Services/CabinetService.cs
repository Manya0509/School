using Alfatraining.Ams.Common.DbRepository;
using School.Db;
using School.Db.Models;
using School.Web.PageModels.Cabinets;

namespace School.Web.Data.Services
{
    public class CabinetService
    {
        private SchoolDbContext _context;
        private readonly TeacherService _teacherService;
        private readonly EFCoreRepository<CabinetModel> _repository;
        private readonly EFCoreRepository<TeacherModel> _repositoryTeacher;

        public CabinetService(TeacherService teacherService, SchoolDbContext schoolDbContext)
        {
            _teacherService = teacherService;
            _context = schoolDbContext;
            _repository = new EFCoreRepository<CabinetModel>(_context, "user123");
            _repositoryTeacher = new EFCoreRepository<TeacherModel>(_context, "user123");
        }

        public List<CabinetItemViewModel> GetCabinets()
        {
            var cabinets = _repository.Get().ToList();
            var teachers = _context.TeacherDbSet.ToList();

            var result = cabinets.ConvertAll(cabinet =>
            {
                var teacher = teachers.FirstOrDefault(t => t.Id == cabinet.TeacherId);
                return ConvertItem(cabinet, teacher);
            });

            return result;
        }

        public CabinetItemViewModel GetCabinet(int id)
        { 
            var cabinet = _repository.FindById(id);
            if (cabinet?.TeacherId != null)
            {
                var teacher = _repositoryTeacher.FindById(cabinet.TeacherId.Value);
                return ConvertItem(cabinet, teacher);
            }
            var result = new CabinetItemViewModel(cabinet);
            return result;
        }

        private CabinetItemViewModel ConvertItem(CabinetModel cabinet, TeacherModel teacher)
        {
            var item = new CabinetItemViewModel(cabinet);

            if (teacher != null)
            {
                item.TeacherFullName = $"{teacher.LastName} {teacher.FirstName} {teacher.MiddleName}";
                item.Teacher = teacher;
            }

            return item;
        }

        public void AddCabinet(CabinetItemViewModel cabinet)
        {
            var entity = cabinet.Item;
            _repository.Create(entity);
            //_context.CabinetDbSet.Add(entity);
            //_context.SaveChanges();
        }

        public void Update(CabinetItemViewModel cabinet)
        {
            var item = _repository.FindByIdForReload(cabinet.Id);

            if (item != null)
            {
                item.Number = cabinet.Number;
                item.TeacherId = cabinet.TeacherId;

                var updateItem = _repository.Update(item, cabinet.Item.RowVersion, "update");
            }
        }

        public void DeleteCabinet(CabinetItemViewModel cabinet)
        {
            if (cabinet.Item != null) 
            {
                var entity = _repository.FindByIdForReload(cabinet.Id);
                if (entity != null)
                {
                    _repository.Remove(entity);
                    //_context.CabinetDbSet.Remove(entity); 
                    //_context.SaveChanges();
                }
            }
        }
    }
}
