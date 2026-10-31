# GitHub Copilot Instructions for Space Worms (Continued) Mod

## Mod Overview and Purpose
Space Worms (Continued) is an update of the original Space Worms mod by Demeggy. It introduces a new parasitic creature called the Space Worm, along with a new Level 3 technology (Xenobiology) to research and a new surgical procedure to extract Space Worm larvae from Hosts. The mod allows players to customize the infection chance, spawn infected raiders/visitors, and more.

## Key Features and Systems
- **Space Worm**: A new parasitic creature that infects humans and can wipe out a colony if not dealt with quickly.
- **Xenobiology Technology**: Allows automatic detection of parasitic infections and surgical extraction of larvae.
- **Mod Settings**: Options to adjust infection chance, spawn infected raiders, prevent re-infection, and allow infection on non-humans.
- **New Texture**: Enhanced textures provided by SirMashedPotato.

## Coding Patterns and Conventions
- **C# Code Organization**: The mod uses a clean and organized C# codebase, with well-defined classes and methods.
- **Mod Settings**: Settings are managed through a dedicated settings class (`ScuttlebugsSettings`) and exposed using the `ExposeData` method.
- **Incident Worker**: The `IncidentWorker_ScuttlebugPodCrash` class handles the logic for the crash of a pod with an infected survivor.
- **Verb Handling**: The `Verb_ScuttlebugDamage` class manages the damage logic for the Space Worm's attacks.
- **World Component**: The `WorldComp` class is used for global state management.

## XML Integration
The mod extensively uses XML for defining various game elements:
- **DamageDefs**: Contains definitions for the damage types used by the Space Worm.
- **ThingDefs_Races**: Defines the Space Worm and related pawn kinds.
- **HediffDefs**: Defines the parasitic infection and larvae hediffs.
- **Maneuvers**: Contains definitions for the Space Worm's maneuvers and damage types.
- **ToolCapacityDefs**: Defines the tool capacities for the Space Worm.

## Harmony Patching
The mod uses Harmony patching to modify the game's behavior:
- **Incident Worker**: Patches the incident worker to handle the crash of a pod with an infected survivor.
- **Verb Handling**: Patches the verb handling to manage the Space Worm's damage.
- **World Component**: Patches the world component to manage global state.

## Suggestions for Copilot
- **Generate XML Definitions**: Use Copilot to generate new XML definitions for additional features or bug fixes.
- **Create Mod Settings**: Use Copilot to create new mod settings or extend existing ones.
- **Patch Existing Game Mechanics**: Use Copilot to create Harmony patches for modifying existing game mechanics.
- **Implement New Features**: Use Copilot to implement new features such as the Worm Queen or Larvae Implantation.
- **Debugging and Error Handling**: Use Copilot to help identify and fix bugs in the mod.

## Example XML Definition
xml
<DamageDef>
  <defName>SpaceWormDamage</defName>
  <label>Space Worm Damage</label>
  <description>Damage dealt by the Space Worm.</description>
  <baseAmount>5</baseAmount>
  <armorPenetration>1</armorPenetration>
</DamageDef>


## Example Harmony Patch
csharp
[HarmonyPatch(typeof(IncidentWorker_PodCrash), "TryExecuteWorker")]
public static class IncidentWorker_PodCrash_Patch
{
    public static void Postfix(ref bool __result, PodCrashIncidentParms parms)
    {
        if (__result && parms.podContents.Any(pawn => pawn.RaceProps.IsHuman))
        {
            // Additional logic for handling infected survivors
        }
    }
}


By following these guidelines and utilizing GitHub Copilot, you can effectively contribute to or develop features for the Space Worms (Continued) mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

