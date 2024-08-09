using UnityEngine;
using UnityEngine.AI;

// Navmeshで動くCharacter
[RequireComponent(typeof(NavMeshAgent))]
public class NavmeshCtrl : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    
    [SerializeField] private GameObject _player; // 追跡対象
    
    private State.EnemyMove _currentState = State.EnemyMove.None; // 現在の状態
    private State.EnemyMove _nextState = State.EnemyMove.None; // 次の状態

    #region アニメーションを管理

    private static readonly int Speed = Animator.StringToHash("Speed");

    #endregion
    

    //Speed
    private float _targetSpeed; 
    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnimatorSpeed();
        TargetPosition();
        StopNavigation();
    }

    // アニメーションの速度を更新する
    private void UpdateAnimatorSpeed()
    {
        _animator.SetFloat(Speed, _targetSpeed);
    }

    // 追跡対象を開始する
    private void TargetPosition()
    {
        // NavmeshがActiveでnullではないかつNavmeshが有効であるか
        if (_navMeshAgent != null && _navMeshAgent.isActiveAndEnabled)
        {
            // 追跡を開始
            _nextState = State.EnemyMove.Chasing;
            _targetSpeed = _navMeshAgent.speed;
            // 目的地をPlayerの位置に設定
            _navMeshAgent.SetDestination(_player.transform.position);
            _navMeshAgent.isStopped = false;
        }
    }

    // ターゲットにたどり着いたとき
    private void StopNavigation()
    {
        // 現在の位置から目的地までの残り距離が目的地に到着したとみなされる距離以下で新しい経路を計算していなければ
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.pathPending)
        {
            // 攻撃を開始
            _nextState = State.EnemyMove.Attacking;
            _targetSpeed = 0.0f;
            _navMeshAgent.isStopped = true;
        }
    }
}