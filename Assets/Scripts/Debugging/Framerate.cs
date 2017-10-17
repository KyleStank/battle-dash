using UnityEngine;

public class Framerate : MonoBehaviour {
    [SerializeField]
    Color color = Color.blue;

    float deltaTime = 0.0f;

    GUIStyle style = new GUIStyle();
    Rect rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    float msec = 0.0f;
    float fps = 0.0f;
    string text = "";
    
    void Update() {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI() {
        rect = new Rect(0, 0, Screen.width, Screen.height * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = Screen.height * 2 / 100;
        style.normal.textColor = color;

        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;
        //text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        text = System.Math.Round(msec, 1) + " ms " + System.Math.Round(fps) + " fps";

        GUI.Label(rect, text, style);
        
    }
}
