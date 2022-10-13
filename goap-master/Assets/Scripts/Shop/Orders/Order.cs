using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyShop;

//[System.Serializable]     //��Ҫ���л� ,��Ϊstaff�Ǹ���order�Ƿ�null���жϵ� ,�༭�����Զ�ʵ����һ���Ͳ�����null��
//����ȷ��ֻ��shop��һ��ʼ��customer��д��,�����Ķ���ֻ��!
public class Order 
{
    public string orderFoodName { get; set; }        //��������
    public string foodSpriteLocation { get; set; }       //���յ�ʳ��ͼƬλ��

    public Transform tableFoodPosition { get; set; }  //�������Ҫ��ǰ��ȡλ�õ�                   //��ʱû����,�����ǿ��˵�λ��,����ʳ���λ�ð�
    public Transform staffPosition { get; set; } //�ϲ˵�ʱ��վ��λ��,��Ϊ��Ҫ��̬�ж�վ��      //���ö�̬�ж���

    public int cookingScore{ get; set; }      //��ǰ��Ʒ�ĳ���,ȡ����staff

public bool canBeAssigned = true;   //�Ƿ��ܱ�����                * �����жϵ����Ƿ��ֶ�����
    public bool orderFinished { get; set; }            //                                      * �����жϵ����Ƿ񶩵�û�������(�˿���ǰ����)��������������
    public Staff staff { get; set; }             //��ǰ����ķ���Ա                      * �����жϵ����Ƿ�ϵͳ����
    public Customer customer { get; set; }  //��ǰ�˿�

    public List<Food> foodList = new List<Food>();          //������foodlist
    public Queue<Food> foodQueue = new Queue<Food>();   //Сʳ��Ķ���
    public Food currentFood { get; set; }                                    //��ǰ����ͷ��ʳ��
    public string date { get; set; }                   //����������ʱ��(��ʵʱ��)
    public float price { get; set; }                    //�۸�
    public bool havePlate { get; set; }          //�Ƿ�������

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
        //// ��ʾ/�ر�ÿ��Сʳ���tag
        //foreach (var val in foodList)
        //{
        //    val.tagPosition.gameObject.SetActive(!canBeAssigned);
        //}
    }
}

public struct Food
{
    public Transform tagPosition { get; set; }  //�ֶ�����ʱ��ǵ�λ��
    public Transform foodPosition { get; set; }      //ÿ��Сʳ���Ӧ�ļҾ�λ��
    public float foodTime { get; set; }              //ÿ��Сʳ���ʱ��
    public string subFoodSpriteLocation { get; set; }      //ÿ��˳���ͼƬλ��
}