using Microsoft.AspNetCore.Components;
using School.Db.Models;
using School.Web.Data.Services;

namespace School.Web.Pages.ClassModel
{
    public class ClassPageViewModel : ComponentBase
    {
        [Inject] 
        public ClassModelService ClassModelService { get; set; }
        protected List<ClassItemViewModel> Classes { get; set; } = new();
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Classes = ClassModelService.GetClasses();
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        //protected override Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        var classModel = ClassModelService.GetClassModel();
        //    }

        //    return base.OnAfterRenderAsync(firstRender);
        //}
    }
}
