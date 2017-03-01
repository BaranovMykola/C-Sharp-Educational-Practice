using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    class Matrix
    {
        public static int[,] makeMagicMatrix(int n)
        {
            int[,] matrix = new int[n, n];
            int[] point = { 0, 0 }; //row, column
            int coordinate = 0; // 0 - row, 1 - column
            int moving = 1;
            int fill = 1;
            int margin = 0;
            for (int i = 0; i < n+1; ++i)
            {
                for (int j = 0; j < n-margin; ++j)
                {
                    if(point[0] >= n || point[1] >= n || point[0] < 0 || point[1] < 0)
                    {
                        Console.WriteLine("Out of range");
                        break;
                    }
                    matrix[point[1], point[0]] = fill++;
                    point[coordinate] += moving;
                }

                printMatrix(matrix, n, n);
                Console.WriteLine(); Console.WriteLine();

                point[coordinate] -= moving;
                coordinate++;
                coordinate = coordinate % 2;
                if ((i + 1) % 2 == 0)
                {
                    moving *= -1;
                    ++margin;
                }
                else
                {
                }
                point[coordinate] += moving;
                
            }
            return matrix;
        }
        public static int[,] makeMatrix(int n)
        {
            int[,] matrix = new int[n, n];
            int act = 0;
            int r = 0;
            int c = 0;
            int margin = 0;
            int fill = 1;
            for(int i=0;i<n+n;++i)
            {
                for (int j=0;j<n-margin;++j)
                {
                    matrix[c,r] = fill++;
                    switch(act)
                    {
                        case 0: ++r; break;
                        case 1: ++c; break;
                        case 2: --r; break;
                        case 3: --c; break;
                    }
                }
                switch (act)
                {
                    case 0: --r; break;
                    case 1: --c; break;
                    case 2: ++r; break;
                    case 3: ++c; break;
                }
                ++act;
                act = act % 4;
                switch (act)
                {
                    case 0: ++r; break;
                    case 1: ++c; break;
                    case 2: --r; break;
                    case 3: --c; break;
                }
                if (i % 2 == 0)
                {
                    ++margin;
                }
                if(n == margin)
                {
                    break;
                }
                
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
            Console.WriteLine();
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            {
                int s = 5;
                var m = makeMatrix(s);
                printMatrix(m, s, s);
                Console.ReadKey();
            }
        }
    }
}
