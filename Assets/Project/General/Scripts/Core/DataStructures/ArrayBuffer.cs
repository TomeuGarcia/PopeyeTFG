using System;

namespace Project.Scripts.Core.DataStructures
{
    public class ArrayBuffer<T>
    {
        private T[] _elements;
        public T[] Elements => _elements;
        public int Size { get; private set; }
        

        public ArrayBuffer(int capacity)
        {
            _elements = new T[capacity];
            FullClear();

            Size = 0;
        }

        private void FullClear()
        {
            for (int i = 0; i < _elements.Length; ++i)
            {
                _elements[i] = default;
            }
        }
        private void SizeClear()
        {
            for (int i = 0; i < Size; ++i)
            {
                _elements[i] = default;
            }
        }
        
        public void ClearAndResize(int size)
        {
            SizeClear();
            Size = size;
        }


        public T[] ToArray()
        {
            T[] copy = new T[Size];
            Array.Copy(_elements, copy, Size);
            return copy;
        }
        
    }
}