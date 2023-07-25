using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZFramework.Core;
namespace GamePlay
{
	public class LoadingView_Bind:UIBind
	{
		[SerializeField]
		private TextMeshProUGUI _rate_text;
		public TextMeshProUGUI Get_rate_text => _rate_text;
		[SerializeField]
		private Image _fill_img;
		public Image Get_fill_img => _fill_img;
		[SerializeField]
		private Image _bg_img;
		public Image Get_bg_img => _bg_img;
	}
}
