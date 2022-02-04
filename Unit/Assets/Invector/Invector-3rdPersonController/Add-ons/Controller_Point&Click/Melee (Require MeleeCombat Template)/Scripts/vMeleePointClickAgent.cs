using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Invector.vCharacterController.PointClick
{
    [vClassHeader("Melee PointClick Input")]
    public class vMeleePointClickAgent : vMeleePointClickInput
    {
       
        protected UnityEngine.AI.NavMeshAgent agent;

        protected override void Start()
        {
            base.Start();
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
   
        public override void MoveToPoint()
        {           
            if (target && meleeManager.hitProperties.hitDamageTags.Contains(target.gameObject.tag))
            {
                if (Physics.Raycast(cc._capsuleCollider.bounds.center, (target.bounds.center - cc._capsuleCollider.bounds.center).normalized, out hitEnemy, meleeManager.GetAttackDistance()) && hitEnemy.collider.gameObject == target.gameObject)
                {
                    cc.StopCharacterWithLerp();
                    RotateTo(target.transform.position - transform.position);
                    ClearTarget();
                    TriggerAttack();
                    return;
                }
            }
            if (!NearPoint(cursorPoint, transform.position) && target)
            {
                Vector3 dir = cursorPoint - transform.position;
                if (agent)
                {
                    if (agent.updatePosition) agent.updatePosition = false;
                    if (agent.updateRotation) agent.updateRotation = false;

                    if (dir.magnitude > 0.1f) agent.destination = cursorPoint;
                    dir = agent.desiredVelocity.normalized.normalized * Mathf.Clamp(agent.desiredVelocity.normalized.magnitude, 0.1f, 1f);
                    var targetDir = cc.isStrafing ? transform.InverseTransformDirection(dir).normalized : dir.normalized;
                    cc.input = targetDir;
                    agent.nextPosition = transform.position;
                }
                else
                {

                    dir = dir.normalized * Mathf.Clamp(dir.magnitude, 0f, 1f);
                    var targetDir = cc.isStrafing ? transform.InverseTransformDirection(dir).normalized : dir.normalized;
                    cc.input = targetDir;
                }
            }
            else
            {
                if (onDisableCursor != null)
                    onDisableCursor.Invoke();
                cc.input = Vector2.Lerp(cc.input, Vector3.zero, 20 * Time.deltaTime);
            }
        }
    }
}