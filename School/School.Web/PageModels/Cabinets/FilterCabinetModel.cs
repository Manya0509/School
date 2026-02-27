using School.Db.Views;

namespace School.Web.PageModels.Cabinets
{
    public class FilterCabinetModel
    {
        public string Number { get; set; } = "";
        public string TeacherName { get; set; } = "";
        public List<FilterModel> Teachers { get; set; } = new();
        public int? TeacherId { get; set; }

        public void Clear()
        { 
            Number = string.Empty;
            TeacherName = string.Empty;
            TeacherId = 0;
        }
    }
}
