namespace Popeye.Scripts.ValueGating
{
    public class ValueGate<T> : IGateToggle, IGateValueReader<T>
    {
        private readonly T _openValue;
        private readonly T _closedValue;
        private T _currentValue;
        
        public ValueGate(T openValue, T closedValue, bool startOpen)
        {
            _openValue = openValue;
            _closedValue = closedValue;

            if (startOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        public void Open()
        {
            _currentValue = _openValue;
        }

        public void Close()
        {
            _currentValue = _closedValue;
        }

        public T GetValue()
        {
            return _currentValue;
        }
    }
}