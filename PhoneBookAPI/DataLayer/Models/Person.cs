namespace PhoneBookAPI.DataLayer.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public long? CompanyId { get; set; }
    }
}
