using School.Db;
using School.Db.Models;
using School.Web.PageModels.Cabinets;

namespace School.Web.Data.Services
{
    public class CabinetService
    {
        private SchoolDbContext _context;
        private readonly TeacherService _teacherService;

        public CabinetService(TeacherService teacherService, SchoolDbContext schoolDbContext)
        {
            _teacherService = teacherService;
            _context = schoolDbContext;
        }

        public List<CabinetItemViewModel> GetCabinets()
        {
            var cabinets = _context.CabinetDbSet.ToList();
            var teachers = _context.TeacherDbSet.ToList();

            var result = cabinets.ConvertAll(cabinet =>
            {
                var teacher = teachers.FirstOrDefault(t => t.Id == cabinet.TeacherId);
                return ConvertItem(cabinet, teacher);
            });

            return result;
            //var teachers = _teacherService.GetTeachers();

            //return new List<CabinetModel>
            //{

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
            //};
        }

        private CabinetItemViewModel ConvertItem(CabinetModel cabinet, TeacherModel teacher)
        {
            var item = new CabinetItemViewModel(cabinet);

            if (teacher != null)
            {
                item.TeacherFullName = $"{teacher.LastName} {teacher.FirstName} {teacher.MiddleName}";
                item.Teacher = teacher;
            }

            return item;
        }

        public void AddCabinet(CabinetItemViewModel cabinet)
        {
            var entity = cabinet.Item;
            _context.CabinetDbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(CabinetItemViewModel cabinet)
        {
            //var item = _context.CabinetDbSet.FirstOrDefault(c => c.Id == cabinet.Id);

            //if (item == null)
            //{
            //    item.Number = cabinet.Number;

                var updateItem = _context.UpdateCabinet(cabinet.Item);
            //}
        }

        public void DeleteCabinet(CabinetItemViewModel cabinet)
        {
            if (cabinet.Item != null) 
            {
                var entity = _context.CabinetDbSet.FirstOrDefault(c => c.Id == cabinet.Id);
                if (entity != null)
                {
                    _context.CabinetDbSet.Remove(entity); 
                    _context.SaveChanges();
                }
            }
        }
    }
}
