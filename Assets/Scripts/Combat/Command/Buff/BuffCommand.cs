using System.Threading;
using Combat.Buffs;

namespace Combat.Command.Buff
{

    /// <summary>
    /// 施加buff的命令，需要指定目标和buff
    /// </summary>
    /// <typeparam name="T">要施加的buff的类型</typeparam>
    public class ApplyBuffCommand<T> : ICommand where T : IBuff
    {
        public ApplyBuffCommand(Character source, Character target, T buff, int count = 1)
        {
            Source = source;
            Target = target;
            Buff = buff;
            Count = count;
        }

        public Character Source { get; set; }

        public Character Target { get; set; }

        public T Buff;

        public int Count;

        /// <summary>
        /// 强制不施加buff，用于需要抵消buff的场景
        /// </summary>
        public bool ForceUnaviable;

        public void Execute()
        {
            if (ForceUnaviable || !Buff.IsAvaliable(Count))
            {
                return;
            }
            Target._ApplyBuff<T>(Buff, Count);
        }
    }

    /// <summary>
    /// 更新buff的层数的命令，需要指定目标，不需要指定buff，但是目标需要有这个buff
    /// 因此只推荐buff本身更新自己的时候，使用该命令。
    /// </summary>
    /// <typeparam name="T">要更新的buff类型</typeparam>
    public class UpdateBuffCountCommand<T> : ICommand where T : IBuff
    {
        public UpdateBuffCountCommand(Character source, Character target, int count = 1)
        {
            Source = source;
            Target = target;
            Count = count;
        }

        public Character Source { get; set; }

        public Character Target { get; set; }

        public int Count { get; set; }

        public void Execute()
        {
            Target._UpdateBuffCount<T>(Count);
        }
    }

    /// <summary>
    /// 移除buff的命令，需要指定目标，需要目标有这个buff
    /// </summary>
    /// <typeparam name="T">要移除的buff类型</typeparam>
    public class RemoveBuffCommand<T> : ICommand where T : IBuff
    {
        public RemoveBuffCommand(Character source, Character target)
        {
            Source = source;
            Target = target;
        }

        public Character Source { get; set; }

        public Character Target { get; set; }

        public void Execute()
        {
            Target._RemoveBuff<T>();
        }
    }
}