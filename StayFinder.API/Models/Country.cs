namespace StayFinder.API.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        //one to one relationship  between hotels and country
        public virtual IList<Hotel> Hotels { get; set; }
    }
}
