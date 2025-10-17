
using School.Db.Models;

namespace School.Web.Data.Services
{
    public class ManagementService
    {
        public List<ManagementModel> GetManagements()
        {
            return new List<ManagementModel>
            {
                new ManagementModel {Id = 1, Function = "Директор школы", FirstName = "Лариса", MiddleName = "Петровна", LastName = "Зуева", Age = 51},
                new ManagementModel {Id = 2, Function = "Заместитель директора по учебно-воспитательной работе", FirstName = "Нина", MiddleName = "Федоровна", LastName = "Ланцева", Age = 49},
                new ManagementModel {Id = 3, Function = "Заместитель директора по воспитательной работе", FirstName = "Виктория", MiddleName = "Юрьевна", LastName = "Родина", Age = 40},
                new ManagementModel {Id = 4, Function = "Заместитель директора по административно-хозяйственной работе", FirstName = "Иван", MiddleName = "Сергеевич", LastName = "Ямилов", Age = 50},
                new ManagementModel {Id = 5, Function = "Заместитель директора по безопасности", FirstName = "Виталий", MiddleName = "Дмитриевич", LastName = "Липин", Age = 45},
                new ManagementModel {Id = 6, Function = "Заместитель директора по информатизации", FirstName = "Арсений", MiddleName = "Антонович", LastName = "Буров", Age = 41},
                new ManagementModel {Id = 7, Function = "Социальный педагог", FirstName = "Ирина", MiddleName = "Юрьевна", LastName = "Михеева", Age = 38}
            };
        }
    }
}
