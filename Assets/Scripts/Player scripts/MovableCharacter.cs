using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCharacter : MonoBehaviour
{
    public bool IsRunning = false;
    public float WalkSpeed = 200;
    public float RunSpeed = 600;

    private Vector3? _targetPosition;
    private Animation _animationController;

    void Start()
    {
        this.TryGetComponent<Animation>(out this._animationController);
    }

    void FixedUpdate()
    {
        if (_targetPosition != null) {
            this.RotateToTarget();
            this.MoveToTarget();
            this.CheckStopMovement();
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
    }

    private void RotateToTarget()
    {
        Vector3 toTarget = (Vector3)_targetPosition - transform.position;

        Vector3 v1 = transform.forward;
        v1.y = 0;
        Vector3 v2 = toTarget;
        v2.y = 0;

        float angle = Vector3.SignedAngle(v1, v2, Vector3.up);
        Vector3 rotation = new Vector3(0, angle, 0);

        transform.Rotate(rotation);
    }

    private void MoveToTarget()
    {
        if (this._animationController) {
            if (this.IsRunning) {
                this._animationController.Play("run");
            } else {
                this._animationController.Play("walk");
            }
        }

        float currentSpeed = this.IsRunning ? this.RunSpeed : this.WalkSpeed;
        Vector3 velocity = transform.forward * currentSpeed * Time.fixedDeltaTime;

        this.GetComponent<Rigidbody>().velocity = velocity;
    }
    
    private void CheckStopMovement()
    {
        Vector3 toTargetPosition = transform.position - (Vector3)_targetPosition;
        if ((toTargetPosition).magnitude < 1) {
            _targetPosition = null;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;

            if (this._animationController) {
                float fadeLength = this.IsRunning ? 0.5f : 0.3f;
                this._animationController.CrossFade("idle01", fadeLength);
            }
        }
    }
}
