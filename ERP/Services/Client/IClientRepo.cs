using ERP.Models;


namespace ERP.Services
{
    public interface IClientRepo
    {

        bool SaveChanges();
        IEnumerable<Client> GetAllClient();
        Client GetClient(string id);
        void CreateClient(Client client);
        void DeleteClient(string id);
        void UpdateClient(string id,Client client);
    }
}
