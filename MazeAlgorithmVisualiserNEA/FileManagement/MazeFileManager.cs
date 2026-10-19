using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeAlgorithmVisualiserNEA.Algorithms;
using MazeAlgorithmVisualiserNEA.Algorithms.Generation;
using MazeAlgorithmVisualiserNEA.Algorithms.Solving;
using MazeAlgorithmVisualiserNEA.Converters;
using MazeAlgorithmVisualiserNEA.DataStructures;
using MazeAlgorithmVisualiserNEA.Models;
using MazeAlgorithmVisualiserNEA.Rendering;

namespace MazeAlgorithmVisualiserNEA.FileManagement
{
    // THIS IMPORTS AND EXPORTS MAZES
    // KEEP FORMAT AS
    // MAZE, ROWS, COLUMNS, START, END, EACH CELL (WITH EXISTENCE OF WALLS)

    public class MazeFileManager
    {
        public static void SaveMaze(Maze maze, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // file name
                writer.WriteLine("Maze");

                // maze dimensions
                writer.WriteLine(maze.Rows);
                writer.WriteLine(maze.Columns);

                // start and end cell
                Cell startCell = null;
                Cell endCell = null;

                // find start and end cell
                for (int row = 0; row < maze.Rows; row++)
                {
                    for (int column = 0; column < maze.Columns; column++)
                    {
                        Cell cell = maze.GetCell(row, column);

                        if (cell.IsStart)
                        {
                            startCell = cell;
                        }

                        if (cell.IsEnd)
                        {
                            endCell = cell;
                        }
                    }
                }

                // writes start and end cell
                if (startCell != null)
                {
                    writer.WriteLine($"{startCell.Row},{startCell.Column}");
                }
                else
                {
                    writer.WriteLine("-1,-1");
                }

                if (endCell != null)
                {
                    writer.WriteLine($"{endCell.Row},{endCell.Column}");
                }
                else
                {
                    writer.WriteLine("-1,-1");
                }

                // saves walls of every cell
                for (int row = 0; row < maze.Rows; row++)
                {
                    for (int column = 0; column < maze.Columns; column++)
                    {
                        Cell cell = maze.GetCell(row, column);

                        string walls =
                            (cell.TopWall ? "1" : "0") +
                            (cell.BottomWall ? "1" : "0") +
                            (cell.LeftWall ? "1" : "0") +
                            (cell.RightWall ? "1" : "0");

                        writer.WriteLine($"{row},{column},{walls}");
                    }
                }
            }
        }

        public static Maze LoadMaze(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            int lineIndex = 1;

            // reads rows
            int rows = int.Parse(lines[lineIndex]);
            lineIndex++;

            // reads columns
            int columns = int.Parse(lines[lineIndex]);
            lineIndex++;

            Maze maze = new Maze(rows, columns);

            // start position
            string[] startParts = lines[lineIndex].Split(',');
            int startRow = int.Parse(startParts[0]);
            int startColumn = int.Parse(startParts[1]);
            lineIndex++;

            // end position
            string[] endParts = lines[lineIndex].Split(',');
            int endRow = int.Parse(endParts[0]);
            int endColumn = int.Parse(endParts[1]);
            lineIndex++;

            // allows for optional start 
            if (startRow != -1)
            {
                maze.SetStart(maze.GetCell(startRow, startColumn));
            }

            if (endRow != -1)
            {
                maze.SetEnd(maze.GetCell(endRow, endColumn));
            }

            // load walls
            for (int i = 0; i < rows * columns; i++)
            {
                string[] parts = lines[lineIndex].Split(',');

                int row = int.Parse(parts[0]);
                int column = int.Parse(parts[1]);
                string walls = parts[2];

                Cell cell = maze.GetCell(row, column);

                // top
                if (walls[0] == '1')
                    cell.AddWall(Direction.Top);
                else
                    cell.RemoveWall(Direction.Top);

                // bottom
                if (walls[1] == '1')
                    cell.AddWall(Direction.Bottom);
                else
                    cell.RemoveWall(Direction.Bottom);

                // left
                if (walls[2] == '1')
                    cell.AddWall(Direction.Left);
                else
                    cell.RemoveWall(Direction.Left);

                // right
                if (walls[3] == '1')
                    cell.AddWall(Direction.Right);
                else
                    cell.RemoveWall(Direction.Right);

                lineIndex++;
            }
            return maze;

        }


    }
}
