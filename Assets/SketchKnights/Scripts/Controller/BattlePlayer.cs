using System;
using System.Collections.Generic;
using UnityEngine;
using R3;
using SketchKnights.Scripts.Manager;
using KanKikuchi.AudioManager; // 効果音再生のために追加

namespace SketchKnights.Scripts.Controller
{
    public class BattlePlayer: MonoBehaviour
    {
        [SerializeField] private XrObjectResolver xrObjectResolver;
        [SerializeField] private GlobalData globalData;
        [SerializeField] private GameObject leftHand;
        [SerializeField] private GameObject rightHand;
        [SerializeField] private WeaponIconsView leftWeaponView;
        [SerializeField] private WeaponIconsView rightWeaponView;
        
        private BattleWeaponHolder _leftHolder;
        private BattleWeaponHolder _rightHolder;
        
        public GameObject LeftHand => leftHand;
        public GameObject RightHand => rightHand;

        private readonly Subject<Unit> _onDead = new ();
        public Observable<Unit> OnDead => _onDead;

        private CompositeDisposable _disposable;
        
        public void Initialize(BattlePlayerData data, int layer)
        {
            _rightHolder?.Dispose();
            _leftHolder?.Dispose();
            _rightHolder = new(data.SwordNodes, WeaponStyle.Sword, globalData.prefabDictionary[WeaponStyle.Sword], rightHand, layer);
            _leftHolder = new(data.GuardNodes, WeaponStyle.Guard, globalData.prefabDictionary[WeaponStyle.Guard], leftHand, layer);

            var rightHandMax = data.SwordNodes.Length;
            var leftHandMax = data.GuardNodes.Length;
            
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            _rightHolder.WeaponRemaining.Subscribe(remaining =>
            {
                rightWeaponView.UpdateCount(remaining, rightHandMax);
                if (remaining <= 0)
                {
                    _onDead.OnNext(Unit.Default);
                }
            }).AddTo(_disposable);
            _leftHolder.WeaponRemaining.Subscribe(remaining => leftWeaponView.UpdateCount(remaining, leftHandMax))
                .AddTo(_disposable);
            rightWeaponView.UpdateCount(rightHandMax, rightHandMax);
            leftWeaponView.UpdateCount(leftHandMax, leftHandMax);
        }
        
        public void HeadHit()
        {
            Debug.Log("HeadHit!");
            // プレイヤー被弾SE（必要に応じて他のクリップに差し替えてください）
            SEManager.Instance.Play(SEPath.DAMAGE1);
            _rightHolder?.BreakWeapon();
        }

        private void OnDestroy()
        {
            _leftHolder?.Dispose();
            _rightHolder?.Dispose();
        }
    }
}