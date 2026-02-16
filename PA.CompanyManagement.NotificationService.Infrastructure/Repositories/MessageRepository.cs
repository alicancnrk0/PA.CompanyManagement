using Microsoft.EntityFrameworkCore;
using PA.CompanyManagement.Core.Exceptions;
using PA.CompanyManagement.NotificationService.Application.DTOs.Requests;
using PA.CompanyManagement.NotificationService.Application.DTOs.Responses;
using PA.CompanyManagement.NotificationService.Application.Repositories;
using PA.CompanyManagement.NotificationService.Domain.Entities;
using PA.CompanyManagement.NotificationService.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.NotificationService.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly NotificationDbContext _context;

        public MessageRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<MessageResponse?> CreateAsync(MessageCreateRequest request, bool sendMail = false)
        {
            try
            {
                var message = new Message
                {
                    Id = Guid.NewGuid(),
                    Subject = request.Subject,
                    Body = request.Body,
                    Sender = request.Sender,
                    Receiver = request.Reciever,
                    SendDate = DateTimeOffset.Now
                };

                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();

                //mail send

                return new MessageResponse
                {
                    Id = message.Id,
                    Subject = message.Subject,
                    Body = message.Body,
                    Sender = message.Sender,
                    Receiver = message.Receiver,
                    SendDate = message.SendDate
                };
            }
            catch (Exception ex)
            {
                throw new PAContextAddException(ex.Message, ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var message = await _context.Messages.FindAsync(id);
                if (message is null)
                    throw new PAContextQueryException("Mesaj bulunamadı!");

                message.DeletedAt = DateTimeOffset.Now;
                message.IsDeleted = true;

                _context.Messages.Update(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextRemoveException(ex.Message, ex);
            }
        }

        public async Task<List<MessageResponse>> GetAllAsync()
        {
            try
            {
                return await _context
                    .Messages
                    .AsNoTracking()
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new MessageResponse
                    {
                        Id = x.Id,
                        Subject = x.Subject,
                        Body = x.Body,
                        Sender = x.Sender,
                        Receiver = x.Receiver,
                        SendDate = x.SendDate
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException(ex.Message, ex);
            }
        }

        public async Task<MessageResponse?> GetAsync(Guid id)
        {
            try
            {
                return await _context
                    .Messages
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new MessageResponse
                    {
                        Id = x.Id,
                        Subject = x.Subject,
                        Body = x.Body,
                        Sender = x.Sender,
                        Receiver = x.Receiver,
                        SendDate = x.SendDate
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException(ex.Message, ex);
            }
        }

        public async Task<DetailedMessageResponse?> GetDetailedAsync(Guid id)
        {

            try
            {
                var message = await _context
                    .Messages
                    .FindAsync(id);

                if (message is null)
                    return null;

                return message as DetailedMessageResponse; 
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException(ex.Message, ex);
            }
        }

        public async Task<MinimalMessageResponse?> GetMinimalAsync(Guid id)
        {
            try
            {
                return await _context
                    .Messages
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new MinimalMessageResponse
                    {
                        Id = x.Id,
                        Subject = x.Subject,
                        Body = x.Body,
                        SendDate = x.SendDate,
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new PAContextQueryException(ex.Message, ex);
            }
        }
    }
}
