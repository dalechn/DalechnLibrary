using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[System.Serializable]     //不要序列化 ,因为staff是根据order是否null来判断的 ,编辑器会自动实例化一个就不会是null了
public class Order 
{
    public Queue<Vector3> foodPositionList = new Queue<Vector3>();
    public Vector3 customerPosition;   //这个是需要提前获取位置的                   //暂时没用了
    public Vector3 staffPosition;   //上菜的时候站的位置,因为需要动态判断站哪      //不用动态判断了
    public Customer customer;

    public string foodSpriteLocation;
    public int cookingScore;        //当前菜品的厨艺,取决于staff
    public string orderFoodName;
}
