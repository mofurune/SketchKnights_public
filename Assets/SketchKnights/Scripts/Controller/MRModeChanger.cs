using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MRModeChanger : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;

    [SerializeField]
    private bool mrMode = false;

    [SerializeField]
    private Color solidColor = new Color(0, 0, 0, 0);
    [SerializeField]
    private GameObject battleField;

    [Header("Input Action Reference")]
    [SerializeField] private InputActionReference rightYButtonAction;

    [Header("Triple Click Settings")]
    [SerializeField] private float tripleClickTimeWindow = 1.5f;
    [SerializeField] private bool enableDebugLog = true;

    private int yClickCount = 0;
    private Coroutine yTripleClickCoroutine;

    private void OnEnable()
    {
        if (rightYButtonAction?.action != null)
        {
            rightYButtonAction.action.Enable();
            rightYButtonAction.action.performed += OnYButtonPressed;
        }
    }

    private void OnDisable()
    {
        if (rightYButtonAction?.action != null)
        {
            rightYButtonAction.action.performed -= OnYButtonPressed;
            rightYButtonAction.action.Disable();
        }
        if (yTripleClickCoroutine != null)
        {
            StopCoroutine(yTripleClickCoroutine);
        }
    }

    private void OnYButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            yClickCount++;
            if (enableDebugLog)
            {
                Debug.Log($"Y Button pressed. Count: {yClickCount}");
            }
            if (yClickCount == 1)
            {
                if (yTripleClickCoroutine != null)
                {
                    StopCoroutine(yTripleClickCoroutine);
                }
                yTripleClickCoroutine = StartCoroutine(ResetYCountAfterDelay());
            }
            else if (yClickCount >= 3)
            {
                if (enableDebugLog)
                {
                    Debug.Log("Triple Y click detected! Toggling MR mode.");
                }
                if (yTripleClickCoroutine != null)
                {
                    StopCoroutine(yTripleClickCoroutine);
                    yTripleClickCoroutine = null;
                }
                mrMode = !mrMode;
                ApplyCameraMode();
                yClickCount = 0;
            }
        }
    }

    private IEnumerator ResetYCountAfterDelay()
    {
        yield return new WaitForSeconds(tripleClickTimeWindow);
        if (yClickCount < 3)
        {
            if (enableDebugLog)
            {
                Debug.Log("Triple Y click timeout. Count reset.");
            }
            yClickCount = 0;
        }
        yTripleClickCoroutine = null;
    }

    private void OnValidate()
    {
        ApplyCameraMode();
    }

    private void ApplyCameraMode()
    {
        if (targetCamera == null) return;
        if (mrMode)
        {
            targetCamera.clearFlags = CameraClearFlags.SolidColor;
            targetCamera.backgroundColor = solidColor;
            battleField.SetActive(false);
        }
        else
        {
            targetCamera.clearFlags = CameraClearFlags.Skybox;
            battleField.SetActive(true);
        }
    }
}
