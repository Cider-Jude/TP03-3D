using UnityEngine;

// A cause de l'animation qui bouge le rig de l'areignée
public class LockVisualRootXZ : MonoBehaviour
{
    private Vector3 initialLocalPos;

    void Start()
    {
        initialLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.localPosition;
        pos.x = initialLocalPos.x;
        pos.z = initialLocalPos.z;
        transform.localPosition = pos;
        // Garde pos.y libre si l'animation a un léger mouvement vertical
    }
}