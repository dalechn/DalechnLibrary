using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//正数的反码/补码是其本身
//负数的反码是在其原码的基础上, 符号位不变，其余各个位取反
//负数的补码即在反码的基础上+1
public class VisualBit : MonoBehaviour
{
    public Text textDecimal;
    public Text originalCode; //原码(original code)
    public Text inverseCode; //反码(inverse code)
    public Text complementCode; //补码(complement code)
    public Text textOctonary;
    public Text textHexadecimal;

    public int val;

    void Start()
    {

    }

    [ContextMenu("ConvertDecimal")]
    public void ConvertDecimal()
    {
        val = int.Parse(textDecimal.text);
        Show();
    }

    public void Show()
    {
        originalCode.text = "0b" + Convert.ToString(val, 2)+ " 原码(original code)"; //0B
        inverseCode.text = "0b" + Convert.ToString(~val, 2)+ " 反码(inverse code)";
        complementCode.text = "0b" + Convert.ToString(~val+1, 2)+ " 补码(complement code)";
        textOctonary.text = "0" + Convert.ToString(val, 8); //0o/0O(js)
        textHexadecimal.text = "0x" + Convert.ToString(val, 16); //0X
    }

    //8 << 1的值为8* 2 = 16
    //8 << 2的值为8* (2 ^ 2) = 32
    //8 << n的值为8* (2 ^ n)
    [ContextMenu("shift logical left")]
    public void SHL()//shift logical left：逻辑左移指令,SAL一样的
    {
        val = val << 1;
        textDecimal.text = val.ToString();

        Show();
    }

    //[ContextMenu("shift logical right")]
    //public void SHR()//shift logical right:逻辑右移指令,无符号右移,最高位补0
    //{
    //    textDecimal.text = val.ToString();
    //    val = val >>> 1;
    //    Show();
    //}

    [ContextMenu("shift arithmetic right")]
    public void SAR()//shift arithmetic right: 算数右移指令,带符号右移,最高位移动带符号
    {
        textDecimal.text = val.ToString();
        val = val >> 1;
        Show();
    }

    [ContextMenu("NOT")]
    public void NOT()//~
    {
        textDecimal.text = val.ToString();
        val = ~val;
        Show();
    }
}
