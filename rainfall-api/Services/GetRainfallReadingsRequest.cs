using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using rainfall_api.Common.Exceptions;
using rainfall_api.Dtos;
using rainfall_api.Dtos.RainfallApi;
using rainfall_api.Validators;
using System.Net;

namespace rainfall_api.Services
{
    public class GetRainfallReadingsRequest : IRequest<RainfallReadingResponseModel>
    {
        public string StationId { get; set; }
        public int? Count { get; set; }

        public GetRainfallReadingsRequest(string stationId, int? count) => (StationId, Count) = (stationId, count);
    }
    public class GetRainfallReadingsRequestHandler : IRequestHandler<GetRainfallReadingsRequest, RainfallReadingResponseModel>
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public GetRainfallReadingsRequestHandler(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            httpClient = httpClientFactory.CreateClient("EnvironmentAgencyHttpClient");
            this.mapper = mapper;
        }

        public async Task<RainfallReadingResponseModel> Handle(GetRainfallReadingsRequest request, CancellationToken cancellationToken)
        {
            var validationResult = new GetRainfallReadingsValidator().Validate(request);
            if (!(validationResult.IsValid))
                throw new ValidationException("Validation errors.", validationResult.Errors.Select(error => new ErrorDetailModel { Message = error.ErrorMessage, PropertyName = error.PropertyName }).ToList());

            var queryParameters = new Dictionary<string, string>()
            {
                { "_limit", request.Count.ToString()! } 
            };

            var uri = QueryHelpers.AddQueryString($"https://environment.data.gov.uk/flood-monitoring/id/stations/{request.StationId}/readings?_sorted", queryParameters!);

            var result = await httpClient.GetAsync(uri, cancellationToken).Result.Content.ReadAsStringAsync(cancellationToken);
            var readings = JsonConvert.DeserializeObject<GetReadingsByStationIdDto>(result);

            return mapper.Map<RainfallReadingResponseModel>(readings);
        }
    }
}
