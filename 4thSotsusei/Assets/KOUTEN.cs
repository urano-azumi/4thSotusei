using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOUTEN : MonoBehaviour
{
    [Header("公転の軸となるオブジェクト")]
    [SerializeField] private GameObject TargetObj;
    [Header("回転させるカメラたち(後ろ)")]
    [SerializeField] private GameObject[] Cameras_Back;
    [Header("回転させるカメラたち(前)")]
    [SerializeField] private GameObject[] Cameras_Front;
    [Header("回転速度")]
    public float speed = 100f;
    [Header("CameraSwitchSystemをアタッチ")]
    [SerializeField]private CameraSwitcingSystem switcingSystem;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (!switcingSystem.Change_CameraRaw)
            {
                Cameras_Back[switcingSystem.currentPos].transform.RotateAround(TargetObj.transform.position, Vector3.up, speed * Time.deltaTime);
            }
            else
            {
                Cameras_Front[switcingSystem.currentPos_Front].transform.RotateAround(TargetObj.transform.position, Vector3.up, speed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (!switcingSystem.Change_CameraRaw)
            {
                Cameras_Back[switcingSystem.currentPos].transform.RotateAround(TargetObj.transform.position, Vector3.up, -speed * Time.deltaTime);
            }
            else
            {
                Cameras_Front[switcingSystem.currentPos_Front].transform.RotateAround(TargetObj.transform.position, Vector3.up, -speed * Time.deltaTime);
            }
        }
    }
}
