namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    /*
    public record PatchCard
    {
        public static PatchCard Draw(Rect inRect, GravshipStuffForDubsBadHygieneSettings settings, string name)
        {
            var isEnabled = settings.wellsToPatch.TryGetValue(name, out var value) && value;
            return new PatchCard(name, isEnabled).Draw(inRect, settings);
        }
        
        public string Name;
        public bool IsEnabled;

        public PatchCard(string name, bool isEnabled = true)
        {
            Name = name;
            IsEnabled = isEnabled;
        }

        public PatchCard Draw(Rect inRect, GravshipStuffForDubsBadHygieneSettings settings)
        {
            Widgets.textl
            Widgets.DrawBox(inRect);
            Widgets.Label(inRect, "Class");
            Widgets.TextArea(inRect, Name, true);
            Widgets.Label(inRect, "Class");
            Widgets.Checkbox(inRect, ref IsEnabled);
            
            var listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.("Class", Name, 1);
            listingStandard.CheckboxLabeledWithAction("Enabled", ref IsEnabled, act: () => settings.wellsToPatch[Name] = IsEnabled);
            
            listingStandard.End();
            return this;
        }
    }
    */
}