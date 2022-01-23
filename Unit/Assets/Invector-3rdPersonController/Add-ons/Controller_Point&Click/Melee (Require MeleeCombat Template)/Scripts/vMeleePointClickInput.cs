using UnityEngine;

namespace Invector.vCharacterController.PointClick
{
    using vEventSystems;
    using vMelee;
    [vClassHeader("Melee PointClick Input")]
    public class vMeleePointClickInput : vPointAndClickInput, vIMeleeFighter
    {
        protected vMeleeManager meleeManager;
        protected RaycastHit hitEnemy;

        [vEditorToolbar("Inputs")]
        [Header("Melee Inputs")]
        public GenericInput blockInput = new GenericInput("Mouse1", "LB", "LB");

        protected override void Start()
        {
            base.Start();
            meleeManager = gameObject.GetComponent<vMeleeManager>();
        }

        protected override void Update()
        {
            if (lockInput || cc.ragdolled) return;

            base.Update();
            BlockingInput();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            UpdateMeleeAnimations();
            UpdateAttackBehaviour();            
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
            base.MoveToPoint();
        }

        protected virtual void TriggerAttack()
        {
            if (MeleeAttackStaminaConditions())
            {
                animator.SetInteger("AttackID", meleeManager.GetAttackID());
                animator.SetTrigger("WeakAttack");
            }
        }

        public virtual void BlockingInput()
        {
            if (cc.animator == null) return;

            isBlocking = blockInput.GetButton() && cc.currentStamina > 0 && !cc.customAction && !isAttacking;
        }

        protected virtual void RotateTo(Vector3 direction)
        {
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction, transform.up);
        }

        protected virtual bool MeleeAttackStaminaConditions()
        {
            var result = cc.currentStamina - meleeManager.GetAttackStaminaCost();
            return result >= 0;
        }

        #region Update Animations

        protected virtual void UpdateMeleeAnimations()
        {
            if (cc.animator == null || meleeManager == null) return;
            cc.animator.SetInteger("AttackID", meleeManager.GetAttackID());
            cc.animator.SetInteger("DefenseID", meleeManager.GetDefenseID());
            cc.animator.SetBool("IsBlocking", isBlocking);
            cc.animator.SetFloat("MoveSet_ID", meleeManager.GetMoveSetID(), .2f, Time.deltaTime);
        }

        protected virtual void UpdateAttackBehaviour()
        {
            if (cc.IsAnimatorTag("Attack")) return;            
            // force root motion animation while attacking
            cc.forceRootMotion = cc.IsAnimatorTag("Attack") || isAttacking;
        }

        #endregion

        #region Melee Interface

        public bool isBlocking { get; set; }

        public bool isAttacking { get; set; }

        public bool isArmed { get; set; }

        public vICharacter character { get { return cc; } }

        public void OnEnableAttack()
        {
            cc.currentStaminaRecoveryDelay = meleeManager.GetAttackStaminaRecoveryDelay();
            cc.currentStamina -= meleeManager.GetAttackStaminaCost();
            cc.lockAnimRotation = true;
            isAttacking = true;
        }

        public void OnDisableAttack()
        {
            cc.lockAnimRotation = false;
            isAttacking = false;
        }

        public void ResetAttackTriggers()
        {
            cc.animator.ResetTrigger("WeakAttack");
            cc.animator.ResetTrigger("StrongAttack");
        }

        public void BreakAttack(int breakAtkID)
        {
            ResetAttackTriggers();
            OnRecoil(breakAtkID);
        }

        public void OnRecoil(int recoilID)
        {
            cc.animator.SetInteger("RecoilID", recoilID);
            cc.animator.SetTrigger("TriggerRecoil");
            cc.animator.SetTrigger("ResetState");
            cc.animator.ResetTrigger("WeakAttack");
            cc.animator.ResetTrigger("StrongAttack");
        }

        public void OnReceiveAttack(vDamage damage, vIMeleeFighter attacker)
        {
            // character is blocking
            if (!damage.ignoreDefense && isBlocking && meleeManager != null && meleeManager.CanBlockAttack(attacker.character.transform.position))
            {
                var damageReduction = meleeManager.GetDefenseRate();
                if (damageReduction > 0)
                    damage.ReduceDamage(damageReduction);
                if (attacker != null && meleeManager != null && meleeManager.CanBreakAttack())
                    attacker.OnRecoil(meleeManager.GetDefenseRecoilID());
                meleeManager.OnDefense();
                cc.currentStaminaRecoveryDelay = damage.staminaRecoveryDelay;
                cc.currentStamina -= damage.staminaBlockCost;
            }
            // apply damage
            damage.hitReaction = !isBlocking;
            cc.TakeDamage(damage);
        }

        #endregion
    }

}
