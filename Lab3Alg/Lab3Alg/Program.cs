using System.Text;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(CanTransform("011", "AABB"));

        Console.Write("""
            1. Ввести вручную
            2. Сгенерировать
            Ваш выбор: 
            """);

        string x = null, y = null;

        switch(MakeChoice(2))
        {
            case 1:
                (x, y) = GetStrings();
                break;

            case 2:
                (x, y) = GenerateStrings();
                Console.WriteLine(x + '\n' + y);
                break;
        }

        Console.WriteLine("Результат: " + CanTransform(x, y));
    }
    static bool CanTransform(string x, string y)
    {
        int n = x.Length, m = y.Length;
        var dp = new bool[n + 1, m + 1];
        dp[0, 0] = true;
        for (int i = 0; i <= n; i++)
        {
            for (int j = 0; j <= m; j++)
            {
                if (!dp[i, j]) continue;
                if (i == n && j == m) return true;
                if (i < n)
                {
                    char xi = x[i];
                    char[] allowed = xi == '0' ? new[] { 'A' } : new[] { 'A', 'B' };
                    foreach (char c in allowed)
                    {
                        int len = 1;
                        while (j + len <= m && y[j] == c)
                        {
                            bool ok = true;
                            for (int k = j; k < j + len; k++)
                                if (y[k] != c) { ok = false; break; }
                            if (ok) dp[i + 1, j + len] = true;
                            len++;
                        }
                    }
                }
            }
        }
        return dp[n, m];
    }

    private static int MakeChoice(int rightLimit = -1, int leftLimit = 1)
    {
        while (true)
        {
            if(!int.TryParse(
                Console.ReadLine(),
                out int choice
                ))
            {
                Console.WriteLine("Введите целое число.");
                continue;
            }

            if (rightLimit != -1 && (choice > rightLimit || choice < leftLimit))
                Console.WriteLine("Введите целое число 1-2");
            else return choice;
        }
    }

    private static (string, string) GetStrings()
    {
        Console.Write("Введите первую строку из нулей и единиц: ");
        string x = Console.ReadLine();
        CheckLine(x, true);
        Console.Write("Введите вторую строку из A и B: ");
        string y = Console.ReadLine();
        CheckLine(y, false);

        return (x, y);
    }

    private static (string, string) GenerateStrings()
    {
        Console.Write("Введите длину первой строки из нулей и единиц: ");
        int xl = MakeChoice();
        Console.Write("Введите длину второй строки из A и B: ");
        int yl = MakeChoice();

        string x = "", y = "";

        var rng = new Random();

        StringBuilder sb = new(xl);

        for (int i = 0; i < xl; i++)
        {
            sb.Append(rng.Next(2));
        }

        x = sb.ToString();

        sb.Clear();

        for (int i = 0; i < yl;  i++)
        {
            sb.Append(rng.Next(2) == 1 ? 'A' : 'B');
        }

        y = sb.ToString();

        return (x, y);
    }

    private static void CheckLine(string x, bool mode)
    {
        bool isCorrect = true;
        switch(mode)
        {
            case true:
                foreach (char c in x)
                {
                    if (c != '0' && c != '1')   
                    {
                        isCorrect = false;
                        break;
                    }
                }
                break;

            case false:
                foreach (char c in x)
                {
                    if (c != 'A' && c != 'B')
                    {
                        isCorrect = false;
                        break;
                    }
                }
                break;       
        }

        if (!isCorrect) throw new FormatException("Неверный формат введённой строки");
        return;
    }
}