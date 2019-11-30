using System.Collections;
using System.Linq;
using UnityEngine;

public class PartySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject heroPrefab;

    private const float spawnTimeRef = 10f;
    private const float spawnInterTimeRef = .5f;
    private const float spawnChance = 30; // %
    private const int partyMinSize = 2;
    private const int partyMaxSize = 6;
    private float spawnTime;

    private void Start()
    {
        spawnTime = spawnTimeRef;
        StartCoroutine("SpawnParty");
    }

    private void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime < 0f)
        {
            if (Random.Range(0, 100) < spawnChance)
            {
                StartCoroutine("SpawnParty");
            }
            spawnTime = spawnTimeRef;
        }
    }

    private IEnumerator SpawnParty()
    {
        int partySize = Random.Range(partyMinSize, partyMaxSize + 1);
        for (int i = 0; i < partySize; i++)
        {
            GameObject go = Instantiate(heroPrefab, transform.position, Quaternion.identity);
            go.GetComponent<HeroController>().Init(
                GenerateName(),
                (HeroController.HeroClass)Random.Range(0, (int)System.Enum.GetValues(typeof(HeroController.HeroClass)).Cast<HeroController.HeroClass>().Max()));
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
