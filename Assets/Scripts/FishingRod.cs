using UnityEngine;

public class FishingRod : MonoBehaviour
{
    void Start()
    {
        
    }

    public void AdjustRodPosition(bool needsToBeFlipped)
    {
        var localPos = transform.localPosition;
        var fishingRodPosAbs = Mathf.Abs(localPos.x);
        transform.localPosition = needsToBeFlipped ? new Vector3(-fishingRodPosAbs, localPos.y, localPos.z) : new Vector3(fishingRodPosAbs, localPos.y, localPos.z);
    }
}
