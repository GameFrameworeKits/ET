using Cysharp.Threading.Tasks;

namespace ET.Client
{
    public static class UIHelper
    {
        [EnableAccessEntiyChild]
        public static async UniTask<UI> Create(Entity scene, string uiType, UILayer uiLayer)
        {
            return await scene.GetComponent<UIComponent>().Create(uiType, uiLayer);
        }
        
        [EnableAccessEntiyChild]
        public static async UniTask Remove(Entity scene, string uiType)
        {
            scene.GetComponent<UIComponent>().Remove(uiType);
            await UniTask.CompletedTask;
        }
    }
}