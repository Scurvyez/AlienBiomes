using BiomesCore.DefModExtensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using Verse;

namespace AlienBiomes
{
    /*
    [HarmonyPatch(typeof(GenStep_Scatterer), nameof(GenStep_Scatterer.Generate))]
    public static class IslandScatterables
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo waterBiome = AccessTools.Field(type: typeof(GenStep_Scatterer), name: nameof(GenStep_Scatterer.allowInWaterBiome));

            CodeInstruction[] codeInstructions = instructions as CodeInstruction[] ?? instructions.ToArray();
            foreach (CodeInstruction instruction in codeInstructions)
            {
                if (instruction.opcode == OpCodes.Ldfld && (FieldInfo)instruction.operand == waterBiome)
                {
                    yield return new CodeInstruction(opcode: OpCodes.Ldarg_1);
                    yield return new CodeInstruction(opcode: OpCodes.Call, operand: AccessTools.Method(type: typeof(IslandScatterables), name: nameof(AllowedInWaterBiome)));
                }
                else
                {
                    yield return instruction;
                }
            }
        }

        public static bool AllowedInWaterBiome(GenStep_Scatterer step, Map map)
        {
            if (map.Biome.HasModExtension<BiomesMap>())
            {
                if (map.Biome.GetModExtension<BiomesMap>().hasScatterables)
                {
                    return true;
                }
            }
            return step.allowInWaterBiome;
        }
    }
    */
}
