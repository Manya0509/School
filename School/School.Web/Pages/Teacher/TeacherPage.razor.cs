using Microsoft.AspNetCore.Components;
using School.Web.Data.Services;
using School.Web.PageModels.Teachers;

namespace School.Web.Pages.Teacher
{
    public class TeacherPageViewModel : ComponentBase
    {
        [Inject]
        public TeacherService TeacherService { get; set; }
        protected List<TeacherItemViewModel> Teachers { get; set; } = new();
        protected TeacherItemViewModel? SelectedTeacher { get; set; }
        protected EditTeacherModel EditModel { get; set; } = new();

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Teachers = TeacherService.GetTeachers();
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        protected void SelectTeacher(TeacherItemViewModel teacher)
        {
            //SelectedTeacher = new TeacherItemViewModel(teacher.Item);
            EditModel = new();
            EditModel.Model = (TeacherItemViewModel)teacher.Clone();
            EditModel.IsOpenDialog = true;
            StateHasChanged();
        }

        protected void Update(TeacherItemViewModel teacher)
        {
            //teacher.FirstName = teacher.FirstName + "2";
            //TeacherService.Update(teacher);
        }

        protected void SaveChanges(TeacherItemViewModel item)
        {
            if (item != null)
            {
                if (item.Id == 0)
                {
                    TeacherService.AddTeacher(item);
                }
                else
                {
                    TeacherService.Update(item);
                }
                Teachers = TeacherService.GetTeachers();
                item = null;
                StateHasChanged();
            }
            EditModel.IsOpenDialog = false;
        }

        protected void CancelEdit()
        {
            SelectedTeacher = null;
        }
        protected void AddNewTeacher()
        {
            EditModel = new();
            EditModel.Model = new TeacherItemViewModel(new Db.Models.TeacherModel());
            EditModel.IsOpenDialog = true;
            StateHasChanged();
        }
    }
}
