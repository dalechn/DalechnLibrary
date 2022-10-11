using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskCategory("Shop")]
    public class WanderArea : Wander
    {
        public RandomAreaName areaName;
        private RandomArea area;

        public override void OnStart()
        {
            base.OnStart();

            area = ShopInfo.Instance.GetFloor(areaName);
        }

        protected override bool TrySetTarget()
        {
            //var direction = transform.forward;
            var validDestination = false;
            var attempts = targetRetries.Value;
            var destination = transform.position;
            while (!validDestination && attempts > 0)
            {
                //direction = direction + Random.insideUnitSphere * wanderRate.Value;
                //destination = transform.position + direction.normalized * Random.Range(minWanderDistance.Value, maxWanderDistance.Value);

                destination = area.GetPosition();
                validDestination = SamplePosition(destination);
                attempts--;
            }
            if (validDestination)
            {
                SetDestination(destination);
            }
            return validDestination;
        }

    }
}