using ERP.DTOs;
using ERP.DTOs.Receive;
using ERP.Models;

namespace ERP.Services.ReceiveServices
{
    public interface IReceiveService
    {
        public Task<Receive> GetById(int id);
        public Task<List<Receive>> GetMySite();
        public Task<List<Receive>> GetByCondition(GetReceivesDTO getReceivesDTO);
        public Task<Receive> ReceiveItem(CreateReceiveDTO receiveDTO);
        public Task<Receive> ApproveReceive(ApproveReceiveDTO approveDTO);

    }
}
