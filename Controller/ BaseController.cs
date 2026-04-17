using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Shop_Backend.Controllers
{

    public abstract class BaseController : ControllerBase
    {

        protected int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(claim, out var userId))
                throw new UnauthorizedAccessException("Invalid user identity");
            return userId;
        }
    }
}