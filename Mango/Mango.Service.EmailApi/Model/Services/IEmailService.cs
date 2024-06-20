using Mango.Service.EmailApi.Model.Dto;

namespace Mango.Service.EmailApi.Model.Services
{
    public interface IEmailService
    {
        Task EmailCartLog(CartDto cartDto);
    }
}
