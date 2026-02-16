using PA.CompanyManagement.NotificationService.Application.DTOs.Requests;
using PA.CompanyManagement.NotificationService.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.NotificationService.Application.Repositories
{
    public interface IMessageRepository
    {
        Task<List<MessageResponse>> GetAllAsync();

        Task<MinimalMessageResponse?> GetMinimalAsync(Guid id);
        Task<MessageResponse?> GetAsync(Guid id);
        Task<DetailedMessageResponse?> GetDetailedAsync(Guid id);

        Task<MessageResponse?> CreateAsync(MessageCreateRequest request, bool sendMail = false);

        Task DeleteAsync(Guid id);
    }
}
