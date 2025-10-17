namespace School.Db.Models
{
    /// <summary>
    /// Руководство
    /// </summary>
    public class ManagementModel
    {
        /// <summary>
        /// id руководитея
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string Function { get; set; }

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
    }
}
