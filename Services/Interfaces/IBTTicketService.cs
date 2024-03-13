﻿using NewTiceAI.Models;

namespace NewTiceAI.Services.Interfaces
{
    public interface IBTTicketService
    {
        public Task AddNewTicketAsync(Ticket ticket);

        public Task AddTicketAttachmentAsync(TicketAttachment ticketAttachment);

        public Task AddTicketCommentAsync(TicketComment ticketComment);

        public Task ArchiveTicketAsync(Ticket ticket);

        public Task AssignTicketAsync(int ticketId, string userId);


        public Task<List<Ticket>> GetArchivedTicketsAsync(int companyId);

        public Task<TicketAttachment> GetTicketAttachmentByIdAsync(int ticketAttachmentId);

        public Task<Ticket> GetTicketAsNoTrackingAsync(int ticketId);

        public Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int companyId);

        public Task<Ticket> GetTicketByIdAsync(int ticketId);

        public Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId);


        public Task RestoreTicketAsync(Ticket ticket);

        public Task UpdateTicketAsync(Ticket ticket);

    }
}
