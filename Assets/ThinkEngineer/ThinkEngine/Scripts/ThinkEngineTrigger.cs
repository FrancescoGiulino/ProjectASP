using System;
using UnityEngine;
// every method of this class without parameters and that returns a bool value can be used to trigger the reasoner.
namespace ThinkEngine
{
    public class ThinkEngineTrigger : ScriptableObject
    {
        private float lastTriggerTime = 0f;
        private const float interval = 2f;

        public bool EnemyTrigger()
        {
            if (Time.time - lastTriggerTime >= interval)
            {
                lastTriggerTime = Time.time;
                return true;
            }
            return false;
        }
	}
}