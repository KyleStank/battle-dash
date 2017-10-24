using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KyleStankovich.Utils;

namespace KyleStankovich.BattleDash {
    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("Battle Dash/GUI/Floating Text")]
	public class FloatingText : MonoBehaviour {
        [SerializeField]
        [Range(0.0f, 5.0f)]
        float floatSpeed = 1.0f;
        [SerializeField]
        Vector3 offset = Vector3.zero;
        [SerializeField]
        float randomX;

        Animator anim;
        Text text;
        Vector3 start;
        Vector3 end;

        float newX;

        #region Methods

        #region Unity methods
        void Awake() {
            //Get some references
            anim = GetComponent<Animator>();
            text = GetComponent<Text>();

            //Set the starting point
            start = text.rectTransform.anchoredPosition;

            //Set the end point
            end = start + offset;
            end.x = Random.Range(-randomX, randomX);

            Destroy(gameObject, anim.GetCurrentAnimatorClipInfo(0).Length);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Sets the text of the floating text.
        /// </summary>
        /// <param name="text">String to set text to.</param>
        public void SetText(string text) {
            if(text == null)
                return;

            this.text.text = text;
        }
        #endregion

        #region IEnumerator methods
        IEnumerator Animate(Vector3 pos1, Vector3 pos2) {
            float timer = 0.0f;
            float evalMove = 0.0f;
            float evalAlpha = 0.0f;

            while(text.color.a > 0.0f) {
                //Evaluate the speed for moving and fading
                evalMove = timer / floatSpeed;
                evalAlpha = 1.0f - timer / floatSpeed;

                //Lerp the positon
                //text.rectTransform.anchoredPosition = Vector2.Lerp(pos1, pos2, evalMove);
                transform.position = Vector3.Lerp(pos1, pos2, evalMove);

                //Set color/fade
                Color color = text.color;
                color.a = evalAlpha;
                text.color = color;

                //Increment the timer
                timer += Time.deltaTime;

                yield return null;
            }

            Destroy(gameObject);
        }
        #endregion

        #endregion
    }
}
