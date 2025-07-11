using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalonTrackApi.Contracts;

namespace SalonTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(IServiceManager serviceManager) : ControllerBase
    {
    }
}
