using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZFramework.Core;
namespace GamePlay
{
	public class GameDataView_Bind:UIBind
	{
		[SerializeField]
		private Text _character_totalhp_text;
		public Text Get_character_totalhp_text => _character_totalhp_text;
	}
}
