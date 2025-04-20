public class CombatSystem {

    public DamagePipeline damagePipeline = new DamagePipeline(); // 伤害处理管道

    public void ProcessAttack(AttackCommand command) {
        damagePipeline.Process(ref command); // 处理伤害数据

        bool isAmmorBroken = false; // 是否护甲破碎
        if (command.AmmorDamage >= command.Target.CurAmmor && command.HPDamage > 0) {
            isAmmorBroken = true;
        }

        command.Target.TakeAmmorDamage(command.AmmorDamage);
        command.Target.TakeHPDamage(command.HPDamage); // 处理生命值伤害


        if (isAmmorBroken) {
            EventManager.instance.Publish(new AmmorBrokenEvent(command.AmmorDamage, command.HPDamage, command.Attacker, command.Target)); // 发布护甲破碎事件
        }
        EventManager.instance.Publish(new DamageDealtEvent(command.FinalDamage, command.AmmorDamage, command.HPDamage, command.Type, command.Attacker, command.Target)); // 发布伤害事件

        if (command.Target.CurHP <= 0) {
            EventManager.instance.Publish(new CharacterDeathEvent(command.Target, command.Attacker)); // 发布角色死亡事件
        }
    }
}