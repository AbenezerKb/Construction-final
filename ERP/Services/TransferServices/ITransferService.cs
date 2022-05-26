using ERP.DTOs;
using ERP.Models;

namespace ERP.Services.TransferServices
{
    public interface ITransferService
    {
        public Task<Transfer> GetById(int id);
        public Task<List<Transfer>> GetByCondition(GetTransfersDTO getTransfersDTO);
        public Task<Transfer> RequestEquipment(CreateEquipmentTransferDTO transferDTO);
        Task<Transfer> RequestMaterial(CreateMaterialTransferDTO transferDTO);
        Task<Transfer> ApproveTransfer(ApproveTransferDTO approveDTO);
        Task<Transfer> DeclineTransfer(DeclineTransferDTO declineDTO);
        Task<Transfer> SendTransfer(SendTransferDTO sendDTO);
        Task<Transfer> ReceiveTransfer(ReceiveTransferDTO receiveDTO);
    }
}
