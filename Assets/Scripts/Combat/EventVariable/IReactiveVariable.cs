namespace Combat.EventVariable
{
    public interface IReactiveVariable<T>
    {
        T Value { get; set; }
        string Name { get; }
        object Parent { get; }

        /// <summary>
        /// 仅用于ScriptableObject的父物体设置
        /// </summary>
        /// <param name="parent"></param>
        void SetParent(object parent);
    }
}