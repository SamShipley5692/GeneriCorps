using UnityEngine;

public class LightningStrike : MonoBehaviour

{
    [SerializeField] Transform caster;
    [SerializeField] Transform target;
    [SerializeField] LineRenderer line;
    [SerializeField] float damage = 25f;
    [SerializeField] float duration = 0.5f;

    private float timer;

    void OnEnable()
    {
        timer = duration;
        UpdateLine();


        IDamage dmg = target.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(Mathf.RoundToInt(damage));
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            gameObject.SetActive(false);
        }
        else
        {
            UpdateLine();
        }
    }

    void UpdateLine()
    {
        if (line != null && caster != null && target != null)
        {
            line.SetPosition(0, caster.position);
            line.SetPosition(1, target.position);
        }
    }
}
