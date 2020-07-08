using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovableCharacter : MonoBehaviour
{
    public Transform DebugPoint;
    public static bool IsAiming = false;
    public bool IsRunning = false;
    public float WalkSpeed = 200;
    public float RunSpeed = 600;

    //private animation timer
    private float timer;
    

    // private Vector3? _targetPosition;
    private Animator _animationController;
    private NavMeshAgent _navMeshAgent;
    private List<Vector3> _path;

    private float CurrentSpeed 
    {
        get {
            return this.IsRunning ? this.RunSpeed : this.WalkSpeed;
        }
    }
    private bool HasNexPoint { get => (this._path != null) && (this._path.Count > 0); }
    private Vector3? NextPathPoint { get =>  HasNexPoint ? (Vector3?)this._path[0] : null; }

    void Start()
    {
        this.TryGetComponent<Animator>(out this._animationController);
        this._navMeshAgent = gameObject.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;
       
    }
    private void Update()
    {
        if (Input.GetKeyDown("2"))
        {
            if (!IsRunning)
            {
                IsRunning = true;
            }
            else
            {
                IsRunning = false;
            }

        }
    }

    void FixedUpdate()
    {
       
        if (tag == "Player")
        {
            this.Timer();
            this.Aiming_fire();
            if (!IsAiming && timer > 2f)
            {
                if (this.HasNexPoint)
                {
                    this.RotateToTarget();
                    this.MoveToTarget();
                }
                /*else
                {
                    this.StopMovement();
                }*/

            }
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this._path = this.getPathTo(targetPosition);
    }

    public List<Vector3> getPathTo(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (this._navMeshAgent.CalculatePath(targetPosition, path) && path.corners.Length > 1) {
            List<Vector3> result = new List<Vector3>(path.corners);
            result.RemoveAt(0);
            return result;
        } else {
            return null;
        }
    }
    public float? GetDistanceTo(Vector3 targetPosition) 
    {
        List<Vector3> path = this.getPathTo(targetPosition);
        
        float distance = 0.0f;
        if ((path != null) && (path.Count > 1))
        {
            for ( int i = 1; i < path.Count; ++i )
            {
                distance += Vector3.Distance( path[i-1], path[i] );
            }
        } else {
            return null;
        }
    
        return distance;
    }

    private void RotateToTarget()
    {
        Vector3 toTarget = (Vector3)this.NextPathPoint - transform.position;

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
        Vector3 toTarget = (Vector3)this.NextPathPoint - transform.position;
       
        if (this._animationController) {
            if (this.IsRunning) {
                this._animationController.Play("Running");
            } else {
                this._animationController.Play("WalkFWD");
            }
        }

        Vector3 changePosition = toTarget.normalized * this.CurrentSpeed * Time.fixedDeltaTime;
    
        if (changePosition.magnitude < toTarget.magnitude) {
            this.transform.position += changePosition;
        } else {
            this.transform.position += toTarget;
            this._path.RemoveAt(0);
        }
    }

    /* private void StopMovement()
     {
         if (this._animationController) {
             float fadeLength = this.IsRunning ? 0.5f : 0.3f;
             this._animationController.CrossFade("WalkFWD", fadeLength);
         }
     }*/
    private void Aiming_fire()
    {
        if (Input.GetKeyDown("1"))
        {
                IsAiming = true;
                _animationController.Play("Aiming");
        }
        if (Input.GetMouseButtonDown(0) && IsAiming)
        {
            _animationController.Play("Fire");
            IsAiming = false;
        }
    }

    private void Timer()
    {
        
        if (IsAiming)
        {
            timer = 0f;
        }
        else
        {
            
            timer += Time.deltaTime;
        }
        
    }
}
