using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;
using Winch.Core;

namespace DredgeBadApple;

internal class DataLoader
{
    private string frame_file = "frame-data.json";
    private int[][][] frame_data;
    public Vector2 resolution;
    public int frame_count;

    public DataLoader()
    {
        frame_data = ReadFrameJson(frame_file);
        frame_count = frame_data.Length;
        resolution = new Vector2(frame_data[0][0].Length, frame_data[0].Length);
    }

    private int[][][] ReadFrameJson(string target)
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), target);
        string content = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<int[][][]>(content)!;
    }

    public IEnumerable<int[][]> GenerateFrames()
    {
        foreach (var frame in frame_data)
        {
            yield return frame;
        }
    }
}