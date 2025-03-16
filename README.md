# dredge-bad-apple
Bad Apple, recreated in the game [DREDGE](https://store.steampowered.com/app/1562430/DREDGE/) using crab pots

[![Thumbnail of: Bad Apple!! but in DREDGE](https://img.youtube.com/vi/9YCq34A0QnU/0.jpg)](https://www.youtube.com/watch?v=9YCq34A0QnU)

## How?
- Resize video to target resolution
- Threshold video to purely black or white
- Serialize pixels as 0 or 1 to json int[][][] array
- Save for later.

- Create DREDGE mod using [Winch](https://github.com/DREDGE-Mods/Winch)
- Figure out how crab pots work in the base game
- Hijack those methods
- Create your own crab pot to use as a prefab
- Instance (resolution.x * resolution.y) crab pots in a grid
- Iterate over the frames in the json data,
  -   If it's 1, turn on the corresponding crab pot light
  -   If it's 0, turn it off
- Good job.

## Hotkeys
- P to execute animation
- V to spawn crab pots
- U to cleanup crab pots
