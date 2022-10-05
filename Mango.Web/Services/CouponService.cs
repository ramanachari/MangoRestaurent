using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class CouponService : BaseService, ICouponService
    {
        public CouponService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {

        }
        public async Task<T> GetCouponAsync<T>(string couponCode, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponApiBase + "api/coupon/" + couponCode,
                AccessToken = token
            });
        }
    }
}
