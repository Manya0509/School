using Alfatraining.Ams.Common.DbRepository;
using Microsoft.EntityFrameworkCore;
using School.Db;
using School.Db.Models;
using School.Db.Views;
using School.Web.PageModels.Cabinets;

namespace School.Web.Data.Services
{
    public class CabinetService
    {
        private SchoolDbContext _context;
        private readonly TeacherService _teacherService;
        private readonly EFCoreRepository<CabinetModel> _repository;

        public CabinetService(TeacherService teacherService, SchoolDbContext schoolDbContext)
        {
            _teacherService = teacherService;
            _context = schoolDbContext;
            _repository = new EFCoreRepository<CabinetModel>(_context, "user123");
        }

        public List<CabinetItemViewModel> GetCabinets()
        {
            var cabinets = _repository.Get().ToList();
            return cabinets.ConvertAll(cabinet => ConvertItem(cabinet));
        }

        public List<CabinetModel> GetCabinetsModel()
        {
            var cabinets = _context.CabinetDbSet.ToList();
            return cabinets;
        }

        private CabinetItemViewModel ConvertItem(CabinetModel cabinet)
        {
            var item = new CabinetItemViewModel(cabinet);


            if (cabinet.TeacherId.HasValue)
            {
                var teacher = _context.TeacherDbSet.FirstOrDefault(t => t.Id == cabinet.TeacherId.Value);
                if (teacher != null)
                {
                    item.TeacherFullName = $"{teacher.LastName} {teacher.FirstName} {teacher.MiddleName}";
                    item.Teacher = teacher;
                }
            }

            return item;
        }

        public CabinetItemViewModel AddCabinet(CabinetItemViewModel cabinetItem)
        {
            var entity = cabinetItem.Item;
            var result = _repository.Create(entity);
            return ConvertItem(result);
        }

        internal CabinetItemViewModel Update(CabinetItemViewModel cabinetItem)
        {
            var item = _repository.FindByIdForReload(cabinetItem.Id);
            if (item != null)
            {
                item.Number = cabinetItem.Number;
                item.TeacherId = cabinetItem.TeacherId;
                item.IsDeleted = cabinetItem.IsDeleted;

                var updateItem = _repository.Update(item, cabinetItem.Item.RowVersion, "update");
                return ConvertItem(updateItem);
            }
            return null;
        }

        public void DeleteCabinet(CabinetItemViewModel cabinetItem)
        {
            if (cabinetItem.Item != null)
            {
                var entity = _repository.FindByIdForReload(cabinetItem.Id);
                if (entity != null)
                {
                    _repository.Remove(entity);
                }
            }
        }

        public List<CabinetItemViewModel> GetFilterCabinets(string number)
        {
            var query = _repository.Get();

            if (!string.IsNullOrWhiteSpace(number))
            {
                query = query.Where(c => c.Number.ToString().Contains(number));
            }

            var cabinets = query.ToList();
            return cabinets.ConvertAll(cabinet => ConvertItem(cabinet));
        }

        public CabinetItemViewModel GetCabinet(int id)
        {
            var cabinetModel = _repository.FindById(id);
            if (cabinetModel != null)
            {
                return ConvertItem(cabinetModel);
            }
            return null;
        }

        public async Task<bool> RestoreAsync(int cabinetId, bool isDeleted)
        {
            try
            {
                var cabinetItem = await _repository.FindByIdAsync(cabinetId);
                if (cabinetItem == null) return false;

                cabinetItem.IsDeleted = isDeleted;

                var result = _repository.Update(cabinetItem, cabinetItem.RowVersion);
                return result != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<FilterModel> GetFilterModels()
        {
            var list = from c in _context.CabinetDbSet
                       select new FilterModel()
                       {
                           Id = c.Id,
                           Name = c.Number.ToString()
                       };
            return list.ToList();
        }
    }
}