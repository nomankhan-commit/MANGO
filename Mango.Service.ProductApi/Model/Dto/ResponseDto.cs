using Microsoft.AspNetCore.Mvc;


namespace Mango.Service.ProductApi.Model.Dto
{
    public class ResponseDto
    {
        
        public object Result { get; set; }
        public bool Sussess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
