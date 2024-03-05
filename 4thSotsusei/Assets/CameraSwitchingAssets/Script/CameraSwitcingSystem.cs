using System;
using System.Collections.Generic;
using UnityEngine;

// カメラの切り替えをするソースコード

public class CameraSwitcingSystem : MonoBehaviour
{
    #region --- Enum ---

    /// <summary>
    /// マウスのボタン用の列挙型
    /// </summary>
    private enum MouseButton
    {
        // 左クリック
        Left,
        // 右クリック
        Right,
        // ホイール
        Wheel,
    }

    #endregion --- Enum ---

    #region --- Fields ---

    /// <summary>
    /// 映す位置のリスト 
    /// </summary>
    [SerializeField]
    private List<ProjectionInfo> projectionPos = new List<ProjectionInfo>();

    [SerializeField]
    private List<ProjectionInfo> projectionPos_Front= new List<ProjectionInfo>();
    [HideInInspector]public bool Change_CameraRaw = false;
    [HideInInspector]public int currentPos_Front = 0;

    /// <summary>
    /// メインで使用するカメラのオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject mainCamera;

    /// <summary>
    /// メインで使用するカメラのCameraコンポーネント
    /// </summary>
    [SerializeField]
    private Camera cameraCom;

    /// <summary>
    /// 現在の投影している位置の番号
    /// </summary>
    [HideInInspector] public int currentPos = 0;

    /// <summary>
    /// ターゲットの方向を向いておくかの判定
    /// </summary>
    [SerializeField]
    private bool isLookTarget;

    /// <summary>
    /// カメラが視点移動する速さ
    /// </summary>
    [SerializeField]
    private float moveCameraSpeed = 0.1f;

    /// <summary>
    /// カメラの視野角を保存する変数
    /// </summary>
    private float saveFOV = 0;

    #region --- Const Value ---

    /// <summary>
    /// For文の要素数の初期値 => 0
    /// </summary>
    private const int FOR_INITIAL_INDEX = 0;

    /// <summary>
    /// projectionPosの要素数の最小値 => 0
    /// </summary>
    private const int MIN_VIEWERPOS_INDEX = 0;

    /// <summary>
    /// projectionPosの要素数の最大値を求めるための数値 => -1
    /// </summary>
    private const int MAX_VIEWERPOS_INDEX = 1;

    #endregion ---Const Value---

    #endregion --- Fields ---

    #region --- Methods ---

    void Start()
    {
        //cameraCom = mainCamera.GetComponent<Camera>();
        //saveFOV = cameraCom.fieldOfView;

        // リストの要素数分を初期化をする処理
        for (int index = FOR_INITIAL_INDEX; index < projectionPos.Count; index++)
        {
            // リストの中で初期投影位置のトリガーがTrueであった場合
            if (projectionPos[index].initiProjectionPos)
            {
                // カメラの投影位置を設定する
                ChangePosCamera(index,currentPos_Front,Change_CameraRaw);

                currentPos = index;
            }
        }
    }

    void Update()
    {
        // キーボード操作のカメラ切り替えの処理
        InputKeyBoard();

        mainCamera.transform.position = projectionPos[currentPos].projector.transform.position;
        mainCamera.transform.LookAt(projectionPos[currentPos].targetHeadObj);

        if (Change_CameraRaw)
        {
            mainCamera.transform.position = projectionPos_Front[currentPos_Front].projector.transform.position;
            mainCamera.transform.LookAt(projectionPos_Front[currentPos_Front].targetHeadObj);
        }

        // カメラを動かす処理
        //MoveCamera();
    }

    /// <summary>
    /// カメラ切り替えをキーボードとマウスで操作する用の関数
    /// </summary>
    private void InputKeyBoard()
    {
        // カメラを切り替えるかを判定する変数
        bool isChange = false;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Change_CameraRaw = !Change_CameraRaw;
        }

        if (!Change_CameraRaw)
        {
            // 右矢印キーとマウスの右クリックが押された場合
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetMouseButtonDown((int)MouseButton.Right) ||
               OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                // 現在の投影位置の数値がリストの要素数より少ない場合
                if (currentPos < (projectionPos.Count - MAX_VIEWERPOS_INDEX))
                {
                    // 投影位置の順番を１つ増やす
                    currentPos++;
                    //ChangePosCamera(currentPos);

                    // カメラの切り替えをする判定をtrueにする
                    isChange = true;
                }
            }

            // 左矢印キーとマウスの左クリックが押された場合
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetMouseButtonDown((int)MouseButton.Left) ||
                OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
            {
                // 現在の投影位置の数値がリストの先頭の要素数より多い場合
                if (currentPos > MIN_VIEWERPOS_INDEX)
                {
                    // 投影位置の順番を１つ減らす
                    currentPos--;
                    //ChangePosCamera(currentPos);

                    // カメラの切り替えをする判定をtrueにする
                    isChange = true;
                }
            }
        }
        else
        {
            // 右矢印キーとマウスの右クリックが押された場合
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetMouseButtonDown((int)MouseButton.Right) ||
               OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                // 現在の投影位置の数値がリストの要素数より少ない場合
                if (currentPos_Front < (projectionPos_Front.Count - MAX_VIEWERPOS_INDEX))
                {
                    // 投影位置の順番を１つ増やす
                    currentPos_Front++;
                    //ChangePosCamera(currentPos);

                    // カメラの切り替えをする判定をtrueにする
                    isChange = true;
                }
            }

            // 左矢印キーとマウスの左クリックが押された場合
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetMouseButtonDown((int)MouseButton.Left) ||
                OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
            {
                // 現在の投影位置の数値がリストの先頭の要素数より多い場合
                if (currentPos_Front > MIN_VIEWERPOS_INDEX)
                {
                    // 投影位置の順番を１つ減らす
                    currentPos_Front--;
                    //ChangePosCamera(currentPos);

                    // カメラの切り替えをする判定をtrueにする
                    isChange = true;
                }
            }
        }

        // カメラの切り替えをする判定がtrueの場合
        if (isChange)
        {
            // カメラの位置を指定した位置に切り替える処理
            ChangePosCamera(currentPos,currentPos_Front,Change_CameraRaw);
        }
    }

    /// <summary>
    /// カメラの位置を指定した位置に移動させる関数
    /// </summary>
    /// <param name="index"> 移動する位置(要素数)を指定する値 </param>
    private void ChangePosCamera(int index,int index_Front,bool Flag)
    {
        // ターゲットの方向に向かせるかのトリガーの判定ごとに処理を変える
        switch (isLookTarget)
        {
            // ターゲットの方向に向かせる場合
            case true:
                if (!Flag)
                {
                    // カメラの位置を指定した要素の位置に設定する
                    mainCamera.transform.position = projectionPos[index].projector.transform.position;

                    // カメラの角度を指定した要素の位置に設定する
                    mainCamera.transform.LookAt(projectionPos[index].targetHeadObj);
                }
                else
                {
                    mainCamera.transform.position = projectionPos_Front[index_Front].projector.transform.position;

                    mainCamera.transform.LookAt(projectionPos_Front[index_Front].targetHeadObj);
                }
                break;
            // ターゲットの方向に向かせない場合
            case false:
                // カメラの位置を指定した要素の位置に設定する
                mainCamera.transform.position = projectionPos[index].projector.transform.position;

                // カメラの角度を指定した要素の位置に設定する
                mainCamera.transform.rotation = projectionPos[index].projector.transform.rotation;
                break;
        }
    }

    /// <summary>
    /// カメラの視点を移動させるための関数
    /// </summary>
    private void MoveCamera()
    {
        // カメラが動く速さ
        float moveSpeed = moveCameraSpeed * Time.deltaTime;

        // マウスのX軸とY軸の動きを取得する
        float moveX = Input.GetAxis("Mouse X") * moveSpeed;
        float moveY = Input.GetAxis("Mouse Y") * -moveSpeed;

        // スペースキーを押している場合
        if (Input.GetKey(KeyCode.Space))
        {
            // マウスのホイールを押した場合
            if (Input.GetMouseButtonDown((int)MouseButton.Wheel))
            {
                // カメラの視野角を初期に戻す
                cameraCom.fieldOfView = saveFOV;
            }

            // カメラの視野角を変更する（ズームイン/ズームアウト機能）
            cameraCom.fieldOfView += moveY;
        }
        // キーを押していない場合
        else
        {
            // カメラを回転させる
            mainCamera.transform.Rotate(moveY, moveX, 0);
        }
    }

    #endregion --- Methods ---
}

#region --- Class ---

/// <summary>
/// 投影位置の情報を管理するクラス
/// </summary>
[System.Serializable]
public class ProjectionInfo
{
    /// <summary>
    /// 投影位置の説明
    /// </summary>
    [SerializeField]
    private string description;

    /// <summary>
    /// カメラが向くターゲットの頭の位置
    /// </summary>
    public Transform targetHeadObj;

    /// <summary>
    /// 投影位置の情報
    /// </summary>
    public Transform projector;

    /// <summary>
    /// 初期に映す投影位置を設定するトリガー
    /// </summary>
    public bool initiProjectionPos;
}

#endregion --- Class ---