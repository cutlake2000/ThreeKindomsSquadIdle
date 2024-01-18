using Creature.CreatureFSM;
using Creature.SquadScripts.SquadClass;
using Creature.SquadScripts.SquadFSM.States;

namespace Creature.SquadScripts.SquadFSM
{
    public class SquadStateMachine : CreatureStateMachine
    {
        public SquadStateMachine(Squad squad)
        {
            Squad = squad;

            SquadIdleState = new SquadIdleState(this);
            SquadRunState = new SquadRunState(this);
            SquadAttackState = new SquadAttackState(this);
            SquadDieState = new SquadDieState(this);
            SquadNormalAttackState = new SquadNormalAttackState(this);
            SquadSkillAttackState = new SquadSkillAttackState(this);
        }

        public Squad Squad { get; }

        public SquadIdleState SquadIdleState { get; }
        public SquadRunState SquadRunState { get; }
        public SquadAttackState SquadAttackState { get; }
        public SquadDieState SquadDieState { get; }

        public SquadNormalAttackState SquadNormalAttackState { get; }
        public SquadSkillAttackState SquadSkillAttackState { get; }
    }
}