using System;
using System.Threading;
using Combat.Buffs;

namespace Combat.Command.Buff
{

    /// <summary>
    /// 施加buff的命令，需要指定目标和buff
    /// </summary>
    /// <typeparam name="T">要施加的buff的类型</typeparam>
    public class ApplyBuffCommand : ICommand
    {
        public ApplyBuffCommand(Character source, Character target, IBuff buff, int count = 1)
        {
            Source = source;
            Target = target;
            Buff = buff;
            Count = count;
        }

        public Character Source { get; set; }

        public Character Target { get; set; }

        public IBuff Buff;

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
            Target._ApplyBuff(Buff, Count);
        }
    }

    /// <summary>
    /// 更新buff的层数的命令，需要指定目标，不需要指定buff，但是目标需要有这个buff
    /// 因此只推荐buff本身更新自己的时候，使用该命令。
    /// </summary>
    public class UpdateBuffCountCommand : ICommand
    {
        public UpdateBuffCountCommand(Character source, Character target, Type type, int count = 1)
        {
            Source = source;
            Target = target;
            Count = count;
            BuffType = type;
        }

        public Character Source { get; set; }

        public Character Target { get; set; }

        public Type BuffType { get; set; }

        public int Count { get; set; }

        public void Execute()
        {
            Target._UpdateBuffCount(BuffType, Count);
        }
    }

    /// <summary>
    /// 移除buff的命令，需要指定目标，需要目标有这个buff
    /// </summary>
    public class RemoveBuffCommand : ICommand
    {
        public RemoveBuffCommand(Character source, Character target, Type type)
        {
            Source = source;
            Target = target;
            BuffType = type;
        }
        public Character Source { get; set; }

        public Character Target { get; set; }

        public Type BuffType { get; set; }

        public void Execute()
        {
            Target._RemoveBuff(BuffType);
        }
    }
}