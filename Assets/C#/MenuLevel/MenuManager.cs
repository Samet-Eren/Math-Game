using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject toplamaButton, cikarmaButton, cikisButton;

    void Start()
    {
        butonSonradanGel();   
    }

    public void GameLevel(string hangiIslem) //Toplama veya ��karma se�imine g�re sorular�n getirilmesini sa�lar
    {
        PlayerPrefs.SetString("hangiIslem", hangiIslem);
        SceneManager.LoadScene("gameLevel");
    }

    public void RenkEsleme()
    {
        SceneManager.LoadScene("SampleScene"); //Renk e�leme sahnesine gidilmesini sa�lar
    }

    public void oyundanCik()
    {
        Application.Quit();
    }

    void butonSonradanGel() //Butonlar�n fluudan g�r�n�re do�ru efektli bir �ekilde a��lmas�n� sa�lar
    {
        toplamaButton.GetComponent<CanvasGroup>().DOFade(1, 0.8f);
        cikarmaButton.GetComponent<CanvasGroup>().DOFade(1, 0.8f);
        cikisButton.GetComponent<CanvasGroup>().DOFade(1, 0.8f).SetDelay(0.5f);
    }
}
