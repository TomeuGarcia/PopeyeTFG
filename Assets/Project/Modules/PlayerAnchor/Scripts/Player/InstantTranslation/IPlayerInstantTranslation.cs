using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.InstantTranslation
{
    public interface IPlayerInstantTranslation
    {
        void TranslatePlayer(Vector3 position, Quaternion rotation);
        void TranslateAnchor(Vector3 position, Quaternion rotation);
    }
}