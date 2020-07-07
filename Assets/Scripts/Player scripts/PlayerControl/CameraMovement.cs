using UnityEngine;

public class CameraMovement : MonoBehaviour
{
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
    

    private PlayerData _playerData;
    private Vector3 _oldMousePosition;
    private float _oldVerticalAngle;
    private float _oldHorisontalAngle;

    private void Start() {
        this._playerData = this.GetComponent<PlayerData>();
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
            this._oldVerticalAngle   = this.VerticalAngle;
            this._oldHorisontalAngle = this.HorizontalAngle;
            this._oldMousePosition   = mousePosition;
        }
        if (Input.GetMouseButton(1)) {
            float change_x = (mousePosition.x - this._oldMousePosition.x);
            float change_y = (mousePosition.y - this._oldMousePosition.y);

            this.HorizontalAngle = this._oldHorisontalAngle + change_x * this.HorizontalSens;
            this.VerticalAngle   = this._oldVerticalAngle   - change_y * this.VerticalSens;
            
            this.VerticalAngle   = Mathf.Clamp(this.VerticalAngle, this.MinVerticalAnge, this.MaxVerticalAnge);
        }

        this.Distance += Input.GetAxis("Mouse ScrollWheel") * this.ScrollSens;
        this.Distance = Mathf.Clamp(this.Distance, this.MinDistance, this.MaxDistance);
    }

    private void SetCameraPosition()
    {
        ControlledСharacter character = this._playerData.SelectedCharacter;
        Camera camera = this._playerData.SelectedCamera;
        if (character && camera) {
            float horisontalDistance = this.Distance * Mathf.Cos(Mathf.Deg2Rad * this.VerticalAngle);
            
            float offset_up   = this.Distance * Mathf.Sin(Mathf.Deg2Rad * this.VerticalAngle);
            float offset_back = horisontalDistance * Mathf.Sin(Mathf.Deg2Rad * this.HorizontalAngle);
            float offset_left = horisontalDistance * Mathf.Cos(Mathf.Deg2Rad * this.HorizontalAngle);
            Vector3 offsetVector = (Vector3.forward * -1) * offset_back + 
                                    Vector3.up * offset_up +
                                    Vector3.right * offset_left;
            Vector3 cameraPos = character.transform.position + offsetVector;
            

            camera.transform.position = cameraPos;
            camera.transform.LookAt(character.transform.position);
        } 
    }
}
