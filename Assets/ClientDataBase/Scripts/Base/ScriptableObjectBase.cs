namespace ClientDataBase
{
    public abstract class ScriptableObjectBase : UnityEngine.ScriptableObject
    {
        public abstract bool LoadGameTable();
        public abstract TableClassBase GetDataBase(int id);
    }
}

