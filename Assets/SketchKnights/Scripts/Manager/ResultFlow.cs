using R3;
using UnityEngine;
using SketchKnights.Scripts.Controller;

namespace SketchKnights.Scripts.Manager
{
    public class ResultFlow : MonoBehaviour
    {
        [SerializeField] private GameObject win;
        [SerializeField] private GameObject lose;

        private readonly Subject<Unit> _onFinish = new();
        public Observable<Unit> OnFinish => _onFinish;

        public void Initialize(bool isClear)
        {
            gameObject.SetActive(true);
            // 表示対象のみアクティブ化
            win.SetActive(isClear);
            lose.SetActive(!isClear);

            // アニメーション呼び出し
            if (isClear)
            {
                var anim = win.GetComponent<TextObjectAnimator>();
                anim?.SpawnAndShrink();
            }
            else
            {
                var anim = lose.GetComponent<TextObjectAnimator>();
                anim?.SpawnAndShrink();
            }
        }

        private void OnDestroy()
        {
            _onFinish.Dispose();
        }
    }
}
