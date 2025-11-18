using School.Db.Models;
using School.Web.PageModels.Managements;
using System.ComponentModel.DataAnnotations;

namespace School.Web.PageModels.Cabinets
{
    /// <summary>
    /// Кабинеты
    /// </summary>
    public class CabinetItemViewModel : ICloneable
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
        [Required(ErrorMessage = "Номер кабинета обязателен")]
        [Range(100, 500, ErrorMessage = "Номер кабинета должен быть от 100 до 500")]
        public int Number { get => _item.Number; set => _item.Number = value; }

        /// <summary>
        /// id предмета
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Выберите преподавателя")]
        public int? TeacherId { get => _item.TeacherId; set => _item.TeacherId = value; }

        public string TeacherFullName { get; set; }
        public TeacherModel Teacher { get; set; }

        public object Clone()
        {
            CabinetItemViewModel tempObject = (CabinetItemViewModel)MemberwiseClone();
            tempObject._item = (CabinetModel)_item.Clone();
            return tempObject;
        }
    }
}
