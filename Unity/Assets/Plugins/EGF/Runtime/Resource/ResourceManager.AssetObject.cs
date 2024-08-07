using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using ET;

namespace EGF
{
    internal partial class ResourceManager
    {
        /// <summary>
        /// 资源对象。
        /// </summary>
        private sealed class AssetObject : ObjectBase
        {
            private AssetHandle m_AssetHandle;
            private ResourceManager m_ResourceManager;


            public AssetObject()
            {
                m_AssetHandle = null;
            }

            public static AssetObject Create(string name, object target, object assetHandle, ResourceManager resourceManager)
            {
                if (assetHandle == null)
                {
                    Debug.LogError("Resource is invalid.");
                }

                if (resourceManager == null)
                {
                    Debug.LogError("Resource Manager is invalid.");
                }

                AssetObject assetObject = ReferencePool.Acquire<AssetObject>();
                assetObject.Initialize(name, target);
                assetObject.m_AssetHandle = (AssetHandle)assetHandle;
                assetObject.m_ResourceManager = resourceManager;
                return assetObject;
            }

            public override void Clear()
            {
                base.Clear();
                m_AssetHandle = null;
            }

            protected internal override void OnUnspawn()
            {
                base.OnUnspawn();
            }

            protected internal override void Release(bool isShutdown)
            {
                if (!isShutdown)
                {
                    AssetHandle handle = m_AssetHandle;
                    if (handle is { IsValid: true })
                    {
                        handle.Dispose();
                    }
                    handle = null;
                }
            }
        }
    }
}