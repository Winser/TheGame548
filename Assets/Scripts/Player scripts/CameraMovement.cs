using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera SelectedCamera;

    public float Distance = 50;
    public float MinDistance = 10;
    public float MaxDistance = 50;

    public float VerticalAngle = 30;
    public float HorizontalAngle = 0;
    public float MinVerticalAnge = 10;
    public float MaxVerticalAnge = 45;

    public float ScrollSens = 100;
    public float VerticalSens = 0.3f;
    public float HorizontalSens = 0.3f;
    

    private PlayerData playerData;
    private Vector3 oldMousePosition;
    private float oldVerticalAngle;
    private float oldHorisontalAngle;

    private void Start() {
        this.playerData = this.GetComponent<PlayerData>();
        this.SelectedCamera = this.SelectedCamera ?? Camera.main;
    }

    void Update()
    {
        this.ProcessInput();
        this.SetCameraPosition();  
    }

    private void ProcessInput()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(1)) {
            this.oldVerticalAngle   = this.VerticalAngle;
            this.oldHorisontalAngle = this.HorizontalAngle;
            this.oldMousePosition   = mousePosition;
        }
        if (Input.GetMouseButton(1)) {
            float change_x = (mousePosition.x - this.oldMousePosition.x);
            float change_y = (mousePosition.y - this.oldMousePosition.y);

            this.HorizontalAngle = this.oldHorisontalAngle + change_x * this.HorizontalSens;
            this.VerticalAngle   = this.oldVerticalAngle   - change_y * this.VerticalSens;
            
            this.VerticalAngle   = Mathf.Clamp(this.VerticalAngle, this.MinVerticalAnge, this.MaxVerticalAnge);
        }

        this.Distance += Input.GetAxis("Mouse ScrollWheel") * this.ScrollSens;
        this.Distance = Mathf.Clamp(this.Distance, this.MinDistance, this.MaxDistance);
    }

    private void SetCameraPosition()
    {
        ControlledСharacter character = this.playerData.selectedCharacter;
        if (character) {
            float horisontalDistance = this.Distance * Mathf.Cos(Mathf.Deg2Rad * this.VerticalAngle);
            
            float offset_up   = this.Distance * Mathf.Sin(Mathf.Deg2Rad * this.VerticalAngle);
            float offset_back = horisontalDistance * Mathf.Sin(Mathf.Deg2Rad * this.HorizontalAngle);
            float offset_left = horisontalDistance * Mathf.Cos(Mathf.Deg2Rad * this.HorizontalAngle);
            Vector3 offsetVector = (Vector3.forward * -1) * offset_back + 
                                    Vector3.up * offset_up +
                                    Vector3.right * offset_left;
            Vector3 cameraPos = character.transform.position + offsetVector;
            

            this.SelectedCamera.transform.position = cameraPos;
            this.SelectedCamera.transform.LookAt(character.transform.position);
        } 
    }
}
