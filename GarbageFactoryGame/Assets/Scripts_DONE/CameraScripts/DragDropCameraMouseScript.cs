using UnityEngine;

/// <summary>
/// Скрипт перетаскивания камеры мышкой. Цепляется на камеру.
/// Отключается при некоторых сценариях работы.
/// </summary>
public class DragDropCameraMouseScript : MonoBehaviour
{
    #region ReadPlayerInputForAxis

    private Vector3 mouseStartPosition;
    private Vector3 mouseEndPosition;

    private bool isCameraCanMove;

    private bool ReadPlayerInputForAxis(ref Vector3 mouseStartPosition, ref Vector3 mouseEndPosition)
    {
        if (Input.GetMouseButtonDown(0))
        {
            //print("Get mouseStartPosition");
            mouseStartPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            return false;
        }
        else if (Input.GetMouseButton(0))
        {
            //print("Get mouseEndPosition");
            mouseEndPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (mouseEndPosition - mouseStartPosition == Vector3.zero)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
    #endregion

    private void Update()
    {
        isCameraCanMove = ReadPlayerInputForAxis(ref mouseStartPosition, ref mouseEndPosition);

        if (isCameraCanMove)
        {
            targetCameraPosition = GetTargetPositionFromCameraMove();
        }

        // Можно сделать для изометрической карты движение, если камера упирается по аналогии с движением кнопками
    }

    #region GetTargetPositionFromCameraMove and CheckCameraBordersScript

    private Camera mainCamera;

    private CheckCameraBordersScript checkCameraBorderScript;

    //[SerializeField]
    private float speedDragDrop = 12f;

    private Vector3 targetCameraPosition;

    private Vector3 GetTargetPositionFromCameraMove()
    {
        targetCameraPosition = mouseStartPosition - mouseEndPosition;

        targetCameraPosition = checkCameraBorderScript.GetTargetCameraPositionForMove(mainCamera.transform.position, targetCameraPosition, Vector3.zero, speedDragDrop, Color.green);

        return targetCameraPosition;
    }
    #endregion

    #region  CameraMove 

    private void LateUpdate()
    {
        if (isCameraCanMove)
        {
            CameraMove(targetCameraPosition);
        }
    }

    private void CameraMove(Vector3 targetCameraPosition)
    {
        if (targetCameraPosition != Vector3.zero)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetCameraPosition, 1);
        }
    }
    #endregion

    // Reset value
    private void OnEnable()
    {
        mouseStartPosition = Vector3.zero;
        mouseEndPosition = Vector3.zero;

        isCameraCanMove = false;
    }

    #region Ctor
    void Awake()
    {
        mainCamera = GetComponent<Camera>();

        checkCameraBorderScript = GetComponent<CheckCameraBordersScript>();
    }
    #endregion
}
