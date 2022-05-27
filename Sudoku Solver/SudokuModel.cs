using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    public struct SudokuIndex
    {
        public int Row, Col;
    };

    public class SudokuModel
    {
        public static readonly int FIELD_EMPTY = 0;
        public static readonly int SIZE = 9;
        public static readonly int SECTION_SIZE = 3;

        public int[,] Values { get; set; } = new int[SIZE, SIZE];

        public SudokuModel()
        {
            for(int row = 0; row < SIZE; row++)
            {
                for(int col = 0; col < SIZE; col++)
                {
                    Values[row, col] = FIELD_EMPTY;
                }
            }
        }

        public int this[int rowIndex, int colIndex]
        {
            get => Values[rowIndex, colIndex];   
            protected set => Values[rowIndex, colIndex] = value;   
        }

        public int this[(int, int) index]
        {
            get => this[index.Item1, index.Item2];
            protected set => this[index.Item1, index.Item2] = value;
        }

        public int this[SudokuIndex index]
        {
            get => this[index.Row, index.Col];
            protected set => this[index.Row, index.Col] = value;
        }

        public bool SetIfValid((int, int) index, int value)
        {
            return SetIfValid(new SudokuIndex() { Row=index.Item1, Col = index.Item2}, value);
        }

        /// <summary>
        /// Method for trying to put a new value into the Sudoku
        /// </summary>
        /// <param name="index">The index within the sudoku</param>
        /// <param name="value">The value to put</param>
        /// <returns>Returns true, if putting the value onto the specified index would result in a valid field</returns>
        public bool SetIfValid(SudokuIndex index, int value)
        {
 
            //This function is based on induction
            //Base case is, that an empty Sudoku is valid
            //If a value is added, then only check the row/col/field it is in O(3n)
            bool returnValue = true;

            if (value == FIELD_EMPTY)
            {
                this[index] = FIELD_EMPTY;
                return returnValue; //Emptying a field is always allowed
            }
            if (value < 1 || value > SIZE)
                throw new ArgumentException(string.Format("Invalid value for the Sudoku of Size %d: %d", SIZE, value));

            int original = this[index];
            this[index] = value;

            //Check Row
            for(int col = 0; col < SIZE && returnValue; col++)
            {
                if (col != index.Col && this[index.Row, col] == this[index]) returnValue = false;
            }

            //Check Col
            for(int row = 0; row < SIZE && returnValue; row++)
            {
                if (row != index.Row && this[row, index.Col] == this[index]) returnValue = false;
            }

            //Check Section
            int colOffset = index.Col / SECTION_SIZE;   //Integer Division
            int rowOffset = index.Row / SECTION_SIZE;   //Integer Division
            for (int rowDelta = 0; rowDelta < SECTION_SIZE && returnValue; rowDelta++) 
            {
                for (int colDelta = 0; colDelta < SECTION_SIZE && returnValue; colDelta++)
                {
                    int row = rowOffset * 3 + rowDelta;
                    int col = colOffset * 3 + colDelta;
                    if ((row != index.Row || col != index.Col) && this[row, col] == this[index])
                        returnValue = false;
                }
            }

            if (!returnValue) this[index] = original;
            return returnValue;     
        }

        public void Reset()
        {
            for(int i = 0; i < Values.GetLength(0); i++)
            {
                for(int j = 0; j < Values.GetLength(1); j++) this.Values[i, j] = FIELD_EMPTY;
                
            }
        }
    }
}
