using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class gameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody player;
    public GameObject cam;
    public Button startBtn;
    public TextMeshProUGUI startTxt;
    public TextMeshProUGUI lostTxt;
    public TextMeshProUGUI wonTxt;
    public Button resetBtn;
    public Button continueBtn;

    private bool hasWon;

    private void Start()
    {
        player.constraints = RigidbodyConstraints.FreezeAll;
        cam.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        startTxt.gameObject.SetActive(true);
        lostTxt.gameObject.SetActive(false);
        wonTxt.gameObject.SetActive(false);
        resetBtn.gameObject.SetActive(false);

    }

    public void StartGame()
    {
        player.constraints = RigidbodyConstraints.None;
        player.constraints = RigidbodyConstraints.FreezeRotation;
        cam.SetActive(true);
        startBtn.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
  
    }

    public void lostGame()
    {
        if (!hasWon)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            lostTxt.gameObject.SetActive(true);
            resetBtn.gameObject.SetActive(true);

            player.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    public void wonGame()
    {
        hasWon = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        wonTxt.gameObject.SetActive(true);
        resetBtn.gameObject.SetActive(true);
    }
    public void resetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void keepPlaying(){

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        wonTxt.gameObject.SetActive(false);
        resetBtn.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(false);

}

}
