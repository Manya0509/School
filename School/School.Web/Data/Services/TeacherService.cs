
using School.Db;
using School.Db.Models;

namespace School.Web.Data.Services
{
    public class TeacherService
    {
        private SchoolDbContext _context;

        public TeacherService(SchoolDbContext schoolDbContext)
        {
            _context = schoolDbContext;
        }

        public List<TeacherItemViewModel> GetTeachers()
        {
            var list = _context.TeacherDbSet.ToList();
            return list.ConvertAll(x => ConvertItem(x));

            //return new List<TeacherModel>
            //{
            //    new TeacherModel {Id = 1, FirstName = "Виктор", MiddleName = "Сергеевич", LastName = "Игнатов", Age = 45, SubjectName = "Технология" },
            //    new TeacherModel {Id = 2, FirstName = "Ирина", MiddleName = "Юрьевна", LastName = "Голубева", Age = 38, SubjectName = "Русский язык" },
            //    new TeacherModel {Id = 3, FirstName = "Лариса", MiddleName = "Алексеевна", LastName = "Кошкина", Age = 27, SubjectName = "Литература" },
            //    new TeacherModel {Id = 4, FirstName = "Валерия", MiddleName = "Борисовна", LastName = "Терёхина", Age = 53, SubjectName = "Математика" },
            //    new TeacherModel {Id = 5, FirstName = "Максим", MiddleName = "Иванович", LastName = "Усов", Age = 55, SubjectName = "ОБЖ" },
            //    new TeacherModel {Id = 6, FirstName = "Светлана", MiddleName = "Анатольевна", LastName = "Антонова", Age = 49, SubjectName = "Окружающий мир" },
            //    new TeacherModel {Id = 7, FirstName = "Юлия", MiddleName = "Даниловна", LastName = "Лебедева", Age = 28, SubjectName = "Музыка" },
            //    new TeacherModel {Id = 8, FirstName = "Нина", MiddleName = "Федоровна", LastName = "Селина", Age = 44, SubjectName = "Изобразительное искусство" },
            //    new TeacherModel {Id = 9, FirstName = "Вадим", MiddleName = "Константинович", LastName = "Козлов", Age = 35, SubjectName = "Физическая культура" },
            //    new TeacherModel {Id = 10, FirstName = "Тамара", MiddleName = "Васильевна", LastName = "Пнева", Age = 46, SubjectName = "Иностранный язык" }
            //};

        }

        private TeacherItemViewModel ConvertItem(TeacherModel x)
        {
            var item = new TeacherItemViewModel(x);
            return item;
        }
    }
}
