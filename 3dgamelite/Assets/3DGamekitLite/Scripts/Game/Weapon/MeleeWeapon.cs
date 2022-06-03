using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dest.Math;
using Micosmo.SensorToolkit;

namespace Gamekit3D
{
    public class MeleeWeapon : MonoBehaviour
    {
        public int damage = 1;

        [System.Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform attackRoot;

#if UNITY_EDITOR
            //editor only as it's only used in editor to display the path of the attack that is used by the raycast
            [NonSerialized] public List<Vector3> previousPositions = new List<Vector3>();
#endif

        }

        public ParticleSystem hitParticlePrefab;
        public LayerMask targetLayers;

        public AttackPoint[] attackPoints = new AttackPoint[0];

        public TimeEffect[] effects;

        [Header("Audio")] public RandomAudioPlayer hitAudio;
        public RandomAudioPlayer attackAudio;

        public bool throwingHit
        {
            get { return m_IsThrowingHit; }
            set { m_IsThrowingHit = value; }
        }

        protected GameObject m_Owner;

        protected Vector3[] m_PreviousPos = null;
        protected Vector3 m_Direction;

        protected bool m_IsThrowingHit = false;
        protected bool m_InAttack = false;

        const int PARTICLE_COUNT = 10;
        protected ParticleSystem[] m_ParticlesPool = new ParticleSystem[PARTICLE_COUNT];
        protected int m_CurrentParticle = 0;

        protected static RaycastHit[] s_RaycastHitCache = new RaycastHit[32];
        protected static Collider[] s_ColliderCache = new Collider[32];

        //public bool InCombo { get; set; }

        [Serializable]
        public class TargetInfo
        {
            public Transform target;
            public Collider col;
            public Box3 box;
            public Sphere3 sphere;
            private bool isSphere = false;

            public TargetInfo(Transform target, Collider col)
            {
                BoxCollider boxCol = col as BoxCollider;
                CapsuleCollider capsuleCol = col as CapsuleCollider;
                CharacterController characterCol = col as CharacterController;
                SphereCollider sphereCol = col as SphereCollider;

                if (boxCol)
                {
                    box = new Box3(target.TransformPoint(boxCol.center), target.right, target.up, target.forward, Vector3.Scale( boxCol.size /2,target.lossyScale));
                }
                else if (capsuleCol)
                {
                    box = new Box3(target.TransformPoint(capsuleCol.center), target.right, target.up, target.forward, new Vector3(capsuleCol.radius, capsuleCol.height / 2, capsuleCol.radius));
                }
                else if (characterCol)
                {
                    box = new Box3(target.TransformPoint(characterCol.center), target.right, target.up, target.forward, new Vector3(characterCol.radius, characterCol.height / 2, characterCol.radius));
                }
                else if (sphereCol)
                {
                    sphere = new Sphere3(target.TransformPoint(sphereCol.center), sphereCol.radius);
                    isSphere = true;
                }
                else
                {
                    Debug.LogError("NO COLLIDER");
                }

                this.target = target;
                this.col = col;
            }

            public bool Test(Vector3 point0,Vector3 point1, ref Vector3 point)
            {
                Segment3 seg = new Segment3(point0, point1);

                bool find = false;
                if (!isSphere)
                {
                    Segment3Box3Intr info;
                    find = Intersection.FindSegment3Box3(ref seg, ref box, out info);

                    point = info.Point0;
                }
                else
                {
                    Segment3Sphere3Intr info;
                    find = Intersection.FindSegment3Sphere3(ref seg, ref sphere, out info);

                    point = info.Point0;
                }

                find &= target;

                return find;
            }
        }

        [System.Serializable]
        public class AttackSegmentInfo
        {
            public Transform point0;
            public Transform point1;
        }

        protected List<TargetInfo> targetInfoList = new List<TargetInfo>(32);
        protected List<Transform> hitList = new List<Transform>();

        public List<AttackSegmentInfo> attackSegmentInfoArr;

        private void Awake()
        {
            if (hitParticlePrefab != null)
            {
                for (int i = 0; i < PARTICLE_COUNT; ++i)
                {
                    m_ParticlesPool[i] = Instantiate(hitParticlePrefab);
                    m_ParticlesPool[i].Stop();
                }
            }
        }

        private void OnEnable()
        {

        }

        //whoever own the weapon is responsible for calling that. Allow to avoid "self harm"
        public void SetOwner(GameObject owner)
        {
            m_Owner = owner;
        }

        public void BeginAttack(bool thowingAttack)
        {
            hitList.Clear();
            targetInfoList.Clear();

            if (attackAudio != null)
                attackAudio.PlayRandomClip();
            throwingHit = thowingAttack;

            m_InAttack = true;

            m_PreviousPos = new Vector3[attackPoints.Length];

            for (int i = 0; i < attackPoints.Length; ++i)
            {
                Vector3 worldPos = attackPoints[i].attackRoot.position +
                                   attackPoints[i].attackRoot.TransformVector(attackPoints[i].offset);
                m_PreviousPos[i] = worldPos;

#if UNITY_EDITOR
                attackPoints[i].previousPositions.Clear();
                attackPoints[i].previousPositions.Add(m_PreviousPos[i]);
#endif
            }

        }

        public void EndAttack()
        {
            hitList.Clear();
            targetInfoList.Clear();

            m_InAttack = false;


#if UNITY_EDITOR
            for (int i = 0; i < attackPoints.Length; ++i)
            {
                attackPoints[i].previousPositions.Clear();
            }
#endif
        }

        const float k_OverlapRadius = 3;
        private void FixedUpdate()
        {
            if (m_InAttack)
            {
                if (attackSegmentInfoArr.Count < 1)
                {
                    for (int i = 0; i < attackPoints.Length; ++i)
                    {
                        AttackPoint pts = attackPoints[i];

                        Vector3 worldPos = pts.attackRoot.position + pts.attackRoot.TransformVector(pts.offset);
                        Vector3 attackVector = worldPos - m_PreviousPos[i];

                        if (attackVector.magnitude < 0.001f)
                        {
                            // A zero vector for the sphere cast don't yield any result, even if a collider overlap the "sphere" created by radius. 
                            // so we set a very tiny microscopic forward cast to be sure it will catch anything overlaping that "stationary" sphere cast
                            attackVector = Vector3.forward * 0.0001f;
                        }


                        Ray r = new Ray(worldPos, attackVector.normalized);

                        int contacts = Physics.SphereCastNonAlloc(r, pts.radius, s_RaycastHitCache, attackVector.magnitude,
                            ~0,
                            QueryTriggerInteraction.Ignore);

                        for (int k = 0; k < contacts; ++k)
                        {
                            Collider col = s_RaycastHitCache[k].collider;

                            if (col != null)
                                CheckDamage(col.transform, s_RaycastHitCache[k].point);
                        }

                        m_PreviousPos[i] = worldPos;

#if UNITY_EDITOR
                        pts.previousPositions.Add(m_PreviousPos[i]);
#endif
                    }
                }
                else
                {
                    int col = Physics.OverlapSphereNonAlloc(m_Owner.transform.position, k_OverlapRadius, s_ColliderCache, targetLayers, QueryTriggerInteraction.Collide);

                    for (int i = 0; i < col; i++)
                    {
                        targetInfoList.Add(new TargetInfo(s_ColliderCache[i].transform, s_ColliderCache[i]));
                    }

                    for (int i = 0; i < attackSegmentInfoArr.Count; i++)
                    {
                        if (attackSegmentInfoArr[i].point0 && attackSegmentInfoArr[i].point1)
                        {
                            for (int j = 0; j < col; j++)
                            {
                                TargetInfo tr = targetInfoList[j];

                                Vector3 point = default;
                                bool find = tr.Test(attackSegmentInfoArr[i].point0.position, attackSegmentInfoArr[i].point1.position, ref point);
 
                                if (find && !hitList.Contains(tr.target))
                                {
                                    hitList.Add(tr.target);

                                    CheckDamage(tr.target, point);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool CheckDamage(Transform other, Vector3 pts)
        {
            Damageable d = other.GetComponent<Damageable>();
            if (d == null)
            {
                return false;
            }

            if (d.gameObject == m_Owner)
                return true; //ignore self harm, but do not end the attack (we don't "bounce" off ourselves)

            if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
            {
                //hit an object that is not in our layer, this end the attack. we "bounce" off it
                return false;
            }

            //Debug.Log(other+" "+ pts);

            if (hitAudio != null)
            {
                var renderer = other.GetComponent<Renderer>();
                if (!renderer)
                    renderer = other.GetComponentInChildren<Renderer>();
                if (renderer)
                    hitAudio.PlayRandomClip(renderer.sharedMaterial);
                else
                    hitAudio.PlayRandomClip();
            }

            Damageable.DamageMessage data;

            data.amount = damage;
            data.damager = this;
            data.direction = m_Direction.normalized;
            data.damageSource = m_Owner.transform.position;
            data.throwing = m_IsThrowingHit;
            data.stopCamera = false;

            d.ApplyDamage(data);

            if (hitParticlePrefab != null)
            {
                m_ParticlesPool[m_CurrentParticle].transform.position = pts;
                m_ParticlesPool[m_CurrentParticle].time = 0;
                m_ParticlesPool[m_CurrentParticle].Play();
                m_CurrentParticle = (m_CurrentParticle + 1) % PARTICLE_COUNT;
            }

            return true;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            for (int i = 0; i < attackPoints.Length; ++i)
            {
                AttackPoint pts = attackPoints[i];

                if (pts.attackRoot != null)
                {
                    Vector3 worldPos = pts.attackRoot.TransformVector(pts.offset);
                    Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
                    Gizmos.DrawSphere(pts.attackRoot.position + worldPos, pts.radius);
                }

                if (pts.previousPositions.Count > 1)
                {
                    UnityEditor.Handles.DrawAAPolyLine(10, pts.previousPositions.ToArray());
                }
            }

            if (m_Owner)
            {
                SensorGizmos.PushColor(SensorGizmos.GizmoColor);
                SensorGizmos.SphereGizmo(m_Owner.transform.position, k_OverlapRadius);
                SensorGizmos.PopColor();

                foreach (var val in targetInfoList)
                {
                    DrawBox(ref val.box);
                }

            }
        }

        protected void DrawBox(ref Box3 box)
        {
            Vector3 v0, v1, v2, v3, v4, v5, v6, v7;
            box.CalcVertices(out v0, out v1, out v2, out v3, out v4, out v5, out v6, out v7);
            Gizmos.DrawLine(v0, v1);
            Gizmos.DrawLine(v1, v2);
            Gizmos.DrawLine(v2, v3);
            Gizmos.DrawLine(v3, v0);
            Gizmos.DrawLine(v4, v5);
            Gizmos.DrawLine(v5, v6);
            Gizmos.DrawLine(v6, v7);
            Gizmos.DrawLine(v7, v4);
            Gizmos.DrawLine(v0, v4);
            Gizmos.DrawLine(v1, v5);
            Gizmos.DrawLine(v2, v6);
            Gizmos.DrawLine(v3, v7);
        }

#endif
    }
}