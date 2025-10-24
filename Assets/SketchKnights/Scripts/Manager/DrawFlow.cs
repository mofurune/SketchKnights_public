using System;
using System.Collections.Generic;
using DG.Tweening;
using R3;
using SketchKnights.Scripts.Controller;
using SplineMesh;
using UnityEngine;
using UnityEngine.UI;
using KanKikuchi.AudioManager;
using OVR.OpenVR; // 効果音再生のために追加

namespace SketchKnights.Scripts.Manager
{
    public class DrawFlow: MonoBehaviour
    {
        [SerializeField] private XrObjectResolver xrObjectResolver;
        [SerializeField] private OvrInputObserver ovrInputObserver;
        [SerializeField] private DrawWeapon drawSword;
        [SerializeField] private DrawWeapon drawGuard;
        [SerializeField] private BattleSword battleSwordPrefab;
        [SerializeField] private BattleGuard battleGuardPrefab;
        [SerializeField] private WeaponIconsView weaponIconsSwordView;
        [SerializeField] private WeaponIconsView weaponIconsShieldView;
        [SerializeField] private Slider timerSlider;

        [Header("指示文")]
        // 単一 -> 複数（2枚）に変更
        [SerializeField] private Image mainImage1;
        [SerializeField] private Image mainImage2;
        // 剣/盾 それぞれ mainImage1/2 用スプライト
        [SerializeField] private Sprite mainImageSword1;
        [SerializeField] private Sprite mainImageSword2;
        [SerializeField] private Sprite mainImageShield1;
        [SerializeField] private Sprite mainImageShield2;
        
        [Header("武器数設定")]
        [SerializeField] private int swordCount = 3;
        [SerializeField] private int shieldCount = 3;
        
        private int TotalWeaponCount => swordCount + shieldCount;
        
        private readonly Subject<BattlePlayerData> _finishDraw = new();
        public Observable<BattlePlayerData> FinishDraw => _finishDraw;
        
        private OvrWeaponDraw _ovrWeaponDraw;
        private IDisposable _drawSubscription;
        private IDisposable _timerStartSubscription;
        private List<SplineNode[]> _swordNodeList = new ();
        private List<SplineNode[]> _guardNodeList = new ();
        private List<BattleWeapon> _weaponInstanceList = new ();
        private Tween _timerTween;
        
        // タイムアップによる完了かどうかを管理する
        private bool _isTimeout = false;
        
        // Model
        private readonly ReactiveProperty<int> _progress = new (0);
        private readonly ReactiveProperty<float> _timer = new (1);

        private bool IsSword(int progress)
        {
            return progress < swordCount;
        }
        
        private void Awake()
        {
            // View
            _progress.Subscribe(progress =>
            {
                if (IsSword(progress))
                {
                    if (mainImage1 && mainImageSword1) mainImage1.sprite = mainImageSword1;
                    if (mainImage2 && mainImageSword2) mainImage2.sprite = mainImageSword2;

                    weaponIconsSwordView.gameObject.SetActive(true);
                    weaponIconsShieldView.gameObject.SetActive(false);
                    weaponIconsSwordView.UpdateCount(swordCount, progress);
                }
                else
                {
                    if (mainImage1 && mainImageShield1) mainImage1.sprite = mainImageShield1;
                    if (mainImage2 && mainImageShield2) mainImage2.sprite = mainImageShield2;

                    weaponIconsSwordView.gameObject.SetActive(false);
                    weaponIconsShieldView.gameObject.SetActive(true);
                    weaponIconsShieldView.UpdateCount(shieldCount, progress - swordCount);
                }
            }).AddTo(this);
            _timer.Subscribe(value => timerSlider.value = value).AddTo(this);

            // Presenterは自分自身。多分きっとコンナカンジ
            
            
            _progress.Subscribe(progress =>
            {
                if (progress >= TotalWeaponCount)
                {
                    _finishDraw.OnNext(new BattlePlayerData(_swordNodeList.ToArray(), _guardNodeList.ToArray()));
                    return;
                }

                _ovrWeaponDraw?.Dispose();
                _drawSubscription?.Dispose();
                drawSword.gameObject.SetActive(false);
                drawGuard.gameObject.SetActive(false);
                _timer.Value = 1;
                _timerTween?.Kill();
                // 新しい描画ステップを開始するためにタイムアウトフラグをリセットする
                _isTimeout = false;
                
                if (IsSword(progress))
                {
                    drawSword.gameObject.SetActive(true);
                    _drawSubscription = (_ovrWeaponDraw = new OvrWeaponDraw(ovrInputObserver, drawSword, xrObjectResolver))
                        .OnDrawEnd.Subscribe(nodes =>
                        {
                            // 正常完了時のみ描画完了の効果音を鳴らす
                            if (!_isTimeout)
                            {
                                SEManager.Instance.Play(SEPath.DRAW_COMP);
                            }
                            // nullの場合は空配列を使用
                            var safeNodes = nodes ?? Array.Empty<SplineNode>();
                            _swordNodeList.Add(safeNodes);
                            /*
                            var instance = Instantiate(battleSwordPrefab, xrObjectResolver.RightHandObj.transform);
                            _weaponInstanceList.Add(instance);
                            instance.Initialize(data, Utils.SelfLayer);
                            */
                            
                            _progress.Value++;
                        }).AddTo(this);
                }
                else
                {
                    drawGuard.gameObject.SetActive(true);
                    _drawSubscription = (_ovrWeaponDraw = new OvrWeaponDraw(ovrInputObserver, drawGuard, xrObjectResolver))
                        .OnDrawEnd.Subscribe(nodes =>
                        {
                            // 正常完了時のみ描画完了の効果音を鳴らす
                            if (!_isTimeout)
                            {
                                SEManager.Instance.Play(SEPath.DRAW_COMP);
                            }
                            // nullの場合は空配列を使用
                            var safeNodes = nodes ?? Array.Empty<SplineNode>();
                            _guardNodeList.Add(safeNodes);
                            /*
                            var instance = Instantiate(battleGuardPrefab, xrObjectResolver.RightHandObj.transform);
                            _weaponInstanceList.Add(instance);
                            instance.Initialize(data, Utils.SelfLayer);
                            */
                        
                            _progress.Value++;
                        }).AddTo(this);
                }

                _timerStartSubscription?.Dispose();
                _timerStartSubscription = _ovrWeaponDraw.OnDrawStart.Subscribe(_ =>
                {
                    _timerTween?.Kill();
                    _timerTween = DOTween.To(() => _timer.Value, x => _timer.Value = x, 0f, 15f)
                        .OnComplete(() => 
                        {
                            // 時間切れの場合はフラグを立て、タイムアップ効果音を再生してから強制完了させる
                            _isTimeout = true;
                            SEManager.Instance.Play(SEPath.TIMEUP);
                            _ovrWeaponDraw?.ForceDrawComplete();
                        });
                    _timerStartSubscription?.Dispose();
                });
            }).AddTo(this);
        }
        
        private void OnEnable()
        {
            _swordNodeList.Clear();
            _guardNodeList.Clear();
            
            _timer.OnNext(1);
            _progress.OnNext(0);
            xrObjectResolver.PenController.EnablePen(true);
        }
        private void OnDisable()
        {
            xrObjectResolver.PenController.EnablePen(false);
        }
    }
}