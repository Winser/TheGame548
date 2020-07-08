using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEventArgs: System.EventArgs
{
    public Vector3 PositionOnScreen;
    public Vector3 PositionOnWorld;
    public RaycastHit? Hit;
    public GameObject TargetObject;
    public Vector3? TargetPoint;

    ///<summary>Клавиши которые нажаты сейчас</summary>
    public bool[] MouseKeys;

    ///<summary>Клавиши которые были нажаты</summary>
    public bool[] PressedMouseKeys;

    ///<summary>Клавиши которые были отпущены</summary>
    public bool[] ReleasedMouseKeys;
    public bool Handled = false;

    public bool HasTarget { get => this.Hit != null; }

    ///<summary>Была ли нажата клавища</summary>
    public bool WasPressed { get => this.HaveTrue(this.PressedMouseKeys); }

    ///<summary>Была ли отпущена клавища</summary>
    public bool WasReleased { get => this.HaveTrue(this.ReleasedMouseKeys); }

    private bool HaveTrue(bool[] array)
    {
        for (int i = 0; i < array.Length; i++) {
            if (array[i]) {
                return true;
            }
        }
        return false;
    }
}
