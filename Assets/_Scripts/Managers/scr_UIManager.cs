using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scr_UIManager : StaticInstance<scr_UIManager>
{
    public TMP_Text state;
    public TMP_Text weaponState;
    public TMP_Text speed;
    public TMP_Text ammo;

    private void Update()
    {
        UpdateCrosshair();
    }

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
    public void UpdateAmmo(string ammo)
    {
        this.ammo.text = ammo;
    }

    [Header("Crosshair")]
    public RectTransform crosshair;

    [SerializeField]
    private float originSize;
    private float currentSize;
    private float targetSize;
    private float oldTargetSize;

    private float currentAlpha;
    private float targetAlpha;

    [SerializeField]
    private float changeSpeed;

    private int type;

    public void SetVelocity(float velocity)
    {
        targetSize = originSize + velocity * 5;
        oldTargetSize = targetSize;
    }

    public void SetType(int type)
    {
        this.type = type;
    }

    public void ADS()
    {
        targetSize = 0;
        targetAlpha = 0;
    }

    public void NotADS()
    {
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

    [SerializeField]
    private RectTransform hitmarker;
    private bool hitmarkerActive;

    [SerializeField]
    private Vector2 hitmarkerSize;
    [SerializeField]
    private float hitmarkerDuration;

    Coroutine hitmarkerCoroutine;

    public void ShowHitmarker(bool killshot)
    {
        if (hitmarkerActive)
            StopCoroutine(hitmarkerCoroutine);
        hitmarkerCoroutine = StartCoroutine(Hitmarker(killshot));
    }

    private IEnumerator Hitmarker(bool killshot)
    {
        var x = hitmarker.GetChild(0).GetComponent<Image>();
        var cg = hitmarker.GetComponent<CanvasGroup>();
        x.color = killshot ? Color.red : Color.white;
        cg.alpha = 1;
        hitmarkerActive = true;
        hitmarker.gameObject.SetActive(true);

        hitmarker.sizeDelta = hitmarkerSize;

        float timer = 0;
        while (timer < hitmarkerDuration)
        {
            cg.alpha = Mathf.Lerp(cg.alpha, 0, timer);
            hitmarker.sizeDelta = Vector2.Lerp(hitmarkerSize, hitmarkerSize * .2f, timer);

            timer += Time.deltaTime;
            yield return null;
        }


        hitmarker.gameObject.SetActive(false);
        hitmarkerActive = false;
    }


}
