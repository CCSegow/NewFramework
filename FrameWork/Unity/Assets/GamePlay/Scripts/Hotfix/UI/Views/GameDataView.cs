
using ZFramework.Core;
namespace GamePlay
{
	public class GameDataView:UIView
	{
		public GameDataView_Bind Bind;

		public void RefreshTotalHP(string totalHP)
		{
			Bind.Get_character_totalhp_text.text= totalHP;
		}
	}
}
