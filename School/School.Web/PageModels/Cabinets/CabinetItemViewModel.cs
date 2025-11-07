using School.Db.Models;

namespace School.Web.PageModels.Cabinets
{
    /// <summary>
    /// Кабинеты
    /// </summary>
    public class CabinetItemViewModel
    {
        private CabinetModel _item;
        
        public CabinetModel Item => _item;

        public CabinetItemViewModel(CabinetModel item)
        {
            _item = item;
        }

        /// <summary>
        /// id кабинета
        /// </summary>
        public int Id { get => _item.Id; set => _item.Id = value; }

        /// <summary>
        /// Номер
        /// </summary>
        public int Number { get => _item.Number; set => _item.Number = value; }

        /// <summary>
        /// id предмета
        /// </summary>
        public int? TeacherId { get => _item.TeacherId; set => _item.TeacherId = value; }

        public string TeacherFullName { get; set; }
        public TeacherModel Teacher { get; set; }
    }
}
