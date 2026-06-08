using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    public KitChenObjectSO input;
    public KitChenObjectSO output;
    public float fryingTimerMax;

}
