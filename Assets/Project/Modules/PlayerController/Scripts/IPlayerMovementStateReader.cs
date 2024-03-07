using UnityEngine;

namespace Popeye.Modules.PlayerController
{
    public interface IPlayerMovementStateReader
    {
        Vector3 MovementDirection { get; }
        Vector3 LookDirection { get; }
        float CurrentSpeedXZ { get; }
        float CurrentSpeedXZRatio01 { get; }
    }
}