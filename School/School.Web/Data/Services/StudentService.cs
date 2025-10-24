using School.Db;
using School.Db.Models;

namespace School.Web.Data.Services
{
    public class StudentService
    {
        private SchoolDbContext _context;

        public StudentService(SchoolDbContext schoolDbContext)
        {
            _context = schoolDbContext;
        }



        public List<StudentItemViewModel> GetStudents()
        {
            var list = _context.StudentDbSet.ToList();
            return list.ConvertAll(x => ConvertItem(x));

            //return new List<StudentModel>
            //{
            //    new StudentModel {Id = 1, FirstName = "Кристина", MiddleName = "Федоровна", LastName = "Липина", Age = 7, ClassId = 1 },
            //    new StudentModel {Id = 2, FirstName = "Валерий", MiddleName = "Иванович", LastName = "Архипов", Age = 7, ClassId = 1 },
            //    new StudentModel {Id = 3, FirstName = "Елизавета", MiddleName = "Сергеевна", LastName = "Власова", Age = 7, ClassId = 1 },
            //    new StudentModel {Id = 4, FirstName = "София", MiddleName = "Игоревна", LastName = "Викторова", Age = 7, ClassId = 1 },
            //    new StudentModel {Id = 5, FirstName = "Александр", MiddleName = "Сергеевич", LastName = "Сомов", Age = 7, ClassId = 1 },
            //    new StudentModel {Id = 6, FirstName = "Виктор", MiddleName = "Владиморович", LastName = "Ильин", Age = 8, ClassId = 2 },
            //    new StudentModel {Id = 7, FirstName = "Савелий", MiddleName = "Евгеньевич", LastName = "Романов", Age = 8, ClassId = 2 },
            //    new StudentModel {Id = 8, FirstName = "Виктория", MiddleName = "Дмитриевна", LastName = "Карпова", Age = 8, ClassId = 2 },
            //    new StudentModel {Id = 9, FirstName = "Яна", MiddleName = "Сергеевна", LastName = "Антипова", Age = 8, ClassId = 2 },
            //    new StudentModel {Id = 10, FirstName = "Лириса", MiddleName = "Ивановна", LastName = "Бирюкова", Age = 8, ClassId = 2 },
            //    new StudentModel {Id = 11, FirstName = "Артем", MiddleName = "Валентинович", LastName = "Михеев", Age = 9, ClassId = 3 },
            //    new StudentModel {Id = 12, FirstName = "Мария", MiddleName = "Сергеевна", LastName = "Губина", Age = 9, ClassId = 3 },
            //    new StudentModel {Id = 13, FirstName = "Дарья", MiddleName = "Сергеевна", LastName = "Губина", Age = 9, ClassId = 3 },
            //    new StudentModel {Id = 14, FirstName = "Алексей", MiddleName = "Александрович", LastName = "Ямилов", Age = 9, ClassId = 3 },
            //    new StudentModel {Id = 15, FirstName = "Екатерина", MiddleName = "Олеговна", LastName = "Золотова", Age = 9, ClassId = 3 },
            //    new StudentModel {Id = 16, FirstName = "Анастасия", MiddleName = "Сергеевна", LastName = "Шумнова", Age = 10, ClassId = 4 },
            //    new StudentModel {Id = 17, FirstName = "Юрий", MiddleName = "Борисович", LastName = "Игнатьев", Age = 10, ClassId = 4 },
            //    new StudentModel {Id = 18, FirstName = "Максим", MiddleName = "Александрович", LastName = "Носырев", Age = 10, ClassId = 4 },
            //    new StudentModel {Id = 19, FirstName = "Данил", MiddleName = "Олегович", LastName = "Гусев", Age = 10, ClassId = 4 },
            //    new StudentModel {Id = 20, FirstName = "Кристина", MiddleName = "Александровна", LastName = "Киселева", Age = 10, ClassId = 4 }
            //};
        }

        private StudentItemViewModel ConvertItem(StudentModel x)
        {
            var item = new StudentItemViewModel(x);
            //item.Class = _context.ClassDbSet.FirstOrDefault(c => c.Id == student.ClassId);
            return item;

        }
    }
}
