namespace The_PackingHub___algorithm
{
    public class Container
    {
        public float Length { get; }
        public float Width { get; }
        public float Height { get; }
        public float WallThickness { get; }
        public float Weight { get; }

        public float InnerLength => Length - 2 * WallThickness;
        public float InnerWidth => Width - 2 * WallThickness;
        public float InnerHeight => Height - 2 * WallThickness;

        private List<Tuple<Cargo, Vector3>> _packedCargos = new List<Tuple<Cargo, Vector3>>();

        public Container(float length, float width, float height, float wallThickness, float weight)
        {
            Length = length;
            Width = width;
            Height = height;
            WallThickness = wallThickness;
            Weight = weight;
        }

        public bool AddCargo(Cargo cargo, Vector3 position)
        {
            if (position.X + cargo.Length <= InnerLength &&
                position.Y + cargo.Width <= InnerWidth &&
                position.Z + cargo.Height <= InnerHeight &&
                !IsOverlap(cargo, position))
            {
                _packedCargos.Add(Tuple.Create(cargo, position));
                //Console.WriteLine($"Cargo {cargo.Length}x{cargo.Width}x{cargo.Height} added at ({position.X}, {position.Y}, {position.Z}).");
                return true;
            }
            //Console.WriteLine($"Cargo {cargo.Length}x{cargo.Width}x{cargo.Height} failed to add at ({position.X}, {position.Y}, {position.Z}).");
            return false;
        }

        private bool IsOverlap(Cargo cargo, Vector3 position)
        {
            foreach (var packedCargo in _packedCargos)
            {
                if (BoxesOverlap(cargo, position, packedCargo.Item1, packedCargo.Item2))
                {
                    return true;
                }
            }
            return false;
        }

        public bool BoxesOverlap(Cargo box1, Vector3 pos1, Cargo box2, Vector3 pos2)
        {
            return 
                pos1.X < pos2.X + box2.Length && pos1.X + box1.Length > pos2.X &&
                pos1.Y < pos2.Y + box2.Width && pos1.Y + box1.Width > pos2.Y &&
                pos1.Z < pos2.Z + box2.Height && pos1.Z + box1.Height > pos2.Z;
        }

        public List<Tuple<Cargo, Vector3>> GetPackedCargos => _packedCargos;
    }
}