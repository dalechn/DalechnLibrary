using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[System.Serializable]
public class Order 
{
    public Queue<Vector3> foodPositionList = new Queue<Vector3>();
    public Vector3 customerPosition;   //�������Ҫ��ǰ��ȡλ�õ�                   //��ʱû����
    public Vector3 staffPosition;   //�ϲ˵�ʱ��վ��λ��,��Ϊ��Ҫ��̬�ж�վ��      //���ö�̬�ж���
    public Customer customer;

    public Sprite foodSprite;
}
