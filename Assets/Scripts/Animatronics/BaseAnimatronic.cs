using UnityEngine;
using UnityEngine.AI;

public abstract class BaseAnimatronic : MonoBehaviour {
    public enum AIState { Default, Blackout }
    public AIState currentAIState = AIState.Default;

    public Transform[] waypoints;
    public int aiLevel = 1; //1-20
    public float moveSpeed = 6f;
    public float attackTimer = 3f;
    public float movementInterval = 5f;
    public float detectionRange = 10f;
    public float chaseSpeed = 4f;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected Transform playerTransform;
    protected bool isActive = false;
    protected float movementTimer;
    protected int currentWaypoint = 0;
    protected bool reachedDoor = false;
    protected bool waitingAtDoor = false;
    protected float attackCountdown;

    protected virtual void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        
        if (agent != null) {
            agent.speed = moveSpeed;
            agent.enabled = false;
        }
    }

    protected virtual void Start() {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    protected virtual void Update() {
        if (!isActive) return;

        if (GameManager.Instance.IsPoweroutActive && currentAIState != AIState.Blackout) {
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
        if (playerTransform != null) {
            Vector3 direction = playerTransform.position - transform.position;
            direction.y = 0f;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-direction), Time.deltaTime * 5f);
        }
    }

    protected void PatrolWaypoints() {
        if (waypoints.Length == 0) return;
        agent.SetDestination(waypoints[currentWaypoint].position);
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
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

    public void IncreaseDifficulty(int amount) {
        aiLevel = Mathf.Clamp(aiLevel + amount, 0, 20);
    }
}
