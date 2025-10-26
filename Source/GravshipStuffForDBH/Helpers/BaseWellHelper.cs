using DubsBadHygiene;

namespace GravshipStuffForDubsBadHygiene;

public static class BaseWellHelper
{
    /// <summary>
    /// If the well requires having a filter, or it'll self contaminate 
    /// Does not take into account filters on the pipeNet
    /// </summary>
    public static bool RequiresFilter(this CompBaseWell comp) =>
        comp is CompTreatedWaterInlet treated
            ? treated.RequiresFilter
            : comp.props is CompProperties_WaterInlet { Deep: false };
        
    /// <summary>
    /// If the well allows the contamination event to happen
    /// Does not take into account filters on the pipeNet
    /// </summary>
    public static bool EnablesContaminationIncident(this CompBaseWell comp) => 
        comp is not CompTreatedWaterInlet { EnablesContaminationIncident: true} ;
}