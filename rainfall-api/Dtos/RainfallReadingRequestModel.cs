namespace rainfall_api.Dtos
{
    public class RainfallReadingRequestModel
    {
        public string StationId { get; set; }
        public int? Count { get; set; }
    }
}
