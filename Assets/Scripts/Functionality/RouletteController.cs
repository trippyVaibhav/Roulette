using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class RouletteController : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField]
    private Transform CoinContainer_Transform;

    [Header("Lists and Arrays")]
    [SerializeField]
    private GameObject[] Coins_Prefab;
    [SerializeField]
    private GameObject[] activeCoins_Object;
    [SerializeField]
    private List<GameObject> instantiated_Coins;

    [Header("Integers")]
    [SerializeField]
    private int CoinCounter = 0;
    [SerializeField]
    private int[] amount_array;
    [SerializeField]
    private int totalBet = 0;

    [SerializeField]
    private TMP_Text Totalbet_Text;
    [SerializeField]
    private Button cancelBet_Button;


    private void Start()
    {
        if (activeCoins_Object[CoinCounter]) activeCoins_Object[CoinCounter].SetActive(true);

        if (cancelBet_Button) cancelBet_Button.onClick.RemoveAllListeners();
        if (cancelBet_Button) cancelBet_Button.onClick.AddListener(CancelBet);
    }

    internal void CancelBet()
    {
        foreach(GameObject coin in instantiated_Coins)
        {
            Destroy(coin);
        }
        instantiated_Coins.Clear();
        instantiated_Coins.TrimExcess();
        if (Totalbet_Text) Totalbet_Text.text = "your bet amount: $00";
        Canvas.ForceUpdateCanvases();
        totalBet = 0;
    }

    internal void SelectCoin(GameObject activeObject, int counter)
    {
        foreach (GameObject objs in activeCoins_Object)
        {
            objs.SetActive(false);
        }
        activeObject.SetActive(true);
        CoinCounter = counter;
    }

    internal void BetOnButton(Transform parent, string code = null)
    {
        GameObject coin = Instantiate(Coins_Prefab[CoinCounter], CoinContainer_Transform);
        coin.transform.SetParent(parent);
        coin.transform.DOLocalMove(new Vector2(0, 0), 0.5f);
        instantiated_Coins.Add(coin);
        totalBet += amount_array[CoinCounter];
        if (Totalbet_Text) Totalbet_Text.text = "your bet amount: $" + totalBet;
        Canvas.ForceUpdateCanvases();
    }
}
