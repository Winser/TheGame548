using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCharacter : MonoBehaviour
{
    public bool IsRunning = false;
    public float WalkSpeed = 3;
    public float RunSpeed = 9;

    private Vector3? targetPosition;
    private Animation animationController;
    void Start()
    {
        this.TryGetComponent<Animation>(out this.animationController);
    }

    void Update()
    {
        if (targetPosition != null) {
            this.RotateToTarget();
            this.MoveToTarget();
            this.CheckStopMovement();
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void RotateToTarget()
    {
        Vector3 toTarget = (Vector3)targetPosition - transform.position;

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
        if (this.animationController) {
            if (this.IsRunning) {
                this.animationController.Play("run");
            } else {
                this.animationController.Play("walk");
            }
        }

        float currentSpeed = this.IsRunning ? this.RunSpeed : this.WalkSpeed;
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }

    private void CheckStopMovement()
    {
        if ((transform.position - (Vector3)targetPosition).magnitude < 1) {
            targetPosition = null;

            if (this.animationController) {
                float fadeLength = this.IsRunning ? 0.5f : 0.3f;
                this.animationController.CrossFade("idle01", fadeLength);
            }
        }
    }
}
