using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameEnvironment.AssetBundles
{
    public class AssetInstantiater : LocalAssetLoader
    {
        public AssetInstantiater()
        {
            _instances = new List<GameObject>();
        }

        private readonly List<GameObject> _instances; 

        public async Task<GameObject> InstantiateAsync(string assetKey, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            AsyncOperationHandle<GameObject> operation = Addressables.InstantiateAsync
                (
                    assetKey,
                    position,
                    rotation,
                    parent
                );

            if (operation.IsValid())
            {
                GameObject gameObject = await operation.Task;
                _instances.Add(gameObject);

                return gameObject;
            }

            return null;
        }
        
        public async Task<T> InstantiateAsync<T>(string assetKey, Vector3 position, Quaternion rotation, Transform parent = null)
            where T : Component
        {
            GameObject gameObject = await InstantiateAsync(assetKey, position, rotation, parent);
            return gameObject.GetComponent<T>();
        }

        public override void Dispose()
        {
            if (_instances == null) return;

            for (int i = 0; i < _instances.Count; i++)
            {
                var instance = _instances[i];
                Unload(ref instance);
            }
        }
    }
}