using rainfall_api.Dtos;
using System.Net;

namespace rainfall_api.Common.Exceptions
{
    public class ValidationException : CustomException
    {
        public ValidationException(string message, List<ErrorDetailModel>? details = null) : base(message, null, details, HttpStatusCode.BadRequest)
        {

        }
    }
}
