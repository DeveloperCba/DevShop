using System.Net.Http.Headers;

namespace Core.Test.Extensions
{
    public static class TestsExtensions
    {
        public static decimal OnlyNumber(this string value)
        {
            return Convert.ToDecimal(new string(value.Where(char.IsDigit).ToArray()));
        }

        public static void SetToken(this HttpClient client, string token)
        {
            client.SetJsonMediaType();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static void SetJsonMediaType(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}