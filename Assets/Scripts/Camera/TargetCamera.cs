using UnityEngine;

namespace Camera
{
    public class TargetCamera : MonoBehaviour
    {
        public void UpdatePositionToTarget(Vector3 targetPos, Vector2 max, Vector2 min)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, min.x, max.x);
            targetPos.y = Mathf.Clamp(targetPos.y, min.y, max.y);
            transform.position = new Vector3(targetPos.x, targetPos.y, -10);
        }
    }
}