using UnityEngine;

namespace KyleStankovich.BattleDash
{
	public class UIInfoHolder : MonoBehaviour {
		// For more information on this class check BattleManager -- CheckForWin method

		// For this class to work, not also has to be attached to the Button GO itself, but on the ClickEvent List on
		// the Button Component, and we need to select from the drop down list the function FBClick (); to update the
		// BattleManager.
		public int counter = 5;
	
		public void FBClick () {
			Debug.Log ("Called");
			counter -= 1;
			if (counter <= 0) {
				BattleManager.Instance.BossFinished ();
				counter = 5;
			}
		}
	}
}