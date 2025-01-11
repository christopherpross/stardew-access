using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using stardew_access.Translation;
using StardewValley;
using StardewValley.Menus;

namespace stardew_access.Patches;

internal class AdvancedGameOptionsPatch : IPatch
{
    public void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AdvancedGameOptions), nameof(AdvancedGameOptions.draw),
                [typeof(SpriteBatch)]),
            postfix: new HarmonyMethod(typeof(AdvancedGameOptionsPatch), nameof(DrawPatch))
        );

        harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AdvancedGameOptions), "IsDropdownActive"),
            prefix: new HarmonyMethod(typeof(AdvancedGameOptionsPatch), nameof(IsDropdownActivePatch))
        );

        harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AdvancedGameOptions), "receiveLeftClick"),
            prefix: new HarmonyMethod(typeof(AdvancedGameOptionsPatch), nameof(ReceiveLeftClickPatch))
        );
    }

    private static bool IsDropdownActivePatch()
    {
        return false;
    }

    // Prevents left clicking on dropdowns
    private static bool ReceiveLeftClickPatch(AdvancedGameOptions __instance)
    {
        try
        {
            int currentItemIndex = Math.Max(0, Math.Min(__instance.options.Count - 7, __instance.currentItemIndex));
            int x = Game1.getMouseX(true), y = Game1.getMouseY(true);

            for (var i = 0; i < __instance.optionSlots.Count; i++)
            {
                if (!__instance.optionSlots[i].bounds.Contains(x, y) || currentItemIndex + i >= __instance.options.Count || !__instance.options[currentItemIndex + i].bounds .Contains(x - __instance.optionSlots[i].bounds.X, y - __instance.optionSlots[i].bounds.Y))
                    continue;
                if (__instance.options[currentItemIndex + i] is OptionsDropDown) return false;
            }
        }
        catch (Exception e)
        {
            Log.Error( $"An error occurred in advanced game menu patch [ReceiveLeftClickPatch]:\n{e.Message}\n{e.StackTrace}");
        }

        return true;
    }

    private static void DrawPatch(AdvancedGameOptions __instance)
    {
        try
        {
            __instance.okButton.ScreenReaderText = Translator.Instance.Translate("common-ui-ok_button", TranslationCategory.Menu);
        }
        catch (Exception e)
        {
            Log.Error($"An error occurred in advanced game menu patch [DrawPatch]:\n{e.Message}\n{e.StackTrace}");
        }
    }
}
