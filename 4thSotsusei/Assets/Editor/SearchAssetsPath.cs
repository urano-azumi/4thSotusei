using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Runtime.CompilerServices;
using Meta.WitAi;
using System.Web;

// Assets�t�H���_�[�ȉ��ɂ���A�Z�b�g���������A���̃p�X��\������G�f�B�^�g��
// �Q�l�T�C�g�FColorful Palette�uhttps://media.colorfulpalette.co.jp/n/nef215d75b5fc�v

public class SearchAssetsPath : EditorWindow
{
    // �t�B���^�[
    private string _searchFilter = string.Empty;

    // readonly : �ǂݎ���p�ϐ��^�B���s���ɒl�����܂�萔�����ƂȂ�B���s�O�͕ύX���\�ł���
    // �������ʂ�ۑ����郊�X�g
    private readonly List<string> _searchResult = new List<string>();

    // �X�N���[���ʒu
    private Vector2 _scrollPosition = Vector2.zero;

    // Unity�̃��j���[�^�u����E�B���h�E��\�����鏈��
    [MenuItem("Tools/�A�Z�b�g�̃p�X����������c�[��",priority =1)]
    private static void OpenWindow()
    {
        // �E�B���h�E���擾
        SearchAssetsPath editorWindow = GetWindow<SearchAssetsPath>();

        // �E�B���h�E��\��
        editorWindow.Show();
    }

    private void OnGUI()
    {
        // �{�^���̍����̒l
        const int optionHeight = 40;

        // �������ʑO�̃X�y�[�X����
        const int resultSpaceHeight = 2;

        // �{�^���̍�����ݒ�
        GUILayoutOption[] buttonOption = new GUILayoutOption[] { GUILayout.Height(optionHeight) };

        // �e�L�X�g����͂���t�B�[���h�𐶐�
        _searchFilter = EditorGUILayout.TextField("�����t�B���^�[", _searchFilter);

        // �e�L�X�g����͂���t�B�[���h�Ɠ��������̃X�y�[�X���쐬
        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        // �����p�{�^�����쐬�@�{�^���������ꂽ�ꍇ�A�������s��
        if(GUILayout.Button("����",buttonOption))
        {
            // AssetDatabase.FindAssets(�������閼�O , ��������ꏊ�̖��O)
            // �e�L�X�g�t�B�[���h�ɓ��͂��ꂽ������Assets�̒����猟������
            string[] guids = AssetDatabase.FindAssets(_searchFilter, new[] { "Assets" });

            // �������ʂ̃��X�g��������
            _searchResult.Clear();

            foreach (string guid in guids)
            {
                // GUID���p�X�ɕϊ�����
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // �w��̃t�@�C���p�X�Ƀt�H���_�����邩�ǂ������m�F����
                if(File.Exists(assetPath)) 
                {
                    // �������ʂ�ǉ�����
                    _searchResult.Add(assetPath);
                }
            }
        }

        // �e�N�X�g����͂���t�B�[���h�Ɠ�������2���̃X�y�[�X���쐬
        GUILayout.Space(EditorGUIUtility.singleLineHeight * resultSpaceHeight);

        // �������ʂ̃��U���g���쐬
        GUILayout.Label("��������");

        // �e�L�X�g����͂���t�B�[���h�Ɠ��������̃X�y�[�X���쐬
        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        // �������ʂ̌�����0�łȂ��ꍇ
        if(_searchResult.Count!=0)
        {
            // �X�N���[���r���[���쐬
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            //�������̌������ʂ�\��
            foreach(string resule  in _searchResult) 
            {
                GUILayout.Label(resule);
            }

            // �X�N���[���r���[���I������
            EditorGUILayout.EndScrollView();
        }
    }
}
