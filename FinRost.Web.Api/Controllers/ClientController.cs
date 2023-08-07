using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinRost.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class ClientController : ControllerBase
    {
        
        public ClientController()
        {

        }
    }
}
