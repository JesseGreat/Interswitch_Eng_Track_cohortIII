using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Helpers
{
    public class AppSettings
    {
        public string JwtAudience { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtKey { get; set; }
    }
}
