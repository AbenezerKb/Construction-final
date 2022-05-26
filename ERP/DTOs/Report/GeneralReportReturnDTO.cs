namespace ERP.DTOs.Report
{
    public class GeneralReportReturnDTO
    {
        public HashSet<string> Keys { get; set; } = new();

        public Dictionary<string, string> Labels { get; set; } = new();

        public Dictionary<string, ReportSingleItem> TransferOutSummary { get; set; }

        public Dictionary<string, ReportSingleItem> TransferInSummary { get; set; }

        public Dictionary<string, ReportSingleItem> IssueSummary { get; set; }

        public Dictionary<string, ReportSingleItem> BorrowSummary { get; set; }

        public Dictionary<string, ReportSingleItem> ReturnSummary { get; set; }

        public Dictionary<string, ReportSingleItem> StockSummary { get; set; }

        public Dictionary<string, ReportSingleItem> MinStockSummary { get; set; }

        public Dictionary<string, ReportSingleItem> DamageSummary { get; set; }

        public Dictionary<string, ReportSingleItem> ReceiveSummary { get; set; }

        public Dictionary<string, ReportSingleItem> PurchaseSummary { get;  set; }
    }
}
