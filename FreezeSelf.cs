using Landfall.TABS;
using UnityEngine;

namespace PopCulture {
    public class FreezeSelf : MonoBehaviour {
    
        public void Freeze() {
            
            foreach (var rig in transform.root.GetComponentsInChildren<Rigidbody>()) {
                rig.isKinematic = true;
            }
        }
    
        public void UnFreeze() {
            
            foreach (var rig in transform.root.GetComponentsInChildren<Rigidbody>()) {
                rig.isKinematic = false;
            }
        }
    }
}