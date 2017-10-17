using UnityEditor;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Class responsible for handling the editor of the AttackArrow class.
    /// </summary>
    [CustomEditor(typeof(AttackArrow))]
	public class EditorAttackArrow : Editor {
        #region Methods

        #region Unity methods
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            //For some reason, this is all we need for everything to properly work...
        }
        #endregion

        #endregion
    }
}
