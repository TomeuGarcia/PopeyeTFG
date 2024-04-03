namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IThrowDistanceComputer
    {
        float ComputeThrowDistance(float throwForce01);
        void ClearState();
    }
}