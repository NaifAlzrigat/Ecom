namespace Ecom.API.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int stsusCode, string message=null)
        {
            StsusCode = stsusCode;
            Message = message?? GetmessageFromStsusCode(StsusCode);
        }

        private string GetmessageFromStsusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Done !",
                400=>"Bad Request !",
                401=>"UnAuthrize !",
                500=>"Server Error !",
                _=>null
            };
        }

        public int StsusCode { get; set; }
        public string Message { get; set; }
    }
}
