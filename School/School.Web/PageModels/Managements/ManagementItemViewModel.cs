using School.Db.Models;
using System.ComponentModel.DataAnnotations;

namespace School.Web.PageModels.Managements
{
    /// <summary>
    /// Руководство
    /// </summary>
    public class ManagementItemViewModel : ICloneable
    {
        private ManagementModel _item;
        public ManagementModel Item => _item;
        public ManagementItemViewModel(ManagementModel item)
        {
            _item = item;
        }

        /// <summary>
        /// id руководитея
        /// </summary>
        public int Id { get => _item.Id; set => _item.Id = value; }

        /// <summary>
        /// Должность
        /// </summary>
        [Required(ErrorMessage = "Должность обязательна")]
        [StringLength(50, ErrorMessage = "Должность не должна превышать 50 символов")]
        public string Position { get => _item.Position; set => _item.Position = value; }

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

        public object Clone()
        {
            ManagementItemViewModel tempObject = (ManagementItemViewModel)MemberwiseClone();
            tempObject._item = (ManagementModel)_item.Clone();
            return tempObject;
        }
    }
}
