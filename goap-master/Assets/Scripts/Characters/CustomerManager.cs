using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public List<Transform> leftList;                         //�ֱ�Ϊ������ͻ��յ�,����������
    public List<Transform> rightList;

    public List<Transform> leftPointList = new List<Transform>();            //mid�ȴ��ĵط�, һ��Ҫ��˳���ʼ��!���Һ�rightPointList�����Ҫ��shopinfo��maxWaitNumber��ͬ
    public List<Transform> rightPointList = new List<Transform>();
    public List<Customer> currentWaitCustomer = new List<Customer>();
    public Transform midPoint;                                  //�����ж��Ƿ���Ա�����

    private Dictionary<Customer, Vector3> posDict = new Dictionary<Customer, Vector3>();
    //public int CurrentWaitNumber { get { return currentWaitCustomer.Count; } }   //��ǰ�ȴ�������


    private Dalechn.vFisherYatesRandom genRandom = new Dalechn.vFisherYatesRandom();
    private Dalechn.vFisherYatesRandom targetRandom = new Dalechn.vFisherYatesRandom();
    private Dalechn.vFisherYatesRandom customerRandom = new Dalechn.vFisherYatesRandom();

    private List<Customer> customerList = new List<Customer>();                 //�Ѿ�������customer


    void Start()
    {

    }

    public void AddWaitingCustomer(Customer customer)
    {
        currentWaitCustomer.Add(customer);

        //posDict.Add(customer, midPointList[currentWaitCustomer.Count-1].position);
        UpdatePos(customer, false);
    }

    public void RemoveWaitingCustomer(Customer customer)
    {
        //currentWaitCustomer.Remove(customer); //�����Ƴ��Լ� ,ֻҪ���˷��䵽λ�þ��Ƴ���һ��λ��

        currentWaitCustomer.RemoveAt(0);

        posDict.Remove(customer);

        UpdatePos(customer, true);
    }

    // ���������˵�λ����Ϣ
    //int updateTime = 1;      //˫�˶�,,�������ڸ���λ��,,,��������
    private void UpdatePos(Customer c, bool remove)
    {
        //if(remove)
        //{
        //    updateTime++;
        //    if (updateTime % 2 == 0)
        //        return;
        //}

        for (int i = 0; i < currentWaitCustomer.Count; i++)
        {
            //posDict[currentWaitCustomer[i]] = c.IsRight ? rightPointList[i].position : leftPointList[i].position;

            posDict[currentWaitCustomer[i]] = RandomCirclePosition(leftPointList[i].position);
        }
    }

    private Vector3 RandomCirclePosition(Vector3 pos)
    {
        const int radius = 2;
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        return pos + dir * Random.Range(0, radius);
    }

    public Vector3 GetWaitingPoint(Customer customer)
    {
        //int index = currentWaitCustomer.FindIndex(e => { return e == customer; });

        //if(index>=0&&index<midPointList.Count)
        //{
        //    pos = midPointList[index].position;

        //    //Debug.Log(customer.name+"  "+index);


        //    return false;
        //}

        //pos = default(Vector3);
        //return true;

        return posDict[customer];
    }

    public void StartGen(float genRate)
    {
        // �ӳٺ���������???
        //InvokeRepeating("GenCustomer", Time.deltaTime, genRate);
        InvokeRepeating("GenCustomer", 0, genRate);

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
    const int midDistance = 8;
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

    void OnDrawGizmos()
    {

        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(midPoint.position, Vector3.up, midDistance);

        foreach (var c in destroyList)
        {
            UnityEditor.Handles.DrawWireDisc(c.position, Vector3.up, recycledDistance);
        }
    }
}
