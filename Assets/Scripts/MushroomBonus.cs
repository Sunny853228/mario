using UnityEngine; 

public class MushroomBonus : MonoBehaviour, IActivable
{
    public void Activate()
    {
        enabled = true;
    }

    public void Deactivate()
    {
        enabled = false;
    }
}