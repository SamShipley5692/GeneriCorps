using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int numberOfAttacks;
    [SerializeField] private float attackCounterResetCooldown;

    public int CurrentAttackCounter
    {
        get => currentAttackCounter;
        private set => currentAttackCounter = value >= numberOfAttacks ? 0 : value;
    }

    public event Action OnExit;

    private Animator anim;
    private GameObject baseGameObject;


    private int currentAttackCounter;

    

    public void Enter()
    {
        print($"{transform.name} enter");

       

        anim.SetBool("active", true);
        anim.SetInteger("counter", currentAttackCounter);
    }

    private void Exit()
    {
        anim.SetBool("active", false);

        CurrentAttackCounter++;
       

        OnExit?.Invoke();
    }
private void Awake()
    {
            anim = baseGameObject.GetComponent<Animator>();

           

    }
}

