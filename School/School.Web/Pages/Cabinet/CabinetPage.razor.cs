using Microsoft.AspNetCore.Components;
using School.Db.Models;
using School.Web.Data.Services;

namespace School.Web.Pages.Cabinet
{
    public class CabinetPageViewModel : ComponentBase
    {
        [Inject] 
        public CabinetService CabinetService { get; set; }
        protected List<CabinetItemViewModel> Cabinets { get; set; } = new();
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Cabinets = CabinetService.GetCabinets();
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }

    }
}
