using R3;
using UnityEngine;

namespace SketchKnights.Scripts.Manager
{
    public class OvrInputObserver: MonoBehaviour
    {
        private readonly ReactiveProperty<Vector3> _lTouchPosition = new (Vector3.zero);
        private readonly ReactiveProperty<Vector3> _rTouchPosition = new (Vector3.zero);
        private readonly ReactiveProperty<bool> _lIndexTrigger = new (false);
        private readonly ReactiveProperty<bool> _rIndexTrigger = new (false);
        private readonly ReactiveProperty<bool> _lHandTrigger = new (false);
        private readonly ReactiveProperty<bool> _rHandTrigger = new (false);
    
        public ReadOnlyReactiveProperty<Vector3> LTouchPosition => _lTouchPosition;
        public ReadOnlyReactiveProperty<Vector3> RTouchPosition => _rTouchPosition;
        public ReadOnlyReactiveProperty<bool> LIndexTrigger => _lIndexTrigger;
        public ReadOnlyReactiveProperty<bool> RIndexTrigger => _rIndexTrigger;
        public ReadOnlyReactiveProperty<bool> LHandTrigger => _lHandTrigger;
        public ReadOnlyReactiveProperty<bool> RHandTrigger => _rHandTrigger;
    
        // Update is called once per frame
        void Update()
        {
            _lTouchPosition.Value = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            _rTouchPosition.Value = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            
            _lIndexTrigger.Value = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);
            _rIndexTrigger.Value = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
            _lHandTrigger.Value = OVRInput.Get(OVRInput.RawButton.LHandTrigger);
            _rHandTrigger.Value = OVRInput.Get(OVRInput.RawButton.RHandTrigger);
        }

    }
}