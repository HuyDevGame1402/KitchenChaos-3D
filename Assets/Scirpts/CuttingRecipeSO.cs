using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    public KitChenObjectSO input;
    public KitChenObjectSO output;
    public int cuttingProgressMax;
}
