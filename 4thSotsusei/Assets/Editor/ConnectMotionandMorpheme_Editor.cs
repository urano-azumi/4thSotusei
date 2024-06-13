using UnityEngine;
using UnityEditor;

public class ConnectMotionandMorpheme : EditorWindow
{
    // アニメーションを選択するボタンの名前
    private string motionSelectText = "アニメーションを選択する";
    
    // 形態素を選択するボタンの名前
    private string morphemeSelectText = "形態素を選択する";

    // 形態素を追加するボタンの名前
    private string addMorphemeText = "＋ 形態素を追加";

    // 保存するボタンの名前
    private string saveText = "保存する";

    // メニュータブからエディタウィンドウを表示できるようにする
    [MenuItem("Tools/モーションと形態素を結ぶツール")]
    private static void Open()
    {
        // ウィンドウを取得する
        ConnectMotionandMorpheme editorWindow = GetWindow<ConnectMotionandMorpheme>();

        // ウィンドウを表示する
        editorWindow.Show();
    }

    private void OnGUI()
    {

        // アニメーションを選択するボタンを作成
        if (GUILayout.Button(motionSelectText))
        {

        }

        // 形態素を選択するボタンを作成
        if(GUILayout.Button(morphemeSelectText)) 
        {
            
        }

        // 保存するボタンを作成
        if (GUILayout.Button(saveText))
        {

        }
    }
}
 