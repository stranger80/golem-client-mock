using System.Collections.Concurrent;
using System.Collections.Generic;
using GolemClientMockAPI.Models;

namespace GolemClientMockAPI.Repository
{
    public class InProcessAppKeyRepository : IAppKeyRepository
    {
        public IDictionary<string, string> AppKeys { get; set; } = new ConcurrentDictionary<string, string>();

        public string GetNodeForKey(string key)
        {
            return AppKeys[key];
        }

        public void RegisterKeys(KeyDesc[] keys)
        {
            foreach (var key in keys) {
                AppKeys[key.Key] = key.NodeId;
            }
        }
    }
}
