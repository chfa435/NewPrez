﻿using NewTiceAI.Data;
using NewTiceAI.Models;
using NewTiceAI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NewTiceAI.Services
{
    public class BTInviteService : IBTInviteService
    {
        private readonly ApplicationDbContext _context;

        public BTInviteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AcceptInviteAsync(Guid? token, string userId, int companyId)
        {
            try
            {
                Invite? invite = await _context.Invites.FirstOrDefaultAsync(i => i.OrganizationToken == token);

                if(invite == null)
                {
                    return false;
                }

                try
                {
                    invite.IsValid = false;
                    invite.InviteeId = userId;
                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception)
                {

                    throw;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddNewInviteAsync(Invite invite)
        {
            try
            {
                await _context.AddAsync(invite);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> AnyInviteAsync(Guid token, string email, int companyId)
        {
            try
            {
                bool result = await _context.Invites.Where(i => i.OrganizationId == companyId)
                                                    .AnyAsync(i => i.OrganizationToken == token && i.InviteeEmail == email);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Invite> GetInviteAsync(int inviteId, int companyId)
        {
            try
            {
                Invite? invite = await _context.Invites.Where(i => i.OrganizationId == companyId)
                                                      .Include(i => i.Organization)
                                                      .Include(i => i.Project)
                                                      .Include(i=>i.Invitor)
                                                      .FirstOrDefaultAsync(i=> i.Id == inviteId);

                return invite!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Invite> GetInviteAsync(Guid token, string email, int companyId)
        {
            try
            {
                Invite? invite = await _context.Invites.Where(i => i.OrganizationId == companyId)
                                                      .Include(i => i.Organization)
                                                      .Include(i => i.Project)
                                                      .Include(i => i.Invitor)
                                                      .FirstOrDefaultAsync(i => i.OrganizationToken == token && i.InviteeEmail == email);

                return invite!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> ValidateInviteCodeAsync(Guid? token)
        {
            try
            {
                if(token == null)
                {
                    return false;
                }

                bool result = false;

                Invite? invite = await _context.Invites.FirstOrDefaultAsync(i => i.OrganizationToken == token);

                if (invite != null)
                {
                    // Determine invite date
                    DateTime inviteDate = invite.InviteDate;

                    // Custom validation of invite based on the date it was
                    // issued. In this case we will allow an invite to be valid
                    // for 7 days.
                    bool validDate = (DateTime.Now - inviteDate).TotalDays <= 7;

                    if (validDate)
                    {
                        result = invite.IsValid;
                    }
                }

                 return result; 


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
