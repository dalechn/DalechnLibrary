using UnityEngine;

namespace Dalechn
{
    public class vComment : StateMachineBehaviour
    {
#if UNITY_EDITOR
        [Multiline]
        public string comment;
#endif
    }
}