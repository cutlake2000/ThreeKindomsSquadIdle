using Creature.CreatureClass.MonsterClass;
using Creature.CreatureClass.MonsterFSM.States;
using Creature.CreatureFSM;

namespace Creature.CreatureClass.MonsterFSM
{
    public class MonsterStateMachine : CreatureStateMachine
    {
        public MonsterStateMachine(MonsterNew monsterNew)
        {
            MonsterNew = monsterNew;
            
            MonsterIdleState = new MonsterIdleState(this);
            MonsterRunState = new MonsterRunState(this);
            MonsterAttackState = new MonsterAttackState(this);
            MonsterDieState = new MonsterDieState(this);
            MonsterNormalAttackState = new MonsterNormalAttackState(this);
            MonsterSkillAttackState = new MonsterSkillAttackState(this);
        }

        public MonsterNew MonsterNew { get; }

        public MonsterIdleState MonsterIdleState { get; }
        public MonsterRunState MonsterRunState { get; }
        public MonsterAttackState MonsterAttackState { get; }
        public MonsterDieState MonsterDieState { get; }

        public MonsterNormalAttackState MonsterNormalAttackState { get; }
        public MonsterSkillAttackState MonsterSkillAttackState { get; }
    }
}