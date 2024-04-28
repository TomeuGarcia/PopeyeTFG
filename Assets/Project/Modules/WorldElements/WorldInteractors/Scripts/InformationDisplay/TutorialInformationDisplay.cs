using AYellowpaper;
using Popeye.Core.Services.InformationDisplay;
using Popeye.Core.Services.ServiceLocator;


namespace Popeye.Modules.WorldElements.WorldInteractors
{
    public class TutorialInformationDisplay : AWorldInteractor
    {
        private ITutorialDisplayCondition _stopDisplayingCondition;
        private TextDisplayConfig _informationToDisplay;
        
        private IInformationDisplayService _informationDisplayService;

        protected override void DoAwake() { }

        public void Configure(TextDisplayConfig informationToDisplay, ITutorialDisplayCondition stopDisplayingCondition)
        {
            _informationToDisplay = informationToDisplay;
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
            _informationDisplayService.TextDisplayer.StartShowing(_informationToDisplay);            
            _stopDisplayingCondition.StartChecking(StopShowing);
        }
        private void StopShowing()
        {
            _informationDisplayService.TextDisplayer.StopShowing(_informationToDisplay);
        }

    }
}