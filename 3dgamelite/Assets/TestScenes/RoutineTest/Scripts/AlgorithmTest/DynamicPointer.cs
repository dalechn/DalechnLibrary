using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dalechn
{
    [System.Serializable]
    public class Info
    {
        public string valName;
        public object val;
        public bool showString;

        public Info(string valName, object val, bool showString = false)
        {
            this.valName = valName;
            this.val = val;
            this.showString = showString;
        }
        public override string ToString()
        {
            return showString ? valName + ": " + val : val.ToString();
        }
    }

    public class DynamicPointer : MonoBehaviour
    {
        public Text variableTextPrefab;
        public List<Info> info;
        public bool active = true;

        public Image image { get; set; }
        List<Text> textList = new List<Text>();
        const float k_textMagin = 80;

        private Vector3 originScale;

        public void Init(Vector3 pos)
        {
            image = GetComponent<Image>();
            originScale = image.rectTransform.localScale;

            transform.position = pos;
        }

        public void Show()
        {
            for (int i = 0; i < info.Count; i++)
            {
                Text t = Instantiate(variableTextPrefab, transform);
                t.rectTransform.localPosition = new Vector3(0, -(i + 1) * k_textMagin, 0);
                textList.Add(t);

                t.text = info[i].ToString();
            }
        }

        public void Scale(float scale = 1.2f)
        {
            image.rectTransform.localScale *= scale;
        }


        public void SetActive(bool active)
        {
            active = false;
            gameObject.SetActive(active);

            image.rectTransform.localScale = originScale;
        }
    }
}

