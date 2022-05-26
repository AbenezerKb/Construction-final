using ERP.Models;
using ERP.Context;
using ERP.Exceptions;

namespace ERP.Services
{
    public class ClientRepo: IClientRepo
    {
        private readonly AppDbContext _context;

        public ClientRepo(AppDbContext context)
        {
            _context = context;

        }

        public void CreateClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException();
            }
            client.clientId = Guid.NewGuid().ToString();

            //consultant. = DateTime.Now.Date;//DateTime.Now.ToString("yyyy-MM-dd");

            _context.Clients.Add(client);
        }



        public IEnumerable<Client> GetAllClient()
        {
            return _context.Clients.ToList();
        }


        public Client GetClient(string id)
        {
            return _context.Clients.FirstOrDefault(c => c.clientId == id);
        }


        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void DeleteClient(string id)
        {
            var client = _context.Clients.FirstOrDefault(c => c.clientId == id);
            _context.Clients.Remove(client);
        }



        public void UpdateClient(string id,Client updatedClient)
        {
            if (updatedClient.Equals(null))
            {
                throw new ArgumentNullException();
            }

            Client client = _context.Clients.FirstOrDefault(c => c.clientId == id);
            if (client.Equals(null))
                throw new ItemNotFoundException($"Client not found with client Id={id}");
            client.address = updatedClient.address;
            client.clientName = updatedClient.clientName;
            client.contractorId = updatedClient.contractorId;
            client.dateOfContract = updatedClient.dateOfContract;
            client.description = updatedClient.description;
            client.estimatedCost = updatedClient.estimatedCost;
            client.estimatedDuration = updatedClient.estimatedDuration;
            client.remarks = updatedClient.remarks;            

            _context.Clients.Add(client);
        }


    }
}
