using Alfatraining.Ams.Common.DbRepository.Models;
using School.Db.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

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

            if (!string.IsNullOrEmpty(item.ChangeLogJson))
            {
                ChangeLogs = JsonSerializer.Deserialize<List<ChangeLogJson>>(item.ChangeLogJson);
            }
        }

        /// <summary>
        /// Id преподавателя
        /// </summary>
        public int Id { get => _item.Id; set => _item.Id = value; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, ErrorMessage = "Имя не должна превышать 50 символов")]
        public string FirstName { get => _item.FirstName; set => _item.FirstName = value; }

        /// <summary>
        /// Отчество
        /// </summary>
        [Required(ErrorMessage = "Отчество обязательно")]
        [StringLength(50, ErrorMessage = "Отчество не должно превышать 50 символов")]
        public string MiddleName { get => _item.MiddleName; set => _item.MiddleName = value; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов")]
        public string LastName { get => _item.LastName; set => _item.LastName = value; }

        /// <summary>
        /// Возраст
        /// </summary>
        [Range(20, 70, ErrorMessage = "Возраст должен быть от 20 до 70 лет")]
        public int Age { get => _item.Age; set => _item.Age = value; }

        /// <summary>
        /// Предмет
        /// </summary>
        [Required(ErrorMessage = "Предмет обязателен")]
        [StringLength(50, ErrorMessage = "Предмет не должен превышать 50 символов")]
        public string SubjectName { get => _item.SubjectName; set => _item.SubjectName = value; }

        public List<ChangeLogJson> ChangeLogs { get; set; }

        public object Clone()
        {
            TeacherItemViewModel tempObject = (TeacherItemViewModel)MemberwiseClone();
            tempObject._item = (TeacherModel)_item.Clone();
            return tempObject;
        }
    }
}
