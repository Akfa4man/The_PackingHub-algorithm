using System.Diagnostics;

namespace The_PackingHub___algorithm
{
    public class Program
    {
        public static List<Address> addresses = new List<Address>
        {
            new Address("Россия", "Москва", "Тверская улица", "15", "Рядом с Красной площадью"),
            new Address("Россия", "Санкт-Петербург", "Невский проспект", "45", "Исторический центр города"),
            new Address("Россия", "Казань", "Улица Баумана", "7", "Пешеходная зона"),
            new Address("Россия", "Екатеринбург", "Проспект Ленина", "24", "Офисное здание"),
            new Address("Россия", "Новосибирск", "Красный проспект", "100", "Центр города"),
            new Address("Россия", "Сочи", "Курортный проспект", "50", "Близко к морю"),
            new Address("Россия", "Владивосток", "Светланская улица", "10", "С видом на бухту"),
            new Address("Россия", "Нижний Новгород", "Большая Покровская улица", "5", "Пешеходная улица"),
            new Address("Россия", "Ростов-на-Дону", "Пушкинская улица", "20", "Рядом с парком"),
            new Address("Россия", "Калининград", "Ленинский проспект", "8", "Близко к острову Канта")
        };
        public static void Main()
        {
            MainAsync();
        }
        public static async void MainAsync()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            Container container1 = new Container(5, 5, 1, 0, 100);
            Route route1 = new(1, [addresses[0], addresses[8]]);
            List<Cargo> cargos1 = new List<Cargo>
            {
                new Cargo(2, 1, 1, 50, new ManipulativeSigns(false, false, false), addresses[0]){ Name = "Cargo 1"},
                new Cargo(4, 2, 1, 50, new ManipulativeSigns(false, false, false), addresses[0]){ Name = "Cargo 2"},
                new Cargo(3, 2, 1, 50, new ManipulativeSigns(false, false, false), addresses[0]){ Name = "Cargo 3"},
                new Cargo(1, 1, 5, 50, new ManipulativeSigns(false, false, false), addresses[0]){ Name = "Cargo 4"},
                new Cargo(1, 1, 4, 10, new ManipulativeSigns(false, false, false), addresses[0]){ Name = "Cargo 5"}
            };

            BestFitPacker.Step = 1;

            container1 = BestFitPacker.Pack(container1, cargos1, route1);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
            ConsolePrint(container1);

            //stopwatch = new Stopwatch();

            //stopwatch.Start();
            //Container container2 = new Container(3, 3, 1, 0, 100);
            //List<Cargo> cargos2 = new List<Cargo>
            //{
            //    new Cargo(1, 1.5f, 1, 10, new ManipulativeSigns(true, false, false)){ Name = "Cargo 1"},
            //    new Cargo(1, 1.5f, 1, 10, new ManipulativeSigns(false, true, false)){ Name = "Cargo 2"},
            //    new Cargo(1, 1.5f, 1, 10, new ManipulativeSigns(false, false, true)){ Name = "Cargo 3"},
            //    new Cargo(1, 1.5f, 1, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 4"},
            //    new Cargo(0.5f, 1.5f, 1, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 5"},
            //    new Cargo(0.5f, 1.5f, 1, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 6"},
            //    new Cargo(1, 1, 1, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 7"},
            //    new Cargo(1, 0.5f, 1, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 8"}
            //};

            //BestFitPacker.Step = 0.5f;

            //container2 = BestFitPacker.Pack(container2, cargos2);
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.Elapsed);
            //ConsolePrint(container2);

            //stopwatch = new Stopwatch();

            //stopwatch.Start();
            //Container container3 = new Container(100, 100, 100, 0, 100);
            //List<Cargo> cargos3 = new List<Cargo>
            //{
            //    new Cargo(60, 87, 25, 10, new ManipulativeSigns(true, false, false)){ Name = "Cargo 1"},
            //    new Cargo(41, 63, 54, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 2"},
            //    new Cargo(52, 68, 88, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 3"},
            //    new Cargo(66, 76, 31, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 4"},
            //};

            //BestFitPacker.Step = 1f;

            //container3 = BestFitPacker.Pack(container3, cargos3);
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.Elapsed);
            //ConsolePrint(container3);

            //stopwatch = new Stopwatch();

            //stopwatch.Start();
            //Container container4 = new Container(50, 50, 50, 0, 1000);
            //List<Cargo> cargos4 = new List<Cargo>
            //{
            //    new Cargo(10, 10, 10, 200, new ManipulativeSigns(true, false, true)) { Name = "Cargo 1" },
            //    new Cargo(20, 15, 10, 300, new ManipulativeSigns(false, true, false)) { Name = "Cargo 2" },
            //    new Cargo(25, 25, 25, 500, new ManipulativeSigns(false, false, false)) { Name = "Cargo 3" },
            //    new Cargo(10, 40, 10, 400, new ManipulativeSigns(true, true, false)) { Name = "Cargo 4" },
            //    new Cargo(15, 15, 15, 150, new ManipulativeSigns(false, false, true)) { Name = "Cargo 5" },
            //    new Cargo(5, 5, 5, 50, new ManipulativeSigns(false, false, false)) { Name = "Cargo 6" },
            //    new Cargo(30, 30, 30, 600, new ManipulativeSigns(true, false, false)) { Name = "Cargo 7" },
            //    new Cargo(40, 10, 5, 100, new ManipulativeSigns(false, true, true)) { Name = "Cargo 8" },
            //    new Cargo(35, 35, 10, 250, new ManipulativeSigns(false, false, false)) { Name = "Cargo 9" },
            //    new Cargo(5, 5, 40, 80, new ManipulativeSigns(false, false, false)) { Name = "Cargo 10" },
            //    new Cargo(25, 20, 20, 400, new ManipulativeSigns(true, true, true)) { Name = "Cargo 11" },
            //    new Cargo(10, 10, 10, 200, new ManipulativeSigns(false, false, false)) { Name = "Cargo 12" },
            //    new Cargo(50, 5, 5, 150, new ManipulativeSigns(false, false, false)) { Name = "Cargo 13" },
            //    new Cargo(40, 40, 40, 700, new ManipulativeSigns(true, false, true)) { Name = "Cargo 14" },
            //    new Cargo(15, 15, 15, 200, new ManipulativeSigns(false, false, false)) { Name = "Cargo 15" },
            //};

            //BestFitPacker.Step = 1f;

            //container4 = BestFitPacker.Pack(container4, cargos4);
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.Elapsed);
            //ConsolePrint(container4);

            //stopwatch = new Stopwatch();

            //stopwatch.Start();
            //Container container5 = new Container(100, 100, 100, 0, 5000);
            //List<Cargo> cargos5 = new List<Cargo>
            //{
            //    new Cargo(50, 50, 50, 2000, new ManipulativeSigns(false, false, false)) { Name = "Cargo 1" },
            //    new Cargo(40, 40, 10, 500, new ManipulativeSigns(false, true, false)) { Name = "Cargo 2" },
            //    new Cargo(30, 20, 25, 700, new ManipulativeSigns(true, false, false)) { Name = "Cargo 3" },
            //    new Cargo(20, 20, 20, 400, new ManipulativeSigns(false, false, true)) { Name = "Cargo 4" },
            //    new Cargo(15, 15, 15, 200, new ManipulativeSigns(true, false, true)) { Name = "Cargo 6" },
            //    new Cargo(25, 25, 5, 300, new ManipulativeSigns(false, false, false)) { Name = "Cargo 7" },
            //    new Cargo(30, 30, 10, 600, new ManipulativeSigns(false, false, false)) { Name = "Cargo 8" },
            //    new Cargo(20, 10, 10, 150, new ManipulativeSigns(false, true, false)) { Name = "Cargo 9" },
            //    new Cargo(10, 10, 50, 400, new ManipulativeSigns(true, false, false)) { Name = "Cargo 10" },
            //};

            //BestFitPacker.Step = 5f;

            //container5 = BestFitPacker.Pack(container5, cargos5);
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.Elapsed);
            //ConsolePrint(container5);
        }
        private static void ConsolePrint(Container container)
        {
            foreach (var packedCargo in container.GetPackedCargos)
            {
                var cargo = packedCargo.Item1;
                var position = packedCargo.Item2;

                var startCoordinates = position;
                var endCoordinates = new Vector3(
                    position.X + cargo.Length,
                    position.Y + cargo.Width,
                    position.Z + cargo.Height
                );

                Console.Write(cargo.Name);
                Console.WriteLine($" {cargo.Length}x{cargo.Width}x{cargo.Height} placed:");
                Console.WriteLine($"  Start: ({startCoordinates.X}, {startCoordinates.Y}, {startCoordinates.Z})");
                Console.WriteLine($"  End: ({endCoordinates.X}, {endCoordinates.Y}, {endCoordinates.Z})");

            }
            Console.WriteLine();
        }
    }
}