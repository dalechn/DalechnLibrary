using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[System.Serializable]
public class Order 
{
    public Queue<Vector3> foodPositionList = new Queue<Vector3>();
    public Vector3 customerPosition;   //这个是需要提前获取位置的                   //暂时没用了
    public Vector3 staffPosition;   //上菜的时候站的位置,因为需要动态判断站哪      //不用动态判断了
    public Customer customer;

    public Sprite foodSprite;
}
