namespace PhoneBookAPI.DataLayer.Models.Request
{
    public class PersonDetails
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int CompanyId { get; set; }
    }
}
