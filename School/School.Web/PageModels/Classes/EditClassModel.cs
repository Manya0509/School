namespace School.Web.PageModels.Classes
{
    public class EditClassModel
    {
        public bool IsOpenDialog { get; set; }
        public ClassItemViewModel Model { get; set; }
        public bool IsConcurrency { get; set; } = false;
    }
}
