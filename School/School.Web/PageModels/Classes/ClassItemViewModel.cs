using School.Db.Models;
using School.Web.PageModels.Students;
using System.ComponentModel.DataAnnotations;

namespace School.Web.PageModels.Classes
{
    /// <summary>
    /// Класс
    /// </summary>
    public class ClassItemViewModel
    {
        private ClassModel _item;
        public ClassModel Item => _item;
        public ClassItemViewModel(ClassModel item)
        {
            _item = item;
        }

        /// <summary>
        /// id класса
        /// </summary>
        public int Id { get => _item.Id; set => _item.Id = value; }

        /// <summary>
        /// Номер класса
        /// </summary>
        [Range(1, 5, ErrorMessage = "Номер класса должен быть от 1 до 5")]
        public int Number { get => _item.Number; set => _item.Number = value; }

        public virtual ICollection<StudentItemViewModel> Students { get; set; } = new List<StudentItemViewModel>();
        public virtual ICollection<ScheduleModel> Schedules { get; set; } = new List<ScheduleModel>();
    }
}
