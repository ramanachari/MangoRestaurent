using Mango.Services.ShoppingCartAPI.Models.Dto;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient client;

        public CouponRepository(HttpClient client)
        {
            this.client = client;
        }

        public async Task<CouponDto> GetCoupon(string couponName)
        {
            var response = await client.GetAsync($"/api/coupon/{couponName}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var rsp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (rsp != null && rsp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(rsp.Result));
            }

            return new CouponDto();
        }
    }
}
