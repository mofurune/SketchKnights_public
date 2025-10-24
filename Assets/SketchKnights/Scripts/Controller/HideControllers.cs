using UnityEngine;

public class HideControllers : MonoBehaviour
{
    [SerializeField] private bool isHide = true;
    [SerializeField] private Transform leftControllerRoot;
    [SerializeField] private Transform rightControllerRoot;

    void Start()
    {
        if (isHide)
        {
            Hide(leftControllerRoot);
            Hide(rightControllerRoot);
        }
    }

    void Hide(Transform root)
    {
        if (root == null) return;
        foreach (var smr in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            smr.enabled = false;
        foreach (var lr in root.GetComponentsInChildren<LineRenderer>(true))
            lr.enabled = false; // レイも消す場合
    }
}
