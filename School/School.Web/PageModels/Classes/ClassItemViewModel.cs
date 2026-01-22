using Alfatraining.Ams.Common.DbRepository.Models;
using School.Db.Models;
using School.Web.PageModels.Students;
using School.Web.PageModels.Teachers;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace School.Web.PageModels.Classes
{
    /// <summary>
    /// Класс
    /// </summary>
    public class ClassItemViewModel : ICloneable
    {
        private ClassModel _item;
        public ClassModel Item => _item;
        public ClassItemViewModel(ClassModel item)
        {
            _item = item;

            if (!string.IsNullOrEmpty(item.ChangeLogJson))
            {
                ChangeLogs = JsonSerializer.Deserialize<List<ChangeLogJson>>(item.ChangeLogJson);
            }
        }

        /// <summary>
        /// id класса
        /// </summary>
        public int Id { get => _item.Id; set => _item.Id = value; }

        /// <summary>
        /// Номер класса
        /// </summary>
        [Range(1, 11, ErrorMessage = "Номер класса должен быть от 1 до 11")]
        public int Number { get => _item.Number; set => _item.Number = value; }

        public bool IsStudent {get; set; }
        public bool IsDeleted { get => _item.IsDeleted; set => _item.IsDeleted = value; }

        public List<ChangeLogJson> ChangeLogs { get; set; }

        public object Clone()
        {
            ClassItemViewModel tempObject = (ClassItemViewModel)MemberwiseClone();
            tempObject._item = (ClassModel)_item.Clone();
            return tempObject;
        }


        //public virtual ICollection<StudentItemViewModel> Students { get; set; } = new List<StudentItemViewModel>();
        //public virtual ICollection<ScheduleModel> Schedules { get; set; } = new List<ScheduleModel>();
    }
}
