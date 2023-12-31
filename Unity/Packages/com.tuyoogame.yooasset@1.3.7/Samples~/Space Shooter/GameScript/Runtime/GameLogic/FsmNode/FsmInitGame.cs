﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Pooling;
using UniFramework.Window;
using UniFramework.Machine;
using YooAsset;

internal class FsmInitGame : IStateNode
{
	private StateMachine _machine;

	void IStateNode.OnCreate(StateMachine machine)
	{
		_machine = machine;
	}
	void IStateNode.OnEnter()
	{
		var handle = YooAssets.LoadAssetSync<GameObject>("UICanvas");
		var canvas = handle.InstantiateSync();
		var desktop = canvas.transform.Find("Desktop").gameObject;
		GameObject.DontDestroyOnLoad(canvas);

		// 初始化窗口系统
		UniWindow.Initalize(desktop);

		// 初始化对象池系统
		UniPooling.Initalize();

		_machine.ChangeState<FsmSceneHome>();
	}
	void IStateNode.OnUpdate()
	{
	}
	void IStateNode.OnExit()
	{
	}
}