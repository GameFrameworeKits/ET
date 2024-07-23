using System;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace ET
{
    /// <summary>
    /// 远端资源地址查询服务类
    /// </summary>
    public class RemoteServices : IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }

        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }

        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }

    public class ResourcesComponent : Singleton<ResourcesComponent>, ISingletonAwake
    {
        public GlobalConfig GlobalConfig;

        public void Awake()
        {
            YooAssets.Initialize();
#if UNITY_WEBGL
            YooAssets.SetCacheSystemDisableCacheOnWebGL();
#endif
            GlobalConfig = Resources.Load<GlobalConfig>("GlobalConfig");
        }

        protected override void Destroy()
        {
            YooAssets.Destroy();
        }

        public async ETTask CreatePackageAsync(string packageName, bool isDefault = false)
        {
            ResourcePackage package = YooAssets.CreatePackage(packageName);
            if (isDefault)
            {
                YooAssets.SetDefaultPackage(package);
            }

            GlobalConfig globalConfig = Resources.Load<GlobalConfig>("GlobalConfig");
            EPlayMode ePlayMode = globalConfig.EPlayMode;
#if UNITY_WEBGL
            ePlayMode = EPlayMode.WebPlayMode;
#endif

            // 编辑器下的模拟模式
            switch (ePlayMode)
            {
                case EPlayMode.EditorSimulateMode:
                {
                    EditorSimulateModeParameters createParameters = new();
                    createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild("ScriptableBuildPipeline", packageName);
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.OfflinePlayMode:
                {
                    OfflinePlayModeParameters createParameters = new();
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.HostPlayMode:
                {
                    string defaultHostServer = GetHostServerURL(package.PackageName);
                    string fallbackHostServer = GetHostServerURL(package.PackageName);
                    HostPlayModeParameters createParameters = new();
                    createParameters.BuildinQueryServices = new GameQueryServices();
                    createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.WebPlayMode:
                {
                    string defaultHostServer = GetHostServerURL(package.PackageName);
                    string fallbackHostServer = GetHostServerURL(package.PackageName);
                    WebPlayModeParameters createParameters = new();
                    createParameters.BuildinQueryServices = new WebGLGameQueryServices();
                    createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                    await package.InitializeAsync(createParameters).Task;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        string GetHostServerURL(string pacakgeName)
        {
            //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
            string hostServerIP = GlobalConfig.BundleUrl;
            string appVersion = "v1.0";

#if UNITY_EDITOR
                switch (UnityEditor.EditorUserBuildSettings.activeBuildTarget)
                {
                    case UnityEditor.BuildTarget.Android:
                        return $"{hostServerIP}/CDN/Android/{appVersion}";
                    case UnityEditor.BuildTarget.iOS:
                        return $"{hostServerIP}/CDN/IPhone/{appVersion}";
                    case UnityEditor.BuildTarget.WebGL:
                        return $"{hostServerIP}/webgl/StreamingAssets/Bundles/{pacakgeName}";
                    default:
                        return $"{hostServerIP}/CDN/WebGL/{appVersion}";
                }
#else
		        switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        return $"{hostServerIP}/CDN/Android/{appVersion}";
                    case RuntimePlatform.IPhonePlayer:
                        return $"{hostServerIP}/CDN/IPhone/{appVersion}";
                    case RuntimePlatform.WebGLPlayer:
                    {
                        return $"{hostServerIP}/webgl/StreamingAssets/Bundles/{pacakgeName}";
                    }
                    default:
                        return $"{hostServerIP}/CDN/PC/{appVersion}";
                }
#endif
        }

        public void DestroyPackage(string packageName)
        {
            ResourcePackage package = YooAssets.GetPackage(packageName);
            package.UnloadUnusedAssets();
        }

        /// <summary>
        /// 主要用来加载dll config aotdll，因为这时候纤程还没创建，无法使用ResourcesLoaderComponent。
        /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
        /// </summary>
        public async ETTask<T> LoadAssetAsync<T>(string location) where T : UnityEngine.Object
        {
            AssetHandle handle = YooAssets.LoadAssetAsync<T>(location);
            await handle.Task;
            T t = (T)handle.AssetObject;
            handle.Release();
            return t;
        }

        /// <summary>
        /// 主要用来加载dll config aotdll，因为这时候纤程还没创建，无法使用ResourcesLoaderComponent。
        /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
        /// </summary>
        public async ETTask<Dictionary<string, T>> LoadAllAssetsAsync<T>(string location) where T : UnityEngine.Object
        {
            AllAssetsHandle allAssetsOperationHandle = YooAssets.LoadAllAssetsAsync<T>(location);
            await allAssetsOperationHandle.Task;
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            foreach (UnityEngine.Object assetObj in allAssetsOperationHandle.AllAssetObjects)
            {
                T t = assetObj as T;
                dictionary.Add(t.name, t);
            }

            allAssetsOperationHandle.Release();
            return dictionary;
        }
    }
}