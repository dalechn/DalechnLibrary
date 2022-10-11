using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public List<Transform> genList;
    public List<Transform> targetList;

    private Dalechn.vFisherYatesRandom genRandom = new Dalechn.vFisherYatesRandom();
    private Dalechn.vFisherYatesRandom targetRandom = new Dalechn.vFisherYatesRandom();
    private Dalechn.vFisherYatesRandom customerRandom = new Dalechn.vFisherYatesRandom();

    private List<Customer> customerList = new List<Customer>();


    public Transform midPoint;
    public List<Transform> midPointList = new List<Transform>();
    private HashSet<Customer> currentWaitCustomer = new HashSet<Customer>();

    void Start()
    {

    }

    public void StartGen(float genRate)
    {
        // 延迟好像有问题???
        //InvokeRepeating("GenCustomer", Time.deltaTime, genRate);
        InvokeRepeating("GenCustomer", 0, genRate);

        foreach (var val in genList)
        {
            destroyList.Add(val);
        }
        foreach (var val in targetList)
        {
            destroyList.Add(val);
        }
    }

    void GenCustomer()
    {
        Transform gen = GetRandomPosition(genList, genRandom);
        Transform target = GetRandomPosition(targetList, targetRandom);

        Transform customerTr = GetRandomObj();
        if (customerTr)
        {
            Customer customer = customerTr.GetComponent<Customer>();

            customerList.Add(customer);

            int r = UnityEngine.Random.Range(0, 2);
            if (r < 1)
            {
                customerTr.position = gen.position;  
                customer.LeaveTarget = target.gameObject;
            }
            else
            {
                customerTr.position = target.position;
                customer.LeaveTarget = gen.gameObject;
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

    Transform GetRandomPosition(List<Transform> trs, Dalechn.vFisherYatesRandom random)
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
            if (Vector3.SqrMagnitude(midPoint.position- c.tr.position) <Mathf.Pow(midDistance, 2))  //不开方会不会好点,,
            {
                if (!c.Unused&&!c.GetInto())
                {
                    c.Emoji(MessageType.PasserBy, 0.3f);
                }
                c.Unused = true;
            }

            foreach (var val in destroyList)
            {
                if (c.Unused && Vector3.SqrMagnitude(val.position-c.tr.position) < Mathf.Pow(recycledDistance, 2))
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
