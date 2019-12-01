using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Security
{
    public interface IJwtBuilder
    {
        string CreateToken(IDictionary<string, string> claims, string privateKey, string publicKey);

        bool ValidateToken(string token, IDictionary<string, string> expectedClaims, string publicKey);
    }
}
