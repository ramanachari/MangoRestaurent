using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDto responseModel { get; set; }
        public IHttpClientFactory HttpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new ResponseDto();
            this.HttpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = HttpClient.CreateClient("MangoAPI");
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Headers.Add("Accept", "application/json");
                httpRequestMessage.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();

                if (apiRequest.Data != null)
                {
                    httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), 
                        Encoding.UTF8, "application/json");
                }

                HttpResponseMessage responseMessage = null;

                switch (apiRequest.ApiType)
                {
                    case StaticDetails.ApiType.GET:
                        httpRequestMessage.Method = HttpMethod.Get;
                        break;
                    case StaticDetails.ApiType.POST:
                        httpRequestMessage.Method = HttpMethod.Post;
                        break;
                    case StaticDetails.ApiType.PUT:
                        httpRequestMessage.Method = HttpMethod.Put;
                        break;
                    case StaticDetails.ApiType.DELETE:
                        httpRequestMessage.Method = HttpMethod.Delete;
                        break;
                    default:
                        httpRequestMessage.Method = HttpMethod.Get;
                        break;
                }

                responseMessage = await client.SendAsync(httpRequestMessage);

                var apiContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<T>(apiContent);

                return response;
                
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto()
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string>() { ex.Message },
                    IsSuccess = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(res);

                return apiResponseDto;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
