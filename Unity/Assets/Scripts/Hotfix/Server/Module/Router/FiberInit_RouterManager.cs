using System.Net;
using Cysharp.Threading.Tasks;

namespace ET.Server
{
    [Invoke((long)SceneType.RouterManager)]
    public class FiberInit_RouterManager: AInvokeHandler<FiberInit, UniTask>
    {
        public override async UniTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.Get((int)root.Id);
            root.AddComponent<HttpComponent, string>($"http://*:{startSceneConfig.Port}/");

            await UniTask.CompletedTask;
        }
    }
}