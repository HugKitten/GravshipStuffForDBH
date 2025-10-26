using DubsBadHygiene;

namespace GravshipStuffForDubsBadHygiene;

public static class WaterStorageHelper
{
    /// <summary>
    /// Get water storage cap of water storage
    /// </summary>
    public static float GetWaterStorageCap(this CompWaterStorage storage)
    {
        if (storage is CompCompressedWaterStorage compCompressedWaterStorage)
            return compCompressedWaterStorage.WaterStorageCap;
        if (storage.props is CompProperties_WaterStorage props)
            return props.WaterStorageCap;
        return float.NaN;
    }

    /// <summary>
    /// Check if water storage is full
    /// </summary>
    public static bool IsFull(this CompWaterStorage storage)
    {
        if (storage is CompCompressedWaterStorage compCompressedWaterStorage)
            return compCompressedWaterStorage.AmountCanAccept <= 0F;
        else
            return storage.WaterStorage >= storage.GetWaterStorageCap();
    }

    
}