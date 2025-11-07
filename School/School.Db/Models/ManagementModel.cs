using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Db.Models
{
    [Table("Managements")]

    /// <summary>
    /// Руководство
    /// </summary>
    public class ManagementModel : ICloneable
    {
        [Key]
        /// <summary>
        /// id руководитея
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string Position { get; set; }

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

        public object Clone()
        {
            ManagementModel tempObject = (ManagementModel)MemberwiseClone();
            return tempObject;
        }
    }
}
