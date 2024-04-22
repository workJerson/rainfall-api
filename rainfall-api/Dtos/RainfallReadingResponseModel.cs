using Newtonsoft.Json;

namespace rainfall_api.Dtos
{
    /// <summary>
    /// Rainfall reading
    /// </summary>
    public class RainfallReadingResponseModel
    {
        /// <summary>
        /// Details of a rainfall reading
        /// </summary>
        public List<RainfallReading> Readings { get; set; }
    }
}
