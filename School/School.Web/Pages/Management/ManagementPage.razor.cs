using Microsoft.AspNetCore.Components;
using School.Web.Data.Services;

namespace School.Web.Pages.Management
{
    public class ManagementPageViewModel : ComponentBase
    {
        [Inject] ManagementService ManagementService { get; set; }
        protected List<School.Db.Models.ManagementModel> Managements { get; set; } = new();

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Managements = ManagementService.GetManagements();
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }
    }
}
