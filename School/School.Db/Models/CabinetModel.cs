using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Db.Models
{
    [Table("Cabinets")]
    /// <summary>
    /// Кабинеты
    /// </summary>
    public class CabinetModel
    {
        [Key]
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

        [ForeignKey("TeacherId")]
        public virtual TeacherModel Teacher { get; set; }

        //public TeacherModel Teacher { get; set; }

        //public string TeacherFullName =>
        //    Teacher != null ? $"{Teacher.LastName} {Teacher.FirstName} {Teacher.MiddleName}" : "Не назначен";
    }
}
