using System;
using UnityEngine.Internal;
using UnityEngine;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[Serializable]
[DataContract]
public struct FQuat : IEquatable<FQuat>
{
    const float radToDeg = (float)(180.0 / Math.PI);
    const float degToRad = (float)(Math.PI / 180.0);

    public const float kEpsilon = 1E-06f; // should probably be used in the 0 tests in LookRotation or Slerp

    [XmlIgnore]
    public Vector3 xyz
    {
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }
        get
        {
            return new Vector3(x, y, z);
        }
    }

    [DataMember(Order = 1)]
    public float x;

    [DataMember(Order = 2)]
    public float y;

    [DataMember(Order = 3)]
    public float z;

    [DataMember(Order = 4)]
    public float w;

    [XmlIgnore]
    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return this.x;
                case 1:
                    return this.y;
                case 2:
                    return this.z;
                case 3:
                    return this.w;
                default:
                    throw new IndexOutOfRangeException("Invalid Quaternion index: " + index + ", can use only 0,1,2,3");
            }
        }
        set
        {
            switch (index)
            {
                case 0:
                    this.x = value;
                    break;
                case 1:
                    this.y = value;
                    break;
                case 2:
                    this.z = value;
                    break;
                case 3:
                    this.w = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Invalid Quaternion index: " + index + ", can use only 0,1,2,3");
            }
        }
    }

    [XmlIgnore]
    public static FQuat identity
    {
        get
        {
            return new FQuat(0f, 0f, 0f, 1f);
        }
    }

    [XmlIgnore]
    public Vector3 eulerAngles
    {
        get
        {
            return FQuat.ToEulerRad(this) * radToDeg;
        }
        set
        {
            this = FQuat.FromEulerRad(value * degToRad);
        }
    }

    [XmlIgnore]
    public float Length
    {
        get
        {
            return (float)System.Math.Sqrt(x * x + y * y + z * z + w * w);
        }
    }

    [XmlIgnore]
    public float LengthSquared
    {
        get
        {
            return x * x + y * y + z * z + w * w;
        }
    }

    public FQuat(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public FQuat(Vector3 v, float w)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
        this.w = w;
    }

    public void Set(float new_x, float new_y, float new_z, float new_w)
    {
        this.x = new_x;
        this.y = new_y;
        this.z = new_z;
        this.w = new_w;
    }


    public static FQuat Euler(float x, float y, float z)
    {
        return FQuat.FromEulerRad(new Vector3((float)x, (float)y, (float)z) * degToRad);
    }

    public static FQuat Euler(Vector3 euler)
    {
        return FQuat.FromEulerRad(euler * degToRad);
    }

    public static FQuat AngleAxis(float angle, Vector3 axis)
    {
        return FQuat.AngleAxis(angle, ref axis);
    }

    public static FQuat FromToRotation(Vector3 fromDirection, Vector3 toDirection)
    {
        //return RotateTowards(LookRotation(fromDirection), LookRotation(toDirection), float.MaxValue);

        Vector3 crossAxis = Vector3.Cross(fromDirection, toDirection);
        return AngleAxis(Vector3.SignedAngle(fromDirection, toDirection, crossAxis), crossAxis);
    }
 
    public static FQuat LookRotation(Vector3 forward, [DefaultValue("Vector3.up")] Vector3 upwards)
    {
        return FQuat.LookRotation(ref forward, ref upwards);

        //return FromToRotation(Vector3.forward,forward)*FromToRotation(Vector3.up,upwards); //x
    }

    public static FQuat LookRotation(Vector3 forward)
    {
        //return FromToRotation(Vector3.forward, forward); //x

        Vector3 up = Vector3.up;
        return FQuat.LookRotation(ref forward, ref up);
    }

    public static FQuat Slerp(FQuat a, FQuat b, float t)
    {
        return FQuat.Slerp(ref a, ref b, t);
    }

    public static FQuat SlerpUnclamped(FQuat a, FQuat b, float t)
    {
        return FQuat.SlerpUnclamped(ref a, ref b, t);
    }

    //未知:我感觉不是这样的?
    public static FQuat Lerp(FQuat a, FQuat b, float t)
    {
        if (t > 1) t = 1;
        if (t < 0) t = 0;
        return Slerp(ref a, ref b, t); // TODO: use lerp not slerp, "Because quaternion works in 4D. Rotation in 4D are linear" ???
    }

    public static FQuat LerpUnclamped(FQuat a, FQuat b, float t)
    {
        return Slerp(ref a, ref b, t);
    }
  
    public static FQuat RotateTowards(FQuat from, FQuat to, float maxDegreesDelta)
    {
        float num = FQuat.Angle(from, to);
        if (num == 0f)
        {
            return to;
        }
        float t = Math.Min(1f, maxDegreesDelta / num);
        return FQuat.SlerpUnclamped(from, to, t);
    }


    private static Vector3 NormalizeAngles(Vector3 angles)
    {
        angles.x = NormalizeAngle(angles.x);
        angles.y = NormalizeAngle(angles.y);
        angles.z = NormalizeAngle(angles.z);
        return angles;
    }

    private static float NormalizeAngle(float angle)
    {
        while (angle > 360)
            angle -= 360;
        while (angle < 0)
            angle += 360;
        return angle;
    }

    private static FQuat LookRotation(ref Vector3 forward, ref Vector3 up)
    {

        forward = Vector3.Normalize(forward);
        Vector3 right = Vector3.Normalize(Vector3.Cross(up, forward));
        up = Vector3.Cross(forward, right);
        var m00 = right.x;
        var m01 = right.y;
        var m02 = right.z;
        var m10 = up.x;
        var m11 = up.y;
        var m12 = up.z;
        var m20 = forward.x;
        var m21 = forward.y;
        var m22 = forward.z;


        float num8 = (m00 + m11) + m22;
        var quaternion = new FQuat();
        if (num8 > 0f)
        {
            var num = (float)System.Math.Sqrt(num8 + 1f);
            quaternion.w = num * 0.5f;
            num = 0.5f / num;
            quaternion.x = (m12 - m21) * num;
            quaternion.y = (m20 - m02) * num;
            quaternion.z = (m01 - m10) * num;
            return quaternion;
        }
        if ((m00 >= m11) && (m00 >= m22))
        {
            var num7 = (float)System.Math.Sqrt(((1f + m00) - m11) - m22);
            var num4 = 0.5f / num7;
            quaternion.x = 0.5f * num7;
            quaternion.y = (m01 + m10) * num4;
            quaternion.z = (m02 + m20) * num4;
            quaternion.w = (m12 - m21) * num4;
            return quaternion;
        }
        if (m11 > m22)
        {
            var num6 = (float)System.Math.Sqrt(((1f + m11) - m00) - m22);
            var num3 = 0.5f / num6;
            quaternion.x = (m10 + m01) * num3;
            quaternion.y = 0.5f * num6;
            quaternion.z = (m21 + m12) * num3;
            quaternion.w = (m20 - m02) * num3;
            return quaternion;
        }
        var num5 = (float)System.Math.Sqrt(((1f + m22) - m00) - m11);
        var num2 = 0.5f / num5;
        quaternion.x = (m20 + m02) * num2;
        quaternion.y = (m21 + m12) * num2;
        quaternion.z = 0.5f * num5;
        quaternion.w = (m01 - m10) * num2;
        return quaternion;
    }

    private static FQuat AngleAxis(float degress, ref Vector3 axis)
    {
        if (axis.sqrMagnitude == 0.0f)
            return identity;

        FQuat result = identity;
        var radians = degress * degToRad;
        radians *= 0.5f;
        axis.Normalize();
        axis = axis * (float)System.Math.Sin(radians);
        result.x = axis.x;
        result.y = axis.y;
        result.z = axis.z;
        result.w = (float)System.Math.Cos(radians);

        return Normalize(result);
    }

    private static FQuat Slerp(ref FQuat a, ref FQuat b, float t)
    {
        if (t > 1) t = 1;
        if (t < 0) t = 0;
        return SlerpUnclamped(ref a, ref b, t);
    }

    private static FQuat SlerpUnclamped(ref FQuat a, ref FQuat b, float t)
    {
        // if either input is zero, return the other.
        if (a.LengthSquared == 0.0f)
        {
            if (b.LengthSquared == 0.0f)
            {
                return identity;
            }
            return b;
        }
        else if (b.LengthSquared == 0.0f)
        {
            return a;
        }


        float cosHalfAngle = a.w * b.w + Vector3.Dot(a.xyz, b.xyz);

        if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
        {
            // angle = 0.0f, so just return one input.
            return a;
        }
        else if (cosHalfAngle < 0.0f)
        {
            b.xyz = -b.xyz;
            b.w = -b.w;
            cosHalfAngle = -cosHalfAngle;
        }

        float blendA;
        float blendB;
        if (cosHalfAngle < 0.99f)
        {
            // do proper slerp for big angles
            float halfAngle = (float)System.Math.Acos(cosHalfAngle);
            float sinHalfAngle = (float)System.Math.Sin(halfAngle);
            float oneOverSinHalfAngle = 1.0f / sinHalfAngle;
            blendA = (float)System.Math.Sin(halfAngle * (1.0f - t)) * oneOverSinHalfAngle;
            blendB = (float)System.Math.Sin(halfAngle * t) * oneOverSinHalfAngle;
        }
        else
        {
            // do lerp if angle is really small.
            blendA = 1.0f - t;
            blendB = t;
        }

        FQuat result = new FQuat(blendA * a.xyz + blendB * b.xyz, blendA * a.w + blendB * b.w);
        if (result.LengthSquared > 0.0f)
            return Normalize(result);
        else
            return identity;
    }

    private static Vector3 ToEulerRad(FQuat rotation)
    {
        float sqw = rotation.w * rotation.w;
        float sqx = rotation.x * rotation.x;
        float sqy = rotation.y * rotation.y;
        float sqz = rotation.z * rotation.z;
        float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
        float test = rotation.x * rotation.w - rotation.y * rotation.z;
        Vector3 v;

        if (test > 0.4995f * unit)
        { // singularity at north pole
            v.y = 2f * Mathf.Atan2(rotation.y, rotation.x);
            v.x = Mathf.PI / 2;
            v.z = 0;
            return NormalizeAngles(v * Mathf.Rad2Deg);
        }
        if (test < -0.4995f * unit)
        { // singularity at south pole
            v.y = -2f * Mathf.Atan2(rotation.y, rotation.x);
            v.x = -Mathf.PI / 2;
            v.z = 0;
            return NormalizeAngles(v * Mathf.Rad2Deg);
        }
        FQuat q = new FQuat(rotation.w, rotation.z, rotation.x, rotation.y);
        v.y = (float)System.Math.Atan2(2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));     // Yaw
        v.x = (float)System.Math.Asin(2f * (q.x * q.z - q.w * q.y));                             // Pitch
        v.z = (float)System.Math.Atan2(2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));      // Roll
        return NormalizeAngles(v * Mathf.Rad2Deg);
    }

    private static FQuat FromEulerRad(Vector3 euler)
    {
        var yaw = euler.x;
        var pitch = euler.y;
        var roll = euler.z;
        float rollOver2 = roll * 0.5f;
        float sinRollOver2 = (float)System.Math.Sin((float)rollOver2);
        float cosRollOver2 = (float)System.Math.Cos((float)rollOver2);
        float pitchOver2 = pitch * 0.5f;
        float sinPitchOver2 = (float)System.Math.Sin((float)pitchOver2);
        float cosPitchOver2 = (float)System.Math.Cos((float)pitchOver2);
        float yawOver2 = yaw * 0.5f;
        float sinYawOver2 = (float)System.Math.Sin((float)yawOver2);
        float cosYawOver2 = (float)System.Math.Cos((float)yawOver2);
        FQuat result;
        result.x = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
        result.y = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;
        result.z = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
        result.w = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
        return result;

    }

    private static void ToAxisAngleRad(FQuat q, out Vector3 axis, out float angle)
    {
        if (System.Math.Abs(q.w) > 1.0f)
            q.Normalize();
        angle = 2.0f * (float)System.Math.Acos(q.w); // angle
        float den = (float)System.Math.Sqrt(1.0 - q.w * q.w);
        if (den > 0.0001f)
        {
            axis = q.xyz / den;
        }
        else
        {
            // This occurs when the angle is zero. 
            // Not a problem: just set an arbitrary normalized axis.
            axis = new Vector3(1, 0, 0);
        }
    }

    public void ToAngleAxis(out float angle, out Vector3 axis)
    {
        FQuat.ToAxisAngleRad(this, out axis, out angle);
        angle *= radToDeg;
    }

    public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
    {
        this = FQuat.FromToRotation(fromDirection, toDirection);
    }

    public void SetLookRotation(Vector3 view)
    {
        Vector3 up = Vector3.up;
        this.SetLookRotation(view, up);
    }

    public void SetLookRotation(Vector3 view, [DefaultValue("Vector3.up")] Vector3 up)
    {
        this = FQuat.LookRotation(view, up);
    }

    public void Normalize()
    {
        float scale = 1.0f / this.Length;
        xyz *= scale;
        w *= scale;
    }

    public static FQuat Normalize(FQuat q)
    {
        FQuat result;
        Normalize(ref q, out result);
        return result;
    }

    public static void Normalize(ref FQuat q, out FQuat result)
    {
        float scale = 1.0f / q.Length;
        result = new FQuat(q.xyz * scale, q.w * scale);
    }

    public static float Dot(FQuat a, FQuat b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    }

    public static FQuat Inverse(FQuat rotation)
    {
        float lengthSq = rotation.LengthSquared;
        if (lengthSq != 0.0)
        {
            float i = 1.0f / lengthSq;
            return new FQuat(rotation.xyz * -i, rotation.w * i);
        }
        return rotation;
    }

    public static float Angle(FQuat a, FQuat b)
    {
        float num = FQuat.Dot(a, b);
        return num > 0.999999f ? 0f : (Mathf.Acos(Mathf.Min(Mathf.Abs(num), 1f)) * 2f * 57.29578f);
    }


    #region Obsolete methods
    /*
	[Obsolete("Use MyQuaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
	public static MyQuaternion EulerRotation(float x, float y, float z)
	{
		return MyQuaternion.Internal_FromEulerRad(new Vector3(x, y, z));
	}
	[Obsolete("Use MyQuaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
	public static MyQuaternion EulerRotation(Vector3 euler)
	{
		return MyQuaternion.Internal_FromEulerRad(euler);
	}
	[Obsolete("Use MyQuaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
	public void SetEulerRotation(float x, float y, float z)
	{
		this = Quaternion.Internal_FromEulerRad(new Vector3(x, y, z));
	}
	[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
	public void SetEulerRotation(Vector3 euler)
	{
		this = Quaternion.Internal_FromEulerRad(euler);
	}
	[Obsolete("Use Quaternion.eulerAngles instead. This function was deprecated because it uses radians instead of degrees")]
	public Vector3 ToEuler()
	{
		return Quaternion.Internal_ToEulerRad(this);
	}
	[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
	public static Quaternion EulerAngles(float x, float y, float z)
	{
		return Quaternion.Internal_FromEulerRad(new Vector3(x, y, z));
	}
	[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
	public static Quaternion EulerAngles(Vector3 euler)
	{
		return Quaternion.Internal_FromEulerRad(euler);
	}
	[Obsolete("Use Quaternion.ToAngleAxis instead. This function was deprecated because it uses radians instead of degrees")]
	public void ToAxisAngle(out Vector3 axis, out float angle)
	{
		Quaternion.Internal_ToAxisAngleRad(this, out axis, out angle);
	}
	[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
	public void SetEulerAngles(float x, float y, float z)
	{
		this.SetEulerRotation(new Vector3(x, y, z));
	}
	[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees")]
	public void SetEulerAngles(Vector3 euler)
	{
		this = Quaternion.EulerRotation(euler);
	}
	[Obsolete("Use Quaternion.eulerAngles instead. This function was deprecated because it uses radians instead of degrees")]
	public static Vector3 ToEulerAngles(Quaternion rotation)
	{
		return Quaternion.Internal_ToEulerRad(rotation);
	}
	[Obsolete("Use Quaternion.eulerAngles instead. This function was deprecated because it uses radians instead of degrees")]
	public Vector3 ToEulerAngles()
	{
		return Quaternion.Internal_ToEulerRad(this);
	}
	[Obsolete("Use Quaternion.AngleAxis instead. This function was deprecated because it uses radians instead of degrees")]
	public static Quaternion AxisAngle(Vector3 axis, float angle)
	{
		return Quaternion.INTERNAL_CALL_AxisAngle(ref axis, angle);
	}

	private static Quaternion INTERNAL_CALL_AxisAngle(ref Vector3 axis, float angle)
	{

	}
	[Obsolete("Use Quaternion.AngleAxis instead. This function was deprecated because it uses radians instead of degrees")]
	public void SetAxisAngle(Vector3 axis, float angle)
	{
		this = Quaternion.AxisAngle(axis, angle);
	}
	*/
    #endregion

    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2 ^ this.w.GetHashCode() >> 1;
    }

    public override bool Equals(object other)
    {
        if (!(other is FQuat))
        {
            return false;
        }
        FQuat quaternion = (FQuat)other;
        return this.x.Equals(quaternion.x) && this.y.Equals(quaternion.y) && this.z.Equals(quaternion.z) && this.w.Equals(quaternion.w);
    }

    public bool Equals(FQuat other)
    {
        return this.x.Equals(other.x) && this.y.Equals(other.y) && this.z.Equals(other.z) && this.w.Equals(other.w);
    }

    public override string ToString()
    {
        return string.Format("({0:F1}, {1:F1}, {2:F1}, {3:F1})", this.x, this.y, this.z, this.w);
    }

    public string ToString(string format)
    {
        return string.Format("({0}, {1}, {2}, {3})", this.x.ToString(format), this.y.ToString(format), this.z.ToString(format), this.w.ToString(format));
    }

    public static FQuat operator *(FQuat lhs, FQuat rhs)
    {
        return new FQuat(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
    }

    public static Vector3 operator *(FQuat rotation, Vector3 point)
    {
        float num = rotation.x * 2f;
        float num2 = rotation.y * 2f;
        float num3 = rotation.z * 2f;
        float num4 = rotation.x * num;
        float num5 = rotation.y * num2;
        float num6 = rotation.z * num3;
        float num7 = rotation.x * num2;
        float num8 = rotation.x * num3;
        float num9 = rotation.y * num3;
        float num10 = rotation.w * num;
        float num11 = rotation.w * num2;
        float num12 = rotation.w * num3;
        Vector3 result;
        result.x = (1f - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
        result.y = (num7 + num12) * point.x + (1f - (num4 + num6)) * point.y + (num9 - num10) * point.z;
        result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1f - (num4 + num5)) * point.z;
        return result;
    }

    public static bool operator ==(FQuat lhs, FQuat rhs)
    {
        return FQuat.Dot(lhs, rhs) > 0.999999f;
    }

    public static bool operator !=(FQuat lhs, FQuat rhs)
    {
        return FQuat.Dot(lhs, rhs) <= 0.999999f;
    }

    #region Implicit conversions to and from Unity's Quaternion
    public static implicit operator UnityEngine.Quaternion(FQuat me)
    {
        return new UnityEngine.Quaternion((float)me.x, (float)me.y, (float)me.z, (float)me.w);
    }

    public static implicit operator FQuat(UnityEngine.Quaternion other)
    {
        return new FQuat((float)other.x, (float)other.y, (float)other.z, (float)other.w);
    }
    #endregion
}