using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Brick : UI_Base
{
    #region Field

    [HideInInspector] public int _id;
    [HideInInspector] public List<int> neighborNums = new();
    [HideInInspector] public int neighborBombCount;
    [HideInInspector] public bool isAmIBomb;

    [SerializeField] private TMP_Text _ambientBombsCountText;
    [SerializeField] private Image _capImg;

    private Define.BrickState _state;
    private Animator _animator;
    private bool _isNeighborPress = false;
    private bool _initialized = false;

    #endregion

    private void OnDisable()
    {
        Main.Mine.MaskOffAction -= TakeOffYourMask;

    }

    #region Init

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
        Initialize();

        Main.Mine.MaskOffAction -= TakeOffYourMask;
        Main.Mine.MaskOffAction += TakeOffYourMask;

        _isNeighborPress = false;

        Main.Mine.BrickNeighborCheck(ref neighborNums, _id);
        neighborBombCount = Main.Mine.NeighborBombCount(ref neighborNums);
        _ambientBombsCountText.text = $"{neighborBombCount}";
        _ambientBombsCountText.color = Main.Mine.numberColors[neighborBombCount];


        _capImg.sprite = Main.Mine.nullImg;
        _state = Define.BrickState.Live;

        Idle();
    }

    #endregion

    private void Idle()
    {
        if (IsDead()) return;

        _animator.SetTrigger(Main.Mine.idle);
    }

    public void Pressed()
    {
        if (IsDead()) return;

        _state = Define.BrickState.Dead;

        if (isAmIBomb)
        {
            _animator.SetTrigger(Main.Mine.clickBomb);
            Main.Mine.GameOverAction?.Invoke();
        }
        else
        {
            _animator.SetTrigger(Main.Mine.number);

            if (neighborBombCount == 0)
                Main.Mine.ZeroInfection(_id);

            Main.Mine.PressAction?.Invoke();
        }

    }


    #region MousePointerEvent

    private void OnMouseButtonClick(PointerEventData eventData)
    {
        if (Main.Mine.IsGameOver()) return;
        if (IsDead()) return;

        if (Main.Mine._isShield && !Main.Mine.isPressAnotherButton)
        {
            Main.Mine._isShield = false;
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (!Main.Mine.isRigthPress && _capImg.sprite != Main.Mine.flagImg && !Main.Mine.isPressAnotherButton)
                Pressed();
        }
        else
        {
            if (!Main.Mine.isLeftPress && !Main.Mine.isPressAnotherButton)
            {
                if (_capImg.sprite == Main.Mine.flagImg)
                {
                    Main.Mine.currentBombCount++;
                    _capImg.sprite = Main.Mine.questionImg;
                }
                else if (_capImg.sprite == Main.Mine.questionImg)
                {
                    _capImg.sprite = Main.Mine.nullImg;
                }
                else if (_capImg.sprite == Main.Mine.nullImg)
                {
                    Main.Mine.currentBombCount--;
                    _capImg.sprite = Main.Mine.flagImg;
                }

            }
        }

        //    NeighborbrickOff();

        Main.Mine.PressAction?.Invoke();
    }


    private void OnMouseButtonDown(PointerEventData eventData)
    {
        if (Main.Mine.IsGameOver()) return;


        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Main.Mine.isLeftPress = true;

            if (Main.Mine.isRigthPress)
            {
                Main.Mine._isShield = true;
                Main.Mine.isPressAnotherButton = true;
            }


            if (!IsDead())
                _animator.SetTrigger(Main.Mine.press);

        }
        else
        {
            Main.Mine.isRigthPress = true;

            if (Main.Mine.isLeftPress)
            {
                Main.Mine._isShield = true;
                Main.Mine.isPressAnotherButton = true;
            }
        }

        NeighborbrickOn();
    }

    private void OnMouseButtonUp(PointerEventData eventData)
    {
        if (Main.Mine.IsGameOver()) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Main.Mine.isLeftPress = false;

            if (!IsDead())
                Idle();
        }
        else
        {
            Main.Mine.isRigthPress = false;
        }

        if (!Main.Mine.isLeftPress && !Main.Mine.isRigthPress)
        {
            Main.Mine.isPressAnotherButton = false;
        }

        NeighborbrickOff();
    }

    private void OnMouseButtonEnter(PointerEventData eventData)
    {
        if (Main.Mine.IsGameOver()) return;

        NeighborbrickOn();

    }
    private void OnMouseButtonExit(PointerEventData eventData)
    {
        if (Main.Mine.IsGameOver()) return;

        NeighborbrickOff();

    }

    #endregion

    #region Util

    private void NeighborbrickOn()
    {
        if (Main.Mine.isLeftPress && Main.Mine.isRigthPress)
        {
            if (_isNeighborPress) return;

            //  Debug.Log(_id + "ENTER");

            if (_state != Define.BrickState.Dead)
                _animator.SetTrigger(Main.Mine.press);

            for (int i = 0; i < neighborNums.Count; i++)
            {
                if (Main.Mine.bricks[neighborNums[i]]._state != Define.BrickState.Dead)
                    Main.Mine.bricks[neighborNums[i]]._animator.SetTrigger(Main.Mine.press);
            }

            _isNeighborPress = true;
        }
    }
    private void NeighborbrickOff()
    {

        if (!_isNeighborPress) return;

        //  Debug.Log(_id + "EXIT");

        if (_state != Define.BrickState.Dead)
            _animator.SetTrigger(Main.Mine.idle);

        for (int i = 0; i < neighborNums.Count; i++)
        {
            if (Main.Mine.bricks[neighborNums[i]]._state != Define.BrickState.Dead)
                Main.Mine.bricks[neighborNums[i]]._animator.SetTrigger(Main.Mine.idle);

        }

        _isNeighborPress = false;
    }

    private void TakeOffYourMask()
    {
        if (IsDead()) return;

        if (isAmIBomb)
        {
            if (_capImg.sprite == Main.Mine.flagImg)
                return;

            _animator.SetTrigger(Main.Mine.bomb);
        }
        else
        {
            if (_capImg.sprite == Main.Mine.flagImg)
            {
                Main.Mine.currentBombCount++;
                _animator.SetTrigger(Main.Mine.notBomb);
            }
        }
    }

    private bool IsDead()
    {
        return _state == Define.BrickState.Dead;
    }
    public void ReturnPool()
    {
        Main.Pool._brickPool.Release(this);
    }

    #endregion
}
