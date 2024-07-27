using UnityEngine;
using UnityEngine.AI;

//Navmeshで動くCharacter
[RequireComponent(typeof(NavMeshAgent))]
public class NavmeshCtrl : MonoBehaviour
{
    [SerializeField] private GameObject _player; //追跡対象
    private State.EnemyMove _currentState = State.EnemyMove.Walking; //現在の状態
    private State.EnemyMove _nextState = State.EnemyMove.Walking; //次の状態
    
    private NavMeshAgent _navMeshAgent;
    
    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        TargetPosition();
        StopNavigation();
    }

    //追跡対象をSet
    private void TargetPosition()
    {
        //NavmeshがActiveでnullではなければ
        if (_navMeshAgent != null && _navMeshAgent.isActiveAndEnabled)
        {
            _nextState = State.EnemyMove.Chasing; //追跡を開始
            _navMeshAgent.SetDestination(_player.transform.position);
            _navMeshAgent.isStopped = false; //NavMeshをセット
        }  
    }

    //ターゲットにたどり着いたとき
    private void StopNavigation()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.pathPending)
        {
            _nextState = State.EnemyMove.Attacking; //攻撃を開始
            _navMeshAgent.isStopped = true;
        }
    }
}