using UnityEngine;

namespace Invector.vEventSystems
{
    public class vAnimatorTag : vAnimatorTagBase
    {
        public string[] tags = new string[] { "CustomAction" };

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex); 
            if (stateInfos != null)
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    for (int a = 0; a < stateInfos.Count; a++)
                    {
                        stateInfos[a].AddStateInfo(tags[i], layerIndex);
                    }
                }
            }
            OnStateEnterEvent(tags.vToList());
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            if (stateInfos != null)
            {
                for (int a = 0; a < stateInfos.Count; a++)
                {
                    stateInfos[a].UpdateStateInfo(layerIndex, stateInfo.normalizedTime, stateInfo.shortNameHash);
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfos != null)
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    for (int a = 0; a < stateInfos.Count; a++)
                        stateInfos[a].RemoveStateInfo(tags[i], layerIndex);
                }
            }
            base.OnStateExit(animator, stateInfo, layerIndex);
            OnStateExitEvent(tags.vToList());           
        }
    }

    public static partial class vAnimatorTagDefines
    {
        public const string CustomAction = "CustomAction";
        public const string LockMovement = "LockMovement";
        public const string LockRotation = "LockRotation";
        public const string IgnoreHeadtrack = "IgnoreHeadtrack";
        public const string IgnoreIK = "IgnoreIK";
        public const string Attack = "Attack";
        public const string Upperbody = "Upperbody Pose";
        public const string Shot = "Shot";
        public const string IsEquipping = "IsEquipping";
        public const string IsRolling = "IsRolling";
        public const string Airborne = "Airborne";
        public const string TurnOnSpot = "TurnOnSpot";
        public const string ClimbLadder = "ClimbLadder";
    }
}