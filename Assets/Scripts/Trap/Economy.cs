using UnityEngine;
using UnityEngine.UI;

public class Economy : MonoBehaviour
{
    private Character player;
    public Text goldDisplay;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Character>();
        player.GainOr(100);
        goldDisplay = GameObject.Find("GoldText").GetComponent<Text>();
        goldDisplay.text = "Gold: " + player.GetOr();
    }

    public void UpdateGold()
    {
        goldDisplay.text = "Gold: " + player.GetOr();
    }
}
