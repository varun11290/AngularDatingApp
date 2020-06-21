using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helper
{
    public static class ExtentionHelper
    {
        public static void AddApplicationError(this HttpResponse response,string massage){

            response.Headers.Add("Application-Error",massage);
            response.Headers.Add("Access-Control-Expose-Headers","Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");
        }
    }
}