using UnityEngine;

public class UnEffect : MonoBehaviour
{
    public SmoothPath smoothPath;
    public Transform target;

    private int strength;
    private Rigidbody body;
    private Collider boxCollider;

    private Vector3 size;

    private void OnCollisionEnter(Collision colHit)
    {
    }

    private void Awake()
    {
    }

    private void OnDestroy()
    {
    }


    private RaycastHit[] raycastHit = new RaycastHit[0];
    private RaycastHit raycastHit2 = new RaycastHit();
    private bool BoxCast()
    {
        //raycastHit = Physics.BoxCastAll(smoothPath.transform.position,
        //    (smoothPath.size / 2),
        //    (target.position - smoothPath.transform.position).normalized, transform.rotation, smoothPath.circleRadius, -1, QueryTriggerInteraction.Ignore);
        ////raycastHit = Physics.RaycastAll(transform.position,
        ////    (target.position - transform.position).normalized,  smoothPath.unitLength, -1, QueryTriggerInteraction.Ignore);
        //foreach (var val in raycastHit)
        //{
        //    if (val.collider.tag != smoothPath.selfTag &&
        //         val.collider.tag != smoothPath.coinTag && val.collider.tag != smoothPath.flyCoinTag)
        //    {
        //        Vector3 pos = (smoothPath.transform.position + (target.position - smoothPath.transform.position).normalized * val.distance);
        //        //Vector3 pos = val.point + new Vector3(size.x * val.normal.x, size.x * val.normal.y, size.x * val.normal.z) / 2;

        //        transform.position = Vector3.Slerp(transform.position, pos, Time.fixedDeltaTime * 5);

        //        if (Physics.BoxCast(transform.position, smoothPath.size / 2, -Vector3.up, out raycastHit2, transform.rotation, smoothPath.size.y, smoothPath.kartMask, QueryTriggerInteraction.Ignore))
        //        {
        //            transform.position = Vector3.Slerp(transform.position, pos + Vector3.up * raycastHit2.collider.bounds.size.y, Time.fixedDeltaTime * 5);
        //        }
        //        return true;
        //    }
        //}

        return false;
    }

    private void FixedUpdate()
    {
        if (enabled && target)
        {
            if (!BoxCast())
            {
                Vector3 pos = target.position;
                if (Physics.BoxCast(transform.position, smoothPath.size / 2, -Vector3.up, out raycastHit2, transform.rotation, smoothPath.size.y, smoothPath.kartMask, QueryTriggerInteraction.Ignore))
                {
                    pos += Vector3.up * raycastHit2.collider.bounds.size.y;
                }
                transform.position = Vector3.Slerp(transform.position, pos, Time.fixedDeltaTime * 5);
            }
        }
        //transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.identity, Time.fixedDeltaTime*10);
    }

    private void OnTriggerStay(Collider other)
    {
        if (enabled && target)
        {
            BoxCast();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == smoothPath.flyCoinTag)
        {
            UnEffect path = other.gameObject.GetComponent<UnEffect>();
            if (!path || path.smoothPath != smoothPath)
            {
                strength--;

                if (strength == 0)
                {
                    tag = "Untagged";

                    body.isKinematic = false;
                    boxCollider.isTrigger = false;

                    Vector3 dir = (transform.position - smoothPath.transform.position).normalized;
                    body.AddExplosionForce(smoothPath.collisionForce, transform.position + dir * 10, 0, 2, ForceMode.VelocityChange);

                    transform.parent = SmoothPath.cubeDetachedParent;
                    enabled = false;
                    smoothPath.RemoveCoin(gameObject);

                    //DoSchedule.scheduleOnce(gameObject, (float t) =>
                    //{
                    //    Destroy(this);
                    //    tag = smoothPath.coinTag;
                    //}, smoothPath.coinCooldown);
                }
            }

        }

        if (enabled && target)
        {
            BoxCast();
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Gizmos.DrawRay(transform.position, (target.position - transform.position).normalized * raycastHit.distance);
        //Gizmos.DrawWireCube(smoothPath.transform.position + (target.position - smoothPath.transform.position).normalized * raycastHit.distance, smoothPath.size);

        //Gizmos.DrawRay(transform.position + Vector3.up, -Vector3.up * raycastHit.distance);
        //Gizmos.DrawWireCube(transform.position + Vector3.up  -Vector3.up * raycastHit.distance, smoothPath.size);
    }

    public void Init(SmoothPath path)
    {
        smoothPath = path;

        strength = smoothPath.strength;
        tag = smoothPath.flyCoinTag;
        transform.parent = path.transform;

        body = gameObject.GetComponent<Rigidbody>();
        body.isKinematic = true;
        boxCollider = gameObject.GetComponent<Collider>();
        boxCollider.isTrigger = true;
        size = boxCollider.bounds.size;
    }

    //IEnumerator GetCycle()
    //{
    //    float t = 0;
    //    while (t < 1)
    //    {
    //        t += Time.deltaTime * 2;
    //        transform.position = Vector3.Lerp(startPos, targetKart.position, t) + Vector3.up * 2 * Mathf.Sin(t * Mathf.PI);
    //        yield return null;
    //    }

    //    rend.enabled = false;
    //}
}
