using UnityEngine; 
using Verse; 
using RimWorld; 

namespace FamilyRelationsAdoption;

public class FamilyRelationsAdoptionMod : Mod
{
    public FamilyRelationsAdoptionMod(ModContentPack content) : base(content)
    {
        
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard comingSoon = new Listing_Standard(); 
        comingSoon.Begin(inRect); 
        comingSoon.Label("FRA_XmlSettingsComingSoon".Translate()); 
        comingSoon.End(); 
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Family Relations: Adoption";
    }
}
