using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TextObjectAnimator : MonoBehaviour
{
    [SerializeField] float delayTime = 0f;
    [SerializeField] float animationTime;
    [SerializeField] float waitTime;
    [SerializeField] float toScale = 1.5f;

    [SerializeField] bool playOnAwake = true;

    void Start()
    {
        if (playOnAwake)
        {
            SpawnAndShrink();
        }
    }

    // 大きくなって縮む
    public void SpawnAndShrink()
    {
        transform.localScale = Vector3.zero;
        // toScaleになるまで拡大して、waitTime待ってから0になるまで縮小する
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(delayTime)
               .Append(transform.DOScale(toScale, animationTime).SetEase(Ease.OutCubic))
               .AppendInterval(waitTime)
               .Append(transform.DOScale(0, animationTime).SetEase(Ease.InCubic));
    }
}
