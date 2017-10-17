using UnityEngine;

public class Screenshotter : MonoBehaviour {
	void Update() {
		//Screenshot taker
        if(Input.GetKeyDown(KeyCode.F12))
            ScreenCapture.CaptureScreenshot("Assets/Screenshots/Screenshot_" + Time.time + ".png");
	}
}
