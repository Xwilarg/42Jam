using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject heroPrefab;

    private float spawnTimeRef = 5f;
    private const float spawnInterTimeRef = .5f;
    private const float spawnChance = 20; // %
    private const int partyMinSize = 1;
    private const int partyMaxSize = 1;
    private float spawnTime;
    private Node firstNode, finalNode;
    private Node[] finalPath;
    private const float timerIncRef = 5f;
    private const float timerMin = .1f;
    private const float incValue = .5f;
    private float timerInc;

    private void Start()
    {
        spawnTime = spawnTimeRef;
        firstNode = HeroController.GetClosestNode<Node>(transform.position, "Node");
        List<Node> path = new List<Node>();
        finalNode = HeroController.GetClosestNode<Node>(GameObject.FindGameObjectWithTag("Player").transform.position, "Node");
        finalPath = GetShortestWay(path, firstNode).ToArray();
        timerInc = timerIncRef;
    }

    private void Update()
    {
        spawnTime -= Time.deltaTime;
        timerInc -= Time.deltaTime;
        if (spawnTime < 0f)
        {
            if (Random.Range(0, 100) < spawnChance)
            {
                StartCoroutine("SpawnParty");
            }
            spawnTime = spawnTimeRef;
        }
        if (timerInc < 0f)
        {
            spawnTimeRef -= incValue;
            if (spawnTimeRef < timerMin)
                spawnTimeRef = timerMin;
            timerInc = timerIncRef;
        }
    }

    /// <summary>
    /// Get shortest way from point A to B following nodes
    /// </summary>
    /// <param name="path">Current path</param>
    /// <param name="currNode">Next node</param>
    private List<Node> GetShortestWay(List<Node> path, Node currNode)
    {
        if (path == null) // If path is null we ignore it
            return null;
        List<Node> currPath = new List<Node>(path); // Create a new path
        currPath.Add(currNode); // Add current node to path
        if (currNode == finalNode) // If we reach first node then it's okay
            return currPath;
        List<Node> addPath = null; // Final path
        foreach (var l in currNode.nodes) // We go through each nodes
        {
            if (!currPath.Contains(l)) // If current path contains the node, that means we are looping
            {
                var newPath = GetShortestWay(currPath, l); // Get path
                if (newPath != null && (addPath == null || newPath.Count < addPath.Count)) // If it's the shortest path
                    addPath = newPath;
            }
        }
        return addPath;
    }

    private IEnumerator SpawnParty()
    {
        int partySize = Random.Range(partyMinSize, partyMaxSize + 1);
        for (int i = 0; i < partySize; i++)
        {
            GameObject go = Instantiate(heroPrefab, transform.position, Quaternion.identity);
            go.transform.parent = transform.parent;
            go.GetComponent<HeroController>().Init(
                GenerateName(),
                (HeroController.HeroClass)Random.Range(0, (int)System.Enum.GetValues(typeof(HeroController.HeroClass)).Cast<HeroController.HeroClass>().Max() + 1),
                finalPath);
            yield return new WaitForSeconds(spawnInterTimeRef);
        }
    }

    private string GenerateName()
    {
        string name = "";
        int nameLength = Random.Range(5, 10);
        char[] voyels = new char[] { 'a', 'i', 'u', 'e', 'o' };
        char[] consonantes = new char[] { 'r', 't', 'p', 's', 'd', 'f', 'h', 'j', 'k', 'l', 'm', 'v', 'b', 'n' };
        if (Random.Range(0, 2) == 0)
            name += char.ToUpper(voyels[Random.Range(0, voyels.Length)]);
        else
            name += char.ToUpper(consonantes[Random.Range(0, consonantes.Length)]);
        for (int i = 0; i < nameLength; i++)
        {
            if (voyels.Contains(char.ToLower(name.Last()))) // Is last letter a voyel
                name += consonantes[Random.Range(0, consonantes.Length)];
            else
                name += voyels[Random.Range(0, voyels.Length)];
        }
        return name;
    }
}
