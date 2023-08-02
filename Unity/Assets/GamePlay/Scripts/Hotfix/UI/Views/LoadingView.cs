using UnityEngine;
using ZFramework;
using ZFramework.Core;

namespace GamePlay
{
	public class LoadingView:UIView
	{
		public LoadingView_Bind Bind;

		ClassMessageRegister _eventHandler;

		private float _curBarRate;
		private float _targetRate;
		private float _speed = 1f;
		private float _timer;
		public override void OnOpen()
		{
			_eventHandler = new ClassMessageRegister();
			_eventHandler.Register(ZFramework.Core.GameEvents.LoadingEvent_OnBegin,LoadingEvent_OnBegin);
			_eventHandler.Register(ZFramework.Core.GameEvents.LoadingEvent_OnProgressChange,LoadingEvent_OnProgressChange);
			_eventHandler.Register(ZFramework.Core.GameEvents.LoadingEvent_OnEnd,LoadingEvent_OnEnd);

			_timer = 0;
			_curBarRate = 0;
		}

		public override void OnClose()
		{
			_eventHandler.Dispose();
		}

		private void Update()
		{
			if (_curBarRate < 0.99f)
			{
				//_curBarRate = Mathf.Lerp(_curBarRate,_targetRate, Time.deltaTime * _speed);
				_curBarRate = _targetRate;
				Debug.Log(_curBarRate);
				string text = $"{_curBarRate * 100f}%";
				Bind.Get_fill_img.fillAmount = _curBarRate;
				Bind.Get_rate_text.text = text;
			}

			if (_curBarRate >= 0.99f)
			{
				CloseThis();//TODO 做淡入淡出处理
			}
		}

		void LoadingEvent_OnBegin(object data)
		{
			Debug.Log("开始加载");
			Bind.Get_fill_img.fillAmount = 0;
			Bind.Get_rate_text.text = "0%";
		}
		
		void LoadingEvent_OnProgressChange(object data)
		{
			float rate = (float)data;
			_targetRate = rate;			
		}
		
		void LoadingEvent_OnEnd(object data)
		{
			Debug.Log("加载完毕");
			_targetRate = 1;
		}
		
	}
}
