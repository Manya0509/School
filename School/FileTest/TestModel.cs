using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FileTest
{
    internal class TestModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        [JsonIgnore]
        public DateTime DateTime { get; set; }
        public double Number { get; set; }
        public List<TestList> TestLists { get; set; }

    }
    public class TestList
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
