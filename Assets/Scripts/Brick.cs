using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class Brick : UI_Base
{
    #region Field

    [HideInInspector] public int _id;
    [HideInInspector] public bool _isAmIBomb;

    private int _neighborBombCount;
    private Define.BlockState _state;
    private Animator _animator;

    [SerializeField] private TMP_Text _ambientBombsCountText;
    [SerializeField] private Image _capImg;

    public List<int> neighborNums = new();

    #endregion

    public void Initialize() // 현재 풀링방법으로 생성자 사용못함. 생성자 역할 대신함
    {
        _animator = GetComponent<Animator>();

        BindEvent(OnMouseButtonClick, UIEvent.Click);
        BindEvent(OnMouseButtonUp, UIEvent.PointerUp);
        BindEvent(OnMouseButtonDown, UIEvent.PointerDown);
        BindEvent(OnMouseButtonEnter, UIEvent.PointerEnter);
        BindEvent(OnMouseButtonExit, UIEvent.PointerExit);

        Refresh();
    }

    public void Refresh()
    {
        Main.Mine.BrickNeighborCheck(ref neighborNums, _id);
        _neighborBombCount = Main.Mine.NeighborBombCount(ref neighborNums);

        _ambientBombsCountText.text = $"{_neighborBombCount}";
        _ambientBombsCountText.color = Main.Mine.numberColors[_neighborBombCount];

        _capImg.sprite = Main.Mine._nullImg;
        Idle();
    }



    private void Idle()
    {
        if (IsPressed()) return;

        _state = Define.BlockState.Idle;
    }


    private void Pressed()
    {
        if (IsPressed()) return;

        _state = Define.BlockState.Pressed;

        if (_isAmIBomb)
        {
            if (_capImg.sprite == Main.Mine._flagImg)
                Main.Mine._leftBomb++;

            _animator.SetTrigger(Main.Mine._clickBomb);
            Main.Mine.gameOver?.Invoke();
        }
        else
        {
            _animator.SetTrigger(Main.Mine._number);
            // if (Main.Mine._number == 0)   Main.Mine.neighborZeroCheck?.Invoke();
            Main.Mine.PressAction?.Invoke();
        }

    }
    private void NeighborbrickOn()
    {
        if (IsPressed()) return;

        _animator.SetTrigger(Main.Mine._press);

        for (int i = 0; i < neighborNums.Count; i++)
        {
            if (Main.Mine._bricks[neighborNums[i]]._state != Define.BlockState.Pressed)
                Main.Mine._bricks[neighborNums[i]]._animator.SetTrigger(Main.Mine._press);
        }
    }
    private void NeighborbrickOff()
    {
        if (IsPressed()) return;

        _animator.SetTrigger(Main.Mine._idle);

        for (int i = 0; i < neighborNums.Count; i++)
        {
            if (Main.Mine._bricks[neighborNums[i]]._state != Define.BlockState.Pressed)
                Main.Mine._bricks[neighborNums[i]]._animator.SetTrigger(Main.Mine._idle);
        }
    }



    #region 마우스조작

    private void OnMouseButtonClick(PointerEventData eventData)
    {
        if (IsGameOver()) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!Main.Mine._isRigthPress)
                Pressed();
        }
        else
        {
            if (!Main.Mine._isLeftPress)
            {
                if (_capImg.sprite == Main.Mine._flagImg)
                {
                    Main.Mine._leftBomb++;
                    _capImg.sprite = Main.Mine._questionImg;
                }
                else if (_capImg.sprite == Main.Mine._questionImg)
                {
                    _capImg.sprite = Main.Mine._nullImg;
                }
                else if (_capImg.sprite == Main.Mine._nullImg)
                {
                    Main.Mine._leftBomb--;
                    _capImg.sprite = Main.Mine._flagImg;
                }

                Main.Mine.PressAction?.Invoke();
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

            _animator.SetTrigger(Main.Mine._press);

        }
        else
        {
            Main.Mine._isRigthPress = true;

            if (Main.Mine._isLeftPress)
                Main.Mine._isPressAnotherButton = true;

        }

        if (Main.Mine._isLeftPress && Main.Mine._isRigthPress)
            NeighborbrickOn();


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
        {
            Main.Mine._isPressAnotherButton = false;
        }

        if (!Main.Mine._isLeftPress || !Main.Mine._isRigthPress)
            NeighborbrickOff();
    }

    private void OnMouseButtonEnter(PointerEventData data)
    {
        if (IsGameOver()) return;

        if (Main.Mine._isLeftPress && Main.Mine._isRigthPress)
        {
            NeighborbrickOn();
        }
    }
    private void OnMouseButtonExit(PointerEventData data)
    {
        if (IsGameOver()) return;

        if (Main.Mine._isLeftPress && Main.Mine._isRigthPress)
        {
            NeighborbrickOff();
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
                Main.Mine._leftBomb++;
                _animator.SetTrigger(Main.Mine._notBomb);
            }
        }
    }

    public void ZeroBrickInfection()
    {
        if (_neighborBombCount == 0)
        {
            // 자기 이웃한테도 실행시키기 neighborNums   Pressed();
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
        Main.Mine.gameOver -= TakeOffYourMask;
    }

}