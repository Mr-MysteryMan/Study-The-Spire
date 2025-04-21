namespace Combat.Trigger {
    using Combat.Command;

    // 护甲破碎的触发器，用于处理护甲破碎事件
    public class AmmorBrokenTrigger : ITrigger<AttackCommand> {
        public System.Type CommandType => typeof(AttackCommand);

        public int Priority => 20;

        public int TimeStamp => 0;

        private bool _isAmmorBroken = false;

        public void PreCheck(EventManager manager, AttackCommand command) {
            _isAmmorBroken = false;
            // 仅当目标有护甲且护甲伤害超过目标护甲值，才会触发护甲破碎事件
            if (command.AmmorDamage > 0 && command.AmmorDamage > command.Target.Ammor && command.HPDamage > 0) {
                _isAmmorBroken = true;
            }
        }

        public void PostCheck(EventManager manager, AttackCommand command) {
            if (_isAmmorBroken) {
                var eventArgs = new Events.AmmorBrokenEvent(command.AmmorDamage, command.HPDamage, command.Target, command.Source);
                manager.Publish(eventArgs);
                _isAmmorBroken = false;
            }
        }
    }
}