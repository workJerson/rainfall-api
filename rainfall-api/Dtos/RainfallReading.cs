namespace rainfall_api.Dtos
{
    /// <summary>
    /// Details of a rainfall reading
    /// </summary>
    public class RainfallReading
    {
        public DateTime DateMeasured { get; set; }
        public decimal AmountMeasured { get; set; }
    }
}
