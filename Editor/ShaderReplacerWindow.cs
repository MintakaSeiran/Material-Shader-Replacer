using UnityEngine;
using UnityEditor;

public class ShaderReplacerWindow : EditorWindow
{
    private Shader sourceShader;
    private Shader targetShader;
    private string targetFolderPath = "Assets";

    // 上部メニューの「Tools」にこのウィンドウを開くボタンを追加
    [MenuItem("Tools/Shader Replacer")]
    public static void ShowWindow()
    {
        GetWindow<ShaderReplacerWindow>("Shader Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("シェーダー置換設定", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 1. ドラッグ＆ドロップ対応のシェーダー指定フィールド
        sourceShader = (Shader)EditorGUILayout.ObjectField("変更元のシェーダー", sourceShader, typeof(Shader), false);
        targetShader = (Shader)EditorGUILayout.ObjectField("変更先のシェーダー", targetShader, typeof(Shader), false);

        EditorGUILayout.Space();
        GUILayout.Label("対象フォルダ", EditorStyles.boldLabel);

        // 2. フォルダ階層の指定（手入力 ＆ 参照ボタン）
        GUILayout.BeginHorizontal();
        targetFolderPath = EditorGUILayout.TextField(targetFolderPath);
        if (GUILayout.Button("参照...", GUILayout.Width(60)))
        {
            string absPath = EditorUtility.OpenFolderPanel("対象フォルダを選択", Application.dataPath, "");
            if (!string.IsNullOrEmpty(absPath))
            {
                // 絶対パスをUnityプロジェクト用の相対パス (Assets/...) に変換
                if (absPath.StartsWith(Application.dataPath))
                {
                    targetFolderPath = "Assets" + absPath.Substring(Application.dataPath.Length);
                }
                else
                {
                    EditorUtility.DisplayDialog("エラー", "プロジェクト内のフォルダ (Assets以下) を選択してください。", "OK");
                }
            }
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // 3. 実行ボタン
        GUI.backgroundColor = Color.cyan;
        if (GUILayout.Button("指定フォルダ内のシェーダーを置換する", GUILayout.Height(30)))
        {
            ExecuteReplacement();
        }
        GUI.backgroundColor = Color.white;
    }

    private void ExecuteReplacement()
    {
        // エラーハンドリング
        if (sourceShader == null || targetShader == null)
        {
            EditorUtility.DisplayDialog("エラー", "変更元と変更先のシェーダーを両方とも設定してください。", "OK");
            return;
        }

        if (string.IsNullOrEmpty(targetFolderPath))
        {
            EditorUtility.DisplayDialog("エラー", "対象フォルダを指定してください。", "OK");
            return;
        }

        // 指定フォルダ内のマテリアルを全て検索
        string[] materialGuids = AssetDatabase.FindAssets("t:Material", new[] { targetFolderPath });
        int replacedCount = 0;

        for (int i = 0; i < materialGuids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(materialGuids[i]);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            // プログレスバーの表示（プロジェクトが大きい場合に固まったように見えないための配慮）
            if (EditorUtility.DisplayCancelableProgressBar("シェーダー置換中", $"{path} を処理しています...", (float)i / materialGuids.Length))
            {
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog("キャンセル", "処理がキャンセルされました。", "OK");
                return;
            }

            // シェーダーが一致した場合のみ置換
            if (mat != null && mat.shader == sourceShader)
            {
                Undo.RecordObject(mat, "Replace Shader"); // [Ctrl+Z] で元に戻せるようにする
                mat.shader = targetShader;
                EditorUtility.SetDirty(mat); // アセットが変更されたことをUnityに通知
                replacedCount++;
            }
        }

        // 変更をまとめて保存
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();

        // 完了報告
        EditorUtility.DisplayDialog("完了", $"置換が完了しました。\n変更されたマテリアル数: {replacedCount}", "OK");
    }
}