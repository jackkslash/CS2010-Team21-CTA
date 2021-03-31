using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSController : MonoBehaviour
{
    private string secretKey = "charlieAlien"; // Edit this value and make sure it's the same as the one stored on the server
    private string addScoreURL = "http://www.charliethealien.co.uk/databaseScripts/highscore/addscore.php?"; //be sure to add a ? to your url
 
    void Start()
    {

    }
 
    // remember to use StartCoroutine when calling this function!
    public IEnumerator PostScores(int score, string name)
    {
        Debug.Log("trying");
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = MD5test.Md5Sum(name + score + secretKey);
 
        string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
        Debug.Log(post_url);
 
        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done
 
        if (hs_post.error != null)
        {
            Debug.Log("There was an error posting the high score: " + hs_post.error);
        } else {
            Debug.Log("success");
        }
    }

}
