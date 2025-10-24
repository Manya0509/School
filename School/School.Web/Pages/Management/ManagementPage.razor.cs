using Microsoft.AspNetCore.Components;
using School.Db.Models;
using School.Web.Data.Services;

namespace School.Web.Pages.Management
{
    public class ManagementPageViewModel : ComponentBase
    {
        [Inject] ManagementService ManagementService { get; set; }
        protected List<ManagementItemViewModel> Managements { get; set; } = new();

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
