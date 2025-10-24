using System;
using System.Linq;
using SplineMesh;
using UnityEngine;
using Cysharp.Threading.Tasks;
using R3;

namespace SketchKnights.Scripts.Controller
{
    public class BattleWeapon: MonoBehaviour
    {        
        [SerializeField] private Spline spline;
        
        private readonly Subject<Collider> _onHit = new ();
        public Observable<Collider> OnHit => _onHit;
        private readonly Subject<Unit> _onDestruction = new();
        public Observable<Unit> OnDestruction => _onDestruction;

        protected virtual void Awake()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            spline.gameObject.SetActive(false);
        }
        
        public void Initialize(SplineNode[] nodes, WeaponStyle style, int layer)
        {
            if(_isDestructioned) return;
            spline.nodes = nodes.ToList();
            spline.RefreshCurves();
            spline.gameObject.SetActive(true);
            
            tag = Utils.WeaponStyleToTag(style);
            ApplyCollider(layer).Forget();
        }

        private async UniTaskVoid ApplyCollider(int layer)
        {
            await UniTask.DelayFrame(spline.nodes.Count);
            if (!this || !gameObject) return;

            Utils.ApplyLayerToChildren(gameObject, layer);
            foreach (var col in GetComponentsInChildren<MeshCollider>())
            {
                col.convex = true;
                col.isTrigger = true;
                col.gameObject.tag = tag;
            }
        }

        private bool _isDestructioned = false;
        private void OnTriggerEnter(Collider other)
        {
            if(_isDestructioned) return;
            _onHit?.OnNext(other);
        }

        public void Destruction()
        {
            if(_isDestructioned) return;
            _isDestructioned = true;
            _onDestruction.OnNext(Unit.Default);
            _onHit.Dispose();
            _onDestruction.Dispose();
            DestroyNextFrame().Forget();
        }

        private async UniTaskVoid DestroyNextFrame()
        {
            await UniTask.NextFrame();
            Destroy(gameObject);
        }
    }
}