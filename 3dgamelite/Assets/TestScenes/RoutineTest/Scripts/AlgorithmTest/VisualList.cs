using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dalechn
{
    public enum EOPERATION
    {
        NULL,
        //水平翻转              //垂直翻转
        FLIP_HORIZONTAL, FLIP_VERTICAL,
        //主对角线(transpose)(左上-右下) , //副对角线(左下-右上)   //NB:只适用于方阵
        MAIN_DIAGONAL, COUNTER_DIAGONAL,
        ROTATE_CLOCKWISE, ROTATE_ANTICLOCKWISE,
    }

    public enum EColorMode { BLANK, ARRAY2D0_1 }
    public enum ELayout { BOTTOM_TOP,TOP_BOTTOM,LEFT_RIGHT,RIGHT_LEFT}

    public class VisualList : MonoBehaviour
    {
        [System.Serializable]
        public class MultiDimensionalInt
        {
            public List<VisualItem> intArray = new List<VisualItem>();

            public VisualItem this[int index] => intArray[index];

            public int Count => intArray.Count;

            public void Add(VisualItem item)
            {
                intArray.Add(item);
            }

            public void Insert(int index, VisualItem item)
            {
                intArray.Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                intArray.RemoveAt(index);
            }
        }


        public Transform rowText;
        public Transform colText;
        public VisualItem arrayPrefab;

        public PointerList pointerListUp;
        public PointerList pointerListLeft;
        public PointerList pointerListRight;

        public List<MultiDimensionalInt> dp2dEditor = new List<MultiDimensionalInt>();
        public int randomRange = 10;
        [Range(1,100)]
        public int row = 9; // 最少要有一行
        [Range(0, 100)]
        public int col = 9;
        public int initValue = 1; //把数组重置为某个数
        public int addDelIndex; //增加/删除的位置
        public static float k_Margin = 50;

        public EColorMode mode = EColorMode.BLANK;
        public ELayout layout = ELayout.BOTTOM_TOP;

        private List<List<Vector3>> virtualPositionList = new List<List<Vector3>>();

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
            return end ? dp2dEditor[0][col - 1].currentPosition + new Vector3(k_Margin, 0, 0) : dp2dEditor[0][0].currentPosition + new Vector3(-k_Margin, 0, 0);
        }

        [ContextMenu("CleanList")]
        public void CleanList()
        {
            for (int i = 0; i < dp2dEditor.Count; i++)
            {
                for (int j = 0; j < dp2dEditor[0].Count; j++)
                {
                    dp2dEditor[i][j].SetValue(initValue);
                }

            }
        }

        [ContextMenu("SetActiveIndex")]
        public void SetActiveIndex()
        {
            for (int i = 0; i < dp2dEditor.Count; i++)
            {
                for (int j = 0; j < dp2dEditor[0].Count; j++)
                {
                    dp2dEditor[i][j].SetActiveIndex();
                }
            }
        }

        public bool ValidIndex(int i, int j)
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
                        dp2dEditor[i][j].ChangeColor((int)(dp2dEditor[i][j].value) == 1 ? ColorUtils.green : ColorUtils.white);
                    }
                    else
                    {
                        dp2dEditor[i][j].ChangeColor(ColorUtils.white);
                    }
                }
            }
        }


        [ContextMenu("CreateList")]
        public void CreateList()
        {
            InitVirtualPositin();

            VisualItem[] clearTags = GetComponentsInChildren<VisualItem>();
            foreach (var val in clearTags)
            {
                if (val != arrayPrefab)
                {
                    DestroyImmediate(val.gameObject);
                }
            }

            dp2dEditor.Clear(); // 应该把第二层的一起清了????

            for (int i = 0; i < row; i++)
            {
                dp2dEditor.Add(new MultiDimensionalInt());
                for (int j = 0; j < col; j++)
                {
                    int val = Random.Range(0, randomRange);

                    VisualItem item = Instantiate(arrayPrefab, transform.position, transform.rotation, transform);
                    item.SetIndex(i, j, row <= 1);
                    item.SetValue(val);
                    item.Position = virtualPositionList[i + 1][j + 1]; //+1是因为做了包围盒初始位置就是从1,1开始

                    dp2dEditor[i].Add(item);
                }
            }

            if (col > 0 && row > 0)
            {
                rowText.position = new Vector3(virtualPositionList[1][0].x, virtualPositionList[1][0].y);
                colText.position = new Vector3(virtualPositionList[0][1].x, virtualPositionList[0][1].y);
            }

            ResetColor();
        }

        public int GetRow0Last()
        {
            if (dp2dEditor.Count > 0)
            {
                return dp2dEditor[0].Count>0 ?dp2dEditor[0].Count-1:0;
            }
            else
            {
                return 0;
            }
        }

        //创建包围盒
        public void InitVirtualPositin()
        {
            virtualPositionList.Clear();
            int localCol = Mathf.Max(GetRow0Last()+1, col);
            int localRow = Mathf.Max(dp2dEditor.Count, row); // max函数是用于dp2dEditor初始化的时候
            //Debug.Log(localRow);
            //Debug.Log(localCol);

            for (int i = 0; i < localRow + 2; i++)
            {
                virtualPositionList.Add(new List<Vector3>());
                for (int j = 0; j < localCol + 2; j++)
                {
                    int realI = i;
                    int realJ = j;
                    if (layout ==ELayout.TOP_BOTTOM)
                    {
                        realI = localCol + 1 -i; //local -1 -i +2
                    }
                    else if(layout == ELayout.LEFT_RIGHT)
                    {
                        realI = j;
                        realJ = i;
                    }
                    else if(layout == ELayout.RIGHT_LEFT)
                    {
                        realI = j;
                        realJ = localRow + 1-i;
                    }
                    Vector3 pos = transform.position + new Vector3(realJ * k_Margin, realI * k_Margin);

                    virtualPositionList[i].Add(pos);
                }
            }

        }

        //只在第一行做操作
        [ContextMenu("Add")]
        public void AddEditor()
        {
            Add(Random.Range(0, randomRange),addDelIndex);
        }

        [ContextMenu("Delete")]
        public void DeleteEditor()
        {
            Delete(addDelIndex);
        }

        public void AddLast(int value, string valueStr = null)
        {
            Add(GetRow0Last(), value,valueStr);
        }

        public void DeleteFront()
        {
            Delete(0);
        }

        public void DeleteLast()
        {
            Delete(GetRow0Last());
        }

        //这样写是真的jb蠢
        public void Add(int index,int value, string valueStr = null)
        {
            InitVirtualPositin();

            VisualItem item = Instantiate(arrayPrefab, transform.position, transform.rotation, transform);
            item.Position = virtualPositionList[1][1]; //直接在初始位置生成
            item.SetString(valueStr);
            if (valueStr ==null)
            {
                item.SetValue(value);
            }

            dp2dEditor[0].Insert(index, item);

            for(int i = 0; i < dp2dEditor[0].Count;i++)
            {
                dp2dEditor[0][i].SetIndex(0, i, row <= 1);
            }

            ResetColor();

            AdjustPosition();
        }

        public void Delete(int index)
        {
            InitVirtualPositin(); // 不写会有序列化问题

            Destroy(dp2dEditor[0][index].gameObject);

            dp2dEditor[0].RemoveAt(index);
            for (int i = 0; i < dp2dEditor[0].Count; i++)
            {
                dp2dEditor[0][i].SetIndex(0, i, row <= 1);
            }

            AdjustPosition();
        }

        private void AdjustPosition(UnityAction act = null)
        {
            bool hasAction = bl_UpdateManager.s_Instance.HasAction("AdjustPosition");
            if (!hasAction)
            {
                bl_UpdateManager.RunAction("AdjustPosition", 0.3f, (t, r) =>
                {
                    for (int i = 0; i < dp2dEditor[0].Count; i++)
                    {
                        dp2dEditor[0][i].Position = Vector3.Lerp(dp2dEditor[0][i].Position, virtualPositionList[1][i + 1], t);
                    }
                }, act);
            }
        }


        public int[] GetArray(int row)
        {
            int[] intArr = new int[col];
            for (int i = 0; i < col; i++)
            {
                intArr[i] = (int)(dp2dEditor[row][i].value);
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
                    intArr[i, j] = (int)(dp2dEditor[i][j].value);
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
                    int val = (int)(dp2dEditor[i][j].value);
                    intArr[i].Add(val);
                }
            }
            return intArr;
        }
    }
}

