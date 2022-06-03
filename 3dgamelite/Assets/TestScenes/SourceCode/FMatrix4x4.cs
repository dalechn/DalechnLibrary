using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FMatrix4x4
{
  
    // Matrix4x4.TRS(trans, Quaternion.Euler(euler), scale)
    public static Matrix4x4 TRS(Vector3 trans, Vector3 euler, Vector3 scale)
    {
        return Translate(trans) * Rotate(euler) * Scale(scale);
    }

    // Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(euler), Vector3.one)
    public static Matrix4x4 Rotate(Vector3 euler)
    {
        return RotateY(euler.y) * RotateX(euler.x) * RotateZ(euler.z);
    }

    // Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(deg, 0, 0), Vector3.one)
    public static Matrix4x4 RotateX(float deg)
    {
        var rad = deg * Mathf.Deg2Rad;
        var sin = Mathf.Sin(rad);
        var cos = Mathf.Cos(rad);
        var mat = Matrix4x4.identity;
        mat.m11 = cos;
        mat.m12 = -sin;
        mat.m21 = sin;
        mat.m22 = cos;
        return mat;
    }

    // Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, deg, 0), Vector3.one)
    public static Matrix4x4 RotateY(float deg)
    {
        var rad = deg * Mathf.Deg2Rad;
        var sin = Mathf.Sin(rad);
        var cos = Mathf.Cos(rad);
        var mat = Matrix4x4.identity;
        mat.m22 = cos;
        mat.m20 = -sin;
        mat.m02 = sin;
        mat.m00 = cos;
        return mat;
    }


    // Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, deg), Vector3.one)
    public static Matrix4x4 RotateZ(float deg)
    {
        var rad = deg * Mathf.Deg2Rad;
        var sin = Mathf.Sin(rad);
        var cos = Mathf.Cos(rad);
        var mat = Matrix4x4.identity;
        mat.m00 = cos;
        mat.m01 = -sin;
        mat.m10 = sin;
        mat.m11 = cos;
        return mat;
    }

    // Matrix4x4.Scale(scale)
    public static Matrix4x4 Scale(Vector3 scale)
    {
        var mat = Matrix4x4.identity;
        mat.m00 = scale.x;
        mat.m11 = scale.y;
        mat.m22 = scale.z;
        return mat;
    }

    // Matrix4x4.TRS(vec, Quaternion.identity, Vector3.one)
    public static Matrix4x4 Translate(Vector3 vec)
    {
        var mat = Matrix4x4.identity;
        mat.m03 = vec.x;
        mat.m13 = vec.y;
        mat.m23 = vec.z;
        return mat;
    }
}

// 不满足交换律 a * b != b * a (未知:好像有特殊情况?)
// 结合律 a * b * c = a * (b * c), a*(A*B) = a*A*B (a为数)
// 分配律 a * (b+c) = a * b+a * c

// 行列式(determinant) 列(column),行(row)      符号:det(A)或 |A|
// 单位矩阵(E identity matrix)                          符号:E
// 伴随/共轭矩阵( adjoint matrix) 未知?            符号:A*
// 增广矩阵(augmented matrix) 未知?
// 矩阵的秩( rank) 未知?                                  符号:r(A)
// 矩阵的乘法需要左边的矩阵的列向量dot右边的矩阵的行向量,得出的矩阵为左边的矩阵行数+右边矩阵的列数(M*N)*(N*P) = M*P
// 矩阵的逆( inverse) 为倒数运算                      符号:A-¹      A*A-¹ = E     (A*B)-¹  = A-¹ * B-¹
// 矩阵的转置(transpose)为行列互换运算          符号At        (A*B)t = At*Bt
// 转置矩阵和逆矩阵相等的矩阵就被称作正交矩阵(orthogonal matrix)
// a·b = At * B  a的转置矩阵 * b的矩阵
// axb = A* * B  a的对偶矩阵(dual matrix)* b的矩阵 未知?

// 仿射变换(affine transformation) = 线性变换(一个矩阵*一个坐标)+平移变换: 平移(Translation),旋转(Rotation),缩放(Scaling),剪切(Shearing),对称(Mirroring):原点,x,y,z对称,反射(Reflection)
// 齐次坐标系(homogeneous coordinates)
// 笛卡尔坐标系(cartesian coordinates)


