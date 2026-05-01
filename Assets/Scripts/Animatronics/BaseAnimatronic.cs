using UnityEngine;
using UnityEngine.AI;

public abstract class BaseAnimatronic : MonoBehaviour {
    public enum AIState { Default, Blackout }
    public AIState currentAIState = AIState.Default;

    protected NavMeshAgent agent;
    protected Animator animator;
    
    public float moveSpeed = 2f;
    public Transform[] waypoints;
    protected int currentWaypoint = 0;

    public bool isActive = false;
    public bool reachedDoor = false;
    public bool waitingAtDoor = false;
    protected float attackCountdown;
    public int aiLevel = 1; //1-20
    public float attackTimer = 3f;
    public float movementInterval = 5f;
    protected float movementTimer;

    protected virtual void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        
        if (agent != null) {
            agent.speed = moveSpeed;
            agent.enabled = false;
        }
    }

    protected virtual void Update() {
        if (!isActive) return;

        if (GameManager.Instance.isPoweroutActive && currentAIState != AIState.Blackout) {
            SetAIState(AIState.Blackout);
        }

        if (currentAIState == AIState.Default) {
            HandleDefaultAI();
        } else {
            HandleBlackoutAI();
        }
    }

    protected virtual void HandleDefaultAI() {
        if (animator != null && agent != null) {
            bool isMoving = agent.velocity.magnitude > 0.1f && !agent.isStopped;
            animator.SetBool("isWalking", isMoving);
        }

        if (waitingAtDoor) {
            LookAtPlayer();
            HandleDoorWaiting();
        } else if (!reachedDoor) {
            HandleMovementProbability();
        }
    }

    private void HandleMovementProbability() {
        movementTimer += Time.deltaTime;
        if (movementTimer >= movementInterval) {
            movementTimer = 0f;
            int roll = Random.Range(1, 21);
            if (roll <= aiLevel) OnMovementTick();
        }
    }

    protected void LookAtPlayer() {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-direction), Time.deltaTime * 5f);
        }
    }

    protected abstract void OnMovementTick();
    protected abstract void HandleDoorWaiting();
    protected abstract void HandleBlackoutAI();

    public virtual void Activate() {
        isActive = true;
        if (agent != null) agent.enabled = true;
    }

    public virtual void Deactivate() {
        isActive = false;
        if (agent != null) agent.enabled = false;
    }

    public virtual void SetAIState(AIState newState) {
        currentAIState = newState;
        
        if (newState == AIState.Blackout) {
            agent.speed = moveSpeed * 0.5f; 
            reachedDoor = false;
            waitingAtDoor = false;
            currentWaypoint = 0;
        }
    }
}
