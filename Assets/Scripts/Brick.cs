using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Brick : UI_Base
{
    [HideInInspector] public int id = 0;
    [HideInInspector] public int neighborBombCount = 0;
    [HideInInspector] public bool _isAmIBomb = false;
    [HideInInspector] public Sprite _flagImg = null;
    [HideInInspector] public Sprite _questionImg = null;
    [HideInInspector] public Sprite _nullImg = null;

    [SerializeField] private TMP_Text _ambientBombsCountText;
    [SerializeField] public Image _capImg;

    private Animator _animator;
    private readonly int _idle = Animator.StringToHash("Idle");
    private readonly int _isPress = Animator.StringToHash("isPress");
    private readonly int _press = Animator.StringToHash("Press");
    private readonly int _number = Animator.StringToHash("Number");
    private readonly int _clickBomb = Animator.StringToHash("ClickBomb");
    private readonly int _bomb = Animator.StringToHash("Bomb");
    private readonly int _notBomb = Animator.StringToHash("NotBomb");

    private Define.BlockState _state;
    private bool _isLeftPress = false;
    private bool _isRigthPress = false;
    private bool _isPressAnotherButton = false;

    public void Initialize()
    {
        _animator = GetComponent<Animator>();

        BindEvent(OnMouseButtonClick, UIEvent.Click);
        BindEvent(OnMouseButtonUp, UIEvent.PointerUp);
        BindEvent(OnMouseButtonDown, UIEvent.PointerDown);
        BindEvent(OnMouseButtonEnter, UIEvent.PointerEnter);

        Main.Game.neighborZeroCheck -= ZeroBrickInfection;
        Main.Game.neighborZeroCheck += ZeroBrickInfection;
        Main.Game.gameOver -= TakeOffYourMask;
        Main.Game.gameOver += TakeOffYourMask;

        Refresh();
    }

    public void Refresh()
    {
        if (!_isAmIBomb)
        {
            _ambientBombsCountText.text = $"{neighborBombCount}";
            _ambientBombsCountText.color = Main.Game.numberColors[neighborBombCount];
        }

        _capImg.sprite = _nullImg;
        Idle();
    }


    private void Idle()
    {
        if (_state == Define.BlockState.Pressed) return;

        _state = Define.BlockState.Idle;
        _animator.SetBool(_isPress, false);
    }

    private void LookAround()
    {
        Debug.Log("주위범위보기");
    }

    private void Pressed()
    {
        if (_state == Define.BlockState.Pressed) return;

        _state = Define.BlockState.Pressed;
        _animator.SetBool(_isPress, true);

        if (_isAmIBomb)
        {
            _animator.SetTrigger(_clickBomb);
            Main.Game.gameOver?.Invoke();
        }
        else
        {
            _animator.SetTrigger(_number);

            if (_number == 0)
                Main.Game.neighborZeroCheck?.Invoke();

        }

        Main.Game.GameConditionUpdate();

    }

    public void TakeOffYourMask()
    {
        if (_state == Define.BlockState.Pressed) return;

        if (_isAmIBomb)
        {
            _animator.SetTrigger(_bomb);
        }
        else
        {
            if (_capImg.sprite == _flagImg)
            {
                _animator.SetTrigger(_notBomb);
                return;
            }
        }
    }

    public void ZeroBrickInfection()
    {
        if (neighborBombCount == 0)
            Pressed();
    }


    #region 마우스조작

    private void OnMouseButtonClick(PointerEventData eventData)
    {
        if (Main.Game._gameState == Define.GameState.GameOver) return;


        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_isPressAnotherButton && !_isRigthPress)
            {
                _isPressAnotherButton = false;
                return;
            }

            if (!_isRigthPress)
                Pressed();
        }
        else
        {
            if (_isPressAnotherButton && !_isLeftPress)
            {
                _isPressAnotherButton = false;
                return;
            }



            if (!_isLeftPress)
            {
                if (_capImg.sprite == _flagImg)
                    _capImg.sprite = _questionImg;
                else if (_capImg.sprite == _questionImg)
                    _capImg.sprite = _nullImg;
                else if (_capImg.sprite == _nullImg)
                    _capImg.sprite = _flagImg;

            }
        }
    }


    private void OnMouseButtonDown(PointerEventData eventData)
    {
        if (Main.Game._gameState == Define.GameState.GameOver) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _isLeftPress = true;

            if (_state != Define.BlockState.Pressed)
                _animator.SetTrigger(_press);

            if (_isRigthPress && !_isPressAnotherButton)
                _isPressAnotherButton = true;

        }
        else
        {
            _isRigthPress = true;

            if (_isLeftPress && !_isPressAnotherButton)
                _isPressAnotherButton = true;

        }

    }


    private void OnMouseButtonUp(PointerEventData eventData)
    {
        if (Main.Game._gameState == Define.GameState.GameOver) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _animator.SetTrigger(_idle);
            _isLeftPress = false;
        }
        else
        {
            _isRigthPress = false;
        }
    }


    private void OnMouseButtonEnter(PointerEventData data)
    {
        if (Main.Game._gameState == Define.GameState.GameOver) return;

        if (_isLeftPress && _isRigthPress)
        {
            LookAround();
        }
    }

    #endregion



    private void OnDisable()
    {
        Main.Game.neighborZeroCheck -= ZeroBrickInfection;
        Main.Game.gameOver -= TakeOffYourMask;
    }

}