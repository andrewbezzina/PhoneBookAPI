namespace PhoneBookAPI.DataLayer.Models.Response
{
    public class DisplayCompany
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int NumberOfPeople { get; set; }
    }
}
