using School.Db.Models;

namespace School.Web.Data.Services
{
    public class ScheduleService
    {
        public ScheduleModel GetSchedule()
        {
            return new ScheduleModel() { Id = 1 };
        }
    }
}
