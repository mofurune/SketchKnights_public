using System;
using R3;
using UnityEngine;
using KanKikuchi.AudioManager; // 効果音再生のために追加

namespace SketchKnights.Scripts.Controller
{
    public class BattleGuard: BattleWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            
            // 盾を持つ位置と回転を設定
            transform.localPosition = new Vector3(-0.0073f, -0.0306f, -0.0322f);
            transform.Rotate(1.4953f, 275.6326f, 315.9948f, Space.Self);

            OnHit.Subscribe(other =>
            {             
                if (other.gameObject.CompareTag("Enemy"))
                {
                    // 盾が敵に命中したときの効果音を再生する
                    SEManager.Instance.Play(SEPath.GUARD1);
                    Debug.Log("Enemy hit by guard");
                    var enemy = other.gameObject.GetComponent<Enemy>();
                    if (enemy) enemy.Death();
                    else throw new Exception("Enemy was not found");
                    Destruction();
                }
            }).AddTo(this);
        }
    }
}