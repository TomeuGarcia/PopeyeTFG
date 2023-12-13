using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMOD : MonoBehaviour
{
    [field: Header("SFX_Main_Character_mov")]
    [field: SerializeField] public EventReference SFX_Footsteps_rock_soft { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events script in the Scene");
        }
        instance = this;
    }   
}
