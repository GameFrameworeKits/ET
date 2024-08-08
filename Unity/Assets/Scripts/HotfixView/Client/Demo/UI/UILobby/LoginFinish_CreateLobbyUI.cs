﻿using Cysharp.Threading.Tasks;

namespace ET.Client
{
	[Event(SceneType.Demo)]
	public class LoginFinish_CreateLobbyUI: AEvent<Scene, LoginFinish>
	{
		protected override async UniTask Run(Scene scene, LoginFinish args)
		{
			await UIHelper.Create(scene, UIType.UILobby, UILayer.Mid);
		}
	}
}
