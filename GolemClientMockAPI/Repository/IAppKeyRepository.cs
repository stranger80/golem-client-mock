using GolemClientMockAPI.Models;

namespace GolemClientMockAPI.Repository
{
    public interface IAppKeyRepository
    {
        string GetNodeForKey(string key);

        void RegisterKeys(KeyDesc[] keys);
    }
}