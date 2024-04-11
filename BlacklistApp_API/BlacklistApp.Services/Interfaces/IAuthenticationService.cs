using BlacklistApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<AuthenticateResponse>> AuthenticateAsync(AuthenticateRequest model);
    }
}
