using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    /// <summary>
    /// Provides methods for solving an existing sudoku model
    /// </summary>
    public static class SudokuSolver
    {

        /// <summary>
        /// Can be called to solve the provided model
        /// </summary>
        /// <param name="model">An arbitrary Sudoku Model</param>
        /// <returns>true, if the model has been solved</returns>
        public static bool Solve(SudokuModel model)
        {
            NovemTreeNode<int> root = new NovemTreeNode<int>(-1);
            var coordinates = model.FindEmptyIndicess();
            return Solve(model, coordinates, root);
        }

        //Recursive decent by induction
        private static bool Solve(SudokuModel model, List<(int, int)> empty, NovemTreeNode<int> solutionTree)
        {
            if (empty.Count == 0) return true;
            else
            {
                var index = empty.First();
                for(int i = 1; i <= SudokuModel.SIZE; i++)
                {
                    if(model.SetIfValid(index, i))
                    {
                        NovemTreeNode<int> childNode = new NovemTreeNode<int>(i, solutionTree);
                        solutionTree[i - 1] = childNode;
                        if(Solve(model, empty.Skip(1).ToList(), childNode))
                        {
                            solutionTree[i - 1] = childNode;
                            return true;
                        }
                    }
                    model.SetIfValid(index, SudokuModel.FIELD_EMPTY);
                }
            }
            return false;
        }

        //Computes the empty indicess for the provided Model (Extension Method)
        private static List<(int, int)> FindEmptyIndicess(this SudokuModel model)
        {
            List<(int, int)> indices = new List<(int, int)>();
            for(int row = 0; row < SudokuModel.SIZE; row++)
            {
                for (int col = 0; col < SudokuModel.SIZE; col++)
                {
                    if (model[row, col] == SudokuModel.FIELD_EMPTY) indices.Add((row, col));
                }
            }
            return indices;
        }
    }
}
