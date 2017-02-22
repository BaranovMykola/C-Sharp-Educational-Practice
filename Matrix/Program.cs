using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    class Program
    {
        public static int[,] makeMagicMatrix(int n)
        {
            int[,] matrix = new int[n, n];
            int[] point = { 0, 0 }; //row, column
            int coordinate = 0; // 0 - row, 1 - column
            int moving = 1;
            int fill = 1;
            for (int i = 0; i < n+1; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if(point[0] >= n || point[1] >= n || point[0] < 0 || point[1] < 0)
                    {
                        Console.WriteLine("Out of range");
                        break;
                    }
                    matrix[point[1], point[0]] = fill++;
                    point[coordinate] += moving;
                }
                point[coordinate] -= moving;
                coordinate++;
                coordinate = coordinate % 2;
                if ((i + 1) % 2 == 0)
                {
                    moving *= -1;
                }
                else
                {
                }
                    point[coordinate] += moving;
                printMatrix(matrix, n, n);
                Console.WriteLine(); Console.WriteLine();
            }
            return matrix;
        }
        public static void printMatrix(int[,] matrix, int m, int n)
        {
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    Console.Write("{0} ", matrix[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            {
                makeMagicMatrix(3);
                Console.ReadKey();
            }
        }
    }
}
