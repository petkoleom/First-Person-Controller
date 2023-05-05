using TMPro;
using UnityEngine;

public class scr_UIManager : StaticInstance<scr_UIManager>
{
    public TMP_Text state;
    public TMP_Text weaponState;
    public TMP_Text speed;
    public TMP_Text ammo;

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


}
