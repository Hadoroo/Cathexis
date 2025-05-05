using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target; // The player or object to follow
    [SerializeField] private string targetTag = "Player"; // Fallback if target not set
    
    [Header("Follow Settings")]
    [SerializeField] private Vector2 offset = new Vector2(0f, 1f);
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private bool lookForTargetIfNull = true;
    
    [Header("Bounds")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Rect levelBounds; // Set this in the inspector

    private Camera cam;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        
        if (target == null && lookForTargetIfNull)
        {
            FindTarget();
        }
    }

    private void FindTarget()
    {
        GameObject targetObj = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObj != null)
        {
            target = targetObj.transform;
        }
        else
        {
            Debug.LogWarning($"No object with tag '{targetTag}' found!");
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            if (lookForTargetIfNull) FindTarget();
            return;
        }

        Vector3 targetPosition = CalculateTargetPosition();
        transform.position = SmoothToPosition(targetPosition);
        
        if (useBounds)
        {
            ClampCameraToBounds();
        }
    }

    private Vector3 CalculateTargetPosition()
    {
        return new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z // Maintain original Z position
        );
    }

    private Vector3 SmoothToPosition(Vector3 targetPos)
    {
        // SmoothDamp is perfect for camera movement
        return Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothSpeed * Time.deltaTime
        );
    }

    private void ClampCameraToBounds()
    {
        if (!useBounds) return;
        
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        
        float minX = levelBounds.xMin + camWidth;
        float maxX = levelBounds.xMax - camWidth;
        float minY = levelBounds.yMin + camHeight;
        float maxY = levelBounds.yMax - camHeight;
        
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;
    }

    // Draw bounds in editor for easy visualization
    private void OnDrawGizmosSelected()
    {
        if (!useBounds) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(levelBounds.center, levelBounds.size);
    }
}