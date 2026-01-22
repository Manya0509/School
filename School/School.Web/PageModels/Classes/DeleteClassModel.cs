namespace School.Web.PageModels.Classes
{
    public class DeleteClassModel
    {
        public bool IsOpenDialog { get; set; }
        public ClassItemViewModel ClassDelete { get; set; }
        public bool HasStudents { get; set; }
    }
}
