
using Project.Scripts.TweenExtensions;

namespace Project.General.Scripts.Core.Services.ScreenFade
{
    public interface IScreenFadeService
    {
        public void QueueFadeIn();
        public void QueueFadeOut();
        
        public void QueueFade(TweenFadeConfig fade);
    }
}