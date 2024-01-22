using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
   public class PlayOneShot : MonoBehaviour
   {
       public FMODUnity.EventReference sound;
   
       private void Start()
       {
           FMODUnity.RuntimeManager.PlayOneShotAttached(sound, gameObject);
       }
   }
 
}

