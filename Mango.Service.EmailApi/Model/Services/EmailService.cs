﻿using Mango.Service.EmailApi.Data;
using Mango.Service.EmailApi.Message;
using Mango.Service.EmailApi.Model.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Service.EmailApi.Model.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> options;

        public EmailService(DbContextOptions<AppDbContext> options)
        {
            this.options = options;
        }

        public async Task EmailCartLog(CartDto cartDto)
        {

            StringBuilder message = new StringBuilder();
            message.AppendLine("<br/>Cart Email Request ");
            message.AppendLine("<br/>Total "+ cartDto.cartHeader.CartTotal);
            message.AppendLine("<br/>");
            message.AppendLine("<ul>");
            foreach (var item in cartDto.cartDetails)
            {
                message.Append("<li>");
                message.Append(item.ProductDto.ProductName+ " x " + item.Count);
                message.Append("</li>");
            }
            message.AppendLine("</ul>");
            await LogAndEmail(message.ToString(), cartDto.cartHeader.Email);
        }

        public async Task<bool> LogAndEmail(string message, string email) 
        {
            try
            {
                EmailLogger emailLogger = new()
                {
                    Email=email,
                    EmailSent = DateTime.Now,
                    Message = message
                };

                await  using var db = new AppDbContext(options);
                db.Add(emailLogger);
                await db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task RegisterUserEmailandLog(string email)
        {
            string message = "User Registeration Successfull. </br> Email: "+ email;
            await LogAndEmail(message,"todo: admin email  here.");
        }

       public async Task  LogPlacedOrder(RewardMessage rewardMessage)
        {
            string message = "New order placed. </br> Order Id: " + rewardMessage.OrderId;
            await LogAndEmail(message, "todo: admin email  here.");
        }
    }
}
