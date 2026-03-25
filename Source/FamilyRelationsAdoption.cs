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
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Family Relations: Adoption";
    }
}
