using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static bool isSelectMode = false;
    public int sceneId;
    public GameObject gameModeImage;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameModeImage.SetActive(true);
        isSelectMode = false;
    }
    public void TryAgain(int id)
    {
        id = sceneId;
        SceneManager.LoadScene(sceneId);
    }
    public void ClassicMode()
    {
        isSelectMode = true;
        gameManager.isMoveModde = false;
        gameManager.ModeSettings();
        gameModeImage.SetActive(false);
    }
    public void LastMoveMode()
    {
        isSelectMode = true;
        gameManager.isMoveModde = true;
        gameManager.ModeSettings();
        gameModeImage.SetActive(false);
    }
}
