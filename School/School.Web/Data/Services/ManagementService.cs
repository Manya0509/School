
using School.Db;
using School.Db.Models;
using School.Web.PageModels.Managements;

namespace School.Web.Data.Services
{
    public class ManagementService
    {
        private SchoolDbContext _context;

        public ManagementService(SchoolDbContext schoolDbContext)
        {
            _context = schoolDbContext;
        }

        public List<ManagementItemViewModel> GetManagements()
        {
            var list = _context.ManagementDbSet.ToList();
            return list.ConvertAll(x => ConvertItem(x));
        }

        public void AddManagement(ManagementItemViewModel management)
        {
            var entity = management.Item;
            _context.ManagementDbSet.Add(entity);
            _context.SaveChanges();
        }

        internal void Update(ManagementItemViewModel management)
        {
            //var item = _context.ManagementDbSet.FirstOrDefault(x => x.Id == management.Id);
            //if (item != null)
            //{
            //    item.Position  = management.Position ;
            //    item.FirstName = management.FirstName;
            //    item.LastName = management.LastName;
            //    item.MiddleName = management.MiddleName;
            //    item.Age = management.Age;

            var updateItem = _context.UpdateManagement(management.Item);

            //}
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
                var entity = _context.ManagementDbSet.FirstOrDefault(t => t.Id == management.Id);
                if (entity != null)
                {
                    _context.ManagementDbSet.Remove(entity);
                    _context.SaveChanges();
                }
            }
        }
    }
}
