using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Repository
{
    public interface IConfigProvider
    {
        string PrivateKey { get; }
        string PublicKey { get; }
    }
}
