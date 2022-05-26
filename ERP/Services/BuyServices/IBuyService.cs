using ERP.DTOs;
using ERP.DTOs.Buy;
using ERP.Models;

namespace ERP.Services.BuyServices
{
    public interface IBuyService
    {
        public Task<Buy> GetById(int id);
        public Task<List<Buy>> GetByCondition();
        public Task<Buy> RequestBuy(CreateBuyDTO buyDTO);
        public Task<Buy> CheckBuy(CheckBuyDTO checkDTO);
        public Task<Buy> QueueBuy(QueueBuyDTO queueDTO);
        public Task<Buy> ApproveBuy(ApproveBuyDTO approveDTO);
        public Task<Buy> DeclineBuy(ApproveBuyDTO declineDTO);
        public Task<Buy> ConfirmBuy(ConfirmBuyDTO confirmDTO);
    }
}
