using System;
using UnityEngine;
using UnityEngine.Internal;

public static class FVector3
{
    //未知:?https://stackoverflow.com/questions/67919193/how-does-unity-implements-vector3-slerp-exactly
    public static Vector3  Slerp(Vector3 start, Vector3 end, float percent)
    {
        // Dot product - the cosine of the angle between 2 vectors.
        float dot = Vector3.Dot(start, end);

        // Clamp it to be in the range of Acos()
        // This may be unnecessary, but floating point
        // precision can be a fickle mistress.
        Mathf.Clamp(dot, -1.0f, 1.0f);

        // Acos(dot) returns the angle between start and end,
        // And multiplying that by percent returns the angle between
        // start and the final result.
        float theta = Mathf.Acos(dot) * percent;
        Vector3 RelativeVec = end - start * dot;
        RelativeVec.Normalize();

        // Orthonormal basis
        // The final result.
        return ((start * Mathf.Cos(theta)) + (RelativeVec * Mathf.Sin(theta)));
    }

    // Dot 用transform.forward判断前后方位Dot,或者判断是否在三角形内部(3个向量dot结果>0)
    // A·B = |a| *|b| *cosθ
    public static float Dot(Vector3 lhs, Vector3 rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
    }

    // Cross 用transform.forward判断左右方位,或者判断是否在三角形内部(3个向量cross结果同符号)
    // |AxB| = |A| * |B| * sinθ 
    public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
    {
        return new Vector3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
    }

    //取值范围(0-180)
    //cosθ = a·b / |a|*|b|
    public static float Angle(Vector3 from, Vector3 to)
    {
        float num = (float)Mathf.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
        if (num < 1E-15f)
        {
            return 0f;
        }
        float num2 = Mathf.Clamp(Vector3.Dot(from, to) / num, -1f, 1f);
        return (float)Mathf.Acos(num2) * 57.29578f;
    }

    // 取值范围  (-180-180)
    public static float SignedAngle(Vector3 from, Vector3 to, Vector3 axis)
    {
        float num = Angle(from, to);

        //源码方案
        //float num2 = from.y * to.z - from.z * to.y;
        //float num3 = from.z * to.x - from.x * to.z;
        //float num4 = from.x * to.y - from.y * to.x;
        //float num5 = Mathf.Sign(axis.x * num2 + axis.y * num3 + axis.z * num4);

        //return num * num5;

        //----------------------------------------------
        //翻译后的源码
        Vector3 crossDirection = Vector3.Cross(from, to);

        //float dir = Mathf.Sign(90 - Vector3.Angle(crossDirection, axis));
        float dir = Mathf.Sign(Vector3.Dot(crossDirection, axis));

        return num * dir;
    }

    //v|| = n * n·v/(n·n)
    //法向量是单位向量的话就是结果就是n*n·v
    public static Vector3 Project(Vector3 vector, Vector3 onNormal)
    {
        float num = Vector3.Dot(onNormal, onNormal);
        if (num < Mathf.Epsilon)
        {
            return Vector3.zero;
        }
        float num2 = Vector3.Dot(vector, onNormal);
        return new Vector3(onNormal.x * num2 / num, onNormal.y * num2 / num, onNormal.z * num2 / num);
    }

    //v = v⊥ + v|| =>
    //v⊥ = v- n * n·v/(n·n)
    public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal)
    {
        float num = Vector3.Dot(planeNormal, planeNormal);
        if (num < Mathf.Epsilon)
        {
            return vector;
        }
        float num2 = Vector3.Dot(vector, planeNormal);
        return new Vector3(vector.x - planeNormal.x * num2 / num, vector.y - planeNormal.y * num2 / num, vector.z - planeNormal.z * num2 / num);
    }

    //最后一个参数是向量长度,一般取 0或者1
    public static Vector3 RotateTowards(Vector3 current, Vector3 target, float maxRadiansDelta, float maxMagnitudeDelta = 0)
    {
        float delta = Vector3.Angle(current, target) * Mathf.Deg2Rad;
        float magDiff = target.magnitude - current.magnitude;

        return Vector3.SlerpUnclamped(current.normalized, target.normalized, Mathf.Min(1.0f, maxRadiansDelta / delta)) *
        (current.magnitude + Mathf.Sign(magDiff) * Mathf.Min(maxMagnitudeDelta, Mathf.Abs(magDiff)));
    }

    public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        t = Mathf.Clamp01(t);
        return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
    }

    public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
    {
        return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
    }

    public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
    {
        float num = target.x - current.x;
        float num2 = target.y - current.y;
        float num3 = target.z - current.z;
        float num4 = num * num + num2 * num2 + num3 * num3;
        if (num4 == 0f || (maxDistanceDelta >= 0f && num4 <= maxDistanceDelta * maxDistanceDelta))
        {
            return target;
        }
        float num5 = (float)Mathf.Sqrt(num4);
        return new Vector3(current.x + num / num5 * maxDistanceDelta, current.y + num2 / num5 * maxDistanceDelta, current.z + num3 / num5 * maxDistanceDelta);
    }

    //  inDirection: 从unity获得_WorldSpaceLightPos0指向光源 ,指向顶点的话 inDirection*=-1;
    public static Vector3 Reflect(Vector3 inDirection, Vector3 inNormal)
    {
        float num = -2f * Vector3.Dot(inNormal, inDirection);
        return new Vector3(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y, num * inNormal.z + inDirection.z);
    }

    // inDirection:输入的方向指向顶点
    public static Vector3 Refract(Vector3 inDirection, Vector3 inNormal, float refractivity)
    {
        inDirection.Normalize();
        inNormal.Normalize();

        float A = Vector3.Dot(inDirection, inNormal);
        float B = 1.0f - refractivity * refractivity * (1.0f - A * A);
        Vector3 T = refractivity * inDirection - (refractivity * A + Mathf.Sqrt(B)) * inNormal;

        if (B > 0)
            return T;
        else
            return Vector3.zero;
    }


    public static Vector3 ClampMagnitude(Vector3 vector, float maxLength)
    {
        float num = vector.sqrMagnitude;
        if (num > maxLength * maxLength)
        {
            float num2 = (float)Math.Sqrt(num);
            float num3 = vector.x / num2;
            float num4 = vector.y / num2;
            float num5 = vector.z / num2;
            return new Vector3(num3 * maxLength, num4 * maxLength, num5 * maxLength);
        }
        return vector;
    }

    public static float Magnitude(Vector3 vector)
    {
        return (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
    }

    public static float SqrMagnitude(Vector3 vector)
    {
        return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
    }

    public static float Distance(Vector3 a, Vector3 b)
    {
        float num = a.x - b.x;
        float num2 = a.y - b.y;
        float num3 = a.z - b.z;
        return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
    }

    public static Vector3 Normalize(Vector3 value)
    {
        float num = Magnitude(value);

        if (num > 1E-05f)
        {
            return value / num;
        }
        return Vector3.zero;
    }

    public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, [DefaultValue("Mathf.Infinity")] float maxSpeed, [DefaultValue("Time.deltaTime")] float deltaTime)
    {
        float num = 0f;
        float num2 = 0f;
        float num3 = 0f;
        smoothTime = Mathf.Max(0.0001f, smoothTime);
        float num4 = 2f / smoothTime;
        float num5 = num4 * deltaTime;
        float num6 = 1f / (1f + num5 + 0.48f * num5 * num5 + 0.235f * num5 * num5 * num5);
        float num7 = current.x - target.x;
        float num8 = current.y - target.y;
        float num9 = current.z - target.z;
        Vector3 vector = target;
        float num10 = maxSpeed * smoothTime;
        float num11 = num10 * num10;
        float num12 = num7 * num7 + num8 * num8 + num9 * num9;
        if (num12 > num11)
        {
            float num13 = (float)Math.Sqrt(num12);
            num7 = num7 / num13 * num10;
            num8 = num8 / num13 * num10;
            num9 = num9 / num13 * num10;
        }
        target.x = current.x - num7;
        target.y = current.y - num8;
        target.z = current.z - num9;
        float num14 = (currentVelocity.x + num4 * num7) * deltaTime;
        float num15 = (currentVelocity.y + num4 * num8) * deltaTime;
        float num16 = (currentVelocity.z + num4 * num9) * deltaTime;
        currentVelocity.x = (currentVelocity.x - num4 * num14) * num6;
        currentVelocity.y = (currentVelocity.y - num4 * num15) * num6;
        currentVelocity.z = (currentVelocity.z - num4 * num16) * num6;
        num = target.x + (num7 + num14) * num6;
        num2 = target.y + (num8 + num15) * num6;
        num3 = target.z + (num9 + num16) * num6;
        float num17 = vector.x - current.x;
        float num18 = vector.y - current.y;
        float num19 = vector.z - current.z;
        float num20 = num - vector.x;
        float num21 = num2 - vector.y;
        float num22 = num3 - vector.z;
        if (num17 * num20 + num18 * num21 + num19 * num22 > 0f)
        {
            num = vector.x;
            num2 = vector.y;
            num3 = vector.z;
            currentVelocity.x = (num - vector.x) / deltaTime;
            currentVelocity.y = (num2 - vector.y) / deltaTime;
            currentVelocity.z = (num3 - vector.z) / deltaTime;
        }
        return new Vector3(num, num2, num3);
    }

    // a+b =>指向平行四边形对角线(平行四边形法则),或者首尾相连连成的线(三角形法则)
    //public static Vector3 operator +(Vector3 a, Vector3 b)
    //{
    //    return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    //}

    //a-b => b指向a 
    //public static Vector3 operator -(Vector3 a, Vector3 b)
    //{
    //    return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
    //}

    //public static Vector3 operator -(Vector3 a)
    //{
    //    return new Vector3(0f - a.x, 0f - a.y, 0f - a.z);
    //}

    //public static Vector3 operator *(Vector3 a, float d)
    //{
    //    return new Vector3(a.x * d, a.y * d, a.z * d);
    //}

    //public static Vector3 operator *(float d, Vector3 a)
    //{
    //    return new Vector3(a.x * d, a.y * d, a.z * d);
    //}

    //public static Vector3 operator /(Vector3 a, float d)
    //{
    //    return new Vector3(a.x / d, a.y / d, a.z / d);
    //}

    //public static bool operator ==(Vector3 lhs, Vector3 rhs)
    //{
    //    float num = lhs.x - rhs.x;
    //    float num2 = lhs.y - rhs.y;
    //    float num3 = lhs.z - rhs.z;
    //    float num4 = num * num + num2 * num2 + num3 * num3;
    //    return num4 < 9.99999944E-11f;
    //}

    //public static bool operator !=(Vector3 lhs, Vector3 rhs)
    //{
    //    return !(lhs == rhs);
    //}


}
