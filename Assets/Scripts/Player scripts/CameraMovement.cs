using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float rotationSens = 0.3f;
    public float speed = 1f;

    private Vector3 mouseOldPosition;
    private Quaternion oldRotation;

    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += transform.rotation.normalized * direction * speed;

        Vector3 mousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(1)) {
            this.oldRotation = transform.rotation;
            this.mouseOldPosition = mousePosition;
        }
        if (Input.GetMouseButton(1)) {
            Vector3 mouseDeltaPosition = mousePosition - mouseOldPosition;
            Vector3 rotation = new Vector3(mouseDeltaPosition.y, mouseDeltaPosition.x, 0) * this.rotationSens;
            transform.eulerAngles = oldRotation.eulerAngles + rotation;
        }        
    }
}
