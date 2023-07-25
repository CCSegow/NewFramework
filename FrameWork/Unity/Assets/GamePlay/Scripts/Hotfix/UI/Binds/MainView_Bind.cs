using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZFramework.Core;
namespace GamePlay
{
	public class MainView_Bind:UIBind
	{
		[SerializeField]
		private Button _start_btn;
		public Button Get_start_btn => _start_btn;
		[SerializeField]
		private TextMeshProUGUI _title_text;
		public TextMeshProUGUI Get_title_text => _title_text;
		[SerializeField]
		private RectTransform _levelList_list;
		public RectTransform Get_levelList_list => _levelList_list;
	}
}
