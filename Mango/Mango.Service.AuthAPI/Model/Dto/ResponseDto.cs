using Microsoft.AspNetCore.Mvc;


namespace Mango.Services.AuthApi.Models.Dto
{
    public class ResponseDto
    {
        
        public object Result { get; set; }
        public bool Sussess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
