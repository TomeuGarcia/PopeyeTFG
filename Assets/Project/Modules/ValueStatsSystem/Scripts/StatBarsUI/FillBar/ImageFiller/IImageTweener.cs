using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public interface IImageTweener
    {
        float FillValue { get; }
        
        void SetFillValue(float value01);
        void ToFillValue(float value01, float duration, Ease ease);
        
        void SetColor(Color color);
        void PunchColor(Color toColor, Color backColor, float duration);
    }
}