using School.Web.PageModels.Students;

namespace School.Db.Models
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
        public int Number { get => _item.Number; set => _item.Number = value; }

        public virtual ICollection<StudentItemViewModel> Students { get; set; } = new List<StudentItemViewModel>();
        public virtual ICollection<ScheduleModel> Schedules { get; set; } = new List<ScheduleModel>();
    }
}
