using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class ShowProjectType : EditorWindow
{
    [MenuItem("Tuna/Window/ShowProjectType")]
    public static void ShowExample()
    {
        ShowProjectType wnd = GetWindow<ShowProjectType>();
        wnd.minSize = new Vector2(10, 10);
        wnd.titleContent = new GUIContent("ProjectType");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Client");
        label.style.fontSize = 30;
        label.style.alignSelf = Align.Center;
        label.style.unityFontStyleAndWeight = FontStyle.Bold;
        root.Add(label);
    }
}