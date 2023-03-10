using UnityEngine;

/// <summary>
/// Скрип с вычислением бордюров.
/// Весит на камере. Его использут для перемещения камеры в пространстве
/// </summary>
public class CheckCameraBordersScript : MonoBehaviour
{
    private int numberLayerBorder;

    private float rayCastDistance = 1f;
        
    void Start()
    {
        /// Должен быть объект с маской Border, за границы которого выходить камера не может
        numberLayerBorder = LayerMask.GetMask("Border");
    }

    public Vector3 GetTargetCameraPositionForMove(Vector3 currentCameraPosition, Vector3 directionForX, Vector3 directionForY, float speed, Color color)
    {
        if (directionForX == Vector3.zero && directionForY == Vector3.zero)
        {
            return Vector3.zero;
        }

        /// Куда переместится объект?
        Vector3 targetCameraPosition = currentCameraPosition + (directionForX + directionForY) * Time.deltaTime * speed;

        return CheckHitInBordersTargetCameraPosition(currentCameraPosition, targetCameraPosition, color);
    }
    private Vector3 CheckHitInBordersTargetCameraPosition(Vector3 currentCameraPosition, Vector3 targetCameraPosition, Color color)
    {
        /// Стреляй туда лучом, если там пол, то перемещайся в LateUpdate
        RaycastHit2D searchHitBorder = Physics2D.Raycast(targetCameraPosition, Vector3.zero, rayCastDistance, numberLayerBorder);

        Debug.DrawRay(currentCameraPosition, targetCameraPosition, color, 3f);

        if (searchHitBorder.collider != null)
        {
            //print("Move");            
            return targetCameraPosition;
        }
        else
        {
            //print("STOP ");            
            return Vector3.zero;
        }
    }

    // Use only with Isometric map
    #region GetAngleCameraMoveForIsometricMap
    public Vector3 GetAngleCameraMoveForIsometricMap(Vector3 currentCameraPosition, float horizontalMove, float verticalMove, float speed, Color color)
    {
        // Какой угол взять
        if (horizontalMove > 0 && verticalMove > 0)
        {
            Vector3 angle45 = currentCameraPosition + new Vector3(0.5f, 0.25f, 0) * Time.deltaTime * speed;
            //print("angle45");

            return CheckHitInBordersTargetCameraPosition(currentCameraPosition, angle45, color);

        }
        else if (horizontalMove > 0 && verticalMove < 0)
        {
            Vector3 angle135 = currentCameraPosition + new Vector3(0.5f, -0.25f, 0) * Time.deltaTime * speed;
            //print("angle135");

            return CheckHitInBordersTargetCameraPosition(currentCameraPosition, angle135, color);
        }
        else if (horizontalMove < 0 && verticalMove < 0)
        {
            Vector3 angle225 = currentCameraPosition + new Vector3(-0.5f, -0.25f, 0) * Time.deltaTime * speed;
            //print("angle225");

            return CheckHitInBordersTargetCameraPosition(currentCameraPosition, angle225, color);
        }
        else if (horizontalMove < 0 && verticalMove > 0)
        {
            Vector3 angle315 = currentCameraPosition + new Vector3(-0.5f, 0.25f, 0) * Time.deltaTime * speed;
            // print("angle315");

            return CheckHitInBordersTargetCameraPosition(currentCameraPosition, angle315, color);
        }

        return Vector3.zero;
    }
    #endregion
}
