using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public delegate void MouseEvent(MouseEventArgs e);
    public event MouseEvent mouseDown;
    public event MouseEvent mouseUp;
    public event MouseEvent leftMouseDown;
    public event MouseEvent leftMouseUp;
    public event MouseEvent rightMouseDown;
    public event MouseEvent rightMouseUp;
    public event MouseEvent middleMouseDown;
    public event MouseEvent middleMouseUp;

    private PlayerData _playerData;
    private int _lastFrame;
    private bool[] _mouseButtonsStates = {false, false, false};

    public void DropState()
    {
        this._mouseButtonsStates = new bool[]{false, false, false};
    }

    private void Start() {
        this._playerData = GetComponent<PlayerData>();
    }
    private void Update()
    {
        if (Time.frameCount == this._lastFrame) return;
        this._lastFrame = Time.frameCount;
        
        if (_playerData.SelectedCamera != null)
        {
            MouseEventArgs mouseEventArgs = this.BuildMouseEventArgs();

            if (mouseEventArgs != null) {
                if ((!mouseEventArgs.Handled) && mouseEventArgs.PressedMouseKeys[0])
                    this.leftMouseDown?.Invoke(mouseEventArgs);
                if ((!mouseEventArgs.Handled) && mouseEventArgs.ReleasedMouseKeys[0])
                    this.leftMouseUp?.Invoke(mouseEventArgs);

                if ((!mouseEventArgs.Handled) && mouseEventArgs.PressedMouseKeys[1])
                    this.rightMouseDown?.Invoke(mouseEventArgs);
                if ((!mouseEventArgs.Handled) && mouseEventArgs.ReleasedMouseKeys[1])
                    this.rightMouseUp?.Invoke(mouseEventArgs);

                if ((!mouseEventArgs.Handled) && mouseEventArgs.PressedMouseKeys[2])
                    this.middleMouseDown?.Invoke(mouseEventArgs);
                if ((!mouseEventArgs.Handled) && mouseEventArgs.ReleasedMouseKeys[2])
                    this.middleMouseUp?.Invoke(mouseEventArgs);

                if ((!mouseEventArgs.Handled) && mouseEventArgs.WasPressed)
                    this.mouseDown?.Invoke(mouseEventArgs);
                if ((!mouseEventArgs.Handled) && mouseEventArgs.WasReleased)
                    this.mouseUp?.Invoke(mouseEventArgs);
            }
        }
    }

    private MouseEventArgs BuildMouseEventArgs()
    {
        if (_playerData.SelectedCamera != null)
        {
            Camera camera = _playerData.SelectedCamera;
            Vector3 positionOnScreen = Mouse.GetMousePositionOnScreen();
            Vector3 positionOnWorld  = Mouse.GetMousePositionOnWorld(camera);
            RaycastHit? hit          = Mouse.GetHit(camera);
            GameObject targetObject  = null;
            Vector3? targetPoint     = null;
            if (hit != null) {
                targetObject = ((RaycastHit)hit).collider.gameObject;
                targetPoint = (Vector3?)((RaycastHit)hit).point;
            }


            bool[] pressedMouseKeys = {false, false, false};
            bool[] releasedMouseKeys = {false, false, false};;
            for (int key = 0; key < 3; key++) {
                if (Input.GetMouseButton(key)) {
                    pressedMouseKeys[key] = !this._mouseButtonsStates[key];
                    this._mouseButtonsStates[key] = true;
                } else {
                    releasedMouseKeys[key] = this._mouseButtonsStates[key];
                    this._mouseButtonsStates[key] = false;
                }
            }


            MouseEventArgs eventArgs = new MouseEventArgs() {
                PositionOnScreen = positionOnScreen,
                PositionOnWorld = positionOnWorld,
                Hit = hit,
                TargetObject = targetObject,
                TargetPoint = targetPoint,
                MouseKeys = this._mouseButtonsStates,
                PressedMouseKeys = pressedMouseKeys,
                ReleasedMouseKeys = releasedMouseKeys
            };

            return eventArgs;
        }

        return null;
    }
}
