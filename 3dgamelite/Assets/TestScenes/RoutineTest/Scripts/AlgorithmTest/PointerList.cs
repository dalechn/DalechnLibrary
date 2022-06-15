using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dalechn
{
    public class PointerList : MonoBehaviour
    {
        public List<DynamicPointer> pointerList = new List<DynamicPointer>();
        public LinkedList<DynamicPointer> pointerLinkList = new LinkedList<DynamicPointer>();
        //public HashSet<DynamicPointer> pointerSet = new HashSet<DynamicPointer>();
        public Dictionary<DynamicPointer, VisualList.Item> pointerDict = new Dictionary<DynamicPointer, VisualList.Item>();

        public DynamicPointer prefab;

        private VisualList list;

        public void Add(DynamicPointer dp, VisualList.Item item)
        {
            pointerList.Add(dp);
            pointerLinkList.AddLast(dp);
            //pointerSet.Add(dp);
            pointerDict.Add(dp, item);
        }

        public DynamicPointer GetCurrent()
        {
            return pointerList.Count > 0 ? pointerList[pointerList.Count - 1] : null;
        }

        // 需要把位置设置在边界时使用
        public void Init(ref List<Info> info, bool end, int color = ColorUtils.white)
        {
            DynamicPointer newPointer = Instantiate(prefab, transform.parent);
            Add(newPointer, null);
            newPointer.transform.position = list.GetSidePosition(end);

            SetValue(ref info, color);
        }

        //移动不需要数据改变时使用
        public void Translate(VisualList.Item item, bool showTrail = true, int color = ColorUtils.white)
        {
            List<Info> info = new List<Info>();
            Translate(ref info, item, showTrail, color);
        }

        public void Translate(ref List<Info> info, VisualList.Item item, bool showTrail = true, int color = ColorUtils.white)
        {
            DynamicPointer pointer = GetCurrent();

            //必须是新的item才能移动
            if (pointer && pointerDict[pointer] == item)
            {
                return;
            }

            //隐藏上一个对象
            if (!showTrail && pointer)
            {
                pointer.SetActive(false);
            }

            DynamicPointer newPointer = Instantiate(prefab, transform.parent);
            newPointer.Init(item.currentPosition);

            //foreach (var val in pointerDict)
            //{
            //    if (val.Key.active&&val.Value == item)
            //    {
            //        newPointer.Scale();
            //        break;
            //    }
            //}

            Add(newPointer, item);
            SetValue(ref info, color);
        }

        public void SetActivePath(bool active = false)
        {
            for (int i = 0; i < pointerList.Count; i++)
            {
                pointerList[i].SetActive(active);
            }
        }

        public void ChangeColor(int color = ColorUtils.white)
        {
            DynamicPointer pointer = GetCurrent();

            pointer.image.color = ColorUtils.Int2Color(color);
        }

        public void SetValue(ref List<Info> info, int color = ColorUtils.white)
        {
            DynamicPointer pointer = GetCurrent();
            pointer.info = info;
            pointer.Show();

            ChangeColor(color);
        }

        void Awake()
        {
            list = FindObjectOfType<VisualList>();
        }

    }
}
