namespace School.Db.Models
{
    /// <summary>
    /// Преподаватель
    /// </summary>
    public class TeacherModel
    {
        /// <summary>
        /// Id преподавателя
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Предмет
        /// </summary>
        public string SubjectName { get; set; }
    }
}
