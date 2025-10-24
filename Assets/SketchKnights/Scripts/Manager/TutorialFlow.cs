using System;
using R3;
using SketchKnights.Scripts.Controller;
using UnityEngine;

namespace SketchKnights.Scripts.Manager
{
    public class TutorialFlow: MonoBehaviour
    {
        [SerializeField] private XrObjectResolver xrObjectResolver;
        [SerializeField] private OvrInputObserver ovrInputObserver;
        [SerializeField] private GlobalData globalData;
        [SerializeField] private DrawWeapon drawWeapon;
        [SerializeField] private TMPro.TMP_Text text;
        [SerializeField] private Collider startObject;
        [SerializeField] private TutorialLineController tutorialLineController;
        
        private readonly Subject<Unit> _finishTutorial = new ();
        public Observable<Unit> FinishTutorial => _finishTutorial;
        
        private OvrWeaponDraw _ovrWeaponDraw;
        private BattleWeapon _battleWeaponInstance;
        private IDisposable _drawSubscription;
        private IDisposable _startSubscription;

        private void Awake()
        {
            if(!xrObjectResolver)
            {
                throw new Exception("No XrObjectResolver found");
            }

            drawWeapon.SetPathColor(Color.crimson);

            _ovrWeaponDraw = new OvrWeaponDraw(ovrInputObserver, drawWeapon, xrObjectResolver);
            
            tutorialLineController.From = xrObjectResolver.PenController.WriteTransform;

            _ovrWeaponDraw.OnDrawStart.Subscribe(_ =>
            {
                // ライン矢印を非表示にする
                tutorialLineController.OnDisable();
                tutorialLineController.gameObject.SetActive(false);
            });

            _ovrWeaponDraw.OnDrawEnd.Subscribe(nodes =>
            {
                text.text = "ぶった斬れ！";
                drawWeapon.gameObject.SetActive(false);
                startObject.gameObject.SetActive(true);
                xrObjectResolver.PenController.EnablePen(false);

                _battleWeaponInstance = Instantiate(globalData.prefabDictionary[WeaponStyle.Sword], xrObjectResolver.BattlePlayer.RightHand.transform);
                _battleWeaponInstance.Initialize(nodes, WeaponStyle.Sword, Utils.SelfLayer);
                
                tutorialLineController.To = startObject.gameObject.transform;
                tutorialLineController.From = _battleWeaponInstance.transform;
                tutorialLineController.OnEnable();
                tutorialLineController.gameObject.SetActive(true);


                _startSubscription?.Dispose();
                _startSubscription = _battleWeaponInstance.OnHit.Subscribe(col =>
                {
                    if(!col.gameObject.CompareTag("StartObject")) return;
                    tutorialLineController.OnDisable();
                    tutorialLineController.gameObject.SetActive(false);
                    Destroy(_battleWeaponInstance.gameObject);
                    _ovrWeaponDraw?.Dispose();
                    _ovrWeaponDraw = null;
                    _startSubscription?.Dispose();
                    _finishTutorial.OnNext(Unit.Default);
                }).AddTo(this);
            }).AddTo(this);
        }

        private void OnEnable()
        {
            xrObjectResolver.PenController.EnablePen(true);
            text.text = "一筆書きで剣を書け！";
            startObject.gameObject.SetActive(false);
        }
    }
}