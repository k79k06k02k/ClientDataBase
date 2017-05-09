/**********************************************************
// Author   : Arkai (k79k06k02k)
// FileName : Singleton.cs
// Reference: http://wiki.unity3d.com/index.php/Singleton
**********************************************************/

namespace ClientDataBase
{
    public class Singleton<T> where T : class, new()
    {
        private static T m_instance;

        private static object m_lock = new object();

        public static T Instance
        {
            get
            {
                //多執行緒
                lock (m_lock)
                {
                    if (m_instance == null)
                    {
                        m_instance = new T();
                    }

                    return m_instance;
                }
            }
        }
    }
}