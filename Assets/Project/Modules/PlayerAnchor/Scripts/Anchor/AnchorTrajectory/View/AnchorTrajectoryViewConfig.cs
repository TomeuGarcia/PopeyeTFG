using NaughtyAttributes;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [System.Serializable]
    public class AnchorTrajectoryViewConfig
    {
        [SerializeField] private Material _trajectoryMaterial;
        [SerializeField, Range(0f, 1f)] private float _widthMultiplier = 1f;
        [SerializeField] private AnimationCurve _widthCurve = AnimationCurve.EaseInOut(0f, 1.0f, 1f, 0.5f);

        [SerializeField] private LineAlignment _alignment = LineAlignment.TransformZ;
        [SerializeField] private LineTextureMode _textureMode = LineTextureMode.Stretch;

        public Material TrajectoryMaterial => _trajectoryMaterial;
        public float WidthMultiplier => _widthMultiplier;
        public AnimationCurve WidthCurve => _widthCurve;
        public LineAlignment Alignment => _alignment;
        public LineTextureMode TextureMode => _textureMode;
    }
}