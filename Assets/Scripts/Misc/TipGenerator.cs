using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace KyleStankovich.BattleDash {
    [RequireComponent(typeof(Text))]
    public class TipGenerator : MonoBehaviour {
        [SerializeField]
        string fileName = "";

        List<string> fileContents = new List<string>();
        Text text;

        #region Methods

        #region Unity methods
        void Start() {
            fileName = Path.GetFileNameWithoutExtension(fileName);
            text = GetComponent<Text>();

            //Load the tips file
            TextAsset tips = Resources.Load(fileName) as TextAsset;
            if(tips != null) {
                //Read all of the file's contents
                using(StreamReader sr = new StreamReader(new MemoryStream(tips.bytes))) {
                    while(true) {
                        string str = sr.ReadLine(); //Read the file

                        if(str != null) { //If there was text that was read
                            if(!string.IsNullOrEmpty(str))
                                fileContents.Add(str);
                        } else //If not
                            break;
                    }
                }
            }

            //Randomly choose a tip from the file
            text.text = GetLine(Random.Range(1, fileContents.Count + 1));
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Selects a line from the text that was retrieved from the tips file.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        string GetLine(int line) {
            if(line <= 0 || fileContents.Count <= line - 1)
                return "";

            return fileContents[line - 1];
        }
        #endregion

        #region IEnumerator methods
        
        #endregion

        #endregion
    }
}
