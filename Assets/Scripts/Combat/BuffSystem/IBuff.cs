namespace Combat.Buffs
{
    public enum BuffType
    {
        Normal,
        Debuff,
        Buff
    }


    public interface IBuff
    {
        string Name { get; }
        string Description { get; }

        int Count { get; }

        BuffType BuffType { get; }

        Character Parent { get; }


        // TODO: 添加几个对Apply和Update的Trigger，检测变化后的IsAvaliable，如果不满足条件，则执行RemoveCommand。

        /// <summary>
        /// 判断是否可用，在OnApply之前调用（此时内部的Count可能不可用（如ReactiveIntVariable，需要在apply的时候初始化））
        /// 可用于命令传递中，在管道中修改buff的属性或命令的Count, 使之无效。
        /// </summary>
        /// <param name="count">在OnApply之前，Count可能不可用，使用参数代替count</param>
        /// <returns>返回Buff是否可用，如果不可用，则不施加</returns>
        bool IsAvaliable(int count);

        /// <summary>
        /// 判断是否可用，在OnApply之后调用。
        /// </summary>
        /// <returns>返回Buff是否可用，如果不可用，则可能触发相应的移除事件</returns>
        bool IsAvaliable();

        void OnApply(Character target, int count = 1);

        void OnRemove();

        /// <summary>
        /// 更新buff的数量
        /// </summary>
        /// <param name="count">表示更新后buff的层数</param>
        void OnUpdate(int count);

        /// <summary>
        /// 更新buff，传入的buff可能与当前buff有差异，通过这个函数解决两者冲突
        /// 如果有复杂的需求，还是先remove再apply吧
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="count"></param>
        void OnUpdate(IBuff buff, int count);
    }

    public class BuffConstants
    {
        public static readonly string ReactiveVariableName = "BuffCount";
    }
}