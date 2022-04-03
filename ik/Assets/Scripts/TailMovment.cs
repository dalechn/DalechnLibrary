using UnityEngine;

public class TailMovment : MonoBehaviour
{
    public int index;

    public void Move(Vector3 targetPosition ,float speed)
    {
        transform.LookAt(targetPosition);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
    }

}
