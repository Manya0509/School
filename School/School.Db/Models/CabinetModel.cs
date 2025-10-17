namespace School.Db.Models
{
    /// <summary>
    /// Кабинеты
    /// </summary>
    public class CabinetModel
    {
        /// <summary>
        /// id кабинета
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// id предмета
        /// </summary>
        public int? TeacherId { get; set; }
        public TeacherModel Teacher { get; set; }

        public string TeacherFullName =>
            Teacher != null ? $"{Teacher.LastName} {Teacher.FirstName} {Teacher.MiddleName}" : "Не назначен";
    }
}
