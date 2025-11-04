using School.Db.Models;

namespace School.Web.PageModels.Teachers
{
    /// <summary>
    /// Преподаватель
    /// </summary>
    public class TeacherItemViewModel : ICloneable
    {
        private TeacherModel _item;
        public TeacherModel Item => _item;
        public TeacherItemViewModel(TeacherModel item)
        {
            _item = item;
        }

        /// <summary>
        /// Id преподавателя
        /// </summary>
        public int Id { get => _item.Id; set => _item.Id = value; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get => _item.FirstName; set => _item.FirstName = value; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get => _item.MiddleName; set => _item.MiddleName = value; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get => _item.LastName; set => _item.LastName = value; }

        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get => _item.Age; set => _item.Age = value; }

        /// <summary>
        /// Предмет
        /// </summary>
        public string SubjectName { get => _item.SubjectName; set => _item.SubjectName = value; }

        public object Clone()
        {
            TeacherItemViewModel tempObject = (TeacherItemViewModel)MemberwiseClone();
            tempObject._item = (TeacherModel)_item.Clone();
            return tempObject;
        }
    }
}
