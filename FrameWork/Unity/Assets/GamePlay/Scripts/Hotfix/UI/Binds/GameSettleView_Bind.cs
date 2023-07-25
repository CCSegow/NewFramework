using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZFramework.Core;
namespace GamePlay
{
	public class GameSettleView_Bind:UIBind
	{
		[SerializeField]
		private Text _tip_text;
		public Text Get_tip_text => _tip_text;
		[SerializeField]
		private Button _back2home_btn;
		public Button Get_back2home_btn => _back2home_btn;
		[SerializeField]
		private Button _rechallenge_btn;
		public Button Get_rechallenge_btn => _rechallenge_btn;
		[SerializeField]
		private Image _bg_img;
		public Image Get_bg_img => _bg_img;
	}
}
