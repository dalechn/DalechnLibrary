using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public List<Transform> leftList;                         //分别为诞生点和回收点,随机左或者右
    public List<Transform> rightList;

    public List<Transform> leftPointList = new List<Transform>();            //mid等待的地方, 一定要按顺序初始化!而且和rightPointList相加需要和shopinfo的maxWaitNumber相同
    public List<Transform> rightPointList = new List<Transform>();
    public List<Customer> currentWaitCustomer = new List<Customer>();
    public Transform midPoint;                                  //用来判断是否可以被回收

    private Dictionary<Customer, Vector3> posDict = new Dictionary<Customer, Vector3>();
    //public int CurrentWaitNumber { get { return currentWaitCustomer.Count; } }   //当前等待的人数


    private Dalechn.vFisherYatesRandom genRandom = new Dalechn.vFisherYatesRandom();
    private Dalechn.vFisherYatesRandom targetRandom = new Dalechn.vFisherYatesRandom();
    private Dalechn.vFisherYatesRandom customerRandom = new Dalechn.vFisherYatesRandom();

    private List<Customer> customerList = new List<Customer>();                 //已经诞生的customer


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
        //currentWaitCustomer.Remove(customer); //不是移除自己 ,只要有人分配到位置就移除第一个位置

        currentWaitCustomer.RemoveAt(0);

        posDict.Remove(customer);

        UpdatePos(customer, true);
    }

    // 更新所有人的位置信息
    //int updateTime = 1;      //双人队,,走两个在更新位置,,,哈哈哈哈
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
        // 延迟好像有问题???
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
    const int midDistance = 8;
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
