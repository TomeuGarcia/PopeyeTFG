namespace Popeye.Core.Services.InformationDisplay
{
    public class InformationDisplayService : IInformationDisplayService
    {
        public ITextDisplayer TextDisplayer { get; private set; }
        public IVideoDisplayer VideoDisplayer { get; private set; }

        public InformationDisplayService(ITextDisplayer textDisplayer, IVideoDisplayer videoDisplayer)
        {
            TextDisplayer = textDisplayer;
            VideoDisplayer = videoDisplayer;
        }
        
    }
}