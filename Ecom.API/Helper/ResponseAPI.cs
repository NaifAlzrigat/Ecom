using System.Net;

namespace Ecom.API.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int stsusCode, string message=null)
        {
            StsusCode = stsusCode;
            Message = message?? GetMessageFromStatusCode(StsusCode);
        }

        //private string GetmessageFromStsusCode(int statusCode)
        //{
        //    return statusCode switch
        //    {
        //        200 => "Done !",
        //        400=>"Bad Request !",
        //        401=>"UnAuthrize !",
        //        404=>"resorse not found",
        //        500=>"Server Error !",
        //        _=>null
        //    };
        //}
        private static string GetMessageFromStatusCode(int statusCode)
        {
            if (Enum.IsDefined(typeof(HttpStatusCode), statusCode))
            {
                return ((HttpStatusCode)statusCode).ToString();
            }
            else
            {
                // Handle custom or undefined status codes
                return statusCode switch
                {
                    418 => "I'm a Teapot", // Example of a non-standard code
                    429 => "Too Many Requests",
                    503 => "Service Unavailable",
                    _ => "Unknown Status Code"
                };
            }
        }

        public int StsusCode { get; set; }
        public string Message { get; set; }
    }
}
