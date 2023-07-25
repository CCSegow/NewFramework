using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZFramework.Core;
namespace GamePlay
{
	public class StartGameView_Bind:UIBind
	{
		[SerializeField]
		private Text _game_data_text;
		public Text Get_game_data_text => _game_data_text;
		[SerializeField]
		private Text _schedule_text;
		public Text Get_schedule_text => _schedule_text;
		[SerializeField]
		private Text _tip_text;
		public Text Get_tip_text => _tip_text;
		[SerializeField]
		private Button _start_btn;
		public Button Get_start_btn => _start_btn;
		[SerializeField]
		private Text _start_text;
		public Text Get_start_text => _start_text;
		[SerializeField]
		private Image _bg_img;
		public Image Get_bg_img => _bg_img;
	}
}
