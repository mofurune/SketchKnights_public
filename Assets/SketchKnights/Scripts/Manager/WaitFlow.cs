﻿using R3;
using SketchKnights.Scripts.Controller;
using UnityEngine;

namespace SketchKnights.Scripts.Manager
{
    public class WaitFlow : MonoBehaviour
    {
        [SerializeField] private bool isDebug = false;
        [SerializeField]
        private XrObjectResolver xrObjectResolver;
        [SerializeField]
        private BattlePlayer scarecrowPlayer;

        private readonly Subject<Unit> _finishWait = new();
        public Observable<Unit> FinishWait => _finishWait;
        [SerializeField] private float waitTime = 17f; // 待機時間
        private float timer;
        // private float countdownTime;

        private void Awake()
        {
            if (isDebug) Initialize(WeaponSampleData.PlayerSample);
        }

        public void Initialize(BattlePlayerData data)
        {
            gameObject.SetActive(true);
            xrObjectResolver.BattlePlayer.Initialize(data, Utils.SelfLayer);
        }

        private void Start()
        {
            // カウントダウンタイマー初期化
            timer = 0f;
        }

        void Update()
        {
            timer += Time.deltaTime;

            // カウントダウン残り時間計算
            // countdownTime = Mathf.Max(waitTime - timer, 0f);

            // 待機時間が経過したら終了
            if (timer >= waitTime)
            {
                _finishWait.OnNext(Unit.Default);
            }
        }
    }
}