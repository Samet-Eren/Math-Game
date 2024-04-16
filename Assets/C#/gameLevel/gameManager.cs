using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class gameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject KareNesne;

    [SerializeField]
    private Transform karelerPaneli;

    private GameObject[] karelerDizisi = new GameObject[25];

    [SerializeField]
    private Transform soruPanel;

    [SerializeField]
    private Text soruText;

    [SerializeField]
    private Sprite[] kareSprites;

    [SerializeField]
    private GameObject sonucPanel;

    List<int> kareDegerleriListesi = new List<int>();

    int birinciSayi, ikinciSayi, kacinciSoru, butonDegeri, dogruSonuc, kalanCan;
    bool butonaBasilsinmi;
    string hangiIslem, zorlukDerecesi;

    kalanCanlarManager kalanCanlarManager;
    puanManager puanManager;

    GameObject gecerliKare;

    private void Awake()
    {
        kalanCan = 3;

        sonucPanel.GetComponent<RectTransform>().localScale = Vector3.zero; //Sonu� panelinin gizlenmesini sa�lar

        kalanCanlarManager=Object.FindObjectOfType<kalanCanlarManager>(); //Kalan canlar i�in a��lan scripti �a��r�r.
        puanManager = Object.FindObjectOfType<puanManager>(); // Puanlama i�in a��lan scripti �a��r�r.

        kalanCanlarManager.kalanCanlariKontrolEt(kalanCan);
    }

    void Start()
    {
        butonaBasilsinmi = false;

        hangiIslem = PlayerPrefs.GetString("hangiIslem"); //Toplama veya ��karma butonlar�ndan se�ilene g�re soru �retmesini sa�lar.

        soruPanel.GetComponent<RectTransform>().localScale= Vector3.zero; //soru panelinin a��l��ta g�r�nmez olmas�n� sa�lar.

        KaraleriOlustur();
    }


    public void KaraleriOlustur()
    {
        for (int i = 0; i < 25; i++) // 25 tane cevap karesi olu�turmay� sa�lar.
        {
            GameObject kare = Instantiate(KareNesne, karelerPaneli);

            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0,kareSprites.Length)]; 
            //karelerin arkas�na resim eklemeyi sa�lar

            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi()); 
            //kareler olu�turulurken soru paneli gelmeden t�klanmalar� engellenir.
            karelerDizisi[i] = kare;
        }
        KareDegerleriYazdir();

        StartCoroutine(DoFadeRoutine());
        
        Invoke("SoruPaneliAc",1f); // Kareler olu�turulduktan sonra sorunun ekrana gelmesini sa�lar.

    }

    void KareDegerleriYazdir() //karelerin i�erisine rastgele say�lar atanmas�n� sa�lar.
    {
        foreach (var kare in karelerDizisi)
        {
            int rndDeger = Random.Range(15, 30);

            kareDegerleriListesi.Add(rndDeger);

            kare.transform.GetChild(0).GetComponent<Text>().text = rndDeger.ToString();
        }
    }

    void ButonaBasildi()
    {
        if (butonaBasilsinmi)
        {
            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);
            // T�klanan butonun i�indeki de�eri de�i�kende saklan�yor.

            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject; 
            // do�ru karenin se�ilip se�ilmedi�i kontrul ediliyor.

            sonucuKontrolEt();
        }
        
    }

    void sonucuKontrolEt()
    {
        if (butonDegeri == dogruSonuc)
        {
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true; 
            //do�ru cevap se�ildi�inde arkas�ndaki resmin g�r�nmesini sa�lad�k

            gecerliKare.transform.GetChild(0).GetComponent<Text>().text = ""; 
            //Do�ru cevaptan sonra resmin �st�nde say�n�n kaybolmas�n� sa�lad�k

            gecerliKare.transform.GetComponent<Button>().interactable = false;
            //do�ru cevaplanan kareye tekrar t�klanmamas�n� sa�lad�k

            puanManager.puaniArttir(zorlukDerecesi); //Cevaplad��� soruya g�re puan verilmesi

            kareDegerleriListesi.RemoveAt(kacinciSoru); //Verdi�i soruyu tekrar vermemesi i�in

            if (kareDegerleriListesi.Count > 0)
            {
                SoruPaneliAc(); //Bir soruyu yan�tlad�ktan sonra yenisinin gelmesi i�in
            }
            else
            {
                OyunBitti();
            }
            
        }
        else
        {
            kalanCan--; // Hatal� cevap verilince canlar�n eksiltilmesini sa�lar.
            kalanCanlarManager.kalanCanlariKontrolEt(kalanCan);
        }

        if (kalanCan <= 0) // Canlar bittikten sonra oyun bitti ekran�n�n a��lmas�n� sa�lar.
        {
            OyunBitti();
        }
    }

    void OyunBitti()
    {
        butonaBasilsinmi = false;
        sonucPanel.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack); 
        //Sonu� panelini oyun bittikten sonra g�r�nmesini sa�lar
    }

    IEnumerator DoFadeRoutine() //kareler olu�turulurken s�rayla efekt g�r�n�m�nde ekrane gelmelerini sa�lar
    {
        foreach (var kare in karelerDizisi)
        {
            kare.GetComponent<CanvasGroup>().DOFade(1, 0.2f); 

            yield return new WaitForSeconds(0.07f);
        }
    }

    void SoruPaneliAc() //Soru panelinin kareler olu�tuktan sonra ekrane gelmesini sa�lar
    {
        SoruyuSor();
        butonaBasilsinmi = true;
        soruPanel.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    public void SoruyuSor() //sorular�n olu�turulmas�n� sa�lar ve zorluk dereceleri belirlenir.
    {
        switch (hangiIslem)
        {
            case "toplama":

                birinciSayi = Random.Range(2, 15);
                kacinciSoru = Random.Range(0, kareDegerleriListesi.Count);

                dogruSonuc = kareDegerleriListesi[kacinciSoru];
                ikinciSayi = kareDegerleriListesi[kacinciSoru] - birinciSayi;

                if (ikinciSayi <= 10 )
                {
                    zorlukDerecesi = "kolay";
                }
                else if (ikinciSayi > 10 && ikinciSayi <= 20)
                {
                    zorlukDerecesi = "orta";
                }
                else
                {
                    zorlukDerecesi = "zor";
                }

                soruText.text = ikinciSayi.ToString() + " + " + birinciSayi.ToString();

                break;

            case "cikarma":

                birinciSayi = Random.Range(2, 15);
                kacinciSoru = Random.Range(0, kareDegerleriListesi.Count);

                dogruSonuc = kareDegerleriListesi[kacinciSoru];
                ikinciSayi = kareDegerleriListesi[kacinciSoru] + birinciSayi;

                if (ikinciSayi <= 20)
                {
                    zorlukDerecesi = "kolay";
                }
                else if (ikinciSayi > 20 && ikinciSayi <= 35)
                {
                    zorlukDerecesi = "orta";
                }
                else
                {
                    zorlukDerecesi = "zor";
                }

                soruText.text = ikinciSayi.ToString() + " - " + birinciSayi.ToString();

                break;
        }
    }
}
