using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class SimpleXRSceneResetter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float tripleClickTimeWindow = 1.5f;
    [SerializeField] private bool enableDebugLog = true;

    [Header("Input Action Reference")]
    [SerializeField] private InputActionReference leftXButtonAction;

    private int clickCount = 0;
    private Coroutine resetTimerCoroutine;

    private void Awake()
    {
        // Input Action Referenceが設定されていない場合の自動設定
        if (leftXButtonAction == null)
        {
            CreateDefaultInputAction();
        }
    }

    private void OnEnable()
    {
        if (leftXButtonAction?.action != null)
        {
            leftXButtonAction.action.Enable();
            leftXButtonAction.action.performed += OnXButtonPressed;
        }
    }

    private void OnDisable()
    {
        if (leftXButtonAction?.action != null)
        {
            leftXButtonAction.action.performed -= OnXButtonPressed;
            leftXButtonAction.action.Disable();
        }

        if (resetTimerCoroutine != null)
        {
            StopCoroutine(resetTimerCoroutine);
        }
    }

    private void CreateDefaultInputAction()
    {
        // プログラムで直接Input Actionを作成
        var inputAction = new InputAction("LeftXButton", InputActionType.Button);

        // XRコントローラーのXボタンをバインド（左手）
        inputAction.AddBinding("<XRController>{LeftHand}/primaryButton");

        // Meta Quest用のバインディングも追加
        inputAction.AddBinding("<OculusTouchController>{LeftHand}/primaryButton");

        // Input Action Referenceとして設定
        leftXButtonAction = InputActionReference.Create(inputAction);

        if (enableDebugLog)
        {
            Debug.Log("Default Input Action created for Left X Button");
        }
    }

    private void OnXButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleXButtonPress();
        }
    }

    private void HandleXButtonPress()
    {
        clickCount++;

        if (enableDebugLog)
        {
            Debug.Log($"X Button pressed. Count: {clickCount}");
        }

        // 初回押下の場合、タイマーを開始
        if (clickCount == 1)
        {
            if (resetTimerCoroutine != null)
            {
                StopCoroutine(resetTimerCoroutine);
            }
            resetTimerCoroutine = StartCoroutine(ResetCountAfterDelay());
        }
        // 3回押下達成
        else if (clickCount >= 3)
        {
            if (enableDebugLog)
            {
                Debug.Log("Triple click detected! Resetting scene...");
            }

            if (resetTimerCoroutine != null)
            {
                StopCoroutine(resetTimerCoroutine);
                resetTimerCoroutine = null;
            }

            ResetScene();
            clickCount = 0;
        }
    }

    private IEnumerator ResetCountAfterDelay()
    {
        yield return new WaitForSeconds(tripleClickTimeWindow);

        // タイムアウトした場合、カウントをリセット
        if (clickCount < 3)
        {
            if (enableDebugLog)
            {
                Debug.Log("Triple click timeout. Count reset.");
            }
            clickCount = 0;
        }

        resetTimerCoroutine = null;
    }

    private void ResetScene()
    {
        try
        {
            // シーンリセット前の処理
            OnSceneResetStarted();

            // 現在のシーンを再読み込み
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);

            if (enableDebugLog)
            {
                Debug.Log($"Scene '{currentSceneName}' has been reset.");
            }

            // シーンリセット後の処理
            OnSceneResetCompleted();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to reset scene: {e.Message}");
        }
    }

    /// <summary>
    /// シーンリセット開始時に呼ばれるイベント
    /// </summary>
    protected virtual void OnSceneResetStarted()
    {
        // ここに必要な前処理を追加
    }

    /// <summary>
    /// シーンリセット完了時に呼ばれるイベント
    /// </summary>
    protected virtual void OnSceneResetCompleted()
    {
        // ここに必要な後処理を追加
    }

    // エディター用のテスト機能（オプション）
#if UNITY_EDITOR
    private void Update()
    {
        // エディターでのテスト用（Spaceキーで代用）
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            HandleXButtonPress();
        }
    }
#endif
}