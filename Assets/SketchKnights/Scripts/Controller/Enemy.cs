using System;
using UnityEngine;
using R3;
using SketchKnights.Scripts.Controller;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    private Vector3 startPos;
    private float moveTime = 5f;
    private float timer = 0f;
    private bool isMoving = false;
    private Transform playerTransform;
    [SerializeField] private GameObject slashEffectPrefab;

    public void Initialize(Transform player)
    {
        playerTransform = player;
        isMoving = true;
        slashEffectPrefab.SetActive(false);
    }

    void Update()
    {
        if (playerTransform == null)
            return;
        timer += Time.deltaTime;
        // 毎フレームプレイヤーの現在位置を目標にする
        // TODO 回り込んで突進するなどのバリエーションをつける
        if(isMoving)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 targetPos = playerTransform.position;
        float distance = Vector3.Distance(transform.position, targetPos);
        float remainingTime = Mathf.Max(moveTime - timer, 0.01f);
        float speed = distance / remainingTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    public void Death()
    {
        isMoving = false;
        // コライダーをオフにする
        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
        // スラッシュエフェクトを可視化
        slashEffectPrefab.SetActive(true);
        // 今の大きさから0.3秒かけて大きさを0にしたのち消す
        transform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                // 1秒待ってから消す
                DOVirtual.DelayedCall(1f, () => Destroy(gameObject));
            });
    }
    
    // 即死。敵がすぐ消える。
    private void InstanceDeath()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Head"))
        {
            var battlePlayer = other.gameObject.GetComponentInParent<BattlePlayer>();
            if (battlePlayer)
            {
                battlePlayer.HeadHit();
                InstanceDeath();
            }
            else throw new Exception("BattlePlayer was not found");
        }
    }
}