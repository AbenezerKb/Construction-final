using ERP.DTOs;

namespace ERP.Services.ReportServices
{
    public interface IGeneralReportService
    {
        Task<IEnumerable<ReportSingleItem>> GetBorrowReport(GeneralReportDTO reportDTO);
        Task<IEnumerable<ReportSingleItem>> GetDamageReport(GeneralReportDTO reportDTO);
        Task<IEnumerable<ReportSingleItem>> GetIssueReport(GeneralReportDTO reportDTO);
        Task<IEnumerable<ReportSingleItem>> GetMinStockReport(GeneralReportDTO reportDTO);
        Task<IEnumerable<ReportSingleItem>> GetPurchaseReport(GeneralReportDTO reportDTO);
        Task<IEnumerable<ReportSingleItem>> GetReceiveReport(GeneralReportDTO reportDTO);
        Task<IEnumerable<ReportSingleItem>> GetReturnReport(GeneralReportDTO reportDTO);
        Task<IEnumerable<ReportSingleItem>> GetStockReport(GeneralReportDTO reportDTO);
        Task<IEnumerable<ReportSingleItem>> GetTransferInReport(GeneralReportDTO reportDTO);
        Task<IEnumerable<ReportSingleItem>> GetTransferOutReport(GeneralReportDTO reportDTO);
    }
}
