using Cysharp.Threading.Tasks;

namespace ET.Client
{
	[Event(SceneType.LockStep)]
	public class LoginFinish_CreateUILSLobby: AEvent<Scene, LoginFinish>
	{
		protected override async UniTask Run(Scene scene, LoginFinish args)
		{
			await UIHelper.Create(scene, UIType.UILSLobby, UILayer.Mid);
		}
	}
}
