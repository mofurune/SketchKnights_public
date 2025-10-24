using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;
using SplineMesh;
using Object = UnityEngine.Object;

namespace SketchKnights.Scripts.Controller
{
    public class BattleWeaponHolder: IDisposable
    {
        private readonly SplineNode[][] _weaponNodes;
        private readonly WeaponStyle _style;
        private readonly BattleWeapon _prefab;
        private readonly GameObject _holdObj;
        private readonly int _weaponLayer;

        public BattleWeaponHolder(SplineNode[][] weaponNodes, WeaponStyle style, BattleWeapon prefab, GameObject holdObj, int weaponLayer)
        {
            _weaponNodes = weaponNodes;
            _style = style;
            _prefab = prefab;
            _holdObj = holdObj;
            _weaponLayer = weaponLayer;
            GenerateWeapon();
        }
        private int _currentWeaponIdx = 0;
        private BattleWeapon _weaponInstance = null;
        private IDisposable _onDestructionSubscription;
        
        private readonly ReactiveProperty<int> _weaponRemaining = new ();
        public ReadOnlyReactiveProperty<int> WeaponRemaining => _weaponRemaining;
        
        public void BreakWeapon()
        {
            _weaponInstance?.Destruction();
            _weaponInstance = null;
        }

        private void GenerateWeapon()
        {
            _weaponInstance?.Destruction();
            
            _weaponInstance = Object.Instantiate(_prefab, _holdObj.transform);
            _weaponInstance.Initialize(_weaponNodes[_currentWeaponIdx], _style, _weaponLayer);

            _onDestructionSubscription = _weaponInstance.OnDestruction.Subscribe(_ =>
            {
                _currentWeaponIdx++;
                _weaponRemaining.Value = _weaponNodes.Length - _currentWeaponIdx;
                if (_weaponRemaining.CurrentValue <= 0) return;
                GenerateNextFrame().Forget();
            }).AddTo(_weaponInstance.gameObject);
        }

        // GameObjectのサイクルとイベント系の管理の都合でこうなってる。あんまり良くはなさそうだが動くのでヨシッ
        private async UniTaskVoid GenerateNextFrame()
        {
            await UniTask.NextFrame();
            GenerateWeapon();
        }
        
        public void Dispose()
        {
            _onDestructionSubscription?.Dispose();
            _weaponRemaining?.Dispose();
            _weaponInstance?.Destruction();
        }
    }
}