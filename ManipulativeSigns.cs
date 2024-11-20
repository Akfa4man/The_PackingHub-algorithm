namespace The_PackingHub___algorithm
{
    internal class ManipulativeSigns
    {
        public bool Fragile { get; }
        public bool Chemically_active { get; }
        public bool Flammable { get; }

        public ManipulativeSigns(bool fragile, bool chemically_active, bool flammable)
        {
            Fragile = fragile;
            Chemically_active = chemically_active;
            Flammable = flammable;
        }
    }
}