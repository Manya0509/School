using System;

namespace Alfatraining.Ams.Common.DbRepository.Models
{
    public class ChangeLogJson
    {
        public string Operation { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
    }
}
