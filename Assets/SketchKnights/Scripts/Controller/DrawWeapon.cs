using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using SplineMesh;
using UnityEngine;

namespace SketchKnights.Scripts.Controller
{
    public class DrawWeapon : MonoBehaviour
    {
        [SerializeField] private Spline spline;
        [SerializeField] private MeshRenderer drawable;
        [SerializeField] private WeaponStyle style;
        private SplineMeshTiling _tiling;
        
        private static readonly Color DisableColor = new Color(1f, 1f, 1f, 0.5f);
        private static readonly Color EnableColor = new Color(0, 0.5f, 0, 0.5f);

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            spline.gameObject.SetActive(false);
            drawable.material.color = DisableColor;
        }

        private bool _isDrawing = false;
        private readonly Vector3 _scale = Vector3.one * 0.13f;
        private Vector3 _previousUp = Vector3.up;

        public bool DrawStart(Vector3 target)
        {
            if (_isDrawing || !_isDrawable) return false;
            drawable.enabled = false;
            _isDrawing = true;
            
            spline.nodes.Clear();
            
            // rootノードの設定
            spline.AddNode(new SplineNode(Vector3.zero, Vector3.zero));
            spline.nodes[0].Scale = Vector3.zero;
            
            target = transform.InverseTransformPoint(target);
            var direction = (target - Vector3.zero).normalized;
            
            // 2番目のノード
            spline.AddNode(new SplineNode(target, direction));
            spline.nodes[1].Scale = _scale;
            
            // 初期のUpベクトルを設定
            _previousUp = Vector3.up;
            if (Mathf.Abs(Vector3.Dot(direction, Vector3.up)) > 0.9f)
            {
                _previousUp = Vector3.right;
            }
            
            spline.RefreshCurves();
            UpdateNodeScales();
            spline.gameObject.SetActive(true);
            return true;
        }
    
        public void Draw(Vector3 target)
        {
            if (!_isDrawing) return;
            
            var lastNodePos = spline.nodes[^2].Position;
            var currentNode = spline.nodes[^1];
            target = transform.InverseTransformPoint(target);
            
            currentNode.Position = target;
            var currentDirection = (target - lastNodePos).normalized;

            var distance = Vector3.Distance(target, lastNodePos);
            if (distance > 0.1f)
            {
                // 新しいノードを追加
                spline.AddNode(new SplineNode(target, currentDirection));
            }
            spline.RefreshCurves();
            UpdateNodeScales();
        }

        private void UpdateNodeScales()
        {
            if (spline.nodes.Count < 2) return;

            float totalLength = CalculateTotalSplineLength();
            
            for (int i = 0; i < spline.nodes.Count; i++)
            {
                float distanceFromRoot = CalculateDistanceFromRoot(i);
                float distanceFromTip = totalLength - distanceFromRoot;
                
                float scale = CalculateScaleAtDistance(distanceFromRoot, distanceFromTip, totalLength);
                spline.nodes[i].Scale = _scale * scale;
            }
        }

        private float CalculateTotalSplineLength()
        {
            float totalLength = 0f;
            for (int i = 1; i < spline.nodes.Count; i++)
            {
                totalLength += Vector3.Distance(spline.nodes[i-1].Position, spline.nodes[i].Position);
            }
            return totalLength;
        }

        private float CalculateDistanceFromRoot(int nodeIndex)
        {
            float distance = 0f;
            for (int i = 1; i <= nodeIndex; i++)
            {
                distance += Vector3.Distance(spline.nodes[i-1].Position, spline.nodes[i].Position);
            }
            return distance;
        }

        private float CalculateScaleAtDistance(float distanceFromRoot, float distanceFromTip, float totalLength)
        {
            // 根本から0.2の距離までは0.5~1の範囲で変化
            if (distanceFromRoot <= 0.2f)
            {
                float t = distanceFromRoot / 0.2f; // 0~1の範囲に正規化
                return Mathf.Lerp(0.5f, 1.0f, t);
            }
            
            // 先端から0.3の距離までは1~0の範囲で変化
            if (distanceFromTip <= 0.3f)
            {
                float t = distanceFromTip / 0.3f; // 0~1の範囲に正規化
                return Mathf.Lerp(0.0f, 1.0f, t);
            }
            
            // 中間部分は1.0で固定
            return 1.0f;
        }

        public SplineNode[] DrawEnd()
        {
            if (!_isDrawing) return null;
            _isDrawing = false;
            drawable.enabled = true;
            ResetSpline().Forget();
            return spline.nodes.ToArray();
        }

        private async UniTaskVoid ResetSpline()
        {
            await UniTask.NextFrame();
            spline.nodes.Clear();
            spline.AddNode(new SplineNode(Vector3.zero, Vector3.zero));
            spline.AddNode(new SplineNode(Vector3.zero, Vector3.zero));
            spline.RefreshCurves();
            await UniTask.NextFrame();
            spline.gameObject.SetActive(false);
        }

        public void SetPathColor(Color color)
        {
            if (_tiling == null)
            {
                if (spline.TryGetComponent<SplineMeshTiling>(out var tiling))
                {
                    _tiling = tiling;
                }
                else throw new Exception("SplineMeshTiling was not found.");
            }
            
            _tiling.material.color = color;
        }

        // writerが1前提のコード、複雑になったら多分バグる
        private bool _isDrawable = false;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Writer"))
            {
                _isDrawable = true;
                drawable.material.color = EnableColor;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Writer"))
            {
                _isDrawable = false;
                drawable.material.color = DisableColor;
            }
        }
    }
}