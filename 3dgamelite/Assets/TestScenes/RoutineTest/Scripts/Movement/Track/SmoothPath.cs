using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SmoothPath : MonoBehaviour
{
    public Vector3 offset;
    public Transform target;
    public LayerMask kartMask = -1;
    public string coinTag = "Coin";
    public string flyCoinTag = "FlyCoin";
    public string selfTag = "Self";

    public float collisionForce = 10;
    public float maxRadius = 10;
    public float coinCooldown = 0.5f;
    //public float animationCooldown = 0.5f;
    public float speed = 5;
    public int strength = 1;
    public Vector3 size = Vector3.one;

    [Header("ReadOnly")]
    public int currentNumber = 0;
    public float circleRadius;
    public float unitLength;
    public float maxCount;
    public float minCount;
    //public float tempCooldowm = 0.5f;

    private CinemachineSmoothPath path;
    private Vector3 originScale;
    private float originRadius;

    private List<GameObject> coinList = new List<GameObject>();
    private List<CinemachineDollyCart> currentPositions = new List<CinemachineDollyCart>();

    public static Transform cubeDetachedParent;

    private void Start()
    {
        //tempCooldowm = animationCooldown;
        currentNumber = 0;
        path = GetComponentInChildren<CinemachineSmoothPath>();
        path.m_Looped = true;
        SphereCollider c = GetComponent<SphereCollider>();
        c.isTrigger = true;
        circleRadius = c.radius;

        originScale = transform.localScale;
        originRadius = circleRadius;

        unitLength = Mathf.Sqrt(size.x * size.x + size.z * size.z + size.y * size.y);
        //circleRadius = (minCount * unitLength) / (2 * Mathf.PI);
        minCount = 2 * Mathf.PI * circleRadius / unitLength;
        maxCount = maxRadius / circleRadius * minCount;

        List<CinemachineSmoothPath.Waypoint> pathSelfs = new List<CinemachineSmoothPath.Waypoint>();
        int k = 0;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += unitLength / 4, k++)
        {
            float x = circleRadius * Mathf.Cos(theta);
            float z = circleRadius * Mathf.Sin(theta);

            CinemachineSmoothPath.Waypoint pathSelf = new CinemachineSmoothPath.Waypoint();
            pathSelf.position = new Vector3(x, 0, z);
            pathSelfs.Add(pathSelf);
        }
        path.m_Waypoints = pathSelfs.ToArray();

        for (int i = 0; i < maxCount; i++)
        {
            GameObject cartGameObject = new GameObject("Cart");
            cartGameObject.transform.parent = transform;
            CinemachineDollyCart cart = cartGameObject.AddComponent<CinemachineDollyCart>();
            cart.m_Path = path;
            cart.m_Speed = speed;
            cart.m_UpdateMethod = CinemachineDollyCart.UpdateMethod.FixedUpdate;
            currentPositions.Add(cart);
        }

        if (cubeDetachedParent == null)
        {
            cubeDetachedParent = new GameObject("Cube - Detached").transform;
        }
    }

    private void FixedUpdate()
    {
        //tempCooldowm -= Time.deltaTime;
        //if (tempCooldowm <= 0)
        //{
        //    tempCooldowm = animationCooldown;
        //    UpdateTarget();
        //}

        //transform.eulerAngles = new Vector3(target.eulerAngles.x, transform.eulerAngles.y, target.eulerAngles.z);
        transform. position = target.position+ offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == coinTag && currentNumber < maxCount)
        {
            currentNumber++;

            if (currentNumber == 1)
            {
                coinList.Add(other.gameObject);
                //currentPositions[0].m_Position = Vector3.Angle(transform.InverseTransformPoint(other.transform.position), Vector3.right) * Mathf.PI * circleRadius / 180;
            }
            else if (currentNumber > 1)
            {
                coinList.Add(other.gameObject);
                //if (currentNumber <= 3)
                //{
                //    coinList.Add(other.gameObject);
                //}
                //else
                //{
                //    List<PositionInfo> sortList = new List<PositionInfo>();
                //    for (int i = 0; i < currentNumber - 1; i++)
                //    {
                //        sortList.Add(new PositionInfo(i, currentPositions[i]));
                //    }
                //    sortList.Sort(new PositionInfo(other.transform.position));
                //    coinList.Insert(sortList[0].index, other.gameObject);
                //}

                var tempPos = currentPositions[0].m_Position;
                var unit = path.PathLength / currentNumber;
                for (int j = 0; j < currentNumber; j++)
                {
                    currentPositions[j].m_Position = j * unit + tempPos;
                }
            }

            UnEffect effect = other.gameObject.AddComponent<UnEffect>();
            effect.Init(this);

            UpdateTarget();
        }
    }

    public void RemoveCoin(GameObject coin)
    {
        if (currentNumber > 0)
        {
            currentNumber--;

            var tempPos = currentPositions[0].m_Position;
            var unit = path.PathLength / currentNumber;
            for (int j = 0; j < currentNumber; j++)
            {
                currentPositions[j].m_Position = j * unit + tempPos;
            }
            coinList.Remove(coin);

            UpdateTarget();
            //tempCooldowm = animationCooldown;
        }
    }

    private void UpdateTarget()
    {
        for (int i = 0; i < coinList.Count; i++)
        {
            UnEffect t = coinList[i].GetComponent<UnEffect>();
            if (t)
                t.target = currentPositions[i].transform;
        }

        float rate = (currentNumber > minCount ? currentNumber * unitLength / (minCount * unitLength) : 1);
        path.transform.localScale = originScale * rate;
        circleRadius = originRadius * rate;
    }
}

public class PositionInfo : IComparer<PositionInfo>
{
    public int index;
    public CinemachineDollyCart cart;

    public Vector3 target;

    public int Compare(PositionInfo x, PositionInfo y)
    {
        var dis1 = Vector3.Distance(x.cart.transform.position, target);
        var dis2 = Vector3.Distance(y.cart.transform.position, target);

        if (dis1 > dis2)
            return 1;
        else if (dis1 < dis2)
            return -1;

        return 0;
    }

    public PositionInfo(Vector3 target)
    {
        this.target = target;
    }

    public PositionInfo(int index, CinemachineDollyCart cart)
    {
        this.index = index;
        this.cart = cart;
    }
}
