using UnityEngine;
using System.Collections;

public class StatusEffectFreeze : MonoBehaviour
{
    [SerializeField] int freezeDuration = 5;
    [SerializeField] GameObject freezeEffectPrefab;

    IMovement movementTarget;
    IAction actionTarget;
    GameObject activeEffect;

    private void Awake()
    {
        movementTarget = GetComponent<IMovement>();
        actionTarget = GetComponent<IAction>();
    }

    public void ApplyFreeze()
    {
        StartCoroutine(HandleFreeze());
    }

    private IEnumerator HandleFreeze()
    {
        if (freezeEffectPrefab != null)
            activeEffect = Instantiate(freezeEffectPrefab, transform);

        movementTarget?.DisableMovement();
        actionTarget?.DisableActions();

        yield return new WaitForSeconds(freezeDuration);

        movementTarget?.EnableMovement();
        actionTarget?.EnableActions();

        if (activeEffect != null) Destroy(activeEffect);
    }
}