using System;
using UnityEngine;

namespace Managers
{
    public class SquadConfigureManager : MonoBehaviour
    {
        public static SquadConfigureManager Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}