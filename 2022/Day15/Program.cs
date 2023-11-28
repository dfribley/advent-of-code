using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using AoC.Shared.Distance;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 15\n");

var inputRegEx = new Regex(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var sensorData = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var match = inputRegEx.Match(line);
            var sensor = new Point(match.Groups[1].Value.ToInt32(), match.Groups[2].Value.ToInt32());
            var beacon = new Point(match.Groups[3].Value.ToInt32(), match.Groups[4].Value.ToInt32());

            return (sensor, beacon, distance: TaxiCab.GetDistance(sensor, beacon));
        })
        .ToList();
    var beacons = sensorData.Select(s => s.beacon).Distinct().ToHashSet();

    int nonBeaconPositions(int row)
    {
        return sensorData
            .Where(s => s.sensor.Y - s.distance <= row && s.sensor.Y + s.distance >= row)
            .SelectMany(s =>
            {
                var mid = new Point(s.sensor.X, row);
                var points = new List<Point> { mid };

                foreach (var dir in new[] { -1, 1 })
                {
                    var point = new Point(mid.X + dir, mid.Y);

                    while (true)
                    {
                        if (TaxiCab.GetDistance(s.sensor, point) > s.distance)
                        {
                            break;
                        }

                        points.Add(point);
                        point = new Point(point.X + dir, point.Y);
                    }
                }

                return points;
            })
            .Distinct()
            .Except(beacons)
            .Count();
    }

    long findUnscannedPoint(int max)
    {
        foreach (var (sensor, beacon, distance) in sensorData)
        {
            for (var i = sensor.X - distance + 1; i <= sensor.X + distance + 1; i++)
            {
                foreach (var point in TaxiCab.GetPerimeterPoints(sensor, i, distance + 1))
                {
                    if (point.X >= 0 && point.X <= max && point.Y >= 0 && point.Y <= max)
                    {
                        var covered = false;

                        for (var j = 0; j < sensorData.Count; j++)
                        {
                            if (TaxiCab.GetDistance(sensorData[j].sensor, point) <= sensorData[j].distance)
                            {
                                covered = true;
                                break;
                            }
                        }

                        if (!covered)
                        {
                            return point.X * 4000000L + point.Y;
                        }
                    }
                }
            }
        }

        throw new Exception("Unscanned point not found!?");
    }

    var sw = new Stopwatch();
    sw.Start();
    var part1 = nonBeaconPositions(inputFile.StartsWith("sample") ? 10 : 2000000);
    sw.Stop();
    Console.WriteLine($"Part 1: ({string.Format("{0:00}:{1:00}.{2:00}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10)}) {part1}");

    sw.Restart();
    long part2;

    if (inputFile.StartsWith("sample"))
    {
        part2 = findUnscannedPoint(20);
    }
    else
    {
        part2 = findUnscannedPoint(4000000);
    }
    sw.Stop();
    Console.WriteLine($"Part 2: ({string.Format("{0:00}:{1:00}.{2:00}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10)}) {part2}\n");
}