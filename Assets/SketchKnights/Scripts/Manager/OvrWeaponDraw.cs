using System;
using R3;
using SketchKnights.Scripts.Controller;
using SplineMesh;
using KanKikuchi.AudioManager; // 効果音再生のために追加

namespace SketchKnights.Scripts.Manager
{
    public class OvrWeaponDraw: IDisposable
    {
        private readonly Subject<SplineNode[]> _onDrawEnd = new ();
        public Observable<SplineNode[]> OnDrawEnd => _onDrawEnd;
        private readonly Subject<Unit> _OnDrawStart = new ();
        public Observable<Unit> OnDrawStart => _OnDrawStart;
        private IDisposable _drawSubscription;
        private IDisposable _touchSubscription;
        private IDisposable _startSubscription;
        private bool _isDrawing = false;
        private DrawWeapon _weapon;

        public OvrWeaponDraw(OvrInputObserver ovrInput, DrawWeapon weapon, XrObjectResolver xrObjectResolver)
        {
            _weapon = weapon;
            // Validate that PenObj is not null during initialization
            _drawSubscription = ovrInput.RIndexTrigger.Subscribe(value =>
            {
                if (value)
                {
                    var penPosition = xrObjectResolver.PenController.PenWritePos;
                    _isDrawing = weapon.DrawStart(penPosition);
                    if (_isDrawing)
                    {
                        // 描画開始時の効果音を再生する
                        SEManager.Instance.Play(SEPath.DRAW_START);
                        // Create a subscription that updates pen position every frame
                        _OnDrawStart.OnNext(Unit.Default);
                        _touchSubscription = Observable.EveryUpdate().Subscribe(_ =>
                        {
                            weapon.Draw(xrObjectResolver.PenController.PenWritePos);
                        });
                    }
                }
                else
                {
                    if (!_isDrawing) return;
                    _isDrawing = false;
                    _touchSubscription?.Dispose();
                    var data = weapon.DrawEnd();
                    _weapon = null;
                    if (data != null) _onDrawEnd.OnNext(data);
                }
            });
        }
        
        public void ForceDrawComplete()
        {
            if (!_weapon) return;
            _isDrawing = false;
            _touchSubscription?.Dispose();
            _onDrawEnd.OnNext(_weapon.DrawEnd());
            _weapon = null;
        }

        public void Dispose()
        {
            _drawSubscription?.Dispose();
            _touchSubscription?.Dispose();
            _startSubscription?.Dispose();
            
            _drawSubscription = null;
            _touchSubscription = null;
            _startSubscription = null;
        }
    }
}