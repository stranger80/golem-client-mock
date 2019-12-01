using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Repository
{
    public class ConfigProvider : IConfigProvider
    {
        public string PrivateKey { get; set; }

        public string PublicKey { get; set; }

        public ConfigProvider(IConfiguration configuration)
        {
            this.PrivateKey = configuration.GetValue<string>("PrivateKey");
            this.PublicKey = configuration.GetValue<string>("PublicKey");
        }

    }
}
