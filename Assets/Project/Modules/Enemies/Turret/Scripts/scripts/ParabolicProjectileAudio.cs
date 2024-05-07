using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;
[CreateAssetMenu(fileName = "ProjectileSoundsConfig",
    menuName = ScriptableObjectsHelper.HAZARDS_ASSET_PATH + "ProjectileSoundsConfig")]
public class ParabolicProjectileAudio : ScriptableObject
{
    [Header("AUDIO MANAGER")]
    [SerializeField] private AFMODAudioManagerReference _audioManager;
    
    [Expandable] [SerializeField] private OneShotFMODSound _projectileCollision;
    [Expandable] [SerializeField] private OneShotFMODSound _turretDamageArea;

    public void PlayAreaDamage(GameObject attachedGameObject)
    {
        PlayOneShotSound(attachedGameObject, _turretDamageArea);
    }
    public void PlayProjectileCollision(GameObject attachedGameObject)
    {
        PlayOneShotSound(attachedGameObject, _projectileCollision);
    }
    private void PlayOneShotSound(GameObject attachedGameObject,
        OneShotFMODSound sound)
    {
        _audioManager.PlayOneShotAttached(sound, attachedGameObject);
    }
}
