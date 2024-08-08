using Cysharp.Threading.Tasks;

namespace ET.Client
{
	[Event(SceneType.LockStep)]
	public class AppStartInitFinish_CreateUILSLogin: AEvent<Scene, AppStartInitFinish>
	{
		protected override async UniTask Run(Scene root, AppStartInitFinish args)
		{
			await UIHelper.Create(root, UIType.UILSLogin, UILayer.Mid);
		}
	}
}
