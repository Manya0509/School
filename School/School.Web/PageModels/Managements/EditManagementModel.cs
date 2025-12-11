namespace School.Web.PageModels.Managements
{
    public class EditManagementModel
    {
        public bool IsOpenDialog { get; set; }
        public ManagementItemViewModel Model { get; set; }
        public bool IsConcurrency { get; set; } = false;
    }
}
