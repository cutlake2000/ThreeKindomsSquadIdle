using System;
using UnityEngine;

namespace Module
{
    public static class InfiniteLoopDetector
    {
        private static string _prevPoint = "";
        private static int _detectionCount;
        private const int DetectionThreshold = 100000;

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Run(
            [System.Runtime.CompilerServices.CallerMemberName] string mn = "",
            [System.Runtime.CompilerServices.CallerFilePath] string fp = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int ln = 0
        )
        {
            string currentPoint = $"{fp}:{ln}, {mn}()";

            if (_prevPoint == currentPoint)
                _detectionCount++;
            else
                _detectionCount = 0;

            if (_detectionCount > DetectionThreshold)
                throw new Exception($"Infinite Loop Detected: \n{currentPoint}\n\n");

            _prevPoint = currentPoint;
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void Init()
        {
            UnityEditor.EditorApplication.update += () =>
            {
                _detectionCount = 0;
            };
        }
#endif
    }
}