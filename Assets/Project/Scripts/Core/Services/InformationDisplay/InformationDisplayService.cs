namespace Popeye.Core.Services.InformationDisplay
{
    public class InformationDisplayService : IInformationDisplayService
    {
        public ITextDisplayer TextDisplayer { get; private set; }
        
        public InformationDisplayService(ITextDisplayer _textDisplayer)
        {
            TextDisplayer = _textDisplayer;
        }
        
    }
}