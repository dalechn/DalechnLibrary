using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyShop;

//public enum OrderState
//{
//    Generated,BySystem,ByPlayer,Finished
//}

//[System.Serializable]     //��Ҫ���л� ,��Ϊstaff�Ǹ���order�Ƿ�null���жϵ� ,�༭�����Զ�ʵ����һ���Ͳ�����null��
//����ȷ��ֻ��shop��һ��ʼ��customer��д��,�����Ķ���ֻ��!
public class Order
{
    //public OrderState state = OrderState.Generated;
    public string orderFoodName { get; set; }        //��������
    public string foodSpriteLocation { get; set; }       //����Ա��������յ�ʳ��λ��
    public Transform foodPrefabLocation { get; set; }      //��ǰ��ʳ��prefab·�����߶���ص�����,���ڸĳ�  ����������ͬʱֱ������prefab.
    public Transform tableFoodPosition { get; set; }  //�������Ҫ��ǰ��ȡλ�õ�                   //��ʱû����,�����ǿ��˵�λ��,����ʳ���λ�ð�
    public Transform staffPosition { get; set; } //�ϲ˵�ʱ��վ��λ��,��Ϊ��Ҫ��̬�ж�վ��      //���ö�̬�ж���

    public int cookingScore { get; set; }      //��ǰ��Ʒ�ĳ���,ȡ����staff

    public bool canBeAssigned = true;   //�Ƿ��ܱ�����                * �����жϵ����Ƿ��ֶ�����
    public bool orderFinished { get; set; }            //                                      * �����жϵ����Ƿ񶩵�û�������(�˿���ǰ����)��������������
    public Staff staff { get; set; }             //��ǰ����ķ���Ա                      * �����жϵ����Ƿ�ϵͳ����
    public Customer customer { get; set; }  //��ǰ�˿�

    public Slot slot { get; set; }  //��ǰ������

    public List<Food> foodList = new List<Food>();          //������foodlist
    public Queue<Food> foodQueue = new Queue<Food>();   //Сʳ��Ķ���
    public Food currentFood { get; set; }                                    //��ǰ����ͷ��ʳ��
    public string date { get; set; }                   //����������ʱ��(��ʵʱ��)
    public float price { get; set; }                    //�۸�
    public bool havePlate { get; set; }          //�Ƿ�������

    public bool GetCurrentFood(out Food food)
    {
        if (foodQueue.TryDequeue(out food))
        {
            currentFood = food;

            return true;
        }
        return false;
    }
}

public struct Food
{
    public Slot foodSlot;           //��ǰ�ļҾ�
    public Transform tagPosition { get; set; }  //�ֶ�����ʱtag��λ��
    public Transform foodPosition { get; set; }      //ÿ��Сʳ���Ӧ�ļҾ�λ��
    public float foodTime { get; set; }              //ÿ��Сʳ���ʱ��
    public string subFoodSpriteLocation { get; set; }      //ÿ��˳���ͼƬλ��
}