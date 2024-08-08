using System;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class SceneChangeStart_AddComponent: AEvent<Scene, SceneChangeStart>
    {
        protected override async UniTask Run(Scene root, SceneChangeStart args)
        {
            try
            {
                Scene currentScene = root.CurrentScene();

                ResourcesLoaderComponent resourcesLoaderComponent = currentScene.GetComponent<ResourcesLoaderComponent>();
            
                // 加载场景资源
                await resourcesLoaderComponent.LoadSceneAsync($"Assets/Bundles/Scenes/{currentScene.Name}.unity", LoadSceneMode.Single);
                // 切换到map场景

                //await SceneManager.LoadSceneAsync(currentScene.Name);

                currentScene.AddComponent<OperaComponent>();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        }
    }
}