using System;
using System.Collections;
using UnityEngine;

public class CameraZoomScript : MonoBehaviour
{
    #region Base Settings
    private Camera mainCamera;

    //[SerializeField] 
    private float minCameraSize = 5f;

    //[SerializeField]
    private float maxCameraSize = 15f;

    //[SerializeField] 
    private float baseStepCameraZoom = 1.7f;

    // We can Enable Smoothing Zoom
    [SerializeField]
    private bool smoothingModeEnableFlag;

    [SerializeField]
    private int frameForSmoothing = 10;

    private float smoothingStepCameraZoom;

    #endregion

    #region Base Zoom Method
    private void ZoomMinus(float stepCameraZoom)
    {
        float currentCamZoom = mainCamera.orthographicSize + stepCameraZoom;

        mainCamera.orthographicSize = Mathf.Clamp(currentCamZoom, minCameraSize, maxCameraSize);

        //print("Zoom - ");
    }
    private void ZoomPlus(float stepCameraZoom)
    {
        float currentCamZoom = mainCamera.orthographicSize - stepCameraZoom;

        mainCamera.orthographicSize = Mathf.Clamp(currentCamZoom, minCameraSize, maxCameraSize);

        //print("Zoom + ");
    }
    #endregion

    #region Smoothing Zoom if Enable

    private bool isWorkingNowSmoothingZoom = false;

    public IEnumerator SmoothingZoomPlusOrMinus(float mouseScroll)
    {
        if (isWorkingNowSmoothingZoom)
        {
            yield break;
        }

        isWorkingNowSmoothingZoom = true;

        for (int i = 0; i < frameForSmoothing; i++)
        {
            if (mouseScroll < 0)
            {
                ZoomMinus(smoothingStepCameraZoom);
            }
            else if (mouseScroll > 0)
            {
                ZoomPlus(smoothingStepCameraZoom);
            }

            yield return null;
        }

        isWorkingNowSmoothingZoom = false;
    }
    #endregion

    void LateUpdate()
    {
        ChangeNewCameraZoom();
    }

    internal void ChangeNewCameraZoom(float mouseScroll = 0)
    {
        if (mouseScroll == 0)
        {
            mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        }

        if (mouseScroll < 0)
        {
            if (smoothingModeEnableFlag)
            {
                StartCoroutine(SmoothingZoomPlusOrMinus(mouseScroll));
            }
            else
            {
                ZoomMinus(baseStepCameraZoom);
            }
        }
        else if (mouseScroll > 0)
        {
            if (smoothingModeEnableFlag)
            {
                StartCoroutine(SmoothingZoomPlusOrMinus(mouseScroll));
            }
            else
            {
                ZoomPlus(baseStepCameraZoom);
            }
        }
    }

    #region Ctor
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();

        smoothingStepCameraZoom = baseStepCameraZoom / frameForSmoothing;
    }
    #endregion
}
