using UnityEngine;
using SketchKnights.Scripts.Controller; // BattlePlayer, PenController

namespace SketchKnights.Scripts.Manager
{
    public class XrObjectResolver: MonoBehaviour
    {
        [SerializeField] private BattlePlayer battlePlayer;
        [SerializeField] private PenController penController;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private GameObject xrBattleUi;
        
        public BattlePlayer BattlePlayer => battlePlayer;
        public PenController PenController => penController;
        public Transform CameraTransform => cameraTransform;
        public GameObject XrBattleUi => xrBattleUi;
    }
}