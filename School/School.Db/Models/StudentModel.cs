namespace School.Db.Models
{
    /// <summary>
    /// Ученик
    /// </summary>
    public class StudentModel
    {
        /// <summary>
        /// Id ученика
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
        /// Класс
        /// </summary>
        public int ClassId { get; set; }
        public virtual ClassModel Class { get; set; }
    }
}
