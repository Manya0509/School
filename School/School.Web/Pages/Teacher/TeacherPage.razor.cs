using Microsoft.AspNetCore.Components;
using School.Db.Models;
using School.Web.Data.Services;

namespace School.Web.Pages.Teacher
{
    public class TeacherPageViewModel : ComponentBase
    {
        [Inject]
        public TeacherService TeacherService { get; set; }
        protected List<TeacherItemViewModel> Teachers { get; set; } = new();
        protected TeacherItemViewModel? SelectedTeacher { get; set; }

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
            SelectedTeacher = new TeacherItemViewModel(teacher.Item);
        }

        protected void Update(TeacherItemViewModel teacher)
        {
            //teacher.FirstName = teacher.FirstName + "2";
            //TeacherService.Update(teacher);
        }

        protected void SaveChanges()
        {
            if (SelectedTeacher != null)
            { 
                TeacherService.Update(SelectedTeacher);
                Teachers = TeacherService.GetTeachers();
                SelectedTeacher = null;
                StateHasChanged();
            }
        }

        protected void CancelEdit()
        {
            SelectedTeacher = null;
        }
    }
}
