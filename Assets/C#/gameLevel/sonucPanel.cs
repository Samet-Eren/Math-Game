using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sonucPanel : MonoBehaviour
{
    public void OyunaYenidenBasla()
    {
        SceneManager.LoadScene("gameLevel"); //Game sahnesine gitmesini sa�lar
    }
    public void OyunaYenidenBasla2()
    {
        SceneManager.LoadScene("SampleScene"); //Renk E�leme sahnesine gitmesini sa�lar
    }

    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("menuLevel"); //Anamen� sahnesine gitmeyi sa�lar
    }
}
