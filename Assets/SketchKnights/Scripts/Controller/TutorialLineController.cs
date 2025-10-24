using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TutorialLineController : MonoBehaviour
{
    [SerializeField] private Transform from;
    [SerializeField] private Transform to;
    private LineRenderer _lineRenderer;

    public Transform From { set => from = value; }
    public Transform To { set => to = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _lineRenderer.SetPosition(0, from ? from.position : Vector3.zero);
        _lineRenderer.SetPosition(1, to.position);
    }

    public void OnEnable()
    {
        _lineRenderer.enabled = true;
    }

    // このオブジェクトを非表示にする
    public void OnDisable()
    {
        _lineRenderer.enabled = false;
    }
}
