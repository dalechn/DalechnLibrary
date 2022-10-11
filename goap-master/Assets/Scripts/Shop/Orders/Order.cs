using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[System.Serializable]     //��Ҫ���л� ,��Ϊstaff�Ǹ���order�Ƿ�null���жϵ� ,�༭�����Զ�ʵ����һ���Ͳ�����null��
//����ȷ��ֻ��shop��һ��ʼ��customer��д��,�����Ķ���ֻ��!
public class Order 
{
    public string orderFoodName;        //��������
    public string foodSpriteLocation;       //���յ�ʳ��ͼƬλ��

    public Vector3 customerPosition;   //�������Ҫ��ǰ��ȡλ�õ�                   //��ʱû����
    public Vector3 staffPosition;   //�ϲ˵�ʱ��վ��λ��,��Ϊ��Ҫ��̬�ж�վ��      //���ö�̬�ж���

    public int cookingScore;        //��ǰ��Ʒ�ĳ���,ȡ����staff

    public bool canBeAssigned = true;   //�Ƿ��ܱ�����                * �����жϵ����Ƿ��ֶ�����
    public bool orderFinished;              //                                      * �����жϵ����Ƿ񶩵�û�������(�˿���ǰ����)��������������
    public Staff staff;                 //��ǰ����ķ���Ա                      * �����жϵ����Ƿ�ϵͳ����
    public Customer customer;   //��ǰ�˿�

    public Queue<Food> foodQueue = new Queue<Food>();   //Сʳ��Ķ���
    public Food currentFood;                                        //��ǰ����ͷ��ʳ��
    public string date;                     //����������ʱ��
    public float price;                     //�۸�
    public bool havePlate;              //�Ƿ�������

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
    public Vector3 foodPosition;        //ÿ��Сʳ���Ӧ�ļҾ�λ��
    public float foodTime;                  //ÿ��Сʳ���ʱ��
    public string subFoodSpriteLocation;        //ÿ��˳���ͼƬλ��
}