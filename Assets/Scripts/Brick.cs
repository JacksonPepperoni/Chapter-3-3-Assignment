using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Brick : UI_Base
{

    private Define.BlockState _state;
    public Define.BlockState State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;

            switch (_state)
            {
                case Define.BlockState.None:
                    _flagObj.sprite = sprites[0];
                    break;
                case Define.BlockState.Flag:
                    _flagObj.sprite = sprites[2];
                    break;
                case Define.BlockState.QuestionMark:
                    _flagObj.sprite = sprites[1];
                    break;
                case Define.BlockState.Pressed:
                    _capObj.SetActive(false);
                    break;
            }

        }
    }

    public Sprite[] sprites;


    [HideInInspector] public int id = 0; // 몇번째 칸인지
    [SerializeField] private TMP_Text _ambientBombsCountText;


    [SerializeField] private GameObject _contentsObj;
    [SerializeField] private GameObject _capObj;

    [SerializeField] private Image _flagObj;

    bool isBomb = false;


    public void Initialize() // 겜시작할때 한번만 실행되면 되는것들. 숫자인지 폭탄인지같은거
    {
        BindEvent(OnMouseButtonClick, UIEvent.Click);
        BindEvent(OnMouseButtonUp, UIEvent.PointerUp);
        BindEvent(OnMouseButtonDown, UIEvent.PointerDown);
        BindEvent(OnMouseButtonEnter, UIEvent.PointerEnter);


        // 기본칸으로 되돌리고 내가 폭탄이 아닐때 
        if (isBomb)
        {


        }
        else
        {
            int num = Main.Game.NumberOfAmbientBombs(id);
            _ambientBombsCountText.text = $"{num}";
            _ambientBombsCountText.color = Main.Game.numberColors[num];
        }

        State = Define.BlockState.None;

    }
    public void ZeroIdCheck()
    {
        if (id == 0)
        {

        }
    }

    bool isLeftPressed = false;
    bool isRigthPressed = false;



    private void OnMouseButtonClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (State == Define.BlockState.Flag)
                return;

            State = Define.BlockState.Pressed;
        }
        else
        {
            if (isLeftPressed) return;

            if (State == Define.BlockState.None)
                State = Define.BlockState.Flag;
            else if (State == Define.BlockState.Flag)
                State = Define.BlockState.QuestionMark;
            else
                State = Define.BlockState.None;

        }
    }

    private void OnMouseButtonUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isLeftPressed = false;
        }

    }
    private void OnMouseButtonDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isLeftPressed = true;
        }
        else
        {
            if (isLeftPressed)
                Debug.Log("주위범위보기");
        }
    }


    private void OnMouseButtonEnter(PointerEventData eventData)
    {
        Debug.Log(id + "스침");
    }
}