using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Rect Transforms")]
    [SerializeField]
    private RectTransform LeftArrowPointer;
    [SerializeField]
    private RectTransform RightArrowPointer;

    [Header("Transforms")]
    [SerializeField]
    private Transform Timer_Transform;
    [SerializeField]
    private Transform Roulette_OuterCircle;
    [SerializeField]
    private Transform Roulette_InnerCircle;
    [SerializeField]
    private Transform Roulette_BallContainer;
    [SerializeField]
    private Transform Ball_Transform;

    [Header("TMP_Texts")]
    [SerializeField]
    private TMP_Text Timer_Text;
    [SerializeField]
    private TMP_Text winNumber_Text;

    [Header("GameObjects")]
    [SerializeField]
    private GameObject StartBettingPopup;
    [SerializeField]
    private GameObject StopBettingPopup;
    [SerializeField]
    private GameObject SpinPanel_Object;
    [SerializeField]
    private GameObject MainPopup_Object;
    [SerializeField]
    private GameObject winNumber_Object;

    [Header("Lists & Arrays")]
    [SerializeField]
    private Transform[] BallStopPoint;
    [SerializeField]
    private List<string> NumberCode;
    [SerializeField]
    private List<int> PreviousNumbers;
    [SerializeField]
    private List<TMP_Text> Previous_Text;
    [SerializeField]
    private List<Image> Previous_Image;

    [Header("Colors")]
    [SerializeField]
    private Color blackColor;
    [SerializeField]
    private Color redColor;
    [SerializeField]
    private Color greenColor;

    [Header("Managers")]
    [SerializeField]
    private RouletteController _rouletteManager;
    [SerializeField]
    private BallScript BallManager;

    [SerializeField]
    private int Timer = 30;
    [SerializeField]
    private int numberAnnounced = 0;
    [SerializeField]
    private Vector3 initialBallPosition;
    [SerializeField]
    private Image winNumber_Image;

    private Tweener ballMovement = null;
    private Tweener OuterRouletteMovement = null;
    private Tweener InnerRouletteMovement = null;


    private void Awake()
    {
        initialBallPosition = Ball_Transform.localPosition;
    }

    private void Start()
    {
        AnimateArrows();
    }


    internal void StartSpinning()
    {
        if (winNumber_Object) winNumber_Object.SetActive(false);                                                                                                                                                                                                                
        if (Ball_Transform) Ball_Transform.SetParent(Roulette_BallContainer);
        if (Ball_Transform) Ball_Transform.localPosition = initialBallPosition;
        if (Roulette_OuterCircle) Roulette_OuterCircle.localEulerAngles = new Vector3(0, 0, 359);
        if (Roulette_OuterCircle) OuterRouletteMovement = Roulette_OuterCircle.DORotate(new Vector3(0, 0, 0), 2, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        if (Roulette_InnerCircle) Roulette_InnerCircle.localEulerAngles = new Vector3(0, 0, 0);
        if (Roulette_InnerCircle) InnerRouletteMovement = Roulette_InnerCircle.DORotate(new Vector3(0, 0, 359), 2, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        if (Roulette_BallContainer) Roulette_BallContainer.localEulerAngles = new Vector3(0, 0, 0);
        if (Roulette_BallContainer) ballMovement = Roulette_BallContainer.DORotate(new Vector3(0, 0, 359), 1, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        if (SpinPanel_Object) SpinPanel_Object.SetActive(true);
        numberAnnounced = Random.Range(0, 37);
        StartCoroutine(BallRevolution(numberAnnounced));
    }

    private void AnimateArrows()
    {
        if (LeftArrowPointer) LeftArrowPointer.DOLocalMoveX(LeftArrowPointer.anchoredPosition.x + 25, 0.5f).SetLoops(-1, LoopType.Yoyo);
        if (RightArrowPointer) RightArrowPointer.DOLocalMoveX(RightArrowPointer.anchoredPosition.x - 25, 0.5f).SetLoops(-1, LoopType.Yoyo);
        if (Timer_Transform) Timer_Transform.localEulerAngles = new Vector3(0, 0, 359);
        if (Timer_Transform) Timer_Transform.DORotate(new Vector3(0, 0, 0), 1, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);

        StartCoroutine(StartBetting());
    }

    private IEnumerator BallRevolution(int number)
    {
        yield return new WaitForSecondsRealtime(5);

        while (Ball_Transform.localPosition.x < -167 && Ball_Transform.localPosition.y > 99) 
        {
            Ball_Transform.localPosition = new Vector2(Ball_Transform.localPosition.x + 5f, Ball_Transform.localPosition.y - 3f);
            ballMovement.timeScale -= 0.05f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        BallManager.parent_Transform = BallStopPoint[number];
        BallStopPoint[number].gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void UpdatePreviousNumbers(int addition)
    {
        PreviousNumbers.RemoveAt(PreviousNumbers.Count - 1);
        PreviousNumbers.Insert(0, addition);
        for (int i = 0; i < PreviousNumbers.Count; i++)
        {
            if (Previous_Text[i]) Previous_Text[i].text = PreviousNumbers[i].ToString();
            if (NumberCode[PreviousNumbers[i]] == "black")
            {
                if (Previous_Image[i]) Previous_Image[i].color = blackColor;
            }
            else if (NumberCode[PreviousNumbers[i]] == "red")
            {
                if (Previous_Image[i]) Previous_Image[i].color = redColor;
            }
            else
            {
                if (Previous_Image[i]) Previous_Image[i].color = greenColor;
            }
        }
    }

    internal IEnumerator StopAtNumber()
    {
        if (winNumber_Text) winNumber_Text.text = numberAnnounced.ToString();

        if (NumberCode[numberAnnounced] == "black")
        {
            if (winNumber_Image) winNumber_Image.color = blackColor;
        }
        else if (NumberCode[numberAnnounced] == "red")
        {
            if (winNumber_Image) winNumber_Image.color = redColor;
        }
        else
        {
            if (winNumber_Image) winNumber_Image.color = greenColor;
        }

        if (winNumber_Object) winNumber_Object.SetActive(true);

        UpdatePreviousNumbers(numberAnnounced);

        yield return new WaitForSecondsRealtime(3);
        BallStopPoint[numberAnnounced].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(StartBetting());
        if (SpinPanel_Object) SpinPanel_Object.SetActive(false);
        OuterRouletteMovement.Pause();
        OuterRouletteMovement.Kill();
        OuterRouletteMovement = null;
        InnerRouletteMovement.Pause();
        InnerRouletteMovement.Kill();
        InnerRouletteMovement = null;
        ballMovement.Pause();
        ballMovement.Kill();
        ballMovement = null;
    }

    private IEnumerator StartBetting()
    {
        _rouletteManager.CancelBet();
        if (MainPopup_Object) MainPopup_Object.SetActive(true);
        if (StartBettingPopup) StartBettingPopup.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        if (StartBettingPopup) StartBettingPopup.SetActive(false);
        if (MainPopup_Object) MainPopup_Object.SetActive(false);
        for (int i = Timer; i >= 0; i--) 
        {
            if (Timer_Text) Timer_Text.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }
        if (StopBettingPopup) StopBettingPopup.SetActive(true);
        if (MainPopup_Object) MainPopup_Object.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        StartSpinning();
        if (StopBettingPopup) StopBettingPopup.SetActive(false);
        if (MainPopup_Object) MainPopup_Object.SetActive(false);
    }
}
