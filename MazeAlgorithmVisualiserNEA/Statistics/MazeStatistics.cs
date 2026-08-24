using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAlgorithmVisualiserNEA.Statistics
{
    public class MazeStatistics
    {

        public int Rows;
        public int Columns;

        public int Walls;

        public int TotalCells;

        public int DeadEnds;
        public int Junctions;

        public double BranchingFactor;

        public double AverageCorridorLength;
        public double MazeDensity;

        public int Passages;



        public MazeStatistics(int rows, int columns, int walls, int totalCells, int deadEnds, int junctions, double branchingFactor, double averageCorridorLength, double mazeDensity, int passages)
        {
            Rows = rows;
            Columns = columns;
            Walls = walls;
            TotalCells = totalCells;
            DeadEnds = deadEnds;
            Junctions = junctions;
            BranchingFactor = branchingFactor;
            AverageCorridorLength = averageCorridorLength;
            MazeDensity = mazeDensity;
            Passages = passages;

        }
    }
}
