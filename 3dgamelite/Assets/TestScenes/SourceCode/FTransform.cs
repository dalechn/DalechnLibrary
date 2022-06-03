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


    private void Rotate(Vector3 eulers, [DefaultValue("Space.Self")]Space relativeTo)
    {
        Quaternion quaternion = Quaternion.Euler(eulers.x, eulers.y, eulers.z);
        if (relativeTo == Space.Self)
            transform.localRotation *= quaternion;
        else
            transform.rotation *= transform.rotation * Quaternion.Inverse(transform.rotation) * quaternion;
    }

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
}
