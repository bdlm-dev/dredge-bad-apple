using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winch.Core;

namespace DredgeBadApple.Patchers;

[HarmonyPatch(typeof(GameSceneInitializer))]
[HarmonyPatch("Start")]
internal class GameSceneInitializerPatcher
{
    public static void Prefix(GameSceneInitializer __instance)
    {
        WinchCore.Log.Debug("Initializing DredgeBadApple");
        BadAppleExecutor executor = new();
    }
}
