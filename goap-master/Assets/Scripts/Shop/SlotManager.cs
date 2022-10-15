using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyShop
{

    public class SlotManager : MonoBehaviour
    {
        //public MeshRenderer floor;
        // 每个家具/桌子start的时候会向这里注册
        //public Dictionary<string, Slot> furnitureDict = new Dictionary<string, Slot>();
        public List<Slot> tableList = new List<Slot>();
        public List<Slot> gameList = new List<Slot>();

        public Dictionary<GameObject, Slot> gameSlotList = new Dictionary<GameObject, Slot>();
        public Dictionary<GameObject, Slot> tableSlotDict = new Dictionary<GameObject, Slot>();
        public Dictionary<string, Slot> furnitureSlotDict = new Dictionary<string, Slot>();


        public void RegistSlot(string objName, Slot table, RegistName registName)
        {
            switch (registName)
            {
                case RegistName.Table:
                    {
                        tableList.Add(table);
                        foreach (var val in table.slotList)
                        {
                            tableSlotDict.Add(val, table);
                        }
                    }
                    break;

                case RegistName.Furniture:
                    {
                        //furnitureDict.Add(objName, table);
                        furnitureSlotDict.Add(objName, table);
                    }
                    break;

                case RegistName.Game:
                    {
                        gameList.Add(table);
                        foreach (var val in table.slotList)
                        {
                            gameSlotList.Add(val, table);
                        }
                    }
                    break;
                default:
                    break;
            }
        }


        public GameObject GetUeableGame()
        {
            List<GameObject> usableList = new List<GameObject>();
            foreach (var val in gameSlotList)
            {
                if (val.Key.activeInHierarchy)
                {
                    usableList.Add(val.Key);
                }
            }
            if (usableList.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, usableList.Count);

                return usableList[index];
            }
            return null;
        }

        public GameObject GetUeableTable()
        {
            List<GameObject> usableList = new List<GameObject>();

            foreach (var val in tableSlotDict)
            {
                if (val.Key.activeInHierarchy)
                {
                    usableList.Add(val.Key);
                }
            }
            if (usableList.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, usableList.Count);

                return usableList[index];
            }
            return null;
        }

        public void ToggleFurnitureEvent(bool en)
        {
            foreach (var val in gameList)
            {
                val.ToggleClick(en);
            }
            foreach (var val in tableList)
            {
                val.ToggleClick(en);
            }
            foreach (var val in furnitureSlotDict)
            {
                val.Value.ToggleClick(en);
            }
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
