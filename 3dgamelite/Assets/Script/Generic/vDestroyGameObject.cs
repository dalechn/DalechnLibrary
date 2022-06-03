using UnityEngine;
using System.Collections;
using PathologicalGames;

namespace Invector
{    
    [vClassHeader("Destroy GameObject", openClose = false)]
    public class vDestroyGameObject : vMonoBehaviour
    {
        public float delay;
        public UnityEngine.Events.UnityEvent onDestroy;
        IEnumerator Start()
        {
            yield return new WaitForSeconds(delay);
            onDestroy.Invoke();
            //Destroy(gameObject);
            if (PoolManager.Pools.Count > 0)
            {
                foreach(var val in PoolManager.Pools)
                {
                    if (val.Value.IsSpawned(transform))
                    {
                        PoolManager.Pools[val.Key].Despawn(transform);
                        yield return null;
                    }
                }
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}