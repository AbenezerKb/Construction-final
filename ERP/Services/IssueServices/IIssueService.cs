using ERP.DTOs;
using ERP.Models;

namespace ERP.Services.IssueServices
{
    public interface IIssueService
    {
        Task<Issue> ApproveIssue(ApproveIssueDTO approveDTO);
        Task<Issue> DeclineIssue(DeclineIssueDTO declineDTO);
        Task<List<Issue>> GetByCondition(GetIssuesDTO getIssuesDTO);
        Task<Issue> GetById(int id);
        Task<Issue> HandIssue(HandIssueDTO handDTO);
        Task<Issue> RequestIssue(CreateIssueDTO issueDTO);
    }
}
