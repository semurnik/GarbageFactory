using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Скрипт приближения Камеры кнопками UI. Висит на кнопках
/// </summary>
public class CameraZoomFromUIButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Camera mainCamera;

    public bool thisButtonPlus;

    private bool pressBotton;

    #region DragDropCameraMouseScript Enable or Disable
    private DragDropCameraMouseScript dragDropCameraMouseScript;
    #endregion

    #region We call first Zoom     

    private CameraZoomScript cameraZoomScript;

    public void OnPointerDown(PointerEventData eventData)
    {
        //print("Button click");

        pressBotton = true;

        dragDropCameraMouseScript.enabled = false;

        // We call first Zoom      
        CallCameraZoom();

        // We start scanning long press 
        getLongClick = StartCoroutine(GetLongClick());
    }

    private void CallCameraZoom()
    {
        if (thisButtonPlus)
        {
            cameraZoomScript.ChangeNewCameraZoom(1);
        }
        else
        {
            cameraZoomScript.ChangeNewCameraZoom(-1);
        }
    }
    #endregion

    #region We scan long press and call zoom

    //[SerializeField]
    private float timeDelayBetweenPress = 0.17f;

    private Coroutine getLongClick;

    private IEnumerator GetLongClick()
    {
        yield return new WaitForSeconds(timeDelayBetweenPress);

        for (int i = 0; i < 100; i++)
        {
            // Проверяем на удерживание кнопки
            if (!pressBotton)
            {
                //print("Not Long Click");
                yield break;
            }

            //print("Long Click");

            // Вызывай зум
            CallCameraZoom();

            // Посмотри, будет ли сохраняться нажатие в след кадре
            yield return null;
        }
    }
    #endregion

    public void OnPointerUp(PointerEventData eventData)
    {
        //print("Up");

        pressBotton = false;        

        dragDropCameraMouseScript.enabled = true;    

        // We disable long press scanning
        StopCoroutine(getLongClick);
    }

    #region Ctor
    private void Awake()
    {
        dragDropCameraMouseScript = mainCamera.GetComponent<DragDropCameraMouseScript>();

        cameraZoomScript = mainCamera.GetComponent<CameraZoomScript>();
    }
    #endregion
}
