using System;
using SketchKnights.Scripts.Controller;
using UnityEngine;
using R3;
using UnityEngine.Serialization;
using KanKikuchi.AudioManager;

namespace SketchKnights.Scripts.Manager
{

    public class MainFlow: MonoBehaviour
    {    
        [SerializeField] private XrObjectResolver xrObjectResolver;
        [SerializeField] private TutorialFlow tutorialFlow;
        [SerializeField] private DrawFlow drawFlow;
        [SerializeField] private WaitFlow waitFlow;
        [SerializeField] private BattleFlow battleFlow;
        [SerializeField] private ResultFlow resultFlow;
        
        private BattlePlayerData _data;
        
        // Start is called before the first frame update
        void OnEnable()
        {
            tutorialFlow.FinishTutorial.Subscribe(_ =>
            {
                tutorialFlow.gameObject.SetActive(false);
                drawFlow.gameObject.SetActive(true);
            });
            drawFlow.FinishDraw.Subscribe(data =>
            {
                _data = data;
                drawFlow.gameObject.SetActive(false);
                waitFlow.Initialize(_data);
            });
            waitFlow.FinishWait.Subscribe(_ =>
            {
                waitFlow.gameObject.SetActive(false);
                battleFlow.Initialize(_data);
            });
            battleFlow.OnFinishBattle.Subscribe(isClear =>
            {
                battleFlow.gameObject.SetActive(false);
                resultFlow.Initialize(isClear);
            });

            drawFlow.gameObject.SetActive(false);
            battleFlow.gameObject.SetActive(false);
            resultFlow.gameObject.SetActive(false);
            tutorialFlow.gameObject.SetActive(true);

            // bgm
            BGMManager.Instance.Play(BGMPath.CREATION, isLoop: true);
        }
    }
}