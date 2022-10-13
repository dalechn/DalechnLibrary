using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyShop;

//[System.Serializable]     //不要序列化 ,因为staff是根据order是否null来判断的 ,编辑器会自动实例化一个就不会是null了
//尽量确保只有shop和一开始的customer能写入,其他的都是只读!
public class Order 
{
    public string orderFoodName { get; set; }        //订单名字
    public string foodSpriteLocation { get; set; }       //最终的食物图片位置

    public Transform tableFoodPosition { get; set; }  //这个是需要提前获取位置的                   //暂时没用了,本来是客人的位置,当作食物的位置吧
    public Transform staffPosition { get; set; } //上菜的时候站的位置,因为需要动态判断站哪      //不用动态判断了

    public int cookingScore{ get; set; }      //当前菜品的厨艺,取决于staff

public bool canBeAssigned = true;   //是否能被分配                * 用来判断单子是否被手动操作
    public bool orderFinished { get; set; }            //                                      * 用来判断单子是否订单没被处理的(顾客提前走人)或者正常结束的
    public Staff staff { get; set; }             //当前服务的服务员                      * 用来判断单子是否被系统分配
    public Customer customer { get; set; }  //当前顾客

    public List<Food> foodList = new List<Food>();          //完整的foodlist
    public Queue<Food> foodQueue = new Queue<Food>();   //小食物的队列
    public Food currentFood { get; set; }                                    //当前队列头的食物
    public string date { get; set; }                   //订单产生的时间(真实时间)
    public float price { get; set; }                    //价格
    public bool havePlate { get; set; }          //是否有盘子

    public bool GetCurrentFood(out Food food)
    {
        if (foodQueue.TryDequeue(out  food))
        {
            currentFood = food;

            return true;
        }
        return false;
    }

    public void ToggleFoodTag(int index,bool en)
    {
        //if (index < foodList.Count)
        //{
        //    foodList[index].tagPosition.gameObject.SetActive(en);
        //}
        //// 显示/关闭每个小食物的tag
        //foreach (var val in foodList)
        //{
        //    val.tagPosition.gameObject.SetActive(!canBeAssigned);
        //}
    }
}

public struct Food
{
    public Transform tagPosition { get; set; }  //手动操作时标记的位置
    public Transform foodPosition { get; set; }      //每个小食物对应的家具位置
    public float foodTime { get; set; }              //每个小食物的时间
    public string subFoodSpriteLocation { get; set; }      //每个顺序的图片位置
}