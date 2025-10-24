using System;
using UnityEngine;
using R3;
using KanKikuchi.AudioManager; // 効果音再生のために追加

namespace SketchKnights.Scripts.Controller
{
    public class BattleSword : BattleWeapon
    {
        protected override void Awake()
        {
            base.Awake();

            // 剣を持つ位置と回転を設定
            transform.localPosition = new Vector3(0, 0.0640f, 0.0810f);
            transform.Rotate(50.5901f, 14.7355f, 12.3392f, Space.Self);

            OnHit.Subscribe(other =>
            {

                // if (other.gameObject.CompareTag("Head"))
                // {
                //     var battlePlayer = other.gameObject.GetComponentInParent<BattlePlayer>();
                //     if (battlePlayer) battlePlayer.HeadHit();
                //     else throw new Exception("BattlePlayer was not found");
                // }

                if (other.gameObject.CompareTag("Enemy"))
                {
                    // 剣が敵に命中したときの効果音を再生する
                    SEManager.Instance.Play(SEPath.ATTACK1);
                    Debug.Log("Enemy hit by sword");
                    var enemy = other.gameObject.GetComponent<Enemy>();
                    if (enemy) enemy.Death();
                    else throw new Exception("Enemy was not found");
                }
            }).AddTo(this);
        }

        // TODO: ラグ対策のため、guardに触れてる間頭攻撃無効化する処理
        
    }
}