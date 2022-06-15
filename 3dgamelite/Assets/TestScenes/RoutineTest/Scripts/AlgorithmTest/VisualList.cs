using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dalechn
{
    public class ClearTag : MonoBehaviour { }


    public enum EDirection { ROW_DOWN, ROW_TOP, ROW_LEFT }
    public enum EColorMode { BLANK, ARRAY2D0_1 }

    public class VisualList : MonoBehaviour
    {
        [System.Serializable]
        public class Pointer
        {
            public GameObject textObject;

            public int value;
            public Text text;
            public Image image;

            public Pointer(GameObject textObject, int val, int i, int j)
            {
                this.textObject = textObject;

                //value
                image = textObject.GetComponentInChildren<Image>();
                text = image.gameObject.GetComponentInChildren<Text>();

                value = val;
                text.text = val.ToString();

            }


            public void ChangeColor(Color c)
            {
                image.color = c;
            }

            public void SetValue(int val, Color c)
            {
                text.text = val.ToString();
                this.value = val;

                ChangeColor(c);
            }
        }



        [System.Serializable]
        public class Item
        {
            public Pointer pointer;
            public Text indexText;
            public Vector3 currentPosition;
            public int indexRow;
            public int indexCol;

            public Item(GameObject item, int val, int row, int col, bool onlyCol, EDirection dir)
            {
                GameObject textObject = Instantiate(item, item.transform.parent);
                textObject.AddComponent<ClearTag>();

                RectTransform tr = textObject.GetComponent<RectTransform>();
                tr.position = new Vector3(tr.position.x + col * k_margin, tr.position.y + row * k_margin);

                indexRow = row;
                indexCol = col;
                indexText = textObject.GetComponentInChildren<Text>();

                pointer = new Pointer(textObject, val, indexRow, indexCol);

                currentPosition = pointer.image.transform.position;


                if (dir == EDirection.ROW_LEFT)
                {
                    col = indexRow;
                    row = indexCol;
                }

                if (onlyCol)
                    indexText.text = row.ToString();
                else
                    indexText.text = row.ToString() + "," + col.ToString();
            }

            public void SetActiveIndex()
            {
                indexText.gameObject.SetActive(!indexText.gameObject.activeInHierarchy);
            }
        }

        [System.Serializable]
        public class MultiDimensionalInt
        {
            public List<Item> intArray = new List<Item>();

            public Item this[int index] => intArray[index];

        }


        public Transform rowText;
        public Transform colText;
        public GameObject arrayPrefab;

        public PointerList pointerListUp;
        public PointerList pointerListLeft;
        public PointerList pointerListRight;

        public List<MultiDimensionalInt> dp2dEditor = new List<MultiDimensionalInt>();
        public int randomRange = 10;
        public int row = 9;
        public int col = 9;
        public int initValue = 1; //把数组重置为某个数
        public static float k_margin = 50;

        public EDirection initFromDirection = EDirection.ROW_DOWN;
        public EColorMode mode = EColorMode.BLANK;


        void Start()
        {
        }

        private Color[] colors = { new Color(176, 23, 31), new Color(176,    48,  96) , new Color(255  , 192 ,203),
            new Color(61  ,  89 , 171),  new Color(0   , 199 ,140), new Color(51 ,   161, 201),
         new Color(199 , 97 , 20),  new Color(160  , 32 , 240), new Color(124 ,  252 ,0)};
        private int colorIndex = -1;
        public Color RandomColor()
        {
            colorIndex = ++colorIndex % colors.Length;
            return colors[colorIndex];
        }

        //考虑做四个方向??
        public Vector3 GetSidePosition(bool end)
        {
            return end ? dp2dEditor[0][col - 1].currentPosition + new Vector3(k_margin, 0, 0) : dp2dEditor[0][0].currentPosition + new Vector3(-k_margin, 0, 0);
        }

        [ContextMenu("RandomList")]
        public void RandomList()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    int val = Random.Range(0, randomRange);

                    if (mode == EColorMode.ARRAY2D0_1)
                    {
                        dp2dEditor[i][j].pointer.SetValue(val, val == 1 ? Color.green : Color.white);
                    }
                    else
                    {
                        dp2dEditor[i][j].pointer.SetValue(val, Color.white);
                    }
                }

            }
        }

        [ContextMenu("CleanList")]
        public void CleanList()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    dp2dEditor[i][j].pointer.SetValue(initValue, Color.white);
                }

            }
        }

        [ContextMenu("SetActiveIndex")]
        public void SetActiveIndex()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    dp2dEditor[i][j].SetActiveIndex();
                }
            }
        }

        public bool validIndex(int i, int j)
        {
            return i >= 0 && i < row && j >= 0 && j < col;
        }

        public void ResetColor()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (mode == EColorMode.ARRAY2D0_1)
                    {
                        dp2dEditor[i][j].pointer.ChangeColor(dp2dEditor[i][j].pointer.value == 1 ? Color.green : Color.white);
                    }
                    else
                    {
                        dp2dEditor[i][j].pointer.ChangeColor(Color.white);
                    }
                }
            }
        }

        [ContextMenu("CreateList")]
        public void CreateList()
        {
            arrayPrefab.gameObject.SetActive(true);

            ClearTag[] clearTags = GetComponentsInChildren<ClearTag>();
            foreach (var val in clearTags)
            {
                DestroyImmediate(val.gameObject);
            }

            dp2dEditor.Clear(); // 应该把第二层的一起清了????

            for (int i = 0; i < row; i++)
            {
                dp2dEditor.Add(new MultiDimensionalInt());
                for (int j = 0; j < col; j++)
                {
                    int val = Random.Range(0, randomRange);

                    int curI = i, curJ = j;
                    switch (initFromDirection)
                    {
                        case EDirection.ROW_DOWN:
                            break;
                        case EDirection.ROW_TOP:
                            curI = row - i;
                            break;
                        case EDirection.ROW_LEFT:
                            curI = j;
                            curJ = i;
                            break;
                        //case EDirection.ROW_RIGHT:
                        //    curI = j;
                        //    curJ = row - i;
                        //    break;
                        default:
                            break;
                    }

                    Item item = new Item(arrayPrefab, val, curI, curJ, row <= 1, initFromDirection);

                    dp2dEditor[i].intArray.Add(item);

                }
            }

            if (col > 0 && row > 0)
            {
                rowText.position = new Vector3(dp2dEditor[0][0].currentPosition.x - k_margin, dp2dEditor[0][0].currentPosition.y);
                colText.position = new Vector3(dp2dEditor[0][0].currentPosition.x, dp2dEditor[0][0].currentPosition.y + k_margin * (initFromDirection != EDirection.ROW_TOP ? -0.5f : 0.5f));
            }

            ResetColor();

            arrayPrefab.gameObject.SetActive(false);
        }

        private void AdjustPosition()
        {
            bl_UpdateManager.RunAction("", 0.3f, (realTime, deltaTime) =>
            {
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                    }
                }
            });
        }



        public int[] GetArray(int row)
        {
            int[] intArr = new int[col];
            for (int i = 0; i < col; i++)
            {
                intArr[i] = dp2dEditor[row][i].pointer.value;
            }
            return intArr;
        }

        public int[,] GetArray()
        {
            int[,] intArr = new int[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    intArr[i, j] = dp2dEditor[i][j].pointer.value;
                }
            }
            return intArr;
        }

        public List<List<int>> GetArr()
        {
            List<List<int>> intArr = new List<List<int>>();
            for (int i = 0; i < row; i++)
            {
                intArr.Add(new List<int>());
                for (int j = 0; j < col; j++)
                {
                    intArr[i].Add(dp2dEditor[i][j].pointer.value);
                }
            }
            return intArr;
        }
    }
}

