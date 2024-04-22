using Newtonsoft.Json;

namespace rainfall_api.Dtos
{
    public class RainfallReadingResponseModel
    {
        public List<RainfallReading> Readings { get; set; }
    }
}
