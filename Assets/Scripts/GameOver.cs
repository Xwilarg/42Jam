using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;
    public bool gameIsOver = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver && Input.GetKeyDown(KeyCode.Return)) {
            Restart();
        }
    }

    public void Over()
    {
        gameOverScreen.SetActive(true);
        gameIsOver = true;
    }

    private void Restart()
    {
        StartCoroutine(GetComponent<ProceduralDungeon>().Regenerate());
        gameOverScreen.SetActive(false);
        gameIsOver = false;
    }
}
