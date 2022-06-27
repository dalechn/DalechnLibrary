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


//Point:需要构造(x,y,z,1)(只要 w 分量!=0 就可以表示点)
//Vector:需要构造(x,y,z,0)
//旋转矩阵(矩阵的乘法)(右手坐标系): // 逆矩阵: 旋转矩阵是一个正交矩阵,所以求转置矩阵就行
// 绕 x 轴矩阵(yz 平面): //pitch,gradient  
//x′=x                          | 1,0, 0, 0 |           |x|
//y′=ycosθ-zsinθ     | 0,cosθ,-sinθ, 0 | *   |y|
//z′=ysinθ+zcosθ    | 0,sinθ, cosθ, 0 |       |z|
//                              | 0,0, 0, 1 |              |1|

// 绕 z 轴矩阵(xy 平面): //roll,banking  
//x′=ysinθ+zcosθ    | cosθ,-sinθ, 0,0 |    |x|
//y′=ycosθ-zsinθ     | sinθ, cosθ,0,0 |   * |y|                   //未知: 旋转矩阵还需要更多推导?
//z′=z                      | 0,0, 1, 0|                |z|
//                              | 0,0, 0, 1 |             |1|

// 绕 y 轴矩阵(xz 平面): // yaw,turn, heading  
//x′=zsinθ+xcosθ     | cosθ,0, sinθ, 0 |      |x|
//y′=y                       | 0, 1,0 ,0 |           *  |y|
//z′=zcosθ-xsinθ    | -sinθ, 0,cosθ, 0 |        |z|
//                           | 0,0, 0, 1 |                   |1|

//最终的顶点*旋转矩阵: E(α,π/2,β) = Rz(β) * Ry(π/2) * Rx(α) = Ryz(α) *Rzx(π/2)_ Rxy(β) = Ry(π/2) _ Rx(α-β)

//// quaternion 顶点*轴角: 3d 空间中任意一个 v 沿着单位向量 u 旋转 θ 角度之后的 v'为: v' = cos(θ)*v + (1-cos(θ))_(u.v).u + sin(θ)_(uxv)
///
/// 赋值给 transform 的 rotation(Quat=Quat),
/// 对已有 Vec3 变换(Quat*Vec3),
/// 运算 Quat*Quat
///

//平移矩阵(矩阵的加法):                  // 逆矩阵(矩阵的减法):
//x' | 1,0,0,dx|  |x|     |x+dx|                  | 0,0,0,-dx|      |x|     |x-dx|  
//y' | 0,1,0,dy|+|y| =  |y+dy|                  | 0,0,0,-dy|  -  |y| = |y-dy|
//z' | 0,0,1,dz|  |z|     |z+dz|                   | 0,0,0,-dz|     |z|     |z-dz|
//  | 0,0,0,1 |   |1|        |1|                      | 0,0,0,1 |       |1|        |1|

//缩放矩阵(矩阵的乘法)               //逆矩阵:
//x' | sx,0,0,0|  |x|                    | 1/sx,0,0,0|       |x|  
//y' | 0,sy,0,0|* |y|                    | 0,1/sy,0,0|*     |y|
//z' | 0,0,sz,0|  |z|                       | 0,0,1/sz,0|      |z|  
//   | 0,0,0,1 |  |1|                       | 0,0,0, 1 |        |1|

//正交投影矩阵:

//透视投影矩阵:

