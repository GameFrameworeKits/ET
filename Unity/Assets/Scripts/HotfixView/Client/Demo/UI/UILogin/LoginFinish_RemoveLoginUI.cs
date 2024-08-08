﻿using Cysharp.Threading.Tasks;

namespace ET.Client
{
	[Event(SceneType.Demo)]
	public class LoginFinish_RemoveLoginUI: AEvent<Scene, LoginFinish>
	{
		protected override async UniTask Run(Scene scene, LoginFinish args)
		{
			await UIHelper.Remove(scene, UIType.UILogin);
		}
	}
}
