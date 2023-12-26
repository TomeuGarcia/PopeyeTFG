
namespace Project.Modules.PlayerAnchor.Chain
{
    public interface IChainPhysics
    {
        public void Configure(ChainConfig chainConfig);
        public void SetFailedThrow(bool failedThrow);
        public void EnableTension();
        public void DisableTension();
    }
}