using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour {

    // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;

    // Reference to image to change sprites
    private Image rend;

    [SerializeField] private GameObject diceHint;

    [SerializeField] private GameLogic gameLogic;

    private int currentDiceNumber;

	// Use this for initialization
	private void Start () 
    {

        // Assign Renderer component
        rend = GetComponent<Image>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
	}

    public bool CheckMovementAvailable()
    {
        if(currentDiceNumber < 1)
        {
            Debug.Log("Not enough dice roll available.");
            return false;
        }
        return true;
    }

    // Reduces the dice number. Will return true if movement is possible, false if not.
    public void ReduceDieNumber()
    {
        if(currentDiceNumber <= 1)
        {
            currentDiceNumber -= 1;
            diceHint.SetActive(false);
            gameObject.SetActive(false);
            return;
        }
        rend.sprite = diceSides[(--currentDiceNumber)-1];
    }

    public void ClearDice()
    {
        currentDiceNumber = 0;
        rend.sprite = diceSides[0];
        diceHint.SetActive(false);
        gameObject.SetActive(false);
    }
	
    // If you left click over the dice then RollTheDice coroutine is started
    public void RollDice()
    {
        diceHint.SetActive(false);
        StartCoroutine(RollTheDice((finalResult) => {
            currentDiceNumber = finalResult;
            gameLogic.DiceResult(currentDiceNumber);
        }));
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice(System.Action<int> callbackOnFinish)
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        int finalSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 6 (Non-inclusive i.e. 0-5 will be selected)
            randomDiceSide = Random.Range(0, 6);

            // Set sprite to upper face of dice from array according to random value
            rend.sprite = diceSides[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;

        callbackOnFinish(finalSide);
    }
}
