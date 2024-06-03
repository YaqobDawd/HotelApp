namespace HotelAppC.Model
{
    public class HomeVM
    {
        public DateTime StartDate { get; set; }=DateTime.Now;
        public DateTime EndDate { get; set; }
        public int NoOfNight { get; set; } = 1;
    }
}
