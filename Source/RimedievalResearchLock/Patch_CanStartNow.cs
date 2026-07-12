using System;
using System.Collections.Generic;
using HarmonyLib;
using Rimedieval;
using RimWorld;
using Verse;

namespace RimedievalResearchLock
{
    [StaticConstructorOnStartup]
    public static class RimedievalResearchLockMod
    {
        // Projects Rimedieval permits, per its own rule (honors its tech-level setting and Odyssey
        // exception). Built once at startup; change Rimedieval's restriction setting -> restart.
        public static readonly HashSet<ResearchProjectDef> AllowedProjects;

        static RimedievalResearchLockMod()
        {
            new Harmony("wishRobber.rimedievalresearchlock").PatchAll();
            try
            {
                List<ResearchProjectDef> allowed =
                    DefCleaner.GetAllowedProjectDefs(DefDatabase<ResearchProjectDef>.AllDefsListForReading);
                AllowedProjects = new HashSet<ResearchProjectDef>(allowed);
            }
            catch (Exception ex)
            {
                AllowedProjects = null;
                Log.Warning("[Rimedieval Research Lock] Could not read Rimedieval's allowed projects; "
                    + "enforcement disabled. " + ex.Message);
            }
        }
    }

    // Rimedieval only hides disallowed techs from the vanilla research tab; their CanStartNow stays
    // true, so Research Whatever (and other "research anything" paths) can still start them. Make
    // disallowed projects report CanStartNow = false so those paths respect Rimedieval's lock.
    [HarmonyPatch(typeof(ResearchProjectDef), nameof(ResearchProjectDef.CanStartNow), MethodType.Getter)]
    public static class Patch_ResearchProjectDef_CanStartNow
    {
        public static void Postfix(ResearchProjectDef __instance, ref bool __result)
        {
            if (!__result) return; // already unstartable

            HashSet<ResearchProjectDef> allowed = RimedievalResearchLockMod.AllowedProjects;
            if (allowed != null && !allowed.Contains(__instance))
            {
                __result = false;
            }
        }
    }
}
