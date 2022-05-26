using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }

        public string Role { get; set; } = string.Empty;

        public bool IsAdmin { get; set; } = false;

        public bool IsFinance { get; set; } = false;

        public bool CanEditUser { get; set; } = false;

        public bool CanRequestPurchase { get; set; } = false;

        public bool CanApprovePurchase { get; set; } = false;

        public bool CanCheckPurchase { get; set; } = false;
        
        public bool CanViewPurchase { get; set; } = false;

        public bool CanConfirmPurchase { get; set; } = false;
         
        public bool CanViewBulkPurchase { get; set; } = false;

        public bool CanRequestBulkPurchase { get; set; } = false;

        public bool CanApproveBulkPurchase { get; set; } = false;

        public bool CanConfirmBulkPurchase { get; set; } = false;

        public bool CanRequestBuy { get; set; } = false;

        public bool CanApproveBuy { get; set; } = false;

        public bool CanCheckBuy { get; set; } = false;

        public bool CanViewBuy { get; set; } = false;

        public bool CanConfirmBuy { get; set; } = false;

        public bool CanReceive { get; set; } = false;

        public bool CanApproveReceive { get; set; } = false;
        
        public bool CanViewReceive { get; set; } = false;

        public bool CanRequestIssue { get; set; } = false;

        public bool CanApproveIssue { get; set; } = false;
        
        public bool CanHandIssue { get; set; } = false; 
        
        public bool CanViewIssue { get; set; } = false; 
        
        public bool CanRequestBorrow { get; set; } = false;

        public bool CanApproveBorrow { get; set; } = false;
        
        public bool CanHandBorrow { get; set; } = false;
        
        public bool CanViewBorrow { get; set; } = false;

        public bool CanReturnBorrow { get; set; } = false;

        public bool CanRequestTransfer { get; set; } = false;

        public bool CanApproveTransfer { get; set; } = false;

        public bool CanSendTransfer { get; set; } = false;
        
        public bool CanReceiveTransfer { get; set; } = false;

        public bool CanViewTransfer { get; set; } = false;

        public bool CanRequestMaintenance { get; set; } = false;

        public bool CanApproveMaintenance { get; set; } = false;
       
        public bool CanFixMaintenance { get; set; } = false;
       
        public bool CanViewMaintenance { get; set; } = false;

        public bool CanGetStockNotification { get; set; } = false;

    }
}
