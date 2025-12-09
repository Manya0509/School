using School.Db.Models;
using School.Web.PageModels.Teachers;

namespace School.Web.PageModels.Cabinets
{
    public class EditCabinetModel
    {
        public bool IsOpenDialog { get; set; }
        public CabinetItemViewModel Model { get; set; }
        public List<TeacherItemViewModel> Teachers { get; set; } = new();
        public bool IsConcurrency { get; set; } = false;
    }
}
