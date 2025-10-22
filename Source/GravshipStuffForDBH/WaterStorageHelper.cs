using DubsBadHygiene;

namespace GravshipStuffForDubsBadHygiene;

public static class WaterStorageHelper
{
    public static float GetWaterStorageCap(this CompWaterStorage storage)
    {
        if (storage is CompCompressedWaterStorage compCompressedWaterStorage)
            return compCompressedWaterStorage.WaterStorageCap;
        if (storage.props is CompProperties_WaterStorage props)
            return props.WaterStorageCap;
        return float.NaN;
    }
    
    public static bool IsFull(this CompWaterStorage storage)
    {
        if (storage is CompCompressedWaterStorage compCompressedWaterStorage)
            return compCompressedWaterStorage.IsFull;
        if (storage.props is CompProperties_WaterStorage props)
            return storage.WaterStorage >= storage.GetWaterStorageCap();
        return true;
    }
    
    public static bool IsAccepting(this CompWaterStorage storage)
    {
        if (storage is CompCompressedWaterStorage compCompressedWaterStorage)
            return compCompressedWaterStorage.IsAccepting;
        if (storage.props is CompProperties_WaterStorage props)
            return storage.WaterStorage < storage.GetWaterStorageCap();
        return true;
    }
    
    public static bool IsEmpty(this CompWaterStorage storage)
    {
        if (storage is CompCompressedWaterStorage compCompressedWaterStorage)
            return compCompressedWaterStorage.IsEmpty;
        if (storage.props is CompProperties_WaterStorage props)
            return storage.WaterStorage <= 0F;
        return true;
    }

    public static float GetAutoGenRate(this CompWaterStorage storage)
    {
        if (storage is CompCompressedWaterStorage compCompressedWaterStorage)
            return compCompressedWaterStorage.AutoGenRate;
        if (storage.props is CompProperties_WaterStorage props)
            return props.AutoGenRate;
        return 0;
    }

    public static int GetTickRate(this CompWaterStorage storage)
    {
        if (storage is CompCompressedWaterStorage compCompressedWaterStorage)
            return compCompressedWaterStorage.Tickrate;
        if (storage.props is CompProperties_WaterStorage props)
            return props.Tickrate;
        return 60;
    }

    public static bool GetAuto(this CompWaterStorage storage)
    {
        if (storage is CompCompressedWaterStorage compCompressedWaterStorage)
            return compCompressedWaterStorage.Auto;
        if (storage.props is CompProperties_WaterStorage props)
            return props.Auto;
        return false;
    }

    public static bool GetAutoOnRain(this CompWaterStorage storage)
    {
        if (storage is CompCompressedWaterStorage compCompressedWaterStorage)
            return compCompressedWaterStorage.AutoOnRain;
        if (storage.props is CompProperties_WaterStorage props)
            return props.AutoOnRain;
        return false;
    }
}