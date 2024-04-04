using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Models
{
    public class AuthenticateResponse
    {
        public string Token { get; set; }
        public UserDetails UserDetails { get; set; }
    }
}
