using Cysharp.Threading.Tasks;

namespace ET.Client
{
    public abstract class AUIEvent: HandlerObject
    {
        public abstract UniTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer);
        public abstract void OnRemove(UIComponent uiComponent);
    }
}