using System;

class Program
{
    static void Main(string[] args)
    {
        const int n = 7, m = 7;
        int[,] maze =
        {
            {1, 0, 1, 1, 1, 0, 1},
            {1, 0, 0, 0, 0, 0, 1},
            {1, 1, 1, 0, 1, 0, 0},
            {1, 0, 0, 0, 0, 1, 1},
            {0, 0, 1, 1, 0, 0, 1},
            {1, 0, 1, 1, 1, 0, 0},
            {1, 0, 1, 1, 1, 0, 1}
        };

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
                if (maze[i, j] == 1) Console.Write("██");
                else Console.Write("  ");
            }
            Console.WriteLine();
        }

        List<(int x, int y)> path = FindShortestPath(maze, (x: 1, y: 0), (x: 1, y: 6));

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();

        for(int i = 0; i < n; i++)
        {
            bool isColored = false;
            for (int j = 0; j < m; j++)
            {
                for(int h = 0; h < path.Count; h++)
                {
                    if (path[h].y == i && path[h].x == j)
                    {
                        Console.Write("..");
                        isColored = true;
                    }
                }
                if (!isColored)
                {
                    if (maze[i, j] == 1) Console.Write("██");
                    else Console.Write("  ");
                }
                isColored = false;
            }
            Console.WriteLine();
        }


        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Путь найден, длина (в шагах): " + (path.Count - 1));
        foreach (var p in path) Console.WriteLine($"({p.x},{p.y})");
    }

    public static List<(int x, int y)> FindShortestPath(int[,] grid, (int x, int y) start, (int x, int y) target)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);
        if (!InBounds(start.x, start.y, width, height) || !InBounds(target.x, target.y, width, height)) return null;
        if (grid[start.y, start.x] != 0 || grid[target.y, target.x] != 0) return null;

        int n = width * height;
        int startIdx = start.y * width + start.x;
        int targetIdx = target.y * width + target.x;

        // Arrays 1D для скорости
        var parent = new int[n];
        var visited = new byte[n];
        for (int i = 0; i < n; i++) parent[i] = -1;

        // Кольцевая очередь
        int[] q = new int[n];
        int qh = 0, qt = 0;
        q[qt++] = startIdx;
        visited[startIdx] = 1;

        // Смещения для 4 направлений
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        bool found = false;
        while (qh < qt)
        {
            int cur = q[qh++];
            if (cur == targetIdx) { found = true; break; }

            int cx = cur % width;
            int cy = cur / width;

            // обход соседей
            for (int k = 0; k < 4; k++)
            {
                int nx = cx + dx[k];
                int ny = cy + dy[k];
                if (!InBounds(nx, ny, width, height)) continue;
                int ni = ny * width + nx;
                if (visited[ni] != 0) continue;
                if (grid[ny, nx] != 0) continue; // стенка

                visited[ni] = 1;
                parent[ni] = cur;
                q[qt++] = ni;
            }
        }

        if (!found) return null;

        // Восстановление пути
        var path = new List<(int x, int y)>();
        int curIdx = targetIdx;
        while (curIdx != -1)
        {
            int x = curIdx % width;
            int y = curIdx / width;
            path.Add((x, y));
            curIdx = parent[curIdx];
        }
        path.Reverse();
        return path;
    }

    static bool InBounds(int x, int y, int w, int h) => x >= 0 && x < w && y >= 0 && y < h;//gggggggg
}