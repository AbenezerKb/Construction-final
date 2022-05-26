using ERP.DTOs;
using ERP.Models;

namespace ERP.Services.BorrowServices
{
    public interface IBorrowService
    {
        Task<Borrow> ApproveBorrow(ApproveBorrowDTO approveDTO);
        Task<Borrow> DeclineBorrow(DeclineBorrowDTO approveDTO);
        Task<List<Borrow>> GetByCondition(GetBorrowsDTO getBorrowsDTO);
        Task<Borrow> GetById(int id);
        Task<Borrow> HandBorrow(HandBorrowDTO handDTO);
        Task<Borrow> RequestBorrow(CreateBorrowDTO borrowDTO);
    }
}
