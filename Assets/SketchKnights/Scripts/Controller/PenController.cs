using UnityEngine;
using DG.Tweening;
using R3;
using System;
using SketchKnights.Scripts.Manager;

namespace SketchKnights.Scripts.Controller
{
    public class PenController : MonoBehaviour
    {
        [SerializeField] private GameObject penWritePos;
        [SerializeField] private MeshRenderer penTip;
        [SerializeField] private OvrInputObserver ovrInputObserver;
        
        private Vector3 initialScale;
        private Material penTipMaterial;
        private bool isActivated = false;
        private bool isLeftIndexPressed = false;
        private IDisposable leftIndexSubscription;
        
        // Optimization: Cache tag string and material property ID
        private static readonly int EmissionIntensityProperty = Shader.PropertyToID("_EmissionIntensity");

        public Vector3 PenWritePos => penWritePos.transform.position;
        public Transform WriteTransform => penWritePos.transform;
        
        private void Awake()
        {
            // 初期スケールを記憶
            initialScale = transform.localScale;

            // ペンを持つ位置と回転を設定
            transform.localPosition = new Vector3(0.0092f, -0.0275f, -0.0307f);
            transform.Rotate(-135.6f, 4.8f, -7.7f, Space.Self);
            
            // penTipのマテリアルを取得
            if (penTip != null)
            {
                penTipMaterial = penTip.material;
            }
            
            // 左インデックストリガーの状態を監視
            if (ovrInputObserver != null)
            {
                leftIndexSubscription = ovrInputObserver.LIndexTrigger.Subscribe(pressed =>
                {
                    isLeftIndexPressed = pressed;
                    UpdateEmissionIntensity();
                });
            }
        }

        public void EnablePen(bool enable)
        {
            // // 既存のTweenを停止
            // transform.DOKill();

            // if (enable)
            // {
            //     // オブジェクトを有効化
            //     gameObject.SetActive(true);

            //     // 0から初期スケールへスケールアップ（0.5秒）
            //     transform.localScale = Vector3.zero;
            //     transform.DOScale(initialScale, 0.5f).SetEase(Ease.OutBack);
            // }
            // else
            // {
            //     // 現在のスケールから0へスケールダウン（0.5秒）
            //     transform.DOScale(Vector3.zero, 0.5f)
            //         .SetEase(Ease.InBack)
            //         .OnComplete(() => gameObject.SetActive(false));
            // }

            // DOTweenでスケールがバグっている可能性があるためアクティブだけを切り替える
            if (enable)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        private void SetActivate(bool activate)
        {
            isActivated = activate;
            UpdateEmissionIntensity();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // Optimization: Use CompareTag instead of tag comparison
            if (other.CompareTag("Drawable"))
            {
                SetActivate(true);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            // Optimization: Use CompareTag instead of tag comparison
            if (other.CompareTag("Drawable"))
            {
                SetActivate(false);
            }
        }
        
        private void UpdateEmissionIntensity()
        {
            if (penTipMaterial != null)
            {
                float emissionIntensity;
                if (!isLeftIndexPressed && !isActivated)
                {
                    emissionIntensity = 0.0f;
                }
                else if (!isLeftIndexPressed && isActivated)
                {
                    emissionIntensity = 1.0f;
                }
                else if (isLeftIndexPressed && !isActivated)
                {
                    emissionIntensity = 0.5f;
                }
                else // isLeftIndexPressed && isActivated
                {
                    emissionIntensity = 1.5f;
                }
                
                penTipMaterial.SetFloat(EmissionIntensityProperty, emissionIntensity);
            }
        }
        
        private void OnDestroy()
        {
            leftIndexSubscription?.Dispose();
        }
    }
}