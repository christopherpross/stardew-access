using System.Text;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using stardew_access.Features;
using stardew_access.Translation;
using stardew_access.Utils;
using StardewValley;
using StardewValley.Menus;

namespace stardew_access.Patches;

// These patches are global, i.e. work on every menus
internal class IClickableMenuPatch : IPatch
{
    private static readonly HashSet<Type> SkipMenuTypes =
    [
        /*********
        ** Bundle Menus
        *********/
        typeof(JojaCDMenu),
        typeof(JunimoNoteMenu),

        /*********
        ** Donation Menus
        *********/
        typeof(FieldOfficeMenu),
        typeof(MuseumMenu),

        /*********
        ** Game Menu Pages
        *********/
        typeof(AnimalPage),
        typeof(CollectionsPage),
        typeof(CraftingPage),
        typeof(ExitPage),
        typeof(InventoryPage),
        typeof(PowersTab),
        typeof(SkillsPage),
        typeof(SocialPage),

        /*********
        ** Menus with Inventory
        *********/
        typeof(ForgeMenu),
        typeof(GeodeMenu),
        typeof(ItemGrabMenu),
        typeof(QuestContainerMenu),
        typeof(ShopMenu),
        typeof(StorageContainer),
        typeof(TailoringMenu),

        /*********
        ** Quest Menus
        *********/
        typeof(Billboard),
        typeof(QuestLog),
        typeof(SpecialOrdersBoard),

        /*********
        ** Title Menus
        *********/
        typeof(TitleMenu),
        // typeof(AdvancedGameOptions),
        typeof(CharacterCustomization),
        typeof(CoopMenu),
        typeof(FarmhandMenu),
        typeof(LoadGameMenu),

        /*********
        ** Other Menus
        *********/
        typeof(AnimalQueryMenu),
        typeof(BuildingSkinMenu),
        typeof(CarpenterMenu),
        typeof(ChooseFromIconsMenu),
        typeof(ConfirmationDialog),
        typeof(DialogueBox),
        typeof(ItemListMenu),
        typeof(LetterViewerMenu),
        typeof(LevelUpMenu),
        typeof(MasteryTrackerMenu),
        typeof(NamingMenu),
        typeof(NumberSelectionMenu),
        typeof(PondQueryMenu),
        typeof(PrizeTicketMenu),
        typeof(PurchaseAnimalsMenu),
        typeof(RenovateMenu),
        typeof(ShippingMenu),
        typeof(TitleTextInputMenu),
        typeof(ReadyCheckDialog),

        /*********
        ** Custom Menus
        *********/
        typeof(TileDataEntryMenu),
    ];

    private static bool _tryHoverPatch = false;

    internal static HashSet<string> ManuallyPatchedCustomMenus = [];

    internal static string? CurrentMenu;
    internal static bool ManuallyCallingDrawPatch = false;

#if DEBUG
    private static bool _justOpened = true;
#endif

    public void Apply(Harmony harmony)
    {
        harmony.Patch(
                original: AccessTools.Method(typeof(IClickableMenu), "exitThisMenu"),
                postfix: new HarmonyMethod(typeof(IClickableMenuPatch), nameof(IClickableMenuPatch.ExitThisMenuPatch))
        );

        harmony.Patch(
            original: AccessTools.Method(typeof(IClickableMenu), "draw", new Type[] { typeof(SpriteBatch) }),
            postfix: new HarmonyMethod(typeof(IClickableMenuPatch), nameof(IClickableMenuPatch.DrawPatch))
        );

        harmony.Patch(
            original: AccessTools.Method(typeof(IClickableMenu), "draw", new Type[] { typeof(SpriteBatch), typeof(int), typeof(int), typeof(int) }),
            postfix: new HarmonyMethod(typeof(IClickableMenuPatch), nameof(IClickableMenuPatch.DrawPatch))
        );

        harmony.Patch(
            original: AccessTools.Method(typeof(IClickableMenu), "drawHoverText", new Type[] { typeof(SpriteBatch), typeof(StringBuilder), typeof(SpriteFont), typeof(int), typeof(int), typeof(int), typeof(string), typeof(int), typeof(string[]), typeof(Item), typeof(int), typeof(string), typeof(int), typeof(int), typeof(int), typeof(float), typeof(CraftingRecipe), typeof(IList<Item>), typeof(Texture2D), typeof(Rectangle?), typeof(Color?), typeof(Color?), typeof(float), typeof(int), typeof(int) }),
            postfix: new HarmonyMethod(typeof(IClickableMenuPatch), nameof(IClickableMenuPatch.DrawHoverTextPatch))
        );
    }

    public static void DrawPatch()
    {
        try
        {
            // The only case when the active menu is null (in vanilla stardew) is when a hud message with no icon is displayed.
            if (Game1.activeClickableMenu is null) return;

            var activeMenu = GetActiveMenu();
            if (activeMenu is null) return;

            // FIXME For some reason custom mod menus don't trigger this patch, even though they implement IClickableMenu,
            // the following method essentially detects if the active menu hasn't triggered this method
            // and then manually calls it from ModEntry::OnUpdateTicked
            if (!ManuallyCallingDrawPatch) CurrentMenu = activeMenu.GetType()?.FullName;
            else ManuallyCallingDrawPatch = false;

            var activeMenuType = activeMenu.GetType();
            if (activeMenuType is not null
                    && (SkipMenuTypes.Contains(activeMenuType) || ManuallyPatchedCustomMenus.Contains(activeMenuType.FullName ?? "")))
            {
                return;
            }

#if DEBUG
            if (_justOpened)
            {
                _justOpened = false;
                Log.Debug($"[IClickableMenuPatch.DrawPatch] Attempting to patch menu {{ManuallyCalled:{ManuallyCallingDrawPatch}}}: {activeMenuType?.FullName}");
            }
#endif

            /* v1.6.9
            if (activeMenu.currentlySnappedComponent == null || string.IsNullOrWhiteSpace(activeMenu!.currentlySnappedComponent.ScreenReaderText))
            {
                if (OptionsElementUtils.NarrateOptionSlotsInMenuUsingReflection(activeMenu))
                    return;
            }
            else
            {
                ClickableComponentUtils.NarrateComponent(activeMenu.currentlySnappedComponent);
                return;
            }

            if (ClickableComponentUtils.NarrateHoveredComponentUsingReflectionInMenu(activeMenu))
            {
                return;
            }

            if (ClickableComponentUtils.NarrateHoveredComponentFromList(activeMenu.allClickableComponents))
            {
                return;
            }
            */

            if (OptionsElementUtils.NarrateOptionSlotsInMenuUsingReflection(activeMenu))
                return;

            if (ClickableComponentUtils.NarrateHoveredComponentUsingReflectionInMenu(activeMenu))
            {
                return;
            }

            if (ClickableComponentUtils.NarrateHoveredComponentFromList(activeMenu.allClickableComponents))
            {
                return;
            }

            if (activeMenu.currentlySnappedComponent != null)
            {
                ClickableComponentUtils.NarrateComponent(activeMenu.currentlySnappedComponent);
                return;
            }

            _tryHoverPatch = true;
        }
        catch (Exception e)
        {
            Log.Error($"[IClickableMenuPatch.DrawPatch]: {e.Message}\n{e.StackTrace}");
        }
    }

    private static IClickableMenu? GetActiveMenu()
    {
        var activeMenu = Game1.activeClickableMenu;

        if (activeMenu.GetParentMenu() != null)
        {
            // To let the parent menu's draw() call set `activeMenu` to the child menu, in the next if condition.
            return null;
        }

        if (activeMenu.GetChildMenu() != null && activeMenu.GetChildMenu().IsActive())
        {
            activeMenu = activeMenu.GetChildMenu();
        }

        if (activeMenu is TitleMenu titleMenu && TitleMenu.subMenu != null)
        {
            return TitleMenu.subMenu;
        }

        if (activeMenu is GameMenu gameMenu)
        {
            return gameMenu.GetCurrentPage();
        }

        return activeMenu;
    }

    private static void DrawHoverTextPatch(StringBuilder text,
                                           int moneyAmountToDisplayAtBottom = -1,
                                           string? boldTitleText = null,
                                           string? extraItemToShowIndex = null,
                                           int extraItemToShowAmount = -1,
                                           string[]? buffIconsToDisplay = null,
                                           Item? hoveredItem = null,
                                           CraftingRecipe? craftingIngredients = null)
    {
        if (!_tryHoverPatch) return;

        try
        {
            string toSpeak = "";

            if (hoveredItem != null)
            {
                toSpeak = InventoryUtils.GetItemDetails(hoveredItem,
                                                        hoverPrice: moneyAmountToDisplayAtBottom,
                                                        extraItemToShowIndex: extraItemToShowIndex,
                                                        extraItemToShowAmount: extraItemToShowAmount,
                                                        customBuffs: buffIconsToDisplay);
                toSpeak += (craftingIngredients is not null)
                    ? $", {InventoryUtils.GetIngredientsFromRecipe(craftingIngredients)}"
                    : "";
            }
            else
            {
                if (!string.IsNullOrEmpty(boldTitleText))
                    toSpeak = $"{boldTitleText}, ";

                if (text.ToString() == "???")
                    toSpeak = Translator.Instance.Translate("common-unknown");
                else
                    toSpeak += text;
            }

            _tryHoverPatch = false;

            // To prevent it from getting conflicted by two hover texts at the same time, two separate methods are used.
            // For example, sometimes `Welcome to Pierre's` and the items in seeds shop get conflicted causing it to speak infinitely.

            if (string.IsNullOrWhiteSpace(toSpeak)) return;

            MainClass.ScreenReader.SayWithMenuChecker(toSpeak, true); // Menu Checker
        }
        catch (Exception e)
        {
            Log.Error($"[IClickableMenuPatch.DrawHoverTextPatch]: {e.Message}\n{e.StackTrace}");
        }
    }

    private static void ExitThisMenuPatch(IClickableMenu __instance)
    {
        try
        {
            Log.Verbose($"[IClickableMenuPatch.ExitThisMenuPatch] Closed {__instance.GetType().FullName} menu, performing cleanup...");
            Cleanup(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"[IClickableMenuPatch.ExitThisMenuPatch]: {e.Message}\n{e.StackTrace}");
        }
    }

    internal static void Cleanup(IClickableMenu menu)
    {
        switch (menu)
        {
            case LetterViewerMenu:
                LetterViewerMenuPatch.Cleanup();
                break;
            case GameMenu:
                CraftingPagePatch.Cleanup();
                break;
            case JunimoNoteMenu:
                JunimoNoteMenuPatch.Cleanup();
                break;
            case CarpenterMenu:
                CarpenterMenuPatch.Cleanup();
                break;
            case PurchaseAnimalsMenu:
                PurchaseAnimalsMenuPatch.Cleanup();
                break;
            case AnimalQueryMenu:
                AnimalQueryMenuPatch.Cleanup();
                break;
            case DialogueBox:
                DialogueBoxPatch.Cleanup();
                break;
            case QuestLog:
                QuestLogPatch.Cleanup();
                break;
            case PondQueryMenu:
                PondQueryMenuPatch.Cleanup();
                break;
            case NumberSelectionMenu:
                NumberSelectionMenuPatch.Cleanup();
                break;
            case NamingMenu:
                NamingMenuPatch.Cleanup();
                break;
            case RenovateMenu:
                RenovateMenuPatch.Cleanup();
                break;
        }

        MainClass.ScreenReader.Cleanup();
        InventoryUtils.Cleanup();
        TextBoxPatch.activeTextBoxes = "";
        CurrentMenu = null;
        ManuallyCallingDrawPatch = false;
        _tryHoverPatch = false;

#if DEBUG
        _justOpened = true;
#endif
    }
}
