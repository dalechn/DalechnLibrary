using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMath : MonoBehaviour
{
    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * Mathf.Clamp01(t);
    }

    public static float LerpUnclamped(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    //计算两个值之间的Lerp参数。也就是value在from和to之间的比例值。
    public static float InverseLerp(float a, float b, float value)
    {
        if (a != b)
        {
            return Mathf.Clamp01((value - a) / (b - a));
        }
        return 0f;
    }

    public static float LerpAngle(float a, float b, float t)
    {
        float num = Mathf.Repeat(b - a, 360f);
        if (num > 180f)
        {
            num -= 360f;
        }
        return a + num * Mathf.Clamp01(t);
    }

    //[-180,180]
    public static float DeltaAngle(float current, float target)
    {
        // [0,360]
        float num = Mathf.Repeat(target - current, 360f);
        if (num > 180f)
        {
            num -= 360f;
        }
        return num;
    }

    public static float MoveTowards(float current, float target, float maxDelta)
    {
        if (Mathf.Abs(target - current) <= maxDelta)
        {
            return target;
        }
        return current + Mathf.Sign(target - current) * maxDelta;
    }

    public static float MoveTowardsAngle(float current, float target, float maxDelta)
    {
        float num = Mathf.DeltaAngle(current, target);
        if (0f - maxDelta < num && num < maxDelta)
        {
            return target;
        }
        target = current + num;
        return MoveTowards(current, target, maxDelta);
    }

    public static float SmoothStep(float from, float to, float t)
    {
        t = Mathf.Clamp01(t);
        t = -2f * t * t * t + 3f * t * t;
        return to * t + from * (1f - t);
    }

    // Repeat(-160,360)  => -160 - (-0.44f => -1f )*360 => 200
    public static float Repeat(float t, float length)
    {
        return Mathf.Clamp(t - Mathf.Floor(t / length) * length, 0f, length);
    }

    public static float PingPong(float t, float length)
    {
        t = Repeat(t, length * 2f);
        return length - Mathf.Abs(t - length);
    }

    public static bool Approximately(float a, float b)
    {
        return Mathf.Abs(b - a) < Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)), Mathf.Epsilon * 8f);
    }

    public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
    {
        float deltaTime = Time.deltaTime;
        return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    }

    public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime)
    {
        float deltaTime = Time.deltaTime;
        float maxSpeed = float.PositiveInfinity;
        return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    }

    public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        smoothTime = Mathf.Max(0.0001f, smoothTime);
        float num = 2f / smoothTime;
        float num2 = num * deltaTime;
        float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
        float value = current - target;
        float num4 = target;
        float num5 = maxSpeed * smoothTime;
        value = Mathf.Clamp(value, 0f - num5, num5);
        target = current - value;
        float num6 = (currentVelocity + num * value) * deltaTime;
        currentVelocity = (currentVelocity - num * num6) * num3;
        float num7 = target + (value + num6) * num3;
        if (num4 - current > 0f == num7 > num4)
        {
            num7 = num4;
            currentVelocity = (num7 - num4) / deltaTime;
        }
        return num7;
    }

    public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
    {
        float deltaTime = Time.deltaTime;
        return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    }

    public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime)
    {
        float deltaTime = Time.deltaTime;
        float maxSpeed = float.PositiveInfinity;
        return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    }

    public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        target = current + DeltaAngle(current, target);
        return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    }

    //  asin的取值范围是[-pi/2,pi/2]
    //  acos的取值范围是[0, pi]
    //  atan的取值范围是[-pi / 2, pi / 2]
    //  //atan2(y, x)的取值范围是[-PI, PI]
    float Atan2(float y, float x)
    {
        float val = y / x;

        if (x < 0 && y >= 0)
            return Mathf.Atan(val) + Mathf.PI;
        else if (x < 0 && y < 0)
            return Mathf.Atan(val) - Mathf.PI;
        else if (Mathf.Abs(x)<1e-5 && y > 0)
            return Mathf.PI / 2;
        else if (Mathf.Abs(x) < 1e-5 && y < 0)
            return -Mathf.PI / 2;

        return Mathf.Atan(val);
    }

    //y= e^x
    public static float Exp(float power) { return Mathf.Exp(power); }
    //y = log p f
    public static float Log(float f, float p) { return Mathf.Log(f,p); }
    // y = log e f //自然对数natural logarithm
    public static float Log(float f){return Mathf.Log(f);}
    // y = log10 f //常用对数common logarithm
    public static float Log10(float f){return Mathf.Log10(f);}

    //向上取整
    public static float Ceil(float f){return Mathf.Ceil(f);}
    // 向下取整
    public static float Floor(float f){ return Mathf.Floor(f); }
    //1.当舍去位的数值小于5时,直接舍去 1.14->1.1
    //2.当舍去位的数值大于6时,进位加1 1.16 ->1.2
    //3.遇到5需要舍去的情况只有一种，即5是最后一位有效数且前一位数是偶数 1.35->1.4 1.25->1.2  1.15->1.2 
    public static float Round(float f) { return Mathf.Round(f); }

    //求和符号  ∑i*x(下标i = 1,上标200) = x+2*x+3*x+...+200*x
    //阶乘符号: n! = 1*2*3*...n-1*n , n =(n-1)!*n,  0!=1
}
