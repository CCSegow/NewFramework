using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZFramework.Core;
namespace GamePlay
{
	public class PopTipsView_Bind:UIBind
	{
		[SerializeField]
		private Button _submit_btn;
		public Button Get_submit_btn => _submit_btn;
		[SerializeField]
		private Image _icon_img;
		public Image Get_icon_img => _icon_img;
		[SerializeField]
		private TextMeshProUGUI _tips_text;
		public TextMeshProUGUI Get_tips_text => _tips_text;
	}
}
