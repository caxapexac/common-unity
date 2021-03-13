using System.Collections.Generic;
using Caxapexac.Common.Sharp.Runtime.Patterns.Service;


namespace Caxapexac.Common.Sharp.Runtime.Services.Pool
{
    /// <summary>
    /// Pools orchestrator
    /// </summary>
    public class PoolManager : MonoBehaviourService<PoolManager>
    {
        private Dictionary<string, PoolContainer> _pools;

        protected override void OnCreateService()
        {
            _pools = new Dictionary<string, PoolContainer>();
        }

        protected override void OnDestroyService()
        {
        }

        public IPoolObject Get(string path)
        {
            if (!_pools.ContainsKey(path))
            {
                _pools[path] = PoolContainer.CreatePool<PoolObject>(path);
            }
            return _pools[path].Get();
        }
    }
}