using System.Collections;
using TMPro;
using UnityEngine;

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

    Coroutine hitmarkerCoroutine;

    public void ShowHitmarker(bool killshot)
    {
        if(hitmarkerActive)
            StopCoroutine(hitmarkerCoroutine);
        hitmarkerCoroutine = StartCoroutine(Hitmarker(killshot));
    }

    private IEnumerator Hitmarker(bool killshot)
    {
        hitmarkerActive = true;
        hitmarker.gameObject.SetActive(true);


        yield return new WaitForSeconds(.2f);
        hitmarker.gameObject.SetActive(false);
        hitmarkerActive = false;
    }


}
