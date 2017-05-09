/**********************************************************
// Author   : Arkai (k79k06k02k)
// FileName : SerializableDictionary.cs
**********************************************************/
using System.Collections.Generic;
using System.Linq;

namespace ClientDataBase
{
    [System.Serializable()]
    public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, UnityEngine.ISerializationCallbackReceiver
    {

        #region Fields

        [System.NonSerialized()]
        private Dictionary<TKey, TValue> m_dict;

        #endregion

        #region IDictionary Interface

        public int Count
        {
            get { return (m_dict != null) ? m_dict.Count : 0; }
        }

        public void Add(TKey key, TValue value)
        {
            if (m_dict == null) m_dict = new Dictionary<TKey, TValue>();
            m_dict.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            if (m_dict == null) return false;
            return m_dict.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get
            {
                if (m_dict == null) m_dict = new Dictionary<TKey, TValue>();
                return m_dict.Keys;
            }
        }

        public bool Remove(TKey key)
        {
            if (m_dict == null) return false;
            return m_dict.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (m_dict == null)
            {
                value = default(TValue);
                return false;
            }
            return m_dict.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get
            {
                if (m_dict == null) m_dict = new Dictionary<TKey, TValue>();
                return m_dict.Values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (m_dict == null) throw new KeyNotFoundException();
                return m_dict[key];
            }
            set
            {
                if (m_dict == null) m_dict = new Dictionary<TKey, TValue>();
                m_dict[key] = value;
            }
        }

        public void Clear()
        {
            if (m_dict != null) m_dict.Clear();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            if (m_dict == null) m_dict = new Dictionary<TKey, TValue>();
            (m_dict as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (m_dict == null) return false;
            return (m_dict as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (m_dict == null) return;
            (m_dict as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (m_dict == null) return false;
            return (m_dict as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            if (m_dict == null) return default(Dictionary<TKey, TValue>.Enumerator);
            return m_dict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            if (m_dict == null) return Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
            return m_dict.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            if (m_dict == null) return Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
            return m_dict.GetEnumerator();
        }

        #endregion

        #region ISerializationCallbackReceiver

        [UnityEngine.SerializeField()]
        private TKey[] m_keys;
        [UnityEngine.SerializeField()]
        private TValue[] m_values;

        void UnityEngine.ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (m_keys != null && m_values != null)
            {
                if (m_dict == null) m_dict = new Dictionary<TKey, TValue>(m_keys.Length);
                else m_dict.Clear();
                for (int i = 0; i < m_keys.Length; i++)
                {
                    if (i < m_values.Length)
                        m_dict[m_keys[i]] = m_values[i];
                    else
                        m_dict[m_keys[i]] = default(TValue);
                }
            }

            m_keys = null;
            m_values = null;
        }

        void UnityEngine.ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (m_dict == null || m_dict.Count == 0)
            {
                m_keys = null;
                m_values = null;
            }
            else
            {
                int cnt = m_dict.Count;
                m_keys = new TKey[cnt];
                m_values = new TValue[cnt];
                int i = 0;
                var e = m_dict.GetEnumerator();
                while (e.MoveNext())
                {
                    m_keys[i] = e.Current.Key;
                    m_values[i] = e.Current.Value;
                    i++;
                }
            }
        }

        #endregion

    }
}