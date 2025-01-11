## Changelog

### New Features

- Enabled braille output.
- Added machines from the mod [Cornucopia - Artisan Machines](https://www.nexusmods.com/stardewvalley/mods/24842)

### Feature Updates

When "Sort By Proximity" is enabled, the ObjectTracker will focus on the closest item after map updates. E.G., after harvesting a specific crop, focus moves to the next closest crop, rather than the next closest crop of the same kind. Sorting alphabetically retains the old behavior.
- Improve object tracker's ability to detect locations in events
- Egg hunt is now accessible with tracked eggs and adjustable timer with the config option `EggHuntTimerMultiplier`. This allows you to give yourself up to 3x longer to find eggs!

### Bug Fixes

- Fix LoadGameMenu keyboard navigation broken by Stardew Valley 1.6.15 update.
- Fix keyboard navigation breaks after clicking any back button from a submenu of title screen, such as Load Game or New Game menus.
- Fixed empty fish ponds being prefixed with "Error Item"; #424
- Fixed social page reading tokenized string in place of relationship status.
- Fixed map page not being read.
ObjectTracker autorefresh is fixed! No more mashing the home button to force a refresh.
Garden Pots have been moved from the "Other" category to the "Pending" category to better reflect their role.
When holding a crop, pots will stay in "Pending" if they are unwatered. They will move to "Crops" once watered.
When holding a bush such as a tea sapling, pots will appear in "Bushes".
When any pot can be harvested, it will appear in "Ready".
- Fixed a bug where the categories of certain items were not being applied correctly from `stardew-access/assets/TileData/QualifiedItemIds.json`
- Removed incorrect door sound that plays when changing maps with grid movement on even when no door has been passed through.
- Fix null references that crashed the object tracker  if a 3rd party mod added trees and then the 3rd party mod was removed. Now  trees are reported as 'unknown"
- Fixed a bug  causing the object tracker to not refresh on schedule.
- Added more events to trigger object tracker refresh
- Fixed the change of direction sound whilst using grid movement.
- Fixed a bug where watered crops would remain in the pending category instead of moving to the crops category.

### Tile Tracker Changes

- Tracked Krobus' hiding bush. #359
- Moved animals with produce or pettable animals to pending category. Also changed the translation to prefix harvestable and/or pettable accordingly.
- Moved dropped animal produces (eggs, truffles, rabbit's foot & duck feathers) in ready category.
- Added new forageables category
- Moved major forageable items into forageables category
- Moved various items into new categories to better reflect their role as part of a category sweep
- Add many new  tracked fishing locations
- Added more bridges and stairs
- Added more tracked golden walnuts
- Auto detect egg hunt eggs, festival shops, fair minigames, and Luau soup so they stay tracked even across years  when they move or if 3rd party mods change locations.
- Track extra garbage cans added by the drbirbdev.BinningSkill mod, only when that mod is installed.

### Guides And Docs


### Misc

- The machines are now tracked by their qualified item ids instead of their names.
- Enabling the config option `ReadTileDebug` will add unknown map action strings to the object tracker, allowing you to find currently unlabeled map items
- Creating a Fluent token matching these unhandled entries will cause them to be spoken properly
- Adding the unhandled token to the `dynamic_tile_categories.json` file under `assets/TileData` along with a category will cause it to be listed in that category.
This effectively makes it possible to name and categorize any map property if desired.

### Translation Changes

- Modified(en.ftl): `npc-farm_animal_info` = [English value](https://github.com/khanshoaib3/stardew-access/blob/ad211b0ae16d7a3bf91eb822befb2660d28a1aea/stardew-access/i18n/en.ftl#L339-L360)
- Added new translation category DynamicTiles, with new file dynamic_tiles.en.ftl. This parallels how static tiles are stored and will improve organization of tokens.
- Began migration of fluent tokens from en.ftl and static_tiles.en.ftl to new dynamic_tiles.en.ftl
#### tokens that moved
These tokens were moved into the new file, but their name and text was unchanged:
- dynamic_tile-mastery_cave-pedestal
- dynamic_tile-farm-lumber_pile
#### Tokens that moved and were renamed
In addition to being moved, these tokens were renamed:
- npc_name-old_mariner changed to  dynamic_tile-beach-old_mariner
- event_tile-luau-pierre_booth changed to dynamic_tile-festival-pierres_booth
- event_tile-soup_pot-name changed to dynamic_tile-luau-luau_soup
- event_tile-stardew_valley_fair_shop-name changed to dynamic_tile-stardew_valley_fair-prize_booth
- event_tile-slingshot_game-name changed to dynamic_tile-stardew_valley_fair-slingshot_game
- event_tile-purchase_star_tokens-name changed to dynamic_tile-stardew_valley_fair-purchase_star_tokens
-event_tile-the_wheel-name changed to dynamic_tile-stardew_valley_fair-the_wheel
- event_tile-fishing_challenge-name changed to dynamic_tile-stardew_valley_fair-fishing_challenge
- event_tile-fortune_teller-name changed to dynamic_tile-stardew_valley_fair-fortune_teller
- event_tile-grange_display-name changed to  dynamic_tile-stardew_valley_fair-grange_display
- event_tile-strength_game-name changed to  dynamic_tile-stardew_valley_fair-strength_game
-event_tile-free_burgers-name changed to dynamic_tile-stardew_valley_fair-free_burgers
#### Tokens that are new
These are new tokens that will need to be created by all translations:
- dynamic_tile-egg_festival-egg
- dynamic_tile-island_hut-potted_tree
- dynamic_tile-stardew_valley_fair-tourist
- dynamic_tile-stardew_valley_fair-strength_game_arrow
- dynamic_tile-stardew_valley_fair-strength_game_sign
- static_tile-common-ocean
- static_tile-common-small_pool
- static_tile-common-swamp
- static_tile-common-driftwood
- static_tile-boat_tunnel-docks_edge
- static_tile-caldera-edge_of_the_lava_pool
- static_tile-desert-southern_pond_overlook
- static_tile-forest-arrow_head_island_south
- static_tile-island_farm_cave-east_pool
- static_tile-island_farm_cave-west_pool
- static_tile-island_north-dig_site_stairs
- static_tile-island_north-plank_bridge_north
- static_tile-island_north-plank_bridge_south
- static_tile-island_north-river_bend
- static_tile-island_north-rope_bridge
- static_tile-island_southeast-starfish_tide_pool
- static_tile-railroad-spa_ponnd
- static_tile-spirits_eve-start_of_maze
- static_tile-festival_of_ice-pool1
- static_tile-festival_of_ice-pool2
- static_tile-binning_skill-trash
- static_tile-binning_skill-roadside_trash
- static_tile-binning_skill-beach_trash
- static_tile-binning_skill-bus_stop_trash
- static_tile-binning_skill-sandy_house_trash
- static_tile-binning_skill-abandoned_house_trash
- static_tile-binning_skill-ranch_trash
- static_tile-binning_skill-wizards_trashstatic_tile-binning_skill-field_office_trash
- static_tile-binning_skill-adventurers_guild_trash
- static_tile-binning_skill-carpenter_shop_trash
- static_tile-binning_skill-spa_trash
- static_tile-binning_skill-summit_path_trash
- static_tile-binning_skill-cliffside_trash
- static_tile-binning_skill-park_trash
- static_tile-binning_skill-witchs_trash
#### Removed tokens
These tokens have been entirely removed:
- event_tile-egg_festival_shop-name
- event_tile-flower_dance_shop-name
- event_tile-spirits_eve_shop-name
- event_tile-feast_of_the_winter_star_shop-name

### Development Chores

- General code cleanup
- Updated refs for v1.6.15
- Fixed null reference exceptions and various other code cleanup tasks (including a missing return in OT  Refresh)
- Add functionality for events to use the same static / dynamic tiles framework as other locations.
- Add ability to detect tile action strings dynamically on all maps as another tool for auto detecting points of interest
    - This resulted in most functions in DynamicTiles.cs gaining a new function parameter, altering the signature.
- Added ability to force refresh ObjectTracker  after  special circumstances.
- remove redundant static tile entries from tiles.json now that they are dynamically detected.
- Add xmldocs and comments to functions

