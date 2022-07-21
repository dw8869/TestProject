using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Text;
using System;
using System.Diagnostics;
using System.Threading;

public class LocalServerWindow : EditorWindow
{
    static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;

        static ToolbarStyles()
        {
            commandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold
            };
        }
    }

    private ScrollView scrollView;
    private TextField logLabel; 
    private StringBuilder logStringBuilder = new StringBuilder();

    [MenuItem("LocalServer/Console")]
    private static void Initialize()
    {
        LocalServerWindow window = (LocalServerWindow)EditorWindow.GetWindow(typeof(LocalServerWindow));
        window.titleContent = new GUIContent("Á¦¸ñ");
    }

    static void OnToolbarLeftGUI()
    {
        GUILayout.FlexibleSpace();
        GUILayout.Button("asd");
    }

    private void CreateGUI()
    {
        // Toolbar
        var toolbar = new Toolbar();
        toolbar.Add(new ToolbarButton(OnClickClearButton) { text = "Clear" });
        toolbar.Add(new ToolbarButton { text = "World.multiplay" });
        toolbar.Add(new ToolbarSpacer { flex = true });
        toolbar.Add(new Label { text = "Process Id: 58861" });
        toolbar.Add(new ToolbarButton { text = "Stop" });

        rootVisualElement.Add(toolbar);

        // Log
        scrollView = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
        rootVisualElement.Add(scrollView);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Scripts/NewUSSFile.uss");

        logLabel = new TextField();
        logLabel.styleSheets.Add(styleSheet);
        logLabel.AddToClassList("noBordersTextField");
        logLabel.isReadOnly = true;

        scrollView.Add(logLabel);
    }

    private void OnClickClearButton()
    {
        logStringBuilder.Clear();
        StartS();
    }

    private void CheckScrollToEnd ()
    {
        scrollView.verticalScroller.value = logLabel.layout.height;
    }

    private void OnInspectorUpdate()
    {

        logLabel.value = logStringBuilder.ToString();
    }

    private void StartS()
    {
        new Thread(() =>
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = @"D:\Git\UCF\U2019415f1_SKT_Jump30_UCF\SupportFiles\Temp\gradlew.bat",
                    Arguments = "assembleRelease",
                    WorkingDirectory = @"D:\Git\UCF\U2019415f1_SKT_Jump30_UCF\SupportFiles\Temp\",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };

            process.OutputDataReceived += OnReceiveMessage;
            process.ErrorDataReceived += OnReceiveMessage;
            process.Exited += Process_Exited;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            process.Dispose();
        }).Start();
    }

    private void Process_Exited(object sender, EventArgs e)
    {

    }

    private void OnReceiveMessage(object sender, DataReceivedEventArgs arg)
    {
        logStringBuilder.AppendLine(arg.Data);
    }
}