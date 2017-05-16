using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOSystem
{
    [Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        public override string ToString() => $"{Key} {Value}";
    }
}
