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
    public TMP_Text state;
    public TMP_Text weaponState;
    public TMP_Text speed;

    public void UpdateState(PlayerState state)
    {
        this.state.text = state.ToString();
    }

    public void UpdateWeaponState(WeaponState state)
    {
        this.weaponState.text = state.ToString();
    }

    public void UpdateSpeed(float speed)
    {
        this.speed.text = speed.ToString("F1");
    }


    [Header("Ammo")]
    public TMP_Text ammo;
    public void UpdateAmmo(string ammo)
    {
        this.ammo.text = ammo;
    }


    [Header("Crosshair")]
    public RectTransform crosshair;

    [SerializeField]
    private float originSize;
    public float currentSize;
    public float targetSize;
    private float oldTargetSize;

    private float currentAlpha;
    private float targetAlpha;

    [SerializeField]
    private float changeSpeed;

    private bool ads;

    private int type;

    public void SetVelocity(float velocity)
    {
        if (ads) return;
        targetSize = originSize + velocity * 5;
        oldTargetSize = targetSize;
    }

    public void SetType(int type)
    {
        this.type = type;
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
        float vel = 0;
        float vel2 = 0;

        currentSize = Mathf.SmoothDamp(currentSize, targetSize, ref vel, changeSpeed);
        crosshair.sizeDelta = new Vector2(currentSize, currentSize);

        currentAlpha = Mathf.SmoothDamp(currentAlpha, targetAlpha, ref vel2, changeSpeed / 2);
        crosshair.GetComponent<CanvasGroup>().alpha = currentAlpha;

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

    public void ShowHitmarker(int shotType)
    {
        if (hitmarkerActive)
            StopCoroutine(hitmarkerCoroutine);
        hitmarkerCoroutine = StartCoroutine(Hitmarker(shotType));
    }

    private IEnumerator Hitmarker(int shotType)
    {
        hitmarkerActive = true;
        var cg = hitmarker.GetComponent<CanvasGroup>();

        foreach (var line in hitmarkerLines)
        {
            switch (shotType)
            {
                case 0:
                    line.color = new Color(0, 0, 0, 0);
                    break;
                case 1:
                    line.color = Color.white;
                    break;
                case 2:
                    line.color = Color.red;
                    break;
                default:
                    line.color = Color.white;
                    break;
            }

        }

        cg.alpha = 1;
        hitmarker.gameObject.SetActive(true);

        hitmarker.sizeDelta = hitmarkerSize;

        float timer = 0;
        while (timer < hitmarkerDuration)
        {
            cg.alpha = Mathf.Lerp(cg.alpha, 0, timer);
            hitmarker.sizeDelta = Vector2.Lerp(hitmarkerSize, hitmarkerSize * 5, timer);

            timer += Time.deltaTime;
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
