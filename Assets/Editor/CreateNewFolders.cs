#if UNITY_EDITOR // 宣言が漏れるとPlayモードやビルド時にエラーが出てしまうので注意すること
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateNewFolders : EditorWindow
{                                                                                                                                                               
    string rootFolderName = "NewFolder"; // デフォルトのフォルダ名

    /// <summary>
    /// 項目をクリックするとウィンドウを開く
    /// </summary>
    [MenuItem("Tools/CreateNewFolders")]
    static void Init()
    {
        // CreateNewFoldersで定義したウィンドウを開く
        CreateNewFolders window = (CreateNewFolders)GetWindow(typeof(CreateNewFolders));
        window.Show();
    }

    /// <summary>
    /// 画面描画時に毎回実行されるイベント
    /// </summary>
    void OnGUI()
    {
        // タイトルテキスト設定
        GUILayout.Label("新規ディレクトリ構成の生成", EditorStyles.boldLabel);
        
        // フォルダ名入力欄を描画する。この値をディレクトリ生成時のルートディレクトリ名になる。
        rootFolderName = EditorGUILayout.TextField("Folder Name", rootFolderName);
        
        // Create Foldersと表示されたボタンを描画し、クリックされたらCreateAppFoldersを実行する
        if (GUILayout.Button("Create Folders"))
        {
            CreateAppFolders(rootFolderName);
        }
    }

    void CreateAppFolders(string rootFolderName)
    {
        // ディレクトリを生成するパスを設定する
        string basePath = Path.Combine("Assets", rootFolderName);

        // 生成するディレクトリのリスト
        string[] folders = new string[]
        {
            "Scripts",
            "ScriptableObjects",
            "Scenes",
            "Prefabs",
            "Editor",
            "Textures",
            "Animations",
            "Materials",
            "PhysicsMaterials",
            "Fonts",
            "Videos",
            "Audio/BGM",
            "Audio/SE",
            "Resources",
            "Editor",
            "Plugins"
        };

        // 上記リストのフォルダを一つずつ作成
        foreach (string folder in folders)
        {
            string path = Path.Combine(basePath, folder);
            if (!Directory.Exists(path))
            {
                // ディレクトリがまだ存在しない場合は作成する
                Directory.CreateDirectory(path);
                Debug.Log("Created Directory: " + path);
            }
        }

        // link.xmlファイルを作成
        string linkXmlPath = Path.Combine(basePath, "Scripts/link.xml");
        if (!File.Exists(linkXmlPath))
        {
            // link.xmlファイルがまだ存在しない場合は作成する
            File.WriteAllText(linkXmlPath, "<link></link>"); // 最低限のタグだけ記述しておく
            Debug.Log("Created File: " + linkXmlPath);
        }

        // Unity側にディレクトリが追加されたことが検知されないかもしれないので、念のため手動で更新する。
        AssetDatabase.Refresh();
    }
}
#endif
