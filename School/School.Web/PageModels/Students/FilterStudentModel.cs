using School.Db.Views;

namespace School.Web.PageModels.Students
{
    public class FilterStudentModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int ClassId { get; set; }
        public List<FilterModel> Classes { get; set; }
    }
}
