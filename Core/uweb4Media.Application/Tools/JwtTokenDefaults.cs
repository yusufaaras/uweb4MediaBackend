using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uweb4Media.Application.Tools
{
    public class JwtTokenDefaults
    {
        public const string ValidAudience = "http://localhost:5174"; // frontend'in local adresi
        public const string ValidIssuer = "http://localhost:5285";   // backend'in local adresi
        public const string Key = "Uweb4SystemsGlobals+*010203CARBOOK01+*..020304CarBookProje";
        public const int Expire = 5;
    }
}
