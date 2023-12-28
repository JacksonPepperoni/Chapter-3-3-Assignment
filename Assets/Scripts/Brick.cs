using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Brick : UI_Base
{
    #region Field

    [HideInInspector] public int _id;
    [HideInInspector] public bool _isAmIBomb;

    [SerializeField] private TMP_Text _ambientBombsCountText;
    [SerializeField] private Image _capImg;

    private int _neighborBombCount;
    private Define.BrickState _state;
    private Animator _animator;

    [SerializeField] private List<int> neighborNums = new();

    private bool _initialized = false;

    #endregion


    public void Initialize()
    {
        if (_initialized) return;

        _animator = GetComponent<Animator>();
        BindEvent(OnMouseButtonClick, UIEvent.Click);
        BindEvent(OnMouseButtonUp, UIEvent.PointerUp);
        BindEvent(OnMouseButtonDown, UIEvent.PointerDown);
        BindEvent(OnMouseButtonEnter, UIEvent.PointerEnter);
        BindEvent(OnMouseButtonExit, UIEvent.PointerExit);


        _initialized = true;
    }

    public void Refresh()
    {
        Main.Mine.DisclosureAction -= TakeOffYourMask;
        Main.Mine.DisclosureAction += TakeOffYourMask;

        Main.Mine.BrickNeighborCheck(ref neighborNums, _id);
        _neighborBombCount = Main.Mine.NeighborBombCount(ref neighborNums);

        _ambientBombsCountText.text = $"{_neighborBombCount}";
        _ambientBombsCountText.color = Main.Mine.numberColors[_neighborBombCount];

        _capImg.sprite = Main.Mine.nullImg;

        _state = Define.BrickState.Idle;
    }



    private void Idle()
    {
        if (IsPressed()) return;

        _state = Define.BrickState.Idle;
    }


    private void Pressed()
    {
        if (IsPressed()) return;

        _state = Define.BrickState.Pressed;

        if (_isAmIBomb)
        {
            if (_capImg.sprite == Main.Mine.flagImg)
                Main.Mine.leftBomb++;

            _animator.SetTrigger(Main.Mine.clickBomb);
            Main.Mine.GameOverAction?.Invoke();
        }
        else
        {
          //  ZeroBrickInfection();

            _animator.SetTrigger(Main.Mine.number);
            // if (Main.Mine._number == 0)   Main.Mine.neighborZeroCheck?.Invoke();
            Main.Mine.PressAction?.Invoke();
        }

    }
    private void NeighborbrickOn()
    {
        for (int i = 0; i < neighborNums.Count; i++)
        {
            if (Main.Mine.bricks[neighborNums[i]]._state == Define.BrickState.Pressed)
                return;

            if (Main.Mine.bricks[neighborNums[i]]._state == Define.BrickState.Idle)
            {
                Main.Mine.bricks[neighborNums[i]]._state = Define.BrickState.Pressing;
                Main.Mine.bricks[neighborNums[i]]._animator.SetTrigger(Main.Mine.press);
            }
        }
    }
    private void NeighborbrickOff()
    {
        for (int i = 0; i < neighborNums.Count; i++)
        {
            if (Main.Mine.bricks[neighborNums[i]]._state == Define.BrickState.Pressed)
                return;

            if (Main.Mine.bricks[neighborNums[i]]._state == Define.BrickState.Pressing)
            {
                Main.Mine.bricks[neighborNums[i]]._state = Define.BrickState.Idle;
                Main.Mine.bricks[neighborNums[i]]._animator.SetTrigger(Main.Mine.idle);
            }
        }
    }



    #region 마우스조작

    private void OnMouseButtonClick(PointerEventData eventData)
    {
        if (IsGameOver()) return;
        if (IsPressed()) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!Main.Mine.isRigthPress)
                Pressed();
        }
        else
        {
            if (!Main.Mine.isLeftPress)
            {
                if (_capImg.sprite == Main.Mine.flagImg)
                {
                    Main.Mine.leftBomb++;
                    _capImg.sprite = Main.Mine.questionImg;
                }
                else if (_capImg.sprite == Main.Mine.questionImg)
                {
                    _capImg.sprite = Main.Mine.nullImg;
                }
                else if (_capImg.sprite == Main.Mine.nullImg)
                {
                    Main.Mine.leftBomb--;
                    _capImg.sprite = Main.Mine.flagImg;
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
            Main.Mine.isLeftPress = true;

            if (Main.Mine.isRigthPress)
                Main.Mine.isPressAnotherButton = true;


            if (!IsPressed()) 
            _animator.SetTrigger(Main.Mine.press);

        }
        else
        {
            Main.Mine.isRigthPress = true;

            if (Main.Mine.isLeftPress)
                Main.Mine.isPressAnotherButton = true;

        }

       // if (Main.Mine.isLeftPress && Main.Mine.isRigthPress) NeighborbrickOn();


    }


    private void OnMouseButtonUp(PointerEventData eventData)
    {
        if (IsGameOver()) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Main.Mine.isLeftPress = false;
            _animator.SetTrigger(Main.Mine.idle);
        }
        else
        {
            Main.Mine.isRigthPress = false;
        }

        if (!Main.Mine.isLeftPress && !Main.Mine.isRigthPress)
            Main.Mine.isPressAnotherButton = false;

     //  if (!Main.Mine.isLeftPress || !Main.Mine.isRigthPress) NeighborbrickOff();
    }

    private void OnMouseButtonEnter(PointerEventData data)
    {
        if (IsGameOver()) return;

      //  if (Main.Mine.isLeftPress && Main.Mine.isRigthPress) NeighborbrickOn();

    }
    private void OnMouseButtonExit(PointerEventData data)
    {
        if (IsGameOver()) return;

      //  if (Main.Mine.isLeftPress && Main.Mine.isRigthPress)  NeighborbrickOff();
    }
    #endregion

    public void TakeOffYourMask()
    {
        if (IsPressed()) return;

        if (_isAmIBomb)
        {
            _animator.SetTrigger(Main.Mine.bomb);
        }
        else
        {
            if (_capImg.sprite == Main.Mine.flagImg)
            {
                Main.Mine.leftBomb++;
                _animator.SetTrigger(Main.Mine.notBomb);
            }
        }
    }

    public void ZeroBrickInfection()
    {
        if (_neighborBombCount == 0 && !IsPressed())
        {
            //  Pressed();

            Debug.Log("0번 전염");

            for (int i = 0; i < neighborNums.Count; i++)
            {
                Main.Mine.bricks[neighborNums[i]].ZeroBrickInfection();
            }

        }
    }

    bool IsGameOver()
    {
        return Main.Mine.gameState == Define.GameState.GameOver;
    }
    bool IsPressed()
    {
        return _state == Define.BrickState.Pressed;
    }

    private void OnDisable()
    {
        Main.Mine.DisclosureAction -= TakeOffYourMask;
    }

    public void PoolReturn()
    {
        Main.Resource.Destroy(this.gameObject);
    }

}