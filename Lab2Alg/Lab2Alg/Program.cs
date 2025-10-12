class Program
{
    public static void Main(string[] args)
    {
        Console.Write("Введите длину массива: ");

        int[] mas = new int[int.Parse(Console.ReadLine())];

        Random random = new Random();
        for (int i = 0; i < mas.Length; i++)
        {
            mas[i] = random.Next(201);
            Console.Write(mas[i] + "  ");
        }

        InsertionSort(mas);

        Console.WriteLine();
        for (int i = 0; i < mas.Length; i++)
        {
            Console.Write(mas[i] + "  ");
        }
    }

    static int[] InsertionSort(int[] mas) //сортировка включением
    {
        for (int i = 1; i < mas.Length; i++)
            for (int j = i; j > 0 && mas[j - 1] > mas[j]; j--) // пока j>0 и элемент j-1 > j, x-массив int
                swap(mas, j, j-1);
        return mas;
    }

    static void swap(int[] mas, int x, int y)
    {
        int buf = mas[x];
        mas[x] = mas[y];
        mas[y] = buf;
    }
}