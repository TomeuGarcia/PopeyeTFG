using UnityEngine;

namespace Popeye.Core.Services.InformationDisplay
{
    public class NullImageTextDisplayViewEffects : MonoBehaviour, ITextDisplayViewEffects
    {
        public void UpdateView(TextDisplaySettings textDisplaySettings) { }
    }
}