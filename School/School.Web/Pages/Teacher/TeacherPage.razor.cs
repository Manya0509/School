using Microsoft.AspNetCore.Components;
using School.Web.Data.Services;

namespace School.Web.Pages.Teacher
{
    public class TeacherPageViewModel : ComponentBase
    {
        [Inject]
        public TeacherService TeacherService { get; set; }
        protected List<School.Db.Models.TeacherModel> Teachers { get; set; } = new();

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Teachers = TeacherService.GetTeachers();
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }
    }
}
