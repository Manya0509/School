using Alfatraining.Ams.Common.DbRepository.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Db.Models
{
    [Table("Cabinets")]
    /// <summary>
    /// Кабинеты
    /// </summary>
    public class CabinetModel : ICloneable, IRowVersion, IEntity
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

        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ChangeLogJson { get; set; }
        public object Clone()
        {
            CabinetModel tempObject = (CabinetModel)MemberwiseClone();
            return tempObject;
        }

        //public TeacherModel Teacher { get; set; }

        //public string TeacherFullName =>
        //    Teacher != null ? $"{Teacher.LastName} {Teacher.FirstName} {Teacher.MiddleName}" : "Не назначен";
    }
}
