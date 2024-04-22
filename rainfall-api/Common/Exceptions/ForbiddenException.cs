using System.Net;

namespace rainfall_api.Common.Exceptions;

public class ForbiddenException : CustomException
{
    public ForbiddenException(string message)
        : base(message, null, null, HttpStatusCode.Forbidden)
    {
    }
}