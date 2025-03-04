using HarmonyLib;
using Winch.Core.API;

namespace DredgeBadApple;

public static class Loader
{
    public static void Initialize()
    {
        new Harmony("mmbluey.badapple").PatchAll();

        /* Have frames of bad apple as json int[frame_count][resx][resy]
         * 
         * Read json into list
         * Find resolution from json
         * Instantiate (resolution.x * resolution.y) crab pots
         * For each frame of bad apple (after key pressed):
         *  disable/enable/move crab pots depending on whether corresponding int in json is 1 or 0
         *  wait 1 second
         *  next frame
         * yes?
         * 
         * 
         * needs:
         *  Instance x crab pots
         *  Crab pot root location
         */
    }
}