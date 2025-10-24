using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace SketchKnights.Scripts.Controller
{
    /// <summary>
    /// OVR controller vibration utility class
    /// </summary>
    public static class OvrVibration
    {
        // Fixed frequency for vibration
        private const float FIXED_FREQUENCY = 1.0f;
        
        // Cancellation tokens for managing ongoing vibrations
        private static CancellationTokenSource _leftVibrationCts;
        private static CancellationTokenSource _rightVibrationCts;
        private static CancellationTokenSource _bothVibrationCts;
        
        /// <summary>
        /// Trigger vibration on the left controller
        /// </summary>
        /// <param name="amp">Vibration amplitude (0.0 to 1.0)</param>
        /// <param name="duration">Duration in seconds</param>
        public static async UniTask LeftVibration(float amp, float duration)
        {
            // Cancel any existing left vibration
            _leftVibrationCts?.Cancel();
            _leftVibrationCts = new CancellationTokenSource();
            
            // Clamp amplitude to valid range
            amp = Mathf.Clamp01(amp);
            
            try
            {
                // Start vibration on left controller
                OVRInput.SetControllerVibration(FIXED_FREQUENCY, amp, OVRInput.Controller.LTouch);
                
                // Wait for the specified duration
                await UniTask.Delay((int)(duration * 1000), cancellationToken: _leftVibrationCts.Token);
                
                // Stop vibration
                OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
            }
            catch (System.OperationCanceledException)
            {
                // Vibration was cancelled, stop it
                OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
            }
        }
        
        /// <summary>
        /// Trigger vibration on the right controller
        /// </summary>
        /// <param name="amp">Vibration amplitude (0.0 to 1.0)</param>
        /// <param name="duration">Duration in seconds</param>
        public static async UniTask RightVibration(float amp, float duration)
        {
            // Cancel any existing right vibration
            _rightVibrationCts?.Cancel();
            _rightVibrationCts = new CancellationTokenSource();
            
            // Clamp amplitude to valid range
            amp = Mathf.Clamp01(amp);
            
            try
            {
                // Start vibration on right controller
                OVRInput.SetControllerVibration(FIXED_FREQUENCY, amp, OVRInput.Controller.RTouch);
                
                // Wait for the specified duration
                await UniTask.Delay((int)(duration * 1000), cancellationToken: _rightVibrationCts.Token);
                
                // Stop vibration
                OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
            }
            catch (System.OperationCanceledException)
            {
                // Vibration was cancelled, stop it
                OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
            }
        }
        
        /// <summary>
        /// Trigger vibration on both controllers simultaneously
        /// </summary>
        /// <param name="amp">Vibration amplitude (0.0 to 1.0)</param>
        /// <param name="duration">Duration in seconds</param>
        public static async UniTask BothVibration(float amp, float duration)
        {
            // Cancel any existing both vibration
            _bothVibrationCts?.Cancel();
            _bothVibrationCts = new CancellationTokenSource();
            
            // Clamp amplitude to valid range
            amp = Mathf.Clamp01(amp);
            
            try
            {
                // Start vibration on both controllers
                OVRInput.SetControllerVibration(FIXED_FREQUENCY, amp, OVRInput.Controller.LTouch);
                OVRInput.SetControllerVibration(FIXED_FREQUENCY, amp, OVRInput.Controller.RTouch);
                
                // Wait for the specified duration
                await UniTask.Delay((int)(duration * 1000), cancellationToken: _bothVibrationCts.Token);
                
                // Stop vibration on both controllers
                OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
                OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
            }
            catch (System.OperationCanceledException)
            {
                // Vibration was cancelled, stop it
                OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
                OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
            }
        }
        
        /// <summary>
        /// Set continuous vibration on the left controller (no duration)
        /// </summary>
        /// <param name="amp">Vibration amplitude (0.0 to 1.0)</param>
        public static void SetLeftVibration(float amp)
        {
            // Cancel any existing left vibration timer
            _leftVibrationCts?.Cancel();
            
            // Clamp amplitude to valid range
            amp = Mathf.Clamp01(amp);
            
            // Set vibration on left controller
            OVRInput.SetControllerVibration(FIXED_FREQUENCY, amp, OVRInput.Controller.LTouch);
        }
        
        /// <summary>
        /// Set continuous vibration on the right controller (no duration)
        /// </summary>
        /// <param name="amp">Vibration amplitude (0.0 to 1.0)</param>
        public static void SetRightVibration(float amp)
        {
            // Cancel any existing right vibration timer
            _rightVibrationCts?.Cancel();
            
            // Clamp amplitude to valid range
            amp = Mathf.Clamp01(amp);
            
            // Set vibration on right controller
            OVRInput.SetControllerVibration(FIXED_FREQUENCY, amp, OVRInput.Controller.RTouch);
        }
        
        /// <summary>
        /// Set continuous vibration on both controllers (no duration)
        /// </summary>
        /// <param name="amp">Vibration amplitude (0.0 to 1.0)</param>
        public static void SetBothVibration(float amp)
        {
            // Cancel any existing both vibration timer
            _bothVibrationCts?.Cancel();
            
            // Clamp amplitude to valid range
            amp = Mathf.Clamp01(amp);
            
            // Set vibration on both controllers
            OVRInput.SetControllerVibration(FIXED_FREQUENCY, amp, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(FIXED_FREQUENCY, amp, OVRInput.Controller.RTouch);
        }
        
        /// <summary>
        /// Stop all vibrations
        /// </summary>
        public static void StopAllVibrations()
        {
            // Cancel all ongoing vibrations
            _leftVibrationCts?.Cancel();
            _rightVibrationCts?.Cancel();
            _bothVibrationCts?.Cancel();
            
            // Stop vibration on both controllers
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
        }
    }
}