using UnityEngine;
using UnityEngine.AI;

public abstract class BaseAnimatronic : MonoBehaviour {
    protected NavMeshAgent agent;
    protected Animator animator;
    
    public float moveSpeed = 2f;
    public Transform[] waypoints;
    protected int currentWaypoint = 0;

    public bool isActive = false;
    public bool reachedDoor = false;

    protected virtual void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        
        if (agent != null) {
            agent.speed = moveSpeed;
            agent.enabled = false;
        }
    }

    public virtual void Activate() {
        isActive = true;
        if (agent != null) agent.enabled = true;
    }

    public virtual void Deactivate() {
        isActive = false;
        if (agent != null) agent.enabled = false;
    }
}
