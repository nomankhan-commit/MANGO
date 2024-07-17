using Microsoft.AspNetCore.Mvc;
using static Mango.web.Utility.SD;

namespace Mango.web.Models.Dto
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string url { get; set; } 
        public object Data { get; set; }
        public string AccessToken { get; set; }

    }
}
