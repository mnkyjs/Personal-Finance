using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Personal.Finance.Domain.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Personal.Finance.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected bool IsOwner(int dbUserId, int userId) => dbUserId == userId;
    }
}
