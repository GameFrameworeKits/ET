using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace ET.Client
{
	[UIEvent(UIType.UIHelp)]
    public class UIHelpEvent: AUIEvent
    {
        public override async UniTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer)
        {
	        try
	        {
		        string assetsName = $"Assets/Bundles/UI/Demo/{UIType.UIHelp}.prefab";
		        GameObject bundleGameObject = await uiComponent.Scene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
		        GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, uiComponent.UIGlobalComponent.GetLayer((int)uiLayer));
		        UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIHelp, gameObject);

				ui.AddComponent<UIHelpComponent>();
				return ui;
	        }
	        catch (Exception e)
	        {
		        Log.Error(e);
		        return null;
	        }
		}

        public override void OnRemove(UIComponent uiComponent)
        {
        }
    }
}