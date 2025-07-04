namespace Ecom.API.Helper
{
    public class ExceptionAPI : ResponseAPI
    {
        public ExceptionAPI(int stsusCode, string message = null,string details=null) : base(stsusCode, message)
        {
        }

        public string details { get; set; }
    }
}
