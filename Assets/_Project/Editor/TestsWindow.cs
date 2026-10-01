using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace Thaka.Platformer.Editor
{
    public class TestsWindow : EditorWindow, ICallbacks
    {
        const string TestAssembly = "Thaka.Platformer.Tests.EditMode";

        readonly List<TestResult> results = new List<TestResult>();
        TestRunnerApi api;
        bool running;
        Vector2 scroll;

        struct TestResult
        {
            public string Name;
            public bool Passed;
            public string Message;
        }

        [MenuItem("Thaka/Tests")]
        static void Open()
        {
            GetWindow<TestsWindow>("Tests");
        }

        void OnEnable()
        {
            api = CreateInstance<TestRunnerApi>();
            api.RegisterCallbacks(this);
        }

        void OnDisable()
        {
            api.UnregisterCallbacks(this);
            DestroyImmediate(api);
        }

        void OnGUI()
        {
            using (new EditorGUI.DisabledScope(running))
            {
                if (GUILayout.Button(running ? "Running..." : "Run All Tests", GUILayout.Height(30)))
                    RunTests();
            }

            if (results.Count == 0)
                return;

            var failed = results.FindAll(r => !r.Passed).Count;
            EditorGUILayout.HelpBox(
                failed == 0 ? $"All {results.Count} tests passed" : $"{failed} of {results.Count} tests failed",
                failed == 0 ? MessageType.Info : MessageType.Error);

            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var result in results)
            {
                var icon = EditorGUIUtility.IconContent(result.Passed ? "TestPassed" : "TestFailed").image;
                EditorGUILayout.LabelField(new GUIContent(" " + result.Name, icon));
                if (!result.Passed)
                    EditorGUILayout.HelpBox(result.Message, MessageType.None);
            }
            EditorGUILayout.EndScrollView();
        }

        void RunTests()
        {
            results.Clear();
            running = true;
            var filter = new Filter { testMode = TestMode.EditMode, assemblyNames = new[] { TestAssembly } };
            api.Execute(new ExecutionSettings(filter));
        }

        public void RunStarted(ITestAdaptor testsToRun)
        {
        }

        public void RunFinished(ITestResultAdaptor result)
        {
            running = false;
            Repaint();
        }

        public void TestStarted(ITestAdaptor test)
        {
        }

        public void TestFinished(ITestResultAdaptor result)
        {
            if (result.Test.IsSuite)
                return;

            results.Add(new TestResult
            {
                Name = result.Test.Parent.Name + " / " + result.Test.Name,
                Passed = result.TestStatus == TestStatus.Passed,
                Message = result.Message
            });
            Repaint();
        }
    }
}
