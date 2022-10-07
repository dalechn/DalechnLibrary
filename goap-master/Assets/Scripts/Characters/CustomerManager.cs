using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    //public float genRate = 1.0f;
    //public List<Transform> customerList;

    public List<Transform> genList;
    public List<Transform> targetList;

    private Dalechn.vFisherYatesRandom genRandom = new Dalechn.vFisherYatesRandom();
    private Dalechn.vFisherYatesRandom targetRandom = new Dalechn.vFisherYatesRandom();
    private Dalechn.vFisherYatesRandom customerRandom = new Dalechn.vFisherYatesRandom();

    private List<Customer> customerList = new List<Customer>();

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
                customerTr.position = gen.position;  //...null pointer
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
    private List<Customer> destroyCustomerList = new List<Customer>();

    void Update()
    {
        destroyCustomerList.Clear();
        foreach (var val in destroyList)
        {
            foreach (var c in customerList)
            {
                if (c.unused && Vector3.Distance(val.position, c.tr.position) < 1)
                {
                    destroyCustomerList.Add(c);
                }
            }
        }

        foreach (var val in destroyCustomerList)
        {
            customerList.Remove(val);
            //Destroy(val.gameObject);

            val.Reset();
            PoolManager.Pools["CustomerPool"].Despawn(val.tr);
        }
    }
}
