using UnityEditor;
using UnityEngine.UI;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Class responsible for handling the editor of the BlockingSlider class.
    /// </summary>
    [CustomEditor(typeof(BlockingSlider))]
    public class EditorBlockingSlider : Editor {
        Image pArea = null;
        Image gArea = null;
        Image oArea = null;

        #region Methods

        #region Unity methods
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            //For some reason, this is all we need for everything to properly work...

            /*
            BlockingSlider blockingSlider = (BlockingSlider)target;

            //Perfect area
            pArea = EditorGUILayout.ObjectField(new GUIContent("Perfect Area:"), pArea, typeof(Image), true) as Image;
            if(pArea != blockingSlider.perfectArea) {
                Undo.RecordObject(blockingSlider, "Changed blocking slider's perfect area.");
                blockingSlider.perfectArea = pArea;
            }

            //Good area
            gArea = EditorGUILayout.ObjectField(new GUIContent("Good Area:"), gArea, typeof(Image), true) as Image;
            if(gArea != blockingSlider.goodArea) {
                Undo.RecordObject(blockingSlider, "Changed blocking slider's good area.");
                blockingSlider.goodArea = gArea;
            }

            //Okay area
            oArea = EditorGUILayout.ObjectField(new GUIContent("Okay Area:"), oArea, typeof(Image), true) as Image;
            if(oArea != blockingSlider.okayArea) {
                Undo.RecordObject(blockingSlider, "Changed blocking slider's okay area.");
                blockingSlider.okayArea = oArea;
            }
            */
        }
        #endregion

        #endregion
    }
}
