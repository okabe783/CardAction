using UnityEngine;

public class MagicCircle : Magic
{
    //魔法陣用のEffect
    [SerializeField] private GameObject _magicCircleTrail;

    protected override void Start()
    {
        base.Start();
        //Effectの再生
        Instantiate(_magicCircleTrail, transform.position, Quaternion.Euler(-90f,0f,0f));
    }
}