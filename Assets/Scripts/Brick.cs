using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class Brick : UI_Base
{
    #region Field

    private int _id;
    private int _neighborBombCount;
    private bool _isAmIBomb;
    private Define.BlockState _state;
    private Animator _animator;

    [SerializeField] private TMP_Text _ambientBombsCountText;
    [SerializeField] private Image _capImg;

    [HideInInspector] public List<int> neighborNums = new();


    #endregion

    public void Initialize(int i, int neighbor, bool bomb) // 현재 풀링방법으로 생성자 사용못함. 생성자 역할 대신함
    {
        _animator = GetComponent<Animator>();

        _id = i;
        _neighborBombCount = neighbor;
        _isAmIBomb = bomb;

        BindEvent(OnMouseButtonClick, UIEvent.Click);
        BindEvent(OnMouseButtonUp, UIEvent.PointerUp);
        BindEvent(OnMouseButtonDown, UIEvent.PointerDown);
        BindEvent(OnMouseButtonEnter, UIEvent.PointerEnter);
        BindEvent(OnMouseButtonExit, UIEvent.PointerExit);

        Main.Mine.neighborZeroCheck -= ZeroBrickInfection;
        Main.Mine.neighborZeroCheck += ZeroBrickInfection;
        Main.Mine.gameOver -= TakeOffYourMask;
        Main.Mine.gameOver += TakeOffYourMask;

        Main.Mine._isLeftPress = false;
        Main.Mine._isRigthPress = false;
        Main.Mine._isPressAnotherButton = false;

        Main.Mine.BrickNeighborCheck(ref neighborNums, _id);

        Refresh();
    }

    public void Refresh()
    {
        if (!_isAmIBomb)
        {
            _ambientBombsCountText.text = $"{_neighborBombCount}";
            _ambientBombsCountText.color = Main.Mine.numberColors[_neighborBombCount];
        }

        _capImg.sprite = Main.Mine._nullImg;

        Idle();
    }



    private void Idle()
    {
        if (IsPressed()) return;

        _state = Define.BlockState.Idle;
        _animator.SetBool(Main.Mine._isPress, false);
    }

    private void LookAround()
    {
        Debug.Log("주위범위보기");
    }

    private void Pressed()
    {
        if (IsPressed()) return;

        _state = Define.BlockState.Pressed;
        _animator.SetBool(Main.Mine._isPress, true);

        if (_isAmIBomb)
        {
            _animator.SetTrigger(Main.Mine._clickBomb);
            Main.Mine.gameOver?.Invoke();
        }
        else
        {
            _animator.SetTrigger(Main.Mine._number);

            if (Main.Mine._number == 0)
                Main.Mine.neighborZeroCheck?.Invoke();


        }

        Main.Mine.PressAction?.Invoke();

    }



    #region 마우스조작

    private void OnMouseButtonClick(PointerEventData eventData)
    {
        if (IsGameOver()) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Main.Mine._isPressAnotherButton && Main.Mine._isRigthPress)
            {
                Main.Mine._isPressAnotherButton = false;
                return;
            }

            if (!Main.Mine._isRigthPress)
                Pressed();
        }
        else
        {
            if (Main.Mine._isPressAnotherButton && Main.Mine._isLeftPress)
            {
                Main.Mine._isPressAnotherButton = false;
                return;
            }

            if (!Main.Mine._isLeftPress)
            {
                if (_capImg.sprite == Main.Mine._flagImg)
                    _capImg.sprite = Main.Mine._questionImg;
                else if (_capImg.sprite == Main.Mine._questionImg)
                    _capImg.sprite = Main.Mine._nullImg;
                else if (_capImg.sprite == Main.Mine._nullImg)
                    _capImg.sprite = Main.Mine._flagImg;

            }
        }
    }


    private void OnMouseButtonDown(PointerEventData eventData)
    {
        if (IsGameOver()) return;


        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Main.Mine._isLeftPress = true;

            if (Main.Mine._isRigthPress)
                Main.Mine._isPressAnotherButton = true;

            if (_state != Define.BlockState.Pressed)
                _animator.SetTrigger(Main.Mine._press);

        }
        else
        {
            Main.Mine._isRigthPress = true;

            if (Main.Mine._isLeftPress)
                Main.Mine._isPressAnotherButton = true;

        }

        if (Main.Mine._isLeftPress && Main.Mine._isRigthPress)
            LookAround();


    }


    private void OnMouseButtonUp(PointerEventData eventData)
    {
        if (IsGameOver()) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Main.Mine._isLeftPress = false;
            _animator.SetTrigger(Main.Mine._idle);
        }
        else
        {
            Main.Mine._isRigthPress = false;
        }


        if (!Main.Mine._isLeftPress && !Main.Mine._isRigthPress)
            Main.Mine._isPressAnotherButton = false;

    }

    private void OnMouseButtonEnter(PointerEventData data)
    {
        if (IsGameOver()) return;

        if (Main.Mine._isLeftPress && Main.Mine._isRigthPress)
        {
            LookAround();
        }
    }
    private void OnMouseButtonExit(PointerEventData data)
    {
        if (IsGameOver()) return;

        if (Main.Mine._isLeftPress && Main.Mine._isRigthPress)
        {
            _state = Define.BlockState.Idle;
            _animator.SetTrigger(Main.Mine._idle);
        }
    }
    #endregion

    public void TakeOffYourMask()
    {
        if (IsPressed()) return;

        if (_isAmIBomb)
        {
            _animator.SetTrigger(Main.Mine._bomb);
        }
        else
        {
            if (_capImg.sprite == Main.Mine._flagImg)
            {
                _animator.SetTrigger(Main.Mine._notBomb);
                return;
            }
        }
    }

    public void ZeroBrickInfection()
    {
        if (_neighborBombCount == 0)
        {
            Pressed();
            // 자기 이웃한테도 실행시키기 neighborNums
        }
    }

    bool IsGameOver()
    {
        return Main.Mine._gameState == Define.GameState.GameOver;
    }
    bool IsPressed()
    {
        return _state == Define.BlockState.Pressed;
    }


    private void OnDisable()
    {
        Main.Mine.neighborZeroCheck -= ZeroBrickInfection;
        Main.Mine.gameOver -= TakeOffYourMask;
    }

}