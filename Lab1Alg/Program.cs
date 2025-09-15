using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public class MazeGenerationResult
{
    public int[,] Grid { get;  }
    public int[] TopXs { get; }
    public int[] BottomXs { get; }
    public MazeGenerationResult(int[,] grid, int[] topXs, int[] bottomXs)
    {
        Grid = grid;
        TopXs = topXs;
        BottomXs = bottomXs;
    }

}

class Program
{
    static void Main(string[] args)
    {
        List<List<(int x, int y)>> paths = new List<List<(int x, int y)>>();
        int n = 10, m = 10, entriesCount = 3;
        int[] entries = { 1, 7, 4, 0, 0, 0, 0, 0, 0, 0 }, exits = { 5, 2, 9, 0, 0, 0, 0, 0, 0, 0 };
        int[,] maze =
        {
            {1, 0, 1, 1, 0, 1, 1, 0, 1, 1 },
            {1, 0, 0, 1, 0, 1, 0, 0, 1, 1 },
            {1, 1, 0, 0, 0, 0, 1, 0, 0, 1 },
            {1, 0, 0, 0, 1, 0, 1, 0, 1, 1 },
            {1, 0, 1, 0, 1, 0, 1, 0, 0, 1 },
            {1, 1, 1, 0, 1, 0, 0, 1, 0, 1 },
            {1, 0, 1, 0, 0, 0, 1, 1, 0, 1 },
            {1, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
            {1, 1, 0, 1, 0, 0, 1, 0, 1, 0 },
            {1, 1, 0, 1, 1, 0, 1, 1, 1, 0 },
        };

        Console.WriteLine("1. Сгенерировать лабиринт");
        Console.WriteLine("2. Написать вручную");
        Console.WriteLine("3. Использовать готовый");

        bool cycleExit = false;

        while (!cycleExit) {
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                default:
                    Console.WriteLine("Неверный ввод. Попробуйте ещё раз.");
                    break;
                case 1:
                    cycleExit = true;

                    Console.Write("Введите длину лабиринта: ");
                    n = int.Parse(Console.ReadLine());
                    Console.Write("Введите ширину лабиринта: ");
                    m = int.Parse(Console.ReadLine());
                    Console.Write("Введите количество входов-выходов: ");
                    entriesCount = int.Parse(Console.ReadLine());

                    MazeGenerationResult mgr = GenerateMaze(n, m, entriesCount);
                    if (n % 2 == 0) n++;
                    if (m % 2 == 0) m++;

                    for (int i = 0; i < entriesCount; i++)
                    {
                        entries[i] = mgr.TopXs[i];
                    }
                    for (int i = 0; i < entriesCount; i++)
                    {
                        exits[i] = mgr.BottomXs[i];
                    }
                    maze = mgr.Grid;
                    break;
                case 2:
                    cycleExit = true;

                    Console.Write("Введите длину лабиринта: ");
                    n = int.Parse(Console.ReadLine());
                    Console.Write("Введите ширину лабиринта: ");
                    m = int.Parse(Console.ReadLine());

                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            Console.Write("Введите элемент [" + i + "][" + j + "]:");
                            maze[i, j] = int.Parse(Console.ReadLine());
                            PrintMaze(maze, i+1, m, j);
                        }
                    }

                    int counter = 0;

                    for (int i = 0; i < m; i++)
                    {
                        if (maze[0, i] == 0) entries[counter++] = i;
                    }

                    counter = 0;

                    for (int i = 0; i < m; i++)
                    {
                        if (maze[m-1, i] == 0)
                        {
                            exits[counter++] = i;
                        }
                    }

                    entriesCount = counter;

                    break;
                case 3:
                    cycleExit = true;
                    break;
            }
        }

        PrintMaze(maze, m, n);

        int[] orderEntries = sortEntries(entries, entriesCount);
        int[] orderExits = sortEntries(exits, entriesCount);

        for(int i = 0; i < entriesCount; i++)
        {
            Console.WriteLine(orderExits[i]);
        }

        for (int i = 0; i < entriesCount; i++)
        {
            List<(int x, int y)> path = FindShortestPath(maze, (x: entries[i], y: 0), (x: exits[i], y: m - 1));
            PrintPathedMaze(maze, m, n, path);
            paths.Add(path);
        }

        bool canDoRef = true;

        if (entriesCount == 1)
        {
            Console.WriteLine("Всего один выход. Мало для проверки условия.");
        }
        else
        {
            for (int i = 0; i < entriesCount - 1; i++)
            {
                for (int j = i + 1; j < entriesCount; j++)
                {

                    for (int k = 0;  k < paths[i].Count; k++)
                    {
                        for (int h = 0; h < paths[j].Count; h++)
                        {
                            if (paths[i][k].x == paths[j][h].x && paths[i][k] == paths[j][h])
                            {
                                canDoRef = false;
                                Console.WriteLine("Пути " + (i + 1) + " и " + (j + 1) + " пересекаются в точке (" + paths[i][k].x + ", " + paths[i][k].y + ").");
                            }
                        }
                    }
                }
            }

            if (canDoRef)
            {
                Console.WriteLine("Условие выполняется. Входы-выходы:");
                for (int i = 0; i < entriesCount; i++)
                {
                    Console.WriteLine("x(" + orderEntries[i] + ")" + entries[orderEntries[i]] + " - y(" + orderExits[i] + ")" + exits[orderExits[i]]);
                }
            }
            else
            {
                Console.WriteLine("Условие не выпоняется");
            }
        }

        //foreach (var p in path) Console.WriteLine($"({p.x},{p.y})");
    }

    static void PrintMaze(int[,] grid, int n, int m, int stop = -1)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (stop != -1 && i == n - 1 && j > stop) { Console.WriteLine("stopped");  break;}
                if (grid[i, j] == 1) Console.Write("██");
                else Console.Write("  ");
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    static void PrintPathedMaze(int[,] grid, int n, int m, List<(int x, int y)> path)
    {

        Console.WriteLine("Путь найден, длина (в шагах): " + (path.Count - 1));
        Console.WriteLine();

        for (int i = 0; i < n; i++)
        {
            bool isColored = false;
            for (int j = 0; j < m; j++)
            {
                for (int h = 0; h < path.Count; h++)
                {
                    if (path[h].y == i && path[h].x == j)
                    {
                        Console.Write("..");
                        isColored = true;
                    }
                }
                if (!isColored)
                {
                    if (grid[i, j] == 1) Console.Write("██");
                    else Console.Write("  ");
                }
                isColored = false;
            }
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
    }

    static List<(int x, int y)> FindShortestPath(int[,] grid, (int x, int y) start, (int x, int y) target)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);
        if (!InBounds(start.x, start.y, width, height) || !InBounds(target.x, target.y, width, height)) return null;
        if (grid[start.y, start.x] != 0 || grid[target.y, target.x] != 0) return null;

        int n = width * height;
        int startIdx = start.y * width + start.x;
        int targetIdx = target.y * width + target.x;

        var parent = new int[n];
        var visited = new byte[n];
        for (int i = 0; i < n; i++) parent[i] = -1;

        int[] q = new int[n];
        int qh = 0, qt = 0;
        q[qt++] = startIdx;
        visited[startIdx] = 1;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        bool found = false;
        while (qh < qt)
        {
            int cur = q[qh++];
            if (cur == targetIdx) { found = true; break; }

            int cx = cur % width;
            int cy = cur / width;

            for (int k = 0; k < 4; k++)
            {
                int nx = cx + dx[k];
                int ny = cy + dy[k];
                if (!InBounds(nx, ny, width, height)) continue;
                int ni = ny * width + nx;
                if (visited[ni] != 0) continue;
                if (grid[ny, nx] != 0) continue;

                visited[ni] = 1;
                parent[ni] = cur;
                q[qt++] = ni;
            }
        }

        if (!found) return null;

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

    static bool InBounds(int x, int y, int w, int h) => x >= 0 && x < w && y >= 0 && y < h;

    public static MazeGenerationResult GenerateMaze(int w, int h, int pairs, int extraConnections = 0, int removeDeadEndsPercent = 0, int? seed = null)
    {
        var rng = seed.HasValue ? new Random(seed.Value) : new Random();

        // Приводим размеры к нечётным (чтобы правильно располагать клетку/стену)
        if (w % 2 == 0) w++;
        if (h % 2 == 0) h++;

        int[,] grid = new int[h, w]; // grid[y,x]

        // Изначально всё — стены
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                grid[y, x] = 1;

        int cellW = (w - 1) / 2; // число "логических" колонок
        int cellH = (h - 1) / 2; // число "логических" строк

        if (pairs < 0) throw new ArgumentException("pairs must be >= 0");
        if (pairs > cellW) throw new ArgumentException($"pairs ({pairs}) cannot exceed number of columns {cellW}");

        // --- Итеративный DFS (backtracker) для генерации базового идеального лабиринта ---
        int cells = cellW * cellH;
        var visited = new byte[cells];
        var stack = new int[cells];
        int stackSize = 0;

        // Стартовая клетка — случайная логическая клетка (чтобы было разнообразие)
        int startCx = rng.Next(cellW);
        int startCy = rng.Next(cellH);
        int startIdx = startCy * cellW + startCx;
        stack[stackSize++] = startIdx;
        visited[startIdx] = 1;

        // Отметим стартовую реальную клетку как проход
        grid[startCy * 2 + 1, startCx * 2 + 1] = 0;

        int[] dirX = { 0, 1, 0, -1 };
        int[] dirY = { -1, 0, 1, 0 };
        int[] order = new int[4];

        while (stackSize > 0)
        {
            int cur = stack[stackSize - 1];
            int cx = cur % cellW;
            int cy = cur / cellW;

            // перемешиваем направления
            for (int i = 0; i < 4; i++) order[i] = i;
            for (int i = 3; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                int tmp = order[i]; order[i] = order[j]; order[j] = tmp;
            }

            bool progressed = false;
            for (int oi = 0; oi < 4; oi++)
            {
                int d = order[oi];
                int nx = cx + dirX[d];
                int ny = cy + dirY[d];
                if (nx < 0 || nx >= cellW || ny < 0 || ny >= cellH) continue;
                int nidx = ny * cellW + nx;
                if (visited[nidx] != 0) continue;

                visited[nidx] = 1;

                // реальные координаты
                int rx = cx * 2 + 1;
                int ry = cy * 2 + 1;
                int rnx = nx * 2 + 1;
                int rny = ny * 2 + 1;
                int wx = (rx + rnx) / 2;
                int wy = (ry + rny) / 2;

                // вырезаем стену и соседнюю клетку
                grid[ry, rx] = 0;
                grid[wy, wx] = 0;
                grid[rny, rnx] = 0;

                stack[stackSize++] = nidx;
                progressed = true;
                break;
            }

            if (!progressed)
            {
                // backtrack
                stackSize--;
            }
        }

        // --- Добавляем случайные соединения (петли) ---
        for (int attempts = 0; attempts < extraConnections; attempts++)
        {
            int cx = rng.Next(cellW);
            int cy = rng.Next(cellH);
            int d = rng.Next(4);
            int nx = cx + dirX[d];
            int ny = cy + dirY[d];
            if (nx < 0 || nx >= cellW || ny < 0 || ny >= cellH) continue;

            int rx = cx * 2 + 1;
            int ry = cy * 2 + 1;
            int rnx = nx * 2 + 1;
            int rny = ny * 2 + 1;
            int wx = (rx + rnx) / 2;
            int wy = (ry + rny) / 2;

            // Защита: не трогаем периметр (ни левую/правую/верх/низ стену)
            if (wx <= 0 || wx >= w - 1 || wy <= 0 || wy >= h - 1) continue;

            grid[wy, wx] = 0;
        }

        // --- Удаление тупиков (опционально) — не трогаем периметр ---
        if (removeDeadEndsPercent > 0)
        {
            int percent = Math.Clamp(removeDeadEndsPercent, 0, 100);
            bool changed;
            do
            {
                changed = false;
                for (int cy = 0; cy < cellH; cy++)
                {
                    for (int cx = 0; cx < cellW; cx++)
                    {
                        int rx = cx * 2 + 1;
                        int ry = cy * 2 + 1;

                        // защита: не обрабатывать клетки на самой границе grid
                        if (rx <= 0 || rx >= w - 1 || ry <= 0 || ry >= h - 1) continue;
                        if (grid[ry, rx] != 0) continue;

                        int deg = 0;
                        if (ry - 1 >= 0 && grid[ry - 1, rx] == 0) deg++;
                        if (ry + 1 < h && grid[ry + 1, rx] == 0) deg++;
                        if (rx - 1 >= 0 && grid[ry, rx - 1] == 0) deg++;
                        if (rx + 1 < w && grid[ry, rx + 1] == 0) deg++;

                        if (deg == 1 && rng.Next(100) < percent)
                        {
                            // попытаться удалить соседнюю стену (если она не на периметре)
                            int[] ord2 = { 0, 1, 2, 3 };
                            for (int i = 3; i > 0; i--)
                            {
                                int j = rng.Next(i + 1);
                                int tmp = ord2[i]; ord2[i] = ord2[j]; ord2[j] = tmp;
                            }
                            for (int oi = 0; oi < 4; oi++)
                            {
                                int d2 = ord2[oi];
                                int nx = cx + dirX[d2];
                                int ny = cy + dirY[d2];
                                if (nx < 0 || nx >= cellW || ny < 0 || ny >= cellH) continue;
                                int rnx2 = nx * 2 + 1;
                                int rny2 = ny * 2 + 1;
                                int wx2 = (rx + rnx2) / 2;
                                int wy2 = (ry + rny2) / 2;
                                if (wx2 <= 0 || wx2 >= w - 1 || wy2 <= 0 || wy2 >= h - 1) continue;
                                if (grid[wy2, wx2] == 1)
                                {
                                    grid[wy2, wx2] = 0;
                                    changed = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            } while (changed);
        }

        // --- Выбор верхних и нижних колонок независимо ---
        int[] topXs = Array.Empty<int>();
        int[] bottomXs = Array.Empty<int>();
        if (pairs > 0)
        {
            // выбираем случайные уникальные колонки для верха
            var cols = Enumerable.Range(0, cellW).ToList();
            Shuffle(cols, rng);
            topXs = cols.Take(pairs).Select(cx => cx * 2 + 1).ToArray();

            // и отдельно для низа
            cols = Enumerable.Range(0, cellW).ToList();
            Shuffle(cols, rng);
            bottomXs = cols.Take(pairs).Select(cx => cx * 2 + 1).ToArray();
        }

        // --- Принудительно делаем всю границу стеной (на всякий случай) ---
        for (int x = 0; x < w; x++)
        {
            grid[0, x] = 1;
            grid[h - 1, x] = 1;
        }
        for (int y = 0; y < h; y++)
        {
            grid[y, 0] = 1;
            grid[y, w - 1] = 1;
        }

        // --- Пробиваем только выбранные отверстия (верх и низ) и соединяем их с внутренней частью ---
        foreach (var rx in topXs)
        {
            if (rx <= 0 || rx >= w - 1) continue;
            grid[0, rx] = 0;           // край
            if (1 < h) grid[1, rx] = 0; // внутренняя клетка под ним
        }
        foreach (var rx in bottomXs)
        {
            if (rx <= 0 || rx >= w - 1) continue;
            grid[h - 1, rx] = 0;         // край
            if (h - 2 >= 0) grid[h - 2, rx] = 0; // внутренняя клетка над ним
        }

        return new MazeGenerationResult(grid, topXs, bottomXs);
    }

    private static void Shuffle(List<int> list, Random rng)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            int tmp = list[i]; list[i] = list[j]; list[j] = tmp;
        }
    }

    static void swapEntries(int[] entries, int firstIdx, int lastIdx)
    {
        int buf = entries[firstIdx];
        entries[firstIdx] = entries[lastIdx];
        entries[lastIdx] = buf;
    }

    static int[] sortEntries(int[] entries, int entriesCount)
    {
        int[] order = new int[entriesCount];
        
        for (int i = 0; i < entriesCount; i++)
        {
            order[i] = i;
        }

        for (int i = 0; i < entriesCount; i++)
        {
            Console.Write(order[i]);
        }
        Console.WriteLine();

        for (int i = 0; i < entriesCount - 1; i++)
        {
            bool needToSwap = false;
            int swapIdx = i;
            int min = entries[i];
            for (int j = i; j < entriesCount; j++)
            {
                if (entries[j] < min)
                {
                    min = entries[j];
                    swapIdx = j;
                    needToSwap = true;
                }
            }

            if (needToSwap)
            {
                order[i] = swapIdx;
                order[swapIdx] = i;

                swapEntries(entries, i, swapIdx);
            }
        }
        return order;
    }
}