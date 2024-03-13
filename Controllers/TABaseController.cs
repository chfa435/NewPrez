using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NewTiceAI.Extensions;

namespace NewTiceAI.Controllers
{
    [Controller]
    public abstract class TABaseController : Controller
    {
        protected string? _userId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        protected int _organizationId => User.Identity!.GetOrganizationId();
    }
}
