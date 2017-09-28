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