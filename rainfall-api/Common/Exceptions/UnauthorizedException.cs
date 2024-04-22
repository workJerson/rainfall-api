using System.Net;

namespace rainfall_api.Common.Exceptions;

public class UnauthorizedException : CustomException
{
    public UnauthorizedException(string message)
       : base(message, null, null, HttpStatusCode.Unauthorized)
    {
    }
}