using UnityEngine;
using UnityEngine.AI;

public class AnimatronicAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected bool isActive = false;

  
    public float moveSpeed = 2f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.enabled = false;
    }

    public virtual void Activate()
    {
        isActive = true;
        agent.enabled = true;
    }

    public virtual void Deactivate()
    {
        isActive = false;
        agent.enabled = false;
    }
}