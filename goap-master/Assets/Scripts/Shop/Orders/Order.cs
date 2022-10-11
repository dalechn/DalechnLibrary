using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[System.Serializable]     //不要序列化 ,因为staff是根据order是否null来判断的 ,编辑器会自动实例化一个就不会是null了
//尽量确保只有shop和一开始的customer能写入,其他的都是只读!
public class Order 
{
    public string orderFoodName;        //订单名字
    public string foodSpriteLocation;       //最终的食物图片位置

    public Vector3 customerPosition;   //这个是需要提前获取位置的                   //暂时没用了
    public Vector3 staffPosition;   //上菜的时候站的位置,因为需要动态判断站哪      //不用动态判断了

    public int cookingScore;        //当前菜品的厨艺,取决于staff

    public bool canBeAssigned = true;   //是否能被分配                * 用来判断单子是否被手动操作
    public bool orderFinished;              //                                      * 用来判断单子是否订单没被处理的(顾客提前走人)或者正常结束的
    public Staff staff;                 //当前服务的服务员                      * 用来判断单子是否被系统分配
    public Customer customer;   //当前顾客

    public Queue<Food> foodQueue = new Queue<Food>();   //小食物的队列
    public Food currentFood;                                        //当前队列头的食物
    public string date;                     //订单产生的时间
    public float price;                     //价格
    public bool havePlate;              //是否有盘子

    public bool GetCurrentFood(out Food food)
    {
        if (foodQueue.TryDequeue(out  food))
        {
            currentFood = food;

            return true;
        }
        return false;
    }
}

public struct Food
{
    public Vector3 foodPosition;        //每个小食物对应的家具位置
    public float foodTime;                  //每个小食物的时间
    public string subFoodSpriteLocation;        //每个顺序的图片位置
}