using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scr_UIManager : StaticInstance<scr_UIManager>
{
    private void Start()
    {
        GetHitmarkers();
    }

    private void Update()
    {
        UpdateCrosshair();
    }

    [Header("Debug")]

    [SerializeField]
    private TMP_Text state; 
    [SerializeField]
    private TMP_Text weaponState; 
    [SerializeField]
    private TMP_Text speed;

    public void UpdateState(PlayerState _state)
    {
        this.state.text = _state.ToString();
    }

    public void UpdateWeaponState(WeaponState _state)
    {
        this.weaponState.text = _state.ToString();
    }

    public void UpdateSpeed(float _speed)
    {
        this.speed.text = _speed.ToString("F1");
    }

    [Header("Ammo")]
    [SerializeField]
    private TMP_Text ammo;
    public void UpdateAmmo(string _ammo)
    {
        this.ammo.text = _ammo;
    }

    [Header("Crosshair")]
    public RectTransform Crosshair;

    [SerializeField]
    private float originSize, changeSpeed;
    private float currentSize, targetSize, oldTargetSize, currentAlpha, targetAlpha;

    private bool ads;

    private int type;

    public void SetVelocity(float _velocity)
    {
        if (ads) return;
        targetSize = originSize + _velocity * 5;
        oldTargetSize = targetSize;
    }

    public void SetType(int _type)
    {
        this.type = _type;
    }

    public void ADS()
    {
        ads = true;
        targetSize = 0;
        targetAlpha = 0;
    }

    public void NotADS()
    {
        ads = false;
        targetSize = oldTargetSize;
        targetAlpha = 1;
    }

    private void UpdateCrosshair()
    {
        float _vel = 0;
        float _vel2 = 0;

        currentSize = Mathf.SmoothDamp(currentSize, targetSize, ref _vel, changeSpeed);
        Crosshair.sizeDelta = new Vector2(currentSize, currentSize);

        currentAlpha = Mathf.SmoothDamp(currentAlpha, targetAlpha, ref _vel2, changeSpeed / 2);
        Crosshair.GetComponent<CanvasGroup>().alpha = currentAlpha;

    }

    [Header("Hitmarker")]
    [SerializeField]
    private RectTransform hitmarker;
    private bool hitmarkerActive;

    [SerializeField]
    private Vector2 hitmarkerSize;
    [SerializeField]
    private float hitmarkerDuration;

    private Image[] hitmarkerLines;
    Coroutine hitmarkerCoroutine;

    private void GetHitmarkers()
    {
        hitmarkerLines = hitmarker.GetChild(0).GetComponentsInChildren<Image>();
    }

    public void ShowHitmarker(int _shotType)
    {
        if (hitmarkerActive)
            StopCoroutine(hitmarkerCoroutine);
        hitmarkerCoroutine = StartCoroutine(Hitmarker(_shotType));
    }

    private IEnumerator Hitmarker(int _shotType)
    {
        hitmarkerActive = true;
        var _cg = hitmarker.GetComponent<CanvasGroup>();

        foreach (var _line in hitmarkerLines)
        {
            switch (_shotType)
            {
                case 0:
                    _line.color = new Color(0, 0, 0, 0);
                    break;
                case 1:
                    _line.color = Color.white;
                    break;
                case 2:
                    _line.color = Color.red;
                    break;
                default:
                    _line.color = Color.white;
                    break;
            }

        }

        _cg.alpha = 1;
        hitmarker.gameObject.SetActive(true);

        hitmarker.sizeDelta = hitmarkerSize;

        float _timer = 0;
        while (_timer < hitmarkerDuration)
        {
            _cg.alpha = Mathf.Lerp(_cg.alpha, 0, _timer);
            hitmarker.sizeDelta = Vector2.Lerp(hitmarkerSize, hitmarkerSize * 5, _timer);

            _timer += Time.deltaTime;
            yield return null;
        }


        hitmarker.gameObject.SetActive(false);
        hitmarkerActive = false;
    }

    [Header("Spawning")]
    [SerializeField]
    private GameObject spawnScreen;

    public void ShowSpawnscreen(bool _state)
    {
        spawnScreen.SetActive(_state);
    }


}
