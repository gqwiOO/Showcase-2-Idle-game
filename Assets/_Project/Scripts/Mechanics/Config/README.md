# Game configuration (Scriptable Objects)

All main game mechanics, data generation, and durations are driven by Scriptable Object configs.

## Setup

1. **Add the installer**  
   Add `GameConfigInstaller` to your SceneContext (same object that has other installers like EconomyInstaller, GameTimeInstaller).

2. **Create config assets**  
   In the Project window: right‑click → Create → **GameAssets** → **Config** → choose:
   - **GameEconomyConfig** – ticks per month (income)
   - **ContractMarketConfig** – offers per night, tier thresholds, rewards, penalties
   - **ContractExecutionConfig** – base execution time, skill time boosts
   - **HiringConfig** – max hire candidates
   - **DefaultPlayerConfig** – new game default character
   - **ContractProductLimitsConfig** – max developers/assignees, default rewards

3. **Assign in installer**  
   Select the SceneContext object with `GameConfigInstaller`, then assign each config asset to the matching field under the "Mechanics" header.

4. **Data generation (optional)**  
   Under "Data generation" you can assign:
   - **CharactersGeneratingSettings**
   - **CompaniesGeneratingSettings**
   - **ProductsGeneratingSettings**  

   If assigned, these are used instead of loading from JSON (Resources).

## Existing day/night config

Day/night timing stays in **DayNightConfig** (GameAssets → DayNight), bound via `GameTimeInstaller`.
