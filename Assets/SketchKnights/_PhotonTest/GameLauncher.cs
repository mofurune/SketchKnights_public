using Fusion;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    [SerializeField]
    private NetworkRunner networkRunnerPrefab;

    private NetworkRunner networkRunner;

    private void Start() {
        // NetworkRunnerを生成する
        networkRunner = Instantiate(networkRunnerPrefab);
        // StartGameArgsに渡した設定で、セッションに参加する
        networkRunner.StartGame(new StartGameArgs {
            GameMode = GameMode.AutoHostOrClient,
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });
    }
}