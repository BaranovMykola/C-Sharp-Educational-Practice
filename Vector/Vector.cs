using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vector
{
    class Vector<T> : IEnumerable<T>
    {
        T[] arr;
        public Vector()
        {
            arr = null;
        }
        public Vector(IEnumerable<T> collection)
        {
            var arrCollection = collection.ToArray<T>();
            arr = new T[arrCollection.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = arrCollection[i];
            }
        }
        //public Vector(string fileName)
        //{
        //    var lines = File.ReadAllLines(fileName);
        //    arr = new T[lines.Length];
        //}
        public int Find(T element)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if(arr[i].Equals(element))
                {
                    return i;
                }
            }
            return -1;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)arr).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)arr).GetEnumerator();
        }
    }
    class VectorEnumerator<T> : IEnumerator<T>
    {
        T[] arrReference;
        int currentIndex;
        public VectorEnumerator(T[] arr)
        {
            arrReference = arr;
            currentIndex = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        T IEnumerator<T>.Current
        {
            get
            {
                return arrReference[currentIndex];
            }
        }

        bool IEnumerator.MoveNext()
        {
            if(arrReference != null && currentIndex < arrReference.Length)
            {
                ++currentIndex;
                return true;
            }
            return false;
        }

        void IEnumerator.Reset()
        {
            arrReference = null;
            currentIndex = -1;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~VectorEnumerator() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
