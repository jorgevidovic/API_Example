using API_Example.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API_Example.Controllers
{
    [ApiController]
    [Route("client")]
    public class ClientController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private const string ClientCacheKey = "clientsList";

        public ClientController(IMemoryCache cache)
        {
            _cache = cache;
            _cache.CreateEntry(ClientCacheKey).Value ??= new List<Client>();
        }


        [HttpGet]
        [Route("list")]
        public ActionResult<Client> ListAllClients()
        {
            var clients = _cache.Get<List<Client>>(ClientCacheKey);
            return Ok(clients);
        }

        [HttpGet]
        [Route("list/{id}")]
        public ActionResult<Client> ListClientById(string id)
        {
            var clients = _cache.Get<List<Client>>(ClientCacheKey) ?? new List<Client>();
            var client = clients.FirstOrDefault(c => c.id == id);
            if (client == null)
            {
                return NotFound(new { message = "Client not found." });
            }
            return Ok(client);
        }


        [HttpPost]
        [Route("create")]
        public IActionResult CreateClient([FromBody] Client client)
        {

            var clients = _cache.Get<List<Client>>(ClientCacheKey) ?? new List<Client>();
            client.id = clients.Count.ToString();
            clients.Add(client);
            _cache.Set(ClientCacheKey, clients);

            return Ok(
                new
                {
                    success = true,
                    message = "Client created successfully.",
                    result = client
                });
        }


        [HttpPut]
        [Route("update/{id}")]
        public IActionResult UpdateClient(string id, [FromBody] Client updatedClient)
        {
            if (updatedClient == null || updatedClient.id != id)
            {
                return BadRequest(new { message = "Invalid client data." });
            }

            var clients = _cache.Get<List<Client>>(ClientCacheKey) ?? new List<Client>();
            var clientIndex = clients.FindIndex(c => c.id == id);

            if (clientIndex == -1)
            {
                return NotFound(new { message = "Client not found." });
            }

            clients[clientIndex].name = updatedClient.name;
            clients[clientIndex].email = updatedClient.email;
            clients[clientIndex].phone = updatedClient.phone;

            _cache.Set(ClientCacheKey, clients);

            return Ok(
                new
                {
                    success = true,
                    message = "Client updated successfully.",
                    result = updatedClient
                });
        }


        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteClient(string id)
        {
            var clients = _cache.Get<List<Client>>(ClientCacheKey) ?? new List<Client>();
            var client = clients.FirstOrDefault(c => c.id == id);
            if (client == null)
            {
                return NotFound(new { message = "Client not found." });
            }

            clients.Remove(client);
            _cache.Set(ClientCacheKey, clients);

            return Ok(
                               new
                               {
                                   success = true,
                                   message = "Client deleted successfully.",

                               });
        }

    }
}
