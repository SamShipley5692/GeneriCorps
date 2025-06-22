using UnityEngine;

public class PlayerBluff : MonoBehaviour
{

    public ParticleSystem speedBuffParticles;
    public ParticleSystem strengthBuffParticles;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void ActivateBuff(PotionType potion)
   // {
        //case PotionType.Speed:
         //   speedBuffParticles.Play();
        //    break;
      //  case PotionType.Strength:
         //   strengthBuffParticles.Play();
         //   break;
       // }
   // }

    //public void DeactivateBuff(PotionType potion)
  //  {
       // case PotionType.Speed:
            //speedBuffParticles.Stop();
            
           // break;
        //case PotionType.Strength:
           // strengthBuffParticles.Stop();
          //  break;
       // }
   // }

    public enum PotionType
    {
        Speed,
        Strength,
    }





}
