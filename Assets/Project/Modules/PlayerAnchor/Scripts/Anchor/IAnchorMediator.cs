namespace Project.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorMediator
    {
        public void CancelChargingThrow();
        public void ResetThrowForce();
        public void IncrementThrowForce(float deltaTime);

        public bool IsBeingThrown();
    }
}