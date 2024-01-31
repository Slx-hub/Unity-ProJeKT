using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.DefaultInputActions;

public class DebugWindow : MonoBehaviour
{
    public Vector2Int PositionOffset = Vector2Int.zero;
    public int Height = 400;
    public int Width = 200;
    public int MimimizedHeight = 20;
    public GUIStyle GUIStyle;
    public bool Minimized = false;
    public int priority = 0;

    private MonoBehaviour[] allScripts;

    void Start()
    {
        if (!Application.isEditor)
            this.enabled = false;

        allScripts = gameObject.GetComponents<MonoBehaviour>();
    }

    void OnGUI()
    {
        var debugTextList = new List<string>();

        if (!Minimized)
        {
            foreach (var script in allScripts)
            {
                var scriptDump = script.ToString();
                if (scriptDump.StartsWith(">"))
                    debugTextList.Add(scriptDump);
            }

            string debugText = string.Join("\n\n", debugTextList);
            float neededHeigth = 18 * debugText.Where(c => c == '\n').Count();

            GUI.Box(new Rect(PositionOffset.x, PositionOffset.y, Width, neededHeigth), gameObject.name + ":\n\n" + debugText, GUIStyle);
            if (GUI.Button(new Rect(PositionOffset.x + Width - 20, PositionOffset.y, 20, MimimizedHeight), "[-]", GUIStyle))
                Minimized = true;
        }
        else
        {
            GUI.Box(new Rect(PositionOffset.x, PositionOffset.y, Width, MimimizedHeight), gameObject.name + ":", GUIStyle);
            if (GUI.Button(new Rect(PositionOffset.x + Width - 20, PositionOffset.y, 20, MimimizedHeight), "[+]", GUIStyle))
                Minimized = false;
        }
    }
}
