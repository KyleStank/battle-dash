using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour {
    [SerializeField]
    GameObject debugContent = null;
    [SerializeField]
    Text debugText = null;
    [SerializeField]
    float destroyTime = 5.0f;

    static Debugger _instance;
    List<Text> activeText = new List<Text>();

    public static Debugger Instance {
        get { return _instance;  }
    }

    void Awake() {
        _instance = this;
    }

    public void WriteLine(object message) {
        StartCoroutine(C_WriteLine(message, destroyTime));
    }

    void SetContentSize() {
        //Set content size
        RectTransform rTransform = debugContent.GetComponent<RectTransform>();
        Vector2 size = rTransform.sizeDelta;
        size.y = debugText.GetComponent<RectTransform>().sizeDelta.y * debugContent.transform.childCount;
        rTransform.sizeDelta = size;
    }

    IEnumerator C_WriteLine(object message, float timeToDestroy) {
        //Create debug text
        Text text = Instantiate(debugText, debugContent.transform) as Text;
        text.text = message.ToString();
        text.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        activeText.Add(text);

        SetContentSize();

        yield return new WaitForSeconds(timeToDestroy);

        Destroy(text.gameObject);

        SetContentSize();
    }
}
