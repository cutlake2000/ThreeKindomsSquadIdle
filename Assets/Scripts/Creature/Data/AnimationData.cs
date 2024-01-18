using System;
using UnityEngine;

namespace Creature.Data
{
    [Serializable]
    public class AnimationData
    {
        [SerializeField] private string runParameterName = "Run";
        [SerializeField] private string attackParameterName = "Attack";
        [SerializeField] private string dieParameterName = "Die";
        [SerializeField] private string attackSpeedParameterName = "AttackSpeed";
        [SerializeField] private string runStateParameterName = "RunState";
        [SerializeField] private string classTypeParameterName = "ClassType";
        [SerializeField] private string attackStateParameterName = "AttackState";
        [SerializeField] private string editChkParameterName = "EditChk";
        
        public int RunParameterHash { get; private set; }
        public int AttackParameterHash { get; private set; }
        public int DieParameterHash { get; private set; }
        public int AttackSpeedParameterHash { get; private set; }
        public int RunStateParameterHash { get; private set; }
        public int ClassTypeParameterHash { get; private set; }
        public int AttackStateParameterHash { get; private set; }
        public int EditChkParameterHash { get; private set; }

        public void InitializeData()
        {
            RunParameterHash = Animator.StringToHash(runParameterName);
            AttackParameterHash = Animator.StringToHash(attackParameterName);
            DieParameterHash = Animator.StringToHash(dieParameterName);
            AttackSpeedParameterHash = Animator.StringToHash(attackSpeedParameterName);
            RunStateParameterHash = Animator.StringToHash(runStateParameterName);
            ClassTypeParameterHash = Animator.StringToHash(classTypeParameterName);
            AttackStateParameterHash = Animator.StringToHash(attackStateParameterName);
            EditChkParameterHash = Animator.StringToHash(editChkParameterName);
        }
    }
}