using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public Queue<Order> orderQueue = new Queue<Order>();

    void Start()
    {

    }

    //public void GenOrder(string orderName, Vector3 orderPosition)
    //{
    //    Order task = new Order();

    //    FoodTem tem = FoodTem.Tem(orderName);
    //    string[] splitParts = tem.Need.Split(';');
    //    foreach (var val in splitParts)
    //    {
    //        if (ShopInfo.Instance.furnitureDict.TryGetValue(val, out Slot furniture))
    //        {
    //            task.foodPositionList.Enqueue(furniture.transform.position);
    //        }
    //    }

    //    task.customerPosition = orderPosition;
    //    task.foodSpriteLocation = tem.Location;

    //    orderQueue.Enqueue(task);
    //}

    void Update()
    {

    }
}
