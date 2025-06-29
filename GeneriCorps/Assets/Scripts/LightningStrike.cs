using UnityEngine;

public class LightningStrike : MonoBehaviour

{
    [SerializeField] Transform caster;
    [SerializeField] Transform target;
    [SerializeField] LineRenderer line;
    [SerializeField] float damage = 25f;
    [SerializeField] float duration = 0.5f;
    [SerializeField] ParticleSystem lightningEffect;


    public float coolDownTime = 4.0f;
    float coolDown;
    private float timer;

    void OnEnable()
    {
        if (lightningEffect != null)
        {
            lightningEffect.Play();
        }

        timer = duration;
        UpdateLine();

        if(coolDown >= 0)
        {
            coolDown -= Time.deltaTime;
        }


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
            if (lightningEffect != null && lightningEffect.isPlaying)
            {
                lightningEffect.Stop();
            }

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

