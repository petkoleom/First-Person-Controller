using TMPro;
using UnityEngine;

public class scr_UIManager : StaticInstance<scr_UIManager>
{
    public TMP_Text state;
    public TMP_Text speed;

    public void UpdateState(MovementState state)
    {
        this.state.text = state.ToString();
    }

    public void UpdateSpeed(float speed)
    {
        this.speed.text = speed.ToString();
    }
}
