using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShot : MonoBehaviour
{
    public FMODUnity.EventReference sound;

    private void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(sound, gameObject);
    }
}
