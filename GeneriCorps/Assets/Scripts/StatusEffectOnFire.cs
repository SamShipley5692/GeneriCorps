using UnityEngine;
using System.Collections;

public class StatusEffectOnFire : MonoBehaviour
{
    [SerializeField] int damagePerTick = 5;
    [SerializeField] int tickInterval = 1;
    [SerializeField] int duration = 5;
    [SerializeField] private GameObject fireEffectPrefab;

    int elapsedTime = 0;
    bool isBurning = false;
    Coroutine fireCoroutine;
    GameObject activeEffect;
    IDamage damageTarget;


    private void Awake()
    {
        damageTarget = GetComponent<IDamage>();
    }

    public void Ignite()
    {
        if (!isBurning)
        {
            isBurning = true;
            fireCoroutine = StartCoroutine(HandleFire());
        }
    }

    public void Extinguish()
    {
        if (isBurning)
        {
            isBurning = false;
            if (fireCoroutine != null) StopCoroutine(fireCoroutine);
            if (activeEffect != null) Destroy(activeEffect);
        }
    }

    private IEnumerator HandleFire()
    {
        elapsedTime = 0;

        if (fireEffectPrefab != null)
            activeEffect = Instantiate(fireEffectPrefab, transform);

        while (elapsedTime < duration && isBurning)
        {
            damageTarget?.takeDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
            elapsedTime += tickInterval;
        }

        Extinguish();
    }
}

