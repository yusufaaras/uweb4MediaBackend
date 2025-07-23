using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uweb4Media.Application.Tools
{
    public class JwtTokenDefaults
    {
        public const string ValidAudience = "https://localhost";  
        public const string ValidIssuer = "https://localhost:7296";    
        public const string Key = "Uweb4SystemsGlobals+*010203CARBOOK01+*..020304CarBookProje";
        public const int Expire = 5;
    }
}
