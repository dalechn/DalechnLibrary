using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

public class FTransform : MonoBehaviour
{
    public Vector3 forward
    {
        get
        {
            return transform.rotation * Vector3.forward;
        }
        set
        {
            transform.rotation = Quaternion.LookRotation(value);
        }
    }

    public void LookAt(Vector3 worldPosition, [DefaultValue("Vector3.up")] Vector3 worldUp)
    {
        transform.rotation = Quaternion.LookRotation(worldPosition, worldUp);
    }

    public Vector3 TransformVector(Vector3 direction)
    {
        return Vector3.Scale(transform.rotation * direction, transform.lossyScale);
    }

    public Vector3 InverseTransformVector(Vector3 direction)
    {
        return Div(Quaternion.Inverse(transform.rotation) * direction, transform.lossyScale);
    }

    public Vector3 TransformDirection(Vector3 direction)
    {
        return transform.rotation * direction;
    }

    public Vector3 InverseTransformDirection(Vector3 direction)
    {
        return Quaternion.Inverse(transform.rotation) * direction;
    }

    //transform.position = transform.parent.TransformPoint(transform.localPosition);
    //transform.localPosition = transform.parent.InverseTransformPoint(transform.position);

    // 矩阵的逆(inverse) 为倒数运算 
    // 矩阵的转置(transpose)为行列互换运算
    public Vector3 TransformPoint(Vector3 localPosition)
    {
        // 方法1
        Vector3 worldPosition = transform.localToWorldMatrix.MultiplyPoint(localPosition);

        // 方法2
        Matrix4x4 localToWorld = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        worldPosition = localToWorld.MultiplyPoint(localPosition);

        // 方法3 矩阵本质
        worldPosition = transform.position+ Vector3.Scale(transform.rotation* localPosition, transform.lossyScale);

        return worldPosition;
    }

    public Vector3 InverseTransformPoint(Vector3 worldPosition)
    {
        Vector3 localPosition = transform.worldToLocalMatrix.MultiplyPoint(worldPosition);

        Matrix4x4 worldToLocal = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale).inverse;
        localPosition =  worldToLocal.MultiplyPoint(worldPosition);

        localPosition = Div(Quaternion.Inverse(transform.rotation) * (worldPosition - transform.position), transform.lossyScale);

        return localPosition;
    }

    /// <summary>
    /// Same as Transform.TransformPoint(), but not using scale.
    /// </summary>
    public  Vector3 TransformPointUnscaled(Vector3 point)
    {
        return transform.position + transform.rotation * point;
    }

    /// <summary>
    /// Same as Transform.InverseTransformPoint(), but not using scale.
    /// </summary>
    public  Vector3 InverseTransformPointUnscaled( Vector3 point)
    {
        return Quaternion.Inverse(transform.rotation) * (point - transform.position);
    }

    /// <summary>
    /// Divides the values of v1 by the values of v2.
    /// </summary>
    public  Vector3 Div(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
    }


    // 顶点*旋转矩阵: E(α,π/2,β) = Rz(β) * Ry(π/2) * Rx(α) =  Ryz(α) *Rzx(π/2)* Rxy(β) = Ry(π/2) * Rx(α-β) 

    // 不满足交换律 a * b != b * a 
    // 结合律 a * b * c = a * (b * c), a*(A*B) = a*A*B (a为数)
    // 分配律 a * (b+c) = a * b+a * c
    private void Rotate(Vector3 eulers, [DefaultValue("Space.Self")]Space relativeTo)
    {
        Quaternion quaternion = Quaternion.Euler(eulers.x, eulers.y, eulers.z);
        if (relativeTo == Space.Self)
            transform.localRotation *= quaternion;
        else
            transform.rotation *= transform.rotation * Quaternion.Inverse(transform.rotation) * quaternion;
    }

    //// 顶点*轴角:  3d空间中任意一个v沿着单位向量u旋转θ角度之后的v'为:  v' = cos(θ)*v + (1-cos(θ))*(u.v).u + sin(θ)*(uxv)
    ///
    /// 赋值给transform的rotation(Quat=Quat), 
    /// 对已有Vec3变换(Quat*Vec3), 
    /// 运算Quat*Quat 
    private void Rotate(Vector3 axis, float angle, [DefaultValue("Space.Self")]Space relativeTo)
    {
        if (relativeTo == Space.Self)
        {
            axis = transform.TransformDirection(axis);
        }

        transform.rotation *= Quaternion.AngleAxis(angle, axis);
    }

    private void RotateAround(Vector3 point, Vector3 axis, float angle)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        Vector3 d = rotation * (transform.position - point);
        transform.position = point + d;

        transform.rotation *= rotation;
    }

    // 位移是速度对时间的积分,direction*speed就是速度
    // vt = v0+a*t  速度和时间的关系
    //s = v0*t+1/2*a*t² 位移和时间的关系
    //vt² -v0² = 2*a*s 速度和位移的关系
    //s = 1/2*(vt+v0)*t 平均速度和位移
    //
    //角速度ω = △θ/△t ,线速度 v= △s/△t ,v=ω×r
    //
    //动能定理(做功,能量的变化)W = F*s  =△Ek  能量守恒定律△Ep+△Ek = 0 动能公式 E = 1/2*m*v²
    //动量定理(冲量,动量的变化) I = F*t =m*a*t = m*(vt-v0) = △P 动量守恒定律△P1+△P2=0 动量公式P = m*v

    public void Translate(Vector3 translation, [DefaultValue("Space.Self")]Space relativeTo)
    {
        if (relativeTo == Space.World)
        {
            transform.position += translation;
        }
        else
        {
            transform.position += transform.rotation * translation;
            //transform.position += transform.TransformDirection(translation);
        }
    }

    public void Translate(Vector3 translation, Transform relativeTo)
    {
        if ((bool)relativeTo)
        {
            transform.position += relativeTo.TransformDirection(translation);
        }
        else
        {
            transform.position += translation;
        }
    }

    // 直线的参数方程 
    // p = p0+ t * u

    //直线的标准方程
    // a*x+b*y+c*z+d = 0

    // 平面方程 点+法向量
    // n.(p - p1) = 0

    // 圆的参数方程 (极坐标系)
    // x = x0 + r*cosθ
    // y = y0 + r*sinθ

    //圆的标准方程
    //(x-x0)²+(y-y0)²=r²

    //球的参数方程 (球面坐标系)
    // x = r*sinθ*cosα
    // y = r*sinθ*sinα
    // z = r*cosα

    //球的标准方程
    //(x-x0)² + (y-y0)² + (z-z0)²=r²
}
