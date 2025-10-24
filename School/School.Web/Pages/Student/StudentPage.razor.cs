using Microsoft.AspNetCore.Components;
using School.Db.Models;
using School.Web.Data.Services;

namespace School.Web.Pages.Student
{
    public class StudentPageViewModel : ComponentBase
    {
        [Inject] 
        public StudentService StudentService { get; set; }

        protected List<StudentItemViewModel> Students { get; set; } = new();

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Students = StudentService.GetStudents();
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }
    }
}
