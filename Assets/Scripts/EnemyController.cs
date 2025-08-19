using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected LightController lightController;
    [SerializeField] protected SoundEventComponent soundEventComponent;
    //[SerializeField] protected AnimationController animationController;
    [SerializeField] private Transform target;
    [SerializeField] private float chaseSpeed=3, patrolSpeed=1;
    private NavMeshAgent navMeshAgent;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (!navMeshAgent) Debug.LogError($"No NavMeshAgent found in enemy {name}");
    }

    private void Update()
    {
        if (navMeshAgent)
        {
            navMeshAgent.SetDestination(target.position);
            navMeshAgent.speed = patrolSpeed;
        }
    }
}
