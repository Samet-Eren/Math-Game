using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage;

    public Sprite[] puzzles;

    public List <Sprite> gamePuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>();

    private bool firsGuess, secondGuess;

    private int countGuesses, countCorrectGuesses, gameGuesses, firstGuessIndex, secondGuessIndex;

    private string firstGuessPuzzle, secondGuessPuzzle;

    [SerializeField]
    private GameObject sonucPanel;

    private void Awake()
    {
        sonucPanel.GetComponent<RectTransform>().localScale = Vector3.zero; //sonu� panelinin ekrandan gizlenmesi sa�lan�r.
        puzzles = Resources.LoadAll<Sprite>("Resimler/colorpalet"); //renklerin i�erisinde oldu�u klas�r tan�t�l�r.
    }
    private void Start() //Oyun ba�lad���nda metotlar�n �a�r�lmas�
    {
        GetButtons ();
        AddListeners ();
        AddGamePuzzles ();
        Shuffle (gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
    }
    void GetButtons() //Renklerin i�erisinde bulundu�u resimler par�alan�r.
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    void AddGamePuzzles() //Renkler butonlar�n i�erisine da��t�l�r.
    {
        int looper = btns.Count;
        int index = 0;
        for (int i = 0; i < looper; i++)
        { 
            if (index == looper / 2 )
            {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    void AddListeners() // Buton t�kland���nda i�erisindeki resmi haf�zaya al�r.
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }
        
    public void PickAPuzzle() //t�klanan iki butonun i�erisindeki renklerin ayn� olup olmad��� kontrol edilir.
    {

        if (!firsGuess)
        {
            firsGuess = true;

            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if (!secondGuess)
        {
            secondGuess = true;

            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;

            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            countGuesses++;

            StartCoroutine(CheckIfThePuzzlesMatch());
        }

    }
    IEnumerator CheckIfThePuzzlesMatch() //T�m e�le�melerin yap�l�p yap�lmad���n� kontrol edilir.
    {
        yield return new WaitForSeconds(1f);

        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(.3f);

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            OyunBitti();
        }
        else
        {
            yield return new WaitForSeconds(.4f);
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }

        yield return new WaitForSeconds(.4f);

        firsGuess = secondGuess = false;
    }

    void OyunBitti()
    {
        countCorrectGuesses++;

        if (countCorrectGuesses == gameGuesses)
        {
            sonucPanel.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
            //Sonu� panelini oyun bittikten sonra g�r�nmesini sa�lar
        }
    }

  

    void Shuffle(List<Sprite> list) //Renklerin her seferinde farkl� gelmesi i�in kar��t�r�l�r.
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

   
}
