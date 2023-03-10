using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Перемещение камеры кнопками с учетом бордюров
/// </summary>
public class CameraMoveForButtonScript : MonoBehaviour
{
    #region ReadPlayerInputForAxis

    private float horizontalMove;
    private float verticalMove;

    private void ReadPlayerInputForAxis(out float horizontalMove, out float verticalMove)
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
    }
    #endregion

    void Update()
    {
        ReadPlayerInputForAxis(out horizontalMove, out verticalMove);

        // Skip a move
        if (horizontalMove == 0 && verticalMove == 0) return;

        GetDirectionForCameraMove(out directionForX, out directionForY);

        targetCameraPositionForMove = checkCameraBordersScript.GetTargetCameraPositionForMove(mainCamera.transform.position, directionForX, directionForY, speedCameraMove, Color.cyan);

        #region Use only with Isometric Map
        //GetAngleTargetPositionFromIsometriMap();
        #endregion
    }

    #region Use only with Isometric Map
    private void GetAngleTargetPositionFromIsometriMap()
    {
        if (targetCameraPositionForMove == Vector3.zero)
        {
            targetCameraPositionForMove = checkCameraBordersScript.GetAngleCameraMoveForIsometricMap(mainCamera.transform.position, horizontalMove, verticalMove, speedCameraMove, Color.cyan);
        }
    }
    #endregion

    #region Get Direction For check Exit Camera For Borders

    private Vector3 directionForX;
    private Vector3 directionForY;
    private void GetDirectionForCameraMove(out Vector3 directionForX, out Vector3 directionForY)
    {
        directionForX = directionForY = Vector3.zero; // Reset value

        if (horizontalMove != 0)
        {
            directionForX = (horizontalMove * Vector3.right);
        }

        if (verticalMove != 0)
        {
            directionForY = (verticalMove * Vector3.up);
        }
    }

    #endregion

    #region CheckCameraBordersScript

    private Camera mainCamera;

    private CheckCameraBordersScript checkCameraBordersScript;

    //[SerializeField]
    private float speedCameraMove = 12f;

    private Vector3 targetCameraPositionForMove;

    #endregion

    #region CameraMove
    void LateUpdate()
    {
        // Skip a move
        if (horizontalMove == 0 && verticalMove == 0) return;

        CameraMove(targetCameraPositionForMove);
    }

    private void CameraMove(Vector3 targetCameraPosition)
    {
        if (targetCameraPosition != Vector3.zero)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetCameraPosition, 1);
        }
    }
    #endregion    

    #region Ctor
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        checkCameraBordersScript = GetComponent<CheckCameraBordersScript>();
    }
    #endregion
}
