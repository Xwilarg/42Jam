using System.Linq;
using UnityEditor;
using UnityEngine;

public class NodeWindow : EditorWindow
{
    [MenuItem("Window/Node Window")]
    public static void ShowWindow()
    {
        GetWindow(typeof(NodeWindow));
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0f, 0f, 200f, 20f), "Show/Hide Node's Links"))
        {
            showLines = !showLines;
        }
        if (GUI.Button(new Rect(0f, 25f, 200f, 20f), "Validate nodes"))
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Node"))
            {
                Node n = go.GetComponent<Node>();
                if (n.nodes.Contains(n))
                {
                    var list = n.nodes.ToList();
                    list.Remove(n);
                    n.nodes = list.ToArray();
                    Debug.Log("Node contains self, deleting...");
                }
                foreach (Node node in n.nodes)
                {
                    if (!node.nodes.Contains(n))
                    {
                        var list = node.nodes.ToList();
                        list.Add(n);
                        node.nodes = list.ToArray();
                        Debug.Log("Node is unidirectional, adding self...");
                    }
                }
            }
            Debug.Log("Nodes were validated");
        }
    }

    private void Start()
    {
        showLines = false;
    }

    private void Update()
    {
        if (showLines)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Node"))
            {
                foreach (Node node in go.GetComponent<Node>().nodes)
                {
                    Debug.DrawLine(go.transform.position, node.transform.position, Color.red);
                }
            }
        }
    }

    private bool showLines;
}