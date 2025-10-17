namespace School.Db.Models
{
    /// <summary>
    /// Класс
    /// </summary>
    public class ClassModel
    {
        /// <summary>
        /// id класса
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Номер класса
        /// </summary>
        public int Number { get; set; }

        public virtual ICollection<StudentModel> Students { get; set; } = new List<StudentModel>();
        public virtual ICollection<ScheduleModel> Schedules { get; set; } = new List<ScheduleModel>();
    }
}
