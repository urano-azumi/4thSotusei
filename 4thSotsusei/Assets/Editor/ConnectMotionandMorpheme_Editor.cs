using UnityEngine;
using UnityEditor;

public class ConnectMotionandMorpheme : EditorWindow
{
    // �A�j���[�V������I������{�^���̖��O
    private string motionSelectText = "�A�j���[�V������I������";
    
    // �`�ԑf��I������{�^���̖��O
    private string morphemeSelectText = "�`�ԑf��I������";

    // �`�ԑf��ǉ�����{�^���̖��O
    private string addMorphemeText = "�{ �`�ԑf��ǉ�";

    // �ۑ�����{�^���̖��O
    private string saveText = "�ۑ�����";

    // ���j���[�^�u����G�f�B�^�E�B���h�E��\���ł���悤�ɂ���
    [MenuItem("Tools/���[�V�����ƌ`�ԑf�����ԃc�[��")]
    private static void Open()
    {
        // �E�B���h�E���擾����
        ConnectMotionandMorpheme editorWindow = GetWindow<ConnectMotionandMorpheme>();

        // �E�B���h�E��\������
        editorWindow.Show();
    }

    private void OnGUI()
    {

        // �A�j���[�V������I������{�^�����쐬
        if (GUILayout.Button(motionSelectText))
        {

        }

        // �`�ԑf��I������{�^�����쐬
        if(GUILayout.Button(morphemeSelectText)) 
        {
            
        }

        // �ۑ�����{�^�����쐬
        if (GUILayout.Button(saveText))
        {

        }
    }
}
 