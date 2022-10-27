using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyShop;

namespace MyShop
{

    public class CustomerManager : MonoBehaviour
    {
        public List<Transform> leftList;                         //分别为诞生点和回收点,随机左或者右
        public List<Transform> rightList;

        public List<Transform> leftPointList = new List<Transform>();            //mid等待的地方, 一定要按顺序初始化!而且和rightPointList相加需要和shopinfo的maxWaitNumber相同
        public List<Transform> rightPointList = new List<Transform>();
        public List<Customer> leftWaitCustomer = new List<Customer>();
        public List<Customer> rightWaitCustomer = new List<Customer>();
        public Transform midPoint;                                  //用来判断是否可以被回收

        private Dictionary<Customer, Vector3> leftPosDict = new Dictionary<Customer, Vector3>();
        private Dictionary<Customer, Vector3> rightPosDict = new Dictionary<Customer, Vector3>();
        //public int CurrentWaitNumber { get { return currentWaitCustomer.Count; } }   //当前等待的人数


        private Dalechn.vFisherYatesRandom genRandom = new Dalechn.vFisherYatesRandom();
        private Dalechn.vFisherYatesRandom targetRandom = new Dalechn.vFisherYatesRandom();
        private Dalechn.vFisherYatesRandom customerRandom = new Dalechn.vFisherYatesRandom();

        private List<Customer> customerList = new List<Customer>();                 //已经诞生的customer


        void Start()
        {

        }

        bool hideMode = false;          //是否进入其他模式(需要隐藏customer的)
        public void ToggleCustomer(bool en, bool toggleAll)
        {
            foreach (var val in customerList)
            {
                if (toggleAll)
                {
                    hideMode = !en;
                    val.TogglePerson(en);           //关全部
                    MessageCenter.Instance.ToggleAllCanvas(val.gameObject, en);
                }
                else
                {
                    //val.ToggleOutline(!en);       //只显示outline
                    MessageCenter.Instance.ToggleHandleCanvas(val.gameObject, en);
                }
            }
        }

        public void AddWaitingCustomer(Customer customer)
        {
            int num = 5;
            int sideMax = ShopInfo.Instance.currentScore.maxWaitNumber / 2;
            int leftCount = leftWaitCustomer.Count;
            int rightCount = rightWaitCustomer.Count;

            if (rightCount >= num && leftCount >= num)      //如果竖着排满了 就按照左右来进
            {
                if ((leftCount >= sideMax || customer.IsRight) && rightCount < sideMax)         //左边3个又排满了也要排到右边
                    rightWaitCustomer.Add(customer);
                else
                {
                    leftWaitCustomer.Add(customer);
                }

            }
            else
            {
                if ((rightCount - leftCount) > 1 || rightCount >= num)     //如果竖着排还没满 哪边少人就进哪边,直到一边超过5了
                    leftWaitCustomer.Add(customer);
                else
                    rightWaitCustomer.Add(customer);
            }

            UpdatePos();
        }

        public void RemoveWaitingCustomer(Customer customer)
        {
            //currentWaitCustomer.Remove(customer); //不是移除自己 ,只要有人分配到位置就移除第一个位置
            if (rightWaitCustomer.Exists(e => { return e == customer; })) //如果不在右边 就在左边,因为不是按照isRight分配的了
            {
                rightWaitCustomer.RemoveAt(0);
                rightPosDict.Remove(customer);
            }
            else if (leftWaitCustomer.Exists(e => { return e == customer; }))
            {
                leftWaitCustomer.RemoveAt(0);
                leftPosDict.Remove(customer);
            }

            UpdatePos();
        }

        // 更新所有人的位置信息
        public void UpdatePos()
        {
            for (int i = 0; i < leftWaitCustomer.Count; i++)
            {
                leftPosDict[leftWaitCustomer[i]] = RandomCirclePosition(leftPointList[i].position);
            }
            for (int i = 0; i < rightWaitCustomer.Count; i++)
            {
                rightPosDict[rightWaitCustomer[i]] = RandomCirclePosition(rightPointList[i].position);
            }
        }

        private Vector3 RandomCirclePosition(Vector3 pos)
        {
            const float radius = 1;           //随机的半径
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            return pos + dir * Random.Range(0, radius);
        }

        public Vector3 GetWaitingPoint(Customer customer)
        {
            if (rightWaitCustomer.Exists(e => { return e == customer; })) //如果不在右边 就在左边,因为不是按照isRight分配的了
            {
                return rightPosDict[customer];
            }
            else if (leftWaitCustomer.Exists(e => { return e == customer; }))
            {
                return leftPosDict[customer];
            }

            return midPoint.position;       //防止还没进入判断圈子的时候给的位置
        }

        public void StartGen(float genRate)
        {
            // 延迟好像有问题???
            //InvokeRepeating("GenCustomer", Time.deltaTime, genRate);
            InvokeRepeating("GenCustomer", 1.0f, genRate);

            foreach (var val in leftList)
            {
                destroyList.Add(val);
            }
            foreach (var val in rightList)
            {
                destroyList.Add(val);
            }
        }

        private void GenCustomer()
        {
            Transform left = GetRandomPosition(leftList, genRandom);
            Transform right = GetRandomPosition(rightList, targetRandom);

            Transform customerTr = GetRandomObj();
            if (customerTr)
            {
                Customer customer = customerTr.GetComponent<Customer>();

                if (hideMode)
                {
                    customer.TogglePerson(false);
                    Dalechn.bl_UpdateManager.RunActionOnce("", Time.deltaTime, () =>//start注册完了之后再发消息,不然可能会不执行?
                    {          
                        MessageCenter.Instance.ToggleAllCanvas(customer.gameObject, false);
                    });
                }
                else
                {
                    customer.TogglePerson(true);           // 可能解决人物隐藏的问题 ?
                }

                customerList.Add(customer);

                int r = UnityEngine.Random.Range(0, 2); //随机诞生在 左 或者 右
                if (r < 1)
                {
                    customerTr.position = left.position;
                    customer.LeaveTarget = right.gameObject;
                }
                else
                {
                    customer.IsRight = true;
                    customerTr.position = right.position;
                    customer.LeaveTarget = left.gameObject;
                }
            }
        }

        private Transform GetRandomObj()
        {
            PrefabsDict dict = PoolManager.Pools[GlobalConfig.CustomerPool].prefabs;
            int index = customerRandom.Next(dict.Count);
            int i = 0;
            string objName = "";
            foreach (var val in dict)
            {
                if (i == index)
                {
                    objName = val.Key;
                }
                //Debug.Log(val.Key);
                i++;
            }

            Transform tr = PoolManager.Pools[GlobalConfig.CustomerPool].Spawn(objName);
            return tr;
        }

        private Transform GetRandomPosition(List<Transform> trs, Dalechn.vFisherYatesRandom random)
        {
            if (trs.Count > 0)
            {
                int index = random.Next(trs.Count);

                return trs[index];
            }
            return null;
        }

        private List<Transform> destroyList = new List<Transform>();
        private HashSet<Customer> destroyCustomerList = new HashSet<Customer>();

        const int recycledDistance = 5;
        const int midDistance = 12;
        void Update()
        {
            destroyCustomerList.Clear();

            foreach (var c in customerList)
            {
                //防止路人不被回收
                if (Vector3.SqrMagnitude(midPoint.position - c.tr.position) < Mathf.Pow(midDistance, 2))  //不开方会不会好点,,
                {
                    //if (!c.Unused && !c.GetInto())
                    //{
                    //    c.Emoji(MessageType.PasserBy, 0.3f);
                    //}
                    c.Unused = true;
                }

                foreach (var val in destroyList)
                {
                    if (c.Unused && Vector3.SqrMagnitude(val.position - c.tr.position) < Mathf.Pow(recycledDistance, 2))
                    {
                        destroyCustomerList.Add(c);
                    }
                }
            }

            foreach (var val in destroyCustomerList)
            {
                customerList.Remove(val);
                //Destroy(val.gameObject);

                //val.Reset();
                PoolManager.Pools["CustomerPool"].Despawn(val.tr);
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {

            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireDisc(midPoint.position, Vector3.up, midDistance);

            foreach (var c in destroyList)
            {
                UnityEditor.Handles.DrawWireDisc(c.position, Vector3.up, recycledDistance);
            }
        }
#endif
    }
}
