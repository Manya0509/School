using Alfatraining.Ams.Common.DbRepository.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Db.Models
{
    [Table("Classes")]
    
    /// <summary>
    /// Класс
    /// </summary>
    public class ClassModel
    {
        [Key]

        /// <summary>
        /// id класса
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Номер класса
        /// </summary>
        public int Number { get; set; }


        public virtual ICollection<StudentModel> Students { get; set; } = new List<StudentModel>();
        //public virtual ICollection<ScheduleModel> Schedules { get; set; } = new List<ScheduleModel>();
    }
}
