using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.InstantTranslation
{
    public interface IPlayerInstantTranslation
    {
        void TranslatePlayer(Vector3 position, Quaternion rotation);
        void TranslatePlayerAndPauseForAFrame(Vector3 position, Quaternion rotation);
        void TranslateAnchorAndReset(Vector3 position, Quaternion rotation);
    }
}