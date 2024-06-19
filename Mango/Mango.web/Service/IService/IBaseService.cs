using Mango.web.Models.Dto;

namespace Mango.web.Service.IService
{
    public interface IBaseService
    {

       Task<ResponseDto?>SendAsync(RequestDto responseDto, bool withBearer = true);

    }
}
