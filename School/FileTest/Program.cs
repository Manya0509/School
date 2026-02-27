using System.ComponentModel.Design;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace FileTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Test1();  //сначала записыват файл
            await Test2();   //потом читает файл
        }

        static TestModel InitModel()  //создаем модель
        {
            var test = new TestModel(); //возвращаем TestModel
            test.Name = "Test"; //создаем пустой объект
            test.Age = 1;
            test.DateTime = DateTime.Now;
            test.Number = 1.23;

            test.TestLists = InitList(); //Заполняем список вложенных объектов

            return test;

            
        }

        //static async Task Test1()
        //{
        //    var test1 = InitModel();

        //    using (FileStream fs = new FileStream("C:\\Users\\Дарья\\Desktop\\School\\file.json", FileMode.OpenOrCreate)) 
        //    {
        //        var json = JsonSerializer.Serialize(test1);
        //        var buffer = Encoding.Default.GetBytes(json);
        //        await fs.WriteAsync(buffer, 0, buffer.Length);
        //    }
        //}

        static async Task Test1()
        {
            Console.WriteLine("Запись файла!");

            var test1 = InitModel();  //Получаем готовый объект

            using (FileStream fs = new FileStream("C:\\Users\\Дарья\\Desktop\\School\\file.json", FileMode.Create))  //Открываем файл,
                                                                                                                     //Create -> перезаписывает файл полностью,
                                                                                                                     //using -> файл гарантированно закроется
            {
                var json = JsonSerializer.Serialize(test1, PrettyOptions); //(объект -> Json) Используем правила PrettyOptions
                var buffer = Encoding.Default.GetBytes(json); //Файл принимает байты -> переводим
                await fs.WriteAsync(buffer, 0, buffer.Length); //Записываем байты в файл
                Console.WriteLine("Файл записан!");
            }
        }

        static async Task Test2()
        {
            using (FileStream fstream = File.OpenRead("C:\\Users\\Дарья\\Desktop\\School\\file.json")) //Открываем файл только для чтения
            {
                byte[] buffer = new byte[fstream.Length]; //Создаём массив байт нужного размера
                await fstream.ReadAsync(buffer, 0, buffer.Length); //Читаем файл целиком
                string textFromFile = Encoding.Default.GetString(buffer); //Байты -> строка (JSON)
                Console.WriteLine($"Текст из файла:\n{textFromFile}");

                var j = JsonSerializer.Deserialize<TestModel>(textFromFile, PrettyOptions); //JSON -> TestModel

            }
        }

                //static async void Test2()
                //{
                //    using (FileStream fstream = File.OpenRead("C:\\Users\\Дарья\\Desktop\\School\\file.json"))
                //    {
                //        // выделяем массив для считывания данных из файла
                //        byte[] buffer = new byte[fstream.Length];
                //        // считываем данные
                //        await fstream.ReadAsync(buffer, 0, buffer.Length);
                //        // декодируем байты в строку
                //        string textFromFile = Encoding.Default.GetString(buffer);
                //        Console.WriteLine($"Текст из файла: {textFromFile}");

                //        var j = JsonSerializer.Deserialize<TestModel>(textFromFile);
                //    }
                //}

        static List<TestList> InitList()
        {
            var test2 = new TestList();
            test2.Id = 1;
            test2.Name = "Test2";
            test2.Age = 2;
            var test3 = new TestList();
            test3.Id = 2;
            test3.Name = "Test3";
            test3.Age = 3;
            return new List<TestList> { test2, test3 };
        }

        private static readonly JsonSerializerOptions PrettyOptions = new()
        {
            WriteIndented = true, //Красивый JSON с отступами
            PropertyNamingPolicy = null, //Имена свойств как в C#
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), //Без этого будут \u0422\u0435\u0441\u0442
            AllowTrailingCommas = true //если есть лишняя запятая
        };

        //static readonly JsonSerializerOptions PrettyOptions = new()
        //{
        //    WriteIndented = true,
        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //    PropertyNameCaseInsensitive = true,
        //    NumberHandling = JsonNumberHandling.AllowReadingFromString
        //};

        //AllowDuplicateProperties = true;  - если написали одно и тоже имя 2 раза
        //options.Converters.Add(new DateTimeConverter()); - переводчик для сложных тем (DateTime, enum, своих классов)
        //JsonSerializerOptions.Default - готовые обычные правила
        //JsonSerializerOptions.Web - готовые веб-настройки
        //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; - если значение null(не писать)
        //NewLine = "\n"; - перенос строки
    }
}
