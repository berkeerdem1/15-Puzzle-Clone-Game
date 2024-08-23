using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int maxMove;
    public int moveAmount = 0;
    public int lastMoveAmount;

    public bool isFinished = false;
    public bool isMoveModde;

    public Text endTimer, newBestScoreText, bestTimeText, moveAmontText, endMoveText, lastMoveAmountText;

    public GameObject wonImage, gameOverImage;
    
    [SerializeField] private Transform emptySpace;
    [SerializeField] private Tiles[] tiles;
    private Camera camera;
    private Timerr timer;
    private int emptySpaceIndex = 15;

    public DifficultyMode currentMode = DifficultyMode.Medium;
    private void Awake()
    {
        timer = GetComponent<Timerr>();
        camera = Camera.main;
    }
    private void Start()
    {
        wonImage.SetActive(false);
        newBestScoreText.gameObject.SetActive(false);
        bestTimeText.transform.parent.gameObject.SetActive(false);
        gameOverImage.SetActive(false);

        currentMode = DifficultyMode.Hard;
        Shuffle(); 

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))  // If the number is clicked
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition); 
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit && !isFinished) 
            {
                if(Vector2.Distance( emptySpace.position, hit.transform.position) <= maxMove) // If the distance of the empty space is less than 2 units from the clicked tile
                {
                    Vector2 lastEmptySpacePos = emptySpace.position;  // Create Vector2 variable, new emptySpace pos.
                    Tiles thisTile = hit.transform.GetComponent<Tiles>(); // Hit tiles component
                    emptySpace.position = thisTile.targetPos;  // Moves the position of the empty space to the target position of the tile.
                    thisTile.targetPos = lastEmptySpacePos; // Move the target position of the tile to the empty space position
                    int tileIndex = FindIndex(thisTile); // Tile index
                    
                    tiles[emptySpaceIndex] = tiles[tileIndex]; // Makes the new index of the empty field a tile index
                    tiles[tileIndex] = null; // Sets the old index of the tile to null
                    emptySpaceIndex = tileIndex; // Makes the index of the empty field the old index of the tile

                    MoveAmount(); // Move Control
                }
            }
        }
        FinishControl();
    }

    void FinishControl()
    {
        if (!isFinished)
        {
            int correctTiles = 0; // Tile amounts on correct Pos

            foreach (var tile in tiles)
            {
                if (tile != null)
                { 
                    if (tile.inRightPlace) // If tile on correct pos
                    {
                        correctTiles += 1;

                    }
                }
            }

            if (correctTiles == tiles.Length - 1) // If all tiles correct Pos 
            {
                isFinished = true;

                wonImage.SetActive(true); // Win Screen active
                timer.StopTimer(); // Stop timer
                timer.EndTimer(endTimer); // Total timer 
                EndMoveAmount(); // Total move

                int bestTimer;

                if (PlayerPrefs.HasKey("bestTime"))
                {
                    bestTimer = PlayerPrefs.GetInt("bestTime");
                }
                else
                {
                    bestTimer = 999;
                }

                int playerTime = timer.minutes * 60 + timer.seconds;

                if (playerTime < bestTimer)
                {
                    bestTimeText.transform.parent.gameObject.SetActive(false);
                    newBestScoreText.gameObject.SetActive(true);
                    PlayerPrefs.SetInt("bestTime", playerTime);
                }
                else
                {
                    int minutes = bestTimer / 60;
                    int seconds = bestTimer % 60;
                    bestTimeText.text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
                    bestTimeText.transform.parent.gameObject.SetActive(true);
                }
            }
        }
    } // Time, move amount and best score show on while finish game

    public void ModeSettings()
    {

        if (isMoveModde)
        {
            lastMoveAmount = 115;
            lastMoveAmountText.text = lastMoveAmount.ToString("Last Move:" + lastMoveAmount);
            lastMoveAmountText.enabled = true;
            moveAmontText.enabled = false;
        }
        else
        {
            lastMoveAmountText.enabled = false;
            moveAmontText.enabled = true;
            moveAmontText.text = moveAmount.ToString("Move:" + moveAmount);
        }
    }  // Game Mode UI settings
    void MoveAmount()
    {
        if (!isMoveModde)
        {
            moveAmount += 1;
            moveAmontText.text = moveAmount.ToString("Move:" + moveAmount);
        }
        else
        {
            moveAmount += 1;
            lastMoveAmount -= 1;
            lastMoveAmountText.text = lastMoveAmount.ToString("Last Move:" + lastMoveAmount);

            if(lastMoveAmount == 0)
            {
                print("Game Over");
                gameOverImage.SetActive(true);
                timer.StopTimer();
            }
        }
        
    } // Move amounts control
    void EndMoveAmount()
    {
        endMoveText.text = moveAmount.ToString("");
    } // Total move amount show while Finish game 
    public void Shuffle()
    {
        // If the empty space is not in the last position, adjust the positions of the tiles
        if (emptySpaceIndex != 15)
        {
            var tileOn15LastPos = tiles[15].targetPos; // Swap the position of the tile at index 15 with the empty space
            tiles[15].targetPos = emptySpace.position;
            emptySpace.position = tileOn15LastPos;
            tiles[emptySpaceIndex] = tiles[15];
            tiles[15] = null;
            emptySpaceIndex = 15;
        }
        int invertion;


        do // Continue shuffling until the puzzle is solvable (inversion count is even)
        {
            for (int i = 0; i < tiles.Length - 2; i++) // Shuffle the of 14 tiles random.
            {
                var lastPos = tiles[i].targetPos;  // Swap the target positions of the current tile with a randomly chosen tile
                int randomIndex = Random.Range(0, 14);
                tiles[i].targetPos = tiles[randomIndex].targetPos;
                tiles[randomIndex].targetPos = lastPos;

                // Swap the tiles in the array
                var tile = tiles[i];
                tiles[i] = tiles[randomIndex];
                tiles[randomIndex] = tile;
            }

            // Calculate the number of inversions in the current configuration
            invertion = GetInvertions();
            Debug.Log("");
        }
        while (invertion % 2 != 0); // continue while inversion is odd. // Ensure that the puzzle is in a solvable state

    } // Puzzle mixer and adjuster

    public int FindIndex(Tiles tile)
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
            {
                if (tiles[i] == tile) // If the tiles element is the same as the tile in the parameter, return the element
                {
                    return i;
                }
            }
        }
        return -1;
    } // Find the index of a tile in the array

    int GetInvertions()
    {
        int inversionSum = 0;
        // Calculate the total number of inversions in the current tile configuration
        for (int i = 0; i < tiles.Length; i++) 
        {
            int thisTileInvertion = 0; // Initialize the inversion count for the current tile
            for (int j = i; j < tiles.Length - 1; j++) 
            {
                if (tiles[j] != null)
                {
                    if (tiles[i].number > tiles[j].number) // Count how many tiles with a higher number are positioned before the current tile
                    {
                        thisTileInvertion++; // Increment the inversion count for the current tile
                    }
                }
            }
            inversionSum += thisTileInvertion;
        } 
        return inversionSum; // Return total number of inversions
    } // Finds the sum of inversion

    public void ChangeDifficulty(DifficultyMode mode)
    {
        currentMode = mode;
    }
    public enum DifficultyMode
    {
        Easy,
        Medium,
        Hard
    }
}
