using UnityEngine;
using System.Collections;

public class StatusEffectPoison : MonoBehaviour
{
    [SerializeField] int damagePerTick = 2;
    [SerializeField] int tickInterval = 1;
    [SerializeField] int duration = 10;
    [SerializeField] private GameObject poisonEffectPrefab;

    int elapsedTime = 0;
    bool isPoisoned = false;
    Coroutine poisonCoroutine;
    GameObject activeEffect;
    IDamage damageTarget;

    private void Awake()
    {
        damageTarget = GetComponent<IDamage>();
        if (damageTarget == null)
        {
            Debug.LogError("No IDamage component found on this GameObject.");
        }
    }

    public void ApplyPoison()
    {
        if (!isPoisoned)
        {
            isPoisoned = true;
            poisonCoroutine = StartCoroutine(HandlePoison());
        }
    }

    public void CurePoison()
    {
        if (isPoisoned)
        {
            isPoisoned = false;

            if (poisonCoroutine != null)
            {
                StopCoroutine(poisonCoroutine);
                poisonCoroutine = null;
            }

            if (activeEffect != null)
            {
                Destroy(activeEffect);
                activeEffect = null;
            }
        }
    }

    private IEnumerator HandlePoison()
    {
        elapsedTime = 0;

        if (poisonEffectPrefab != null)
        {
            activeEffect = Instantiate(poisonEffectPrefab, transform);
        }

        while (elapsedTime < duration && isPoisoned)
        {
            damageTarget?.takeDamage(damagePerTick);

            yield return new WaitForSeconds(tickInterval);
            elapsedTime += tickInterval;
        }

        CurePoison();
    }
}
