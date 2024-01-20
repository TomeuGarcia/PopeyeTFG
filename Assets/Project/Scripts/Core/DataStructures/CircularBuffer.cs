namespace Project.Scripts.Core.DataStructures
{
    public class CircularBuffer<T>
    {
        private readonly T[] _elements;
        private int _tail;
        private int _head;

        public int Length => _elements.Length;

        public CircularBuffer(int size)
        {
            _elements = new T[size];
            _tail = _head = 0;
        }

        public void AddNext(T element)
        {
            _elements[_tail] = element;
            _tail = (_tail + 1) % Length;
        }

        public T GetNext()
        {
            T element = _elements[_head];
            _head = (_head + 1) % Length;
            
            return element;
        }

        public bool HasEnqueuedElements()
        {
            return _head != _tail;
        }
        public bool IsFull()
        {
            return (_tail + 1) % Length == _head;
        }
    }
}