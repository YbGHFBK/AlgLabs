public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(CanTransform("011", "AABB"));
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
}