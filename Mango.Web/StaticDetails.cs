namespace Mango.Web
{
    public static class StaticDetails
    {
        public static string ProductApiBase { get; set; }
        public static string ShoppingCartApiBase { get; set; }
        public static string CouponApiBase { get; set; }

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
