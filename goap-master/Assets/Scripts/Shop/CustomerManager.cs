using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyShop;

namespace MyShop
{

    public class CustomerManager : MonoBehaviour
    {
        public List<Transform> leftList;                         //�ֱ�Ϊ������ͻ��յ�,����������
        public List<Transform> rightList;

        public List<Transform> leftPointList = new List<Transform>();            //mid�ȴ��ĵط�, һ��Ҫ��˳���ʼ��!���Һ�rightPointList�����Ҫ��shopinfo��maxWaitNumber��ͬ
        public List<Transform> rightPointList = new List<Transform>();
        public List<Customer> leftWaitCustomer = new List<Customer>();
        public List<Customer> rightWaitCustomer = new List<Customer>();
        public Transform midPoint;                                  //�����ж��Ƿ���Ա�����

        private Dictionary<Customer, Vector3> leftPosDict = new Dictionary<Customer, Vector3>();
        private Dictionary<Customer, Vector3> rightPosDict = new Dictionary<Customer, Vector3>();
        //public int CurrentWaitNumber { get { return currentWaitCustomer.Count; } }   //��ǰ�ȴ�������


        private Dalechn.vFisherYatesRandom genRandom = new Dalechn.vFisherYatesRandom();
        private Dalechn.vFisherYatesRandom targetRandom = new Dalechn.vFisherYatesRandom();
        private Dalechn.vFisherYatesRandom customerRandom = new Dalechn.vFisherYatesRandom();

        private List<Customer> customerList = new List<Customer>();                 //�Ѿ�������customer


        void Start()
        {

        }

        bool hideMode = false;          //�Ƿ��������ģʽ(��Ҫ����customer��)
        public void ToggleCustomer(bool en, bool toggleAll)
        {
            foreach (var val in customerList)
            {
                if (toggleAll)
                {
                    hideMode = !en;
                    val.TogglePerson(en);           //��ȫ��
                    MessageCenter.Instance.ToggleAllCanvas(val.gameObject, en);
                }
                else
                {
                    //val.ToggleOutline(!en);       //ֻ��ʾoutline
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

            if (rightCount >= num && leftCount >= num)      //������������� �Ͱ�����������
            {
                if ((leftCount >= sideMax || customer.IsRight) && rightCount < sideMax)         //���3����������ҲҪ�ŵ��ұ�
                    rightWaitCustomer.Add(customer);
                else
                {
                    leftWaitCustomer.Add(customer);
                }

            }
            else
            {
                if ((rightCount - leftCount) > 1 || rightCount >= num)     //��������Ż�û�� �ı����˾ͽ��ı�,ֱ��һ�߳���5��
                    leftWaitCustomer.Add(customer);
                else
                    rightWaitCustomer.Add(customer);
            }

            UpdatePos();
        }

        public void RemoveWaitingCustomer(Customer customer)
        {
            //currentWaitCustomer.Remove(customer); //�����Ƴ��Լ� ,ֻҪ���˷��䵽λ�þ��Ƴ���һ��λ��
            if (rightWaitCustomer.Exists(e => { return e == customer; })) //��������ұ� �������,��Ϊ���ǰ���isRight�������
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

        // ���������˵�λ����Ϣ
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
            const float radius = 1;           //����İ뾶
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            return pos + dir * Random.Range(0, radius);
        }

        public Vector3 GetWaitingPoint(Customer customer)
        {
            if (rightWaitCustomer.Exists(e => { return e == customer; })) //��������ұ� �������,��Ϊ���ǰ���isRight�������
            {
                return rightPosDict[customer];
            }
            else if (leftWaitCustomer.Exists(e => { return e == customer; }))
            {
                return leftPosDict[customer];
            }

            return midPoint.position;       //��ֹ��û�����ж�Ȧ�ӵ�ʱ�����λ��
        }

        public void StartGen(float genRate)
        {
            // �ӳٺ���������???
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
                    Dalechn.bl_UpdateManager.RunActionOnce("", Time.deltaTime, () =>//startע������֮���ٷ���Ϣ,��Ȼ���ܻ᲻ִ��?
                    {          
                        MessageCenter.Instance.ToggleAllCanvas(customer.gameObject, false);
                    });
                }
                else
                {
                    customer.TogglePerson(true);           // ���ܽ���������ص����� ?
                }

                customerList.Add(customer);

                int r = UnityEngine.Random.Range(0, 2); //��������� �� ���� ��
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
                //��ֹ·�˲�������
                if (Vector3.SqrMagnitude(midPoint.position - c.tr.position) < Mathf.Pow(midDistance, 2))  //�������᲻��õ�,,
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
