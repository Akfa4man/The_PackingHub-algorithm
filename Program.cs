using System.Diagnostics;

namespace The_PackingHub___algorithm
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            Container container1 = new Container(5, 5, 1, 0, 100);
            List<Cargo> cargos1 = new List<Cargo>
            {
                new Cargo(2, 1, 1, 50, new ManipulativeSigns(false, false, false)){ Name = "Cargo 1"},
                new Cargo(4, 2, 1, 50, new ManipulativeSigns(false, false, false)){ Name = "Cargo 2"},
                new Cargo(3, 2, 1, 50, new ManipulativeSigns(false, false, false)){ Name = "Cargo 3"},
                new Cargo(1, 1, 5, 50, new ManipulativeSigns(false, false, false)){ Name = "Cargo 4"},
                new Cargo(1, 1, 4, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 5"}
            };

            container1 = BestFitPacker.Pack(container1, cargos1);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
            ConsolePrint(container1);

            stopwatch = new Stopwatch();

            stopwatch.Start();
            Container container2 = new Container(3, 3, 1, 0, 100);
            List<Cargo> cargos2 = new List<Cargo>
            {
                new Cargo(1, 1.5f, 1, 10, new ManipulativeSigns(true, false, false)){ Name = "Cargo 1"},
                new Cargo(1, 1.5f, 1, 10, new ManipulativeSigns(false, true, false)){ Name = "Cargo 2"},
                new Cargo(1, 1.5f, 1, 10, new ManipulativeSigns(false, false, true)){ Name = "Cargo 3"},
                new Cargo(1, 1.5f, 1, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 4"},
                new Cargo(0.5f, 1.5f, 1, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 5"},
                new Cargo(0.5f, 1.5f, 1, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 6"},
                new Cargo(1, 1, 1, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 7"},
                new Cargo(1, 0.5f, 1, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 8"}
            };

            container2 = BestFitPacker.Pack(container2, cargos2);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
            ConsolePrint(container2);

            Container container3 = new Container(100, 100, 100, 0, 100);
            List<Cargo> cargos3 = new List<Cargo>
            {
                new Cargo(60, 87, 25, 10, new ManipulativeSigns(true, false, false)){ Name = "Cargo 1"},
                new Cargo(41, 63, 54, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 2"},
                new Cargo(52, 68, 88, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 3"},
                new Cargo(66, 76, 31, 10, new ManipulativeSigns(false, false, false)){ Name = "Cargo 4"},
            };

            container3 = BestFitPacker.Pack(container3, cargos3);
            ConsolePrint(container3);
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