﻿using Mango.Service.EmailApi.Message;
using Mango.Service.EmailApi.Model.Dto;

namespace Mango.Service.EmailApi.Model.Services
{
    public interface IEmailService
    {
        Task EmailCartLog(CartDto cartDto);
        Task RegisterUserEmailandLog(string email);
        Task LogPlacedOrder(RewardMessage rewardMessage);
    }
}
