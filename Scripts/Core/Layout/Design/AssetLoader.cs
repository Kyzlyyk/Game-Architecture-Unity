using GameEnvironment.AssetBundles;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Layout.Design
{
    internal sealed class AssetLoader : LocalAssetLoader
    {
        public bool TryGetAsset<T>(DesignAtlas atlas, out AsyncOperationHandle<T> operation)
        {
            for (int i = 0; i < atlas.MaterialAssetNames.Length; i++)
            {
                operation = Addressables.LoadAssetAsync<T>(atlas.MaterialAssetNames[i]);

                if (operation.IsValid())
                {
                    return true;
                }
            }

            operation = default;
            return false;
        }

        public override void Dispose()
        {
        }
    }
}