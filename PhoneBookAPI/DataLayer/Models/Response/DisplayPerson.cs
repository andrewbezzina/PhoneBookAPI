namespace PhoneBookAPI.DataLayer.Models.Response
{
    public class DisplayPerson
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
