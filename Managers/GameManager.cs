using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public bool isPlaying;
    public string hiddenWord;
    public int guessNum;
    public List<string> guesses;
    public int ALLOWED_GUESSES = 6;

    private InputHandler inputHandler;
    private GameObject keyboard;

    public string[] words;
    private int wordList_LENGTH = 5757;

    public string status;

    void Start()
    {
        // IF WEBGL
        string assetPath = Application.streamingAssetsPath;
        bool isWebGl = assetPath.Contains("://") || assetPath.Contains(":///");

        isPlaying = false;

        guessNum = 0;
        guesses = new List<string>();

        status = "";

        try
        {
            if (isWebGl)
            {
                Debug.Log("Website Application Opened.");
            }
            else
            {
                Debug.Log("Desktop Application Opened.");
            }
        }
        catch
        {

        }

        TextAsset wordListText = Resources.Load<TextAsset>("Text/WordList");

        words = TextAssetToList(wordListText, wordList_LENGTH);

        NewWord(words);
        Debug.Log(hiddenWord);
    }

    void Update()
    {
        CheckStatus();
    }

    string[] GetLine(string fileName, int length)
    {
        TextReader tr = new StreamReader(fileName);

        int NumberOfLines = length - 1;
        string[] ListLines = new string[NumberOfLines];

        for (int i = 1; i < NumberOfLines; i++)
        {
            ListLines[i] = tr.ReadLine();
        }

        /*
        //This will write the 1st line into the console
        Console.WriteLine(ListLines[1]);
        */
        tr.Close();
        return ListLines;
    }

    string[] TextAssetToList(TextAsset ta, int length)
    {
        int NumberOfLines = length;
        string[] arrayToReturn = new string[NumberOfLines];

        arrayToReturn = ta.text.Split('\n');
        return arrayToReturn;
    }

    void NewWord(string[] words_)
    {
        hiddenWord = words_[Random.Range(0, wordList_LENGTH + 1)].ToUpper();
    }

    public void Reset()
    {
        inputHandler = GameObject.Find("Input Handler").GetComponent<InputHandler>();
        keyboard = GameObject.Find("Keyboard");

        isPlaying = true;

        guessNum = 0;
        guesses = new List<string>();

        inputHandler.slotIndex = 0;

        for (int i = 0; i < inputHandler.nodeGrid.transform.childCount; i++)
        {
            for (int j = 0; j < inputHandler.nodeGrid.transform.GetChild(i).childCount; j++)
            {
                LetterBox node = inputHandler.nodeGrid.transform.GetChild(i).gameObject.transform.GetChild(j).gameObject.GetComponent<LetterBox>();

                node.letter.text = " ";
                node.slotImage.color = node.defaultGray;
            }
        }

        for (int i = 0; i < keyboard.transform.childCount; i++)
        {
            LetterBox letter = GameObject.Find("Node").GetComponent<LetterBox>();
            Keyboard key = keyboard.transform.GetChild(i).GetComponent<Keyboard>();

            key.image.color = letter.defaultGray;
            key.letterText.color = letter.white;
        }

        NewWord(words);
        Debug.Log("NEW WORD: " + hiddenWord);
    }

    void CheckStatus()
    {
        foreach (string guess in guesses)
        {
            if (hiddenWord.Contains(guess))
            {
                status = "Win";
                Win();
            }
        }

        if (guessNum == 5)
        {
            status = "Lose";
            Lose();
        }
    }

    public void Win()
    {
        // END GAME
        isPlaying = false;
        Debug.Log("You Win!");
    }

    public void Lose()
    {

    }
}
