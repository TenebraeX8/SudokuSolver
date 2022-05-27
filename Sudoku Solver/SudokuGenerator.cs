using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    public static class SudokuGenerator
    {
        private static Random RGenerator = new Random();
        public static void Generate(SudokuModel model)
        {
            model.Reset();
            GenerateFields(model, (0, 0));  //Generate full field
            var indicess = ShuffledIndices(SudokuModel.SIZE * SudokuModel.SIZE / 2);
            foreach (var index in indicess) model.SetIfValid(index, SudokuModel.FIELD_EMPTY);
        }

        private static bool GenerateFields(SudokuModel model, (int, int) index)
        {
            if (index.Item1 == SudokuModel.SIZE) return true;

            int[] indicess = new int[SudokuModel.SIZE];
            for (int i = 1; i <= SudokuModel.SIZE; i++) indicess[i-1] = i;
            indicess.Shuffle();

            for (int i = 0; i < indicess.Length; i++)
            {
                if(model.SetIfValid(index, indicess[i]))
                {
                    int col = (index.Item2 + 1) % SudokuModel.SIZE;
                    int row = index.Item2 == 8 ? index.Item1 + 1 : index.Item1;
                    if(GenerateFields(model, (row, col)))
                    {
                        return true;
                    }
                }
                model.SetIfValid(index, SudokuModel.FIELD_EMPTY);
            }
            return false;
        }

        private static IEnumerable<(int, int)> ShuffledIndices(int n)
        {
            HashSet<(int, int)> values = new HashSet<(int, int)>();
            int i = 0;
            while(i < n)
            {
                (int, int) index = (RGenerator.Next(SudokuModel.SIZE), RGenerator.Next(SudokuModel.SIZE));
                if (values.Add(index)) i++; //Only count if not in the set already
            }
            return values;
        }

        private static void Shuffle(this int[] arr)
        {
            int n = arr.Length;
            while(n > 1)
            {
                int randomIdx = RGenerator.Next(n);
                n--;
                int tmp = arr[randomIdx];
                arr[randomIdx] = arr[n];
                arr[n] = tmp;
            }
        }
    }
}
