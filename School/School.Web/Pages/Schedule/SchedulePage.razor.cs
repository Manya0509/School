using Microsoft.AspNetCore.Components;
using School.Web.Data.Services;

namespace School.Web.Pages.Schedule
{
    public class SchedulePageViewModel : ComponentBase
    {
        [Inject] 
        public ScheduleService ScheduleService { get; set; }
        
        protected List<School.Db.Models.ScheduleModel> Schedules { get; set; } = new();

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            { 
                var schedule = ScheduleService.GetSchedule();
            }
            return base.OnAfterRenderAsync(firstRender);
        }
    }
}
