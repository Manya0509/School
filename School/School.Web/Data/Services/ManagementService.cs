using Alfatraining.Ams.Common.DbRepository;
using School.Db;
using School.Db.Models;
using School.Web.PageModels.Cabinets;
using School.Web.PageModels.Managements;

namespace School.Web.Data.Services
{
    public class ManagementService
    {
        private SchoolDbContext _context;
        private EFCoreRepository<ManagementModel> _repository;

        public ManagementService(SchoolDbContext schoolDbContext)
        {
            _context = schoolDbContext;
            _repository = new EFCoreRepository<ManagementModel>(_context, "user123");
        }

        public List<ManagementItemViewModel> GetManagements()
        {
            var list = _repository.Get().ToList();
            return list.ConvertAll(x => ConvertItem(x));
        }

        public void AddManagement(ManagementItemViewModel management)
        {
            var entity = management.Item;
            _repository.Create(entity);
            //_context.ManagementDbSet.Add(entity);
            //_context.SaveChanges();
        }

        internal void Update(ManagementItemViewModel management)
        {
            var item = _repository.FindByIdForReload(management.Id);
            if (item != null)
            {
                item.Position = management.Position;
                item.FirstName = management.FirstName;
                item.LastName = management.LastName;
                item.MiddleName = management.MiddleName;
                item.Age = management.Age;

                var updateItem = _repository.Update(item, management.Item.RowVersion, "update");
            }
        }

        private ManagementItemViewModel ConvertItem(ManagementModel x)
        {
            var item = new ManagementItemViewModel(x);
            return item;
        }

        internal void DeleteManagement(ManagementItemViewModel management)
        {
            if (management.Item != null)
            {
                var entity = _repository.FindByIdForReload(management.Id);
                if (entity != null)
                {
                    _repository.Remove(entity);
                    //_context.ManagementDbSet.Remove(entity);
                    //_context.SaveChanges();
                }
            }
        }

        public ManagementItemViewModel GetManagement(int id)
        {
            var management = _repository.FindById(id);
            var result = new ManagementItemViewModel(management);
            return result;
        }

        public List<ManagementItemViewModel> GetManagementsFilter(string lastName, string firstName, string position)
        {
            var list = _repository.GetQueryable().Where(x =>
                (string.IsNullOrEmpty(firstName) ||
                x.FirstName.ToLower().StartsWith(firstName.ToLower())) &&
                (string.IsNullOrEmpty(lastName) ||
                x.LastName.ToLower().StartsWith(lastName.ToLower())) &&
                (string.IsNullOrEmpty(position) ||
                x.Position.ToLower().StartsWith(position.ToLower()))).ToList();
            return list.ConvertAll(x => ConvertItem(x));
        }
    }
}
