using Alfatraining.Ams.Common.DbRepository.Models;
using School.Db.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace School.Web.PageModels.Students
{
    /// <summary>
    /// Ученик
    /// </summary>
    public class StudentItemViewModel : ICloneable
    {
        private StudentModel _item;
        public StudentModel Item => _item;
        public StudentItemViewModel(StudentModel item)
        {
            _item = item;

            if (!string.IsNullOrEmpty(item.ChangeLogJson))
            {
                ChangeLogs = JsonSerializer.Deserialize<List<ChangeLogJson>>(item.ChangeLogJson);
            }
        }

        /// <summary>
        /// Id ученика
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
        [Required(ErrorMessage = "Возраст обязателен")]
        [Range(6, 18, ErrorMessage = "Возраст должен быть от 6 до 18 лет")]
        public int Age { get => _item.Age; set => _item.Age = value; }

        /// <summary>
        /// Класс
        /// </summary>
        [Required(ErrorMessage = "Класс обязателен")]
        [Range(1, 11, ErrorMessage = "Класс должен быть от 1 до 11")]
        public int ClassId { get => _item.ClassId; set => _item.ClassId = value; }
        public bool IsDeleted { get => _item.IsDeleted; set => _item.IsDeleted = value; }
        public List<ChangeLogJson> ChangeLogs {get; set;}

        //public virtual ClassModel Class { get; set; }

        public object Clone()
        {
            StudentItemViewModel tempObject = (StudentItemViewModel)MemberwiseClone();
            tempObject._item = (StudentModel)_item.Clone();
            return tempObject;
        }
    }
}
