using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{
    public KitChenObjectSO input;
    public KitChenObjectSO output;
    public float burningTimerMax;
}
