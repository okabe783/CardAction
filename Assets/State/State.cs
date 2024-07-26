using UnityEngine;

public class State : MonoBehaviour
{
    public enum EnemyMove
    {
        Walking, //探索中
        Chasing,　//追跡中
        Attacking,　//攻撃
        Died　//死亡
    }
}