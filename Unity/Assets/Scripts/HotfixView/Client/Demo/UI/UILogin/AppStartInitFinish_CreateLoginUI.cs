using Cysharp.Threading.Tasks;

namespace ET.Client
{
	[Event(SceneType.Demo)]
	public class AppStartInitFinish_CreateLoginUI: AEvent<Scene, AppStartInitFinish>
	{
		protected override async UniTask Run(Scene root, AppStartInitFinish args)
		{
			await UIHelper.Create(root, UIType.UILogin, UILayer.Mid);
		}
	}
}
