using School.Db.Models;

namespace School.Web.Data.Services
{
    public class CabinetService
    {
        private readonly TeacherService _teacherService;

        public CabinetService(TeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        public List<CabinetModel> GetCabinets()
        {
            var teachers = _teacherService.GetTeachers();

            return new List<CabinetModel>
            {
                //new CabinetModel {Id = 1, Number = 101, TeacherId = 1, Teacher = teachers[0]},
                //new CabinetModel {Id = 2, Number = 102, TeacherId = 2, Teacher = teachers[1]},
                //new CabinetModel {Id = 3, Number = 103, TeacherId = 3, Teacher = teachers[2]},
                //new CabinetModel {Id = 4, Number = 104, TeacherId = 4, Teacher = teachers[3]},
                //new CabinetModel {Id = 5, Number = 105, TeacherId = 5, Teacher = teachers[4]},
                //new CabinetModel {Id = 6, Number = 106, TeacherId = 6, Teacher = teachers[5]},
                //new CabinetModel {Id = 7, Number = 107, TeacherId = 7, Teacher = teachers[6]},
                //new CabinetModel {Id = 8, Number = 108, TeacherId = 8, Teacher = teachers[7]},
                //new CabinetModel {Id = 9, Number = 109, TeacherId = 9, Teacher = teachers[8]},
                //new CabinetModel {Id = 10, Number = 110, TeacherId = 10, Teacher = teachers[9]}
            };
        }
    }
}
