using System.Collections.Generic;
using System.Threading.Tasks;
using IncidentManagement.Models;
using IncidentManagement.Models.DTOs;

namespace IncidentManagement.Services
{
    public interface ICallService
    {
        Task<CallMatchResult> ProcessCallAsync(string phone, string firstName, string lastName);
    }
    public interface IIssueService
    {
        Task<List<Issue>> GetActiveIssuesAsync();
        Task<List<Solution>> GetAllActiveSolutionsAsync();
        Task<List<CallTip>> GetCallTipsForIssueAsync(int issueId);
        Task<List<CallTip>> GetAllCallTipsAsync();
    }

    public interface IIncidentService
    {
        Task<Incident> CreateAsync(CreateIncidentRequest request, string createdBy);
        Task<Incident?> PatchAsync(int incidentId, UpdateIncidentRequest request, string username);
        Task<Incident?> GetByIdAsync(int incidentId);
        Task<Incident?> GetOpenIncidentBySSNAsync(string ssn);
        Task<List<IncidentHistory>> GetByCustomerSSNAsync(string ssn);
        Task<List<Incident>> GetRecentByUserAsync(string username);
        Task UpdateTimeAsync(int incidentId, string username, int timeSpentSeconds);
    }

    public interface IIncidentNoteService
    {
        Task<List<IncidentNote>> GetByIncidentAsync(int incidentId);
        Task<IncidentNote> AddNoteAsync(int incidentId, string username, string note);
    }
}