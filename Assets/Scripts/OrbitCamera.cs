using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    [Header("Cible")]
    public Transform target;                                   // Le Player à suivre
    public Vector3 pivotOffset = new Vector3(0f, 1.6f, 0f);      // Point regardé (hauteur des yeux)

    [Header("Zoom (molette)")]
    public float distance = 5f;
    public float minDistance = 1.5f;
    public float maxDistance = 10f;
    public float zoomSpeed = 4f;

    [Header("Rotation souris")]
    public float mouseSensitivity = 3f;
    public bool invertY = false;
    public float minPitch = -20f;   // Limite basse : évite de regarder sous le sol
    public float maxPitch = 60f;    // Limite haute : évite de passer au-dessus de la tête

    [Header("Anti-clipping (optionnel)")]
    public bool avoidObstacles = true;
    public LayerMask obstacleLayers = ~0;
    public float collisionRadius = 0.3f;

    private float yaw;
    private float pitch = 15f;

    void Start()
    {
        if (target != null)
            yaw = target.eulerAngles.y;

        // Le curseur reste libre tant qu'aucun bouton de souris n'est maintenu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleCursorState();
        HandleZoom();
        HandleRotation();
        ApplyTransform();
    }

    void HandleCursorState()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if ((Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1)) ||
            (Input.GetMouseButtonUp(1) && !Input.GetMouseButton(0)))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDistance, maxDistance);
        }
    }

    void HandleRotation()
    {
        bool rightHeld = Input.GetMouseButton(1);
        bool leftHeld = Input.GetMouseButton(0);

        if (!rightHeld && !leftHeld) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch += invertY ? mouseY : -mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        if (rightHeld)
        {
            // Clic droit : le Player tourne pour faire face à la même direction que la caméra
            // (il lui tourne donc le dos)
            target.rotation = Quaternion.Euler(0f, yaw, 0f);
        }
        // Clic gauche : seule la caméra orbite, le Player garde son orientation
    }

    void ApplyTransform()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 pivot = target.position + pivotOffset;
        float desiredDistance = distance;

        if (avoidObstacles)
        {
            Vector3 direction = rotation * Vector3.back;
            if (Physics.SphereCast(pivot, collisionRadius, direction, out RaycastHit hit, distance, obstacleLayers))
            {
                desiredDistance = Mathf.Max(hit.distance, 0.2f);
            }
        }

        transform.position = pivot + rotation * Vector3.back * desiredDistance;
        transform.rotation = rotation;
    }
}