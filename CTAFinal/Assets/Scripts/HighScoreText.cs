using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{

    private string highscoreURL = "http://www.charliethealien.co.uk/databaseScripts/highscore/display5.php";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetScores());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GetScores()
    {
        gameObject.GetComponent<Text>().text = "Loading Scores";

        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;
 
        if (hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            gameObject.GetComponent<Text>().text = hs_get.text; // this is a GUIText that will display the scores in game.
        }
    }
}
