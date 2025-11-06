using School.Db.Models;
using School.Web.PageModels.Students;

namespace School.Web.PageModels.Managements
{
    /// <summary>
    /// Руководство
    /// </summary>
    public class ManagementItemViewModel : ICloneable
    {
        private ManagementModel _item;
        public ManagementModel Item => _item;
        public ManagementItemViewModel(ManagementModel item)
        {
            _item = item;
        }

        /// <summary>
        /// id руководитея
        /// </summary>
        public int Id { get => _item.Id; set => _item.Id = value; }

        /// <summary>
        /// Должность
        /// </summary>
        public string Position  { get => _item.Position ; set => _item.Position  = value; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get => _item.FirstName; set => _item.FirstName = value; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get => _item.MiddleName; set => _item.MiddleName = value; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get => _item.LastName; set => _item.LastName = value; }

        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get => _item.Age; set => _item.Age = value; }

        public object Clone()
        {
            ManagementItemViewModel tempObject = (ManagementItemViewModel)MemberwiseClone();
            tempObject._item = (ManagementModel)_item.Clone();
            return tempObject;
        }
    }
}
