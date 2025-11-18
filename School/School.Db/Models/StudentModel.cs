using Alfatraining.Ams.Common.DbRepository.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Db.Models
{
    [Table("Students")]

    /// <summary>
    /// Ученик
    /// </summary>
    public class StudentModel : ICloneable, IRowVersion, IEntity
    {
        [Key]

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
        //public virtual ClassModel Class { get; set; }

        [ForeignKey("ClassId")]
        public virtual ClassModel Class { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ChangeLogJson { get; set; }

        public object Clone()
        {
            StudentModel tempObject = (StudentModel)MemberwiseClone();
            return tempObject;
        }
    }
}
