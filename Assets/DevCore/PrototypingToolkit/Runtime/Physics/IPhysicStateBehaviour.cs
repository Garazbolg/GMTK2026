using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    /// <summary>
    /// Implements all physics callbacks of a Physic State
    /// </summary>
    public interface IPhysicStateBehaviour {
        public void OnBeforePhysicsResolved();
        
        public void OnAfterPhysicsResolved();
        
        public void OnStateCollisionEnter(bool active, Collision other);

        public void OnStateCollisionExit(bool active, Collision other);

        public void OnStateCollisionStay(bool active, Collision other);

        public void OnStateTriggerEnter(bool active, Collider other);

        public void OnStateTriggerExit(bool active, Collider other);

        public void OnStateTriggerStay(bool active, Collider other);
    }
}
