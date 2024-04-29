namespace Popeye.Core.Services.InformationDisplay
{
    public interface IInformationDisplayService
    {
        ITextDisplayer TextDisplayer { get; }
        IVideoDisplayer VideoDisplayer { get; }
    }
}