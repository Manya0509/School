
using School.Db;
using School.Db.Models;

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

        private ManagementItemViewModel ConvertItem(ManagementModel x)
        {
            var item = new ManagementItemViewModel(x);
            return item;
        }
    }
}
