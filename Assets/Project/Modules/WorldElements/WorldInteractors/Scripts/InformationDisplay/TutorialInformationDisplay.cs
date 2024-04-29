using AYellowpaper;
using Popeye.Core.Services.InformationDisplay;
using Popeye.Core.Services.ServiceLocator;


namespace Popeye.Modules.WorldElements.WorldInteractors
{
    public class TutorialInformationDisplay : AWorldInteractor
    {
        private TextDisplayConfig _textToDisplay;
        private VideoDisplayConfig _videoToDisplay;
        private ITutorialDisplayCondition _stopDisplayingCondition;
        
        private IInformationDisplayService _informationDisplayService;

        private bool HasVideoToDisplay => _videoToDisplay != null;

        protected override void DoAwake() { }

        public void Configure(
            TextDisplayConfig textToDisplay, 
            VideoDisplayConfig videoToDisplay, 
            ITutorialDisplayCondition stopDisplayingCondition)
        {
            _textToDisplay = textToDisplay;
            _videoToDisplay = videoToDisplay;
            _stopDisplayingCondition = stopDisplayingCondition;
            
            _informationDisplayService = ServiceLocator.Instance.GetService<IInformationDisplayService>();            
        }

        private void OnDestroy()
        {
            _stopDisplayingCondition.FinishChecking();
        }

        protected override void DoEnterActivatedState()
        {
            StartShowing();
        }

        protected override void DoEnterDeactivatedState()
        {
            _stopDisplayingCondition.FinishChecking();
        }

        private void StartShowing()
        {
            _informationDisplayService.TextDisplayer.StartShowing(_textToDisplay);
            if (HasVideoToDisplay)
            {
                _informationDisplayService.VideoDisplayer.StartShowing(_videoToDisplay); 
            }

            _stopDisplayingCondition.StartChecking(StopShowing);
        }
        private void StopShowing()
        {
            _informationDisplayService.TextDisplayer.StopShowing(_textToDisplay);
            if (HasVideoToDisplay)
            {
                _informationDisplayService.VideoDisplayer.StopShowing(_videoToDisplay); 
            }
        }

    }
}