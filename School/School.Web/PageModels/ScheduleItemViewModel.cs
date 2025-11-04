using School.Web.PageModels.Teachers;

namespace School.Db.Models
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
        public DateTime Date { get => _item.Date; set => _item.Date = value; }

        /// <summary>
        /// Номер урока
        /// </summary>
        public int LessonNumber { get => _item.LessonNumber; set => _item.LessonNumber = value; }

        /// <summary>
        /// id преподавателя
        /// </summary>
        public int? TeacherId { get => _item.TeacherId; set => _item.TeacherId = value; }

        /// <summary>
        /// id кабинета
        /// </summary>
        public int? CabinetId { get => _item.CabinetId; set => _item.CabinetId = value; }

        /// <summary>
        /// id класса
        /// </summary>
        public int ClassId { get => _item.ClassId; set => _item.ClassId = value; }


        public virtual CabinetItemViewModel Cabinet { get; set; }
        public virtual ClassItemViewModel Class { get; set; }
        public virtual TeacherItemViewModel Teacher { get; set; }
    }
}
