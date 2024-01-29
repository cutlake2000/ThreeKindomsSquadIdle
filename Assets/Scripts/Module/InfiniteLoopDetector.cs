using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEditor;

namespace Module
{
    public static class InfiniteLoopDetector
    {
        private const int DetectionThreshold = 100000;
        private static string _prevPoint = "";
        private static int _detectionCount;

        [Conditional("UNITY_EDITOR")]
        public static void Run(
            [CallerMemberName] string mn = "",
            [CallerFilePath] string fp = "",
            [CallerLineNumber] int ln = 0
        )
        {
            var currentPoint = $"{fp}:{ln}, {mn}()";

            if (_prevPoint == currentPoint)
                _detectionCount++;
            else
                _detectionCount = 0;

            if (_detectionCount > DetectionThreshold)
                throw new Exception($"Infinite Loop Detected: \n{currentPoint}\n\n");

            _prevPoint = currentPoint;
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void Init()
        {
            EditorApplication.update += () => { _detectionCount = 0; };
        }
#endif
    }
}