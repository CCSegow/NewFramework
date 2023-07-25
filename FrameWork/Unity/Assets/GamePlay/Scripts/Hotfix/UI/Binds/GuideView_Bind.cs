using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZFramework.Core;
namespace GamePlay
{
	public class GuideView_Bind:UIBind
	{
		[SerializeField]
		private Text _tip_text;
		public Text Get_tip_text => _tip_text;
		[SerializeField]
		private Image _finger_img;
		public Image Get_finger_img => _finger_img;
		[SerializeField]
		private Image _fingerArea_img;
		public Image Get_fingerArea_img => _fingerArea_img;
		[SerializeField]
		private Image _mask_img;
		public Image Get_mask_img => _mask_img;
	}
}
