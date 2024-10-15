using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // State thy variables
    public GameManager gameManager;
    public Ground[] allGrounds;
    public Ground[] allGround1;
    public int max = 3;
    // Start is called before the first frame update

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        SetGameLevel();
        allGround1 = FindObjectsOfType<Ground>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void SetGameLevel()
    {
        allGrounds = FindObjectsOfType<Ground>();
    }

    public void CheckIfGameLevelIsFinished() 
    {
        int nagc = 0;
        foreach (var ground in allGrounds)
        {
            if (!ground.isColored) {
                nagc ++;
                break;
            }
        }
        if (nagc == 0) {
            if (SceneManager.GetActiveScene().buildIndex < (max - 1)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            } else {
                SceneManager.LoadScene(0);
            }
        }
    }
}

