using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Runtime.CompilerServices;
using Meta.WitAi;
using System.Web;

// Assetsフォルダー以下にあるアセットを検索し、そのパスを表示するエディタ拡張
// 参考サイト：Colorful Palette「https://media.colorfulpalette.co.jp/n/nef215d75b5fc」

public class SearchAssetsPath : EditorWindow
{
    // フィルター
    private string _searchFilter = string.Empty;

    // readonly : 読み取り専用変数型。実行時に値が決まり定数扱いとなる。実行前は変更が可能である
    // 検索結果を保存するリスト
    private readonly List<string> _searchResult = new List<string>();

    // スクロール位置
    private Vector2 _scrollPosition = Vector2.zero;

    // Unityのメニュータブからウィンドウを表示する処理
    [MenuItem("Tools/アセットのパスを検索するツール",priority =1)]
    private static void OpenWindow()
    {
        // ウィンドウを取得
        SearchAssetsPath editorWindow = GetWindow<SearchAssetsPath>();

        // ウィンドウを表示
        editorWindow.Show();
    }

    private void OnGUI()
    {
        // ボタンの高さの値
        const int optionHeight = 40;

        // 検索結果前のスペース高さ
        const int resultSpaceHeight = 2;

        // ボタンの高さを設定
        GUILayoutOption[] buttonOption = new GUILayoutOption[] { GUILayout.Height(optionHeight) };

        // テキストを入力するフィールドを生成
        _searchFilter = EditorGUILayout.TextField("検索フィルター", _searchFilter);

        // テキストを入力するフィールドと同じ高さのスペースを作成
        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        // 検索用ボタンを作成　ボタンが押された場合、処理を行う
        if(GUILayout.Button("検索",buttonOption))
        {
            // AssetDatabase.FindAssets(検索する名前 , 検索する場所の名前)
            // テキストフィールドに入力された文字をAssetsの中から検索する
            string[] guids = AssetDatabase.FindAssets(_searchFilter, new[] { "Assets" });

            // 検索結果のリストを初期化
            _searchResult.Clear();

            foreach (string guid in guids)
            {
                // GUIDをパスに変換する
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // 指定のファイルパスにフォルダがあるかどうかを確認する
                if(File.Exists(assetPath)) 
                {
                    // 検索結果を追加する
                    _searchResult.Add(assetPath);
                }
            }
        }

        // テクストを入力するフィールドと同じ高さ2個分のスペースを作成
        GUILayout.Space(EditorGUIUtility.singleLineHeight * resultSpaceHeight);

        // 検索結果のリザルトを作成
        GUILayout.Label("検索結果");

        // テキストを入力するフィールドと同じ高さのスペースを作成
        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        // 検索結果の件数が0個でない場合
        if(_searchResult.Count!=0)
        {
            // スクロールビューを作成
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            //件数分の検索結果を表示
            foreach(string resule  in _searchResult) 
            {
                GUILayout.Label(resule);
            }

            // スクロールビューを終了する
            EditorGUILayout.EndScrollView();
        }
    }
}
