namespace School.Db.Models
{
    /// <summary>
    /// Расписание занятий
    /// </summary>
    public class ScheduleModel
    {
        /// <summary>
        /// id расписания занятий
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Номер урока
        /// </summary>
        public int LessonNumber { get; set; }

        /// <summary>
        /// id преподавателя
        /// </summary>
        public int? TeacherId { get; set; }

        /// <summary>
        /// id кабинета
        /// </summary>
        public int? CabinetId { get; set; }

        /// <summary>
        /// id класса
        /// </summary>
        public int ClassId { get; set; }
        
    }
}
