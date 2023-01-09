using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Day19
{
    internal class GameState
    {
        public GameState(GameState gs)
        {
            Minute = gs.Minute;
            OreRobots = gs.OreRobots;
            ClayRobots = gs.ClayRobots;
            ObsidianRobots = gs.ObsidianRobots;
            GeodeRobots = gs.GeodeRobots;
            Ore = gs.Ore;
            Clay = gs.Clay;
            Obsidian = gs.Obsidian;
            Geodes = gs.Geodes;
        }
        public GameState() { }

        public int Minute { get; set; }
        public int OreRobots { get; set; }
        public int ClayRobots { get; set; }
        public int ObsidianRobots { get; set; }
        public int GeodeRobots { get; set; }
        public int Ore { get; set; }
        public int Clay { get; set; }
        public int Obsidian { get; set; }
        public int Geodes { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as GameState;

            return Minute == other.Minute &&
                OreRobots == other.OreRobots &&
                ClayRobots == other.ClayRobots &&
                ObsidianRobots == other.ObsidianRobots &&
                GeodeRobots == other.GeodeRobots &&
                Ore == other.Ore &&
                Clay == other.Clay &&
                Obsidian == other.Obsidian &&
                Geodes == other.Geodes;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HashCode.Combine(Minute, OreRobots, ClayRobots, ObsidianRobots,
                GeodeRobots, Ore, Clay, Obsidian), Geodes);
        }
    }
}
