using School.Db.Models;
using School.Web.PageModels.Classes;

namespace School.Web.PageModels.Students
{
    public class EditStudentModel
    {
        public bool IsOpenDialog { get; set; }
        public StudentItemViewModel Model { get; set; }
        public List<ClassModel> Classes { get; set; }
    }
}
