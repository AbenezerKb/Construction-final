using ERP.DTOs;
using ERP.DTOs.Report;
using ERP.Models;

namespace ERP.Services.ReportServices
{
    public interface IReportService
    {
        Task<ReportReturnDTO<Borrow>> GetBorrowReport(BorrowReportDTO borrowDTO);
        Task<GeneralReportReturnDTO> GetGeneralReport(GeneralReportDTO reportDTO);
        Task<ReportReturnDTO<Issue>> GetIssueReport(IssueReportDTO issueDTO);
        Task<ReportReturnDTO<Maintenance>> GetMaintenanceReport(MaintenanceReportDTO maintenanceDTO);
        Task<ReportReturnDTO<Purchase>> GetPurchaseReport(PurchaseReportDTO purchaseDTO);
        Task<ReportReturnDTO<Receive>> GetReceiveReport(ReceiveReportDTO receiveDTO);
        Task<ReportReturnDTO<Transfer>> GetTansferReport(TransferReportDTO transferDTO);
    }
}
