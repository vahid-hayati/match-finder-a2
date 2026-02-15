using api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers. Helpers;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    
}