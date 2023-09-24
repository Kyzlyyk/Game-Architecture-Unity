using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameEnvironment.AssetBundles
{
    public abstract class LocalAssetLoader : IDisposable
    {
        public abstract void Dispose();

        protected void Unload(ref GameObject cachedInstance)
        {
            if (cachedInstance == null) return;

            cachedInstance.SetActive(false);
            Addressables.ReleaseInstance(cachedInstance);
            cachedInstance = null;
        }
    }
}