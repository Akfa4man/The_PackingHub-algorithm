namespace The_PackingHub___algorithm
{
    public class Address
    {
        public string Name { get; set; }
        public string Сountry { get; }
        public string City { get; }
        public string Street { get; }
        public string House { get; }
        public string Comments { get; set; }
        public Address(string сountry, string city, string street, string house, string comments=null)
        {
            Сountry = сountry;
            City = city;
            Street = street;
            House = house;
            Comments = comments;
        }
    }
}
