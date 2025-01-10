using HarmonyLib;
using UnityEngine;
using XRL;
using XRL.Core;
using XRL.World.Parts;


[HarmonyPatch(typeof(Qud.UI.UIManager), "Update")]
public static class DominationMod
{
    private static float timer = 0f;
    private static float checkInterval = 5f;

    public static void Postfix()
    {
        timer += Time.deltaTime;

        if (timer >= checkInterval)
        {
            timer = 0f;

            XRL.World.GameObject playerBody = XRLCore.Core?.Game?.Player?.Body;

            Mutations part = playerBody.GetPart<Mutations>();
            MutationEntry dominationEntry = MutationFactory.GetMutationEntryByName("Domination");

            if (part.HasMutation("Domination")){
                // UnityEngine.Debug.LogError("Already has domination");
                return;
            }
            else{
                part.AddMutation(dominationEntry.CreateInstance());
            }
        }
    }
}

[HarmonyPatch(typeof(XRL.World.Parts.Mutation.Domination), "GetDuration")]
public static class DurationModifier{
    public static void Postfix(ref int __result){
        __result = int.MaxValue;
        Debug.LogError("Patched GetDuration: " + __result);
    }
}


public static class HarmonyInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        var harmony = new Harmony("com.remghoost.dominatehotkey");

        harmony.PatchAll();
    }
}
