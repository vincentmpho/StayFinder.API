using System.ComponentModel.DataAnnotations.Schema;

namespace StayFinder.API.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int Address { get; set; }
        public int Rating { get; set; }

        [ForeignKey(nameof(CountryId))]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
