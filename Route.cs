using System.Collections.Immutable;

namespace The_PackingHub___algorithm
{
    public class Route
    {
        public int Number { get; }
        public ImmutableArray<Address> Addresses { get; set; }
        public Route(int number, Address[] addresses)
        {
            Number=number;
            Addresses= ImmutableArray.Create(addresses);
        }
    }
}
