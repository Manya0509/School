using School.Db.Models;
using School.Web.PageModels.Cabinets;
using School.Web.PageModels.Classes;
using School.Web.PageModels.Teachers;
using System.ComponentModel.DataAnnotations;

namespace School.Web.PageModels.Schedule
{
    /// <summary>
    /// Расписание занятий
    /// </summary>
    public class ScheduleItemViewModel
    {
        private ScheduleModel _item;
        public ScheduleModel Item => _item;
        public ScheduleItemViewModel(ScheduleModel item)
        {
            _item = item;
        }

        /// <summary>
        /// id расписания занятий
        /// </summary>
        public int Id { get => _item.Id; set => _item.Id = value; }

        /// <summary>
        /// Дата
        /// </summary>
        [Required(ErrorMessage = "Дата обязательна")]
        [DataType(DataType.Date, ErrorMessage = "Неверный формат даты")]
        public DateTime Date { get => _item.Date; set => _item.Date = value; }

        /// <summary>
        /// Номер урока
        /// </summary>
        [Required(ErrorMessage = "Номер урока обязателен")]
        [Range(1, 10, ErrorMessage = "Номер урока должен быть от 1 до 10")]
        public int LessonNumber { get => _item.LessonNumber; set => _item.LessonNumber = value; }

        /// <summary>
        /// id преподавателя
        /// </summary>
        [Required(ErrorMessage = "Преподаватель обязателен")]
        public int? TeacherId { get => _item.TeacherId; set => _item.TeacherId = value; }

        /// <summary>
        /// id кабинета
        /// </summary>
        [Required(ErrorMessage = "Кабинет обязателен")]
        public int? CabinetId { get => _item.CabinetId; set => _item.CabinetId = value; }

        /// <summary>
        /// id класса
        /// </summary>
        [Required(ErrorMessage = "Класс обязателен")]
        public int ClassId { get => _item.ClassId; set => _item.ClassId = value; }


        public virtual CabinetItemViewModel Cabinet { get; set; }
        public virtual ClassItemViewModel Class { get; set; }
        public virtual TeacherItemViewModel Teacher { get; set; }
    }
}
