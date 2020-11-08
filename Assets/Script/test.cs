using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{

    dreamloLeaderBoard dl;
    string lbNameString;
    string lbScoreString;
    int lbPlace;

    // Start is called before the first frame update
    void Start()
    {

        this.dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        
        StartCoroutine(CheckHighScore());
    }

	// Update is called once per frame
	void Update()
    {
       
    }

    IEnumerator CheckHighScore()
    {
        GameObject lbName = GameObject.Find("/Canvas/LBName");
        GameObject lbScore = GameObject.Find("/Canvas/LBScore");

        dl.AddScore("abb", 1);

        List<dreamloLeaderBoard.Score> scoreList = dl.ToListHighToLow();
        while (scoreList.Count == 0)
        {

            yield return new WaitForSeconds(1.0f);
            scoreList = dl.ToListHighToLow();
        }

        Debug.Log(scoreList.Count);
        lbPlace = 1;
        foreach (dreamloLeaderBoard.Score currentScore in scoreList)
        {
            lbNameString += "\n" + lbPlace + ": " + currentScore.playerName;
            lbScoreString += "\n" + currentScore.score.ToString();
            lbPlace++;
        }
        Text myLBName = lbName.GetComponent<Text>();
        myLBName.text = lbNameString;
        Text myLBScore = lbScore.GetComponent<Text>();
        myLBScore.text = lbScoreString;
    }

}
