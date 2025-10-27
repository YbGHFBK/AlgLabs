using System.Data.Common;

struct SortResult
{
    public int[] mas;
    public int comparesCount;
    public int swapsCount;

    public SortResult(int[] mas, int comparesCount, int swapsCount)
    {
        this.mas = mas;
        this.comparesCount = comparesCount;
        this.swapsCount = swapsCount;
    }
}

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
        }
        Console.WriteLine("Исходный массив: ");
        PrintMas(mas);

        Console.WriteLine("Сортировка включением: ");
        SortResult res1 = InsertionSort(mas);
        PrintResult(mas, res1);

        Console.WriteLine("Сортировка выбором: ");
        SortResult res2 = SelectionSort(mas);
        PrintResult(mas, res2);

        Console.WriteLine("Сортировка обменом(пузырьком): ");
        SortResult res3 = BubbleSort(mas);
        PrintResult(mas, res3);

        Console.WriteLine("Сортировка разделением(быстрая сортировка): ");
        SortResult res4 = QuickSort(mas, 0, mas.Length - 1);
        PrintResult(mas, res4);

        Console.WriteLine("Пирамидальная сортировка: ");
        SortResult res5 = HeapSort(mas);
        PrintResult(mas, res5);
    }

    static void PrintMas(int[] mas)
    {
        foreach(int elem in mas)
        {
            Console.Write(elem + " ");
        }
        Console.WriteLine('\n');
    }

    static void PrintResult(int[] mas, SortResult res)
    {
        for (int i = 0; i < mas.Length; i++)
        {
            Console.Write(mas[i] + "  ");
        }
        Console.WriteLine("\nСравнений: " + res.comparesCount + "\nПерестановок: " + res.swapsCount + '\n');
    }

    static SortResult InsertionSort(int[] mas1) //сортировка включением
    {
        int[] mas = mas1.ToArray();
        int comparesCount = 0;
        int swapsCount = 0;

        for (int i = 1; i < mas.Length; i++)
        {
            for (int j = i; j > 0 && mas[j - 1] > mas[j]; j--) // пока j>0 и элемент j-1 > j, переставляет пока больше
            {
                comparesCount++;
                Swap(ref mas[j], ref mas[j - 1]);
                swapsCount++;
            }
            comparesCount++;
        }

        return new SortResult(mas, comparesCount, swapsCount);
    }

    static SortResult SelectionSort(int[] mas1) //Сортировка выбором
    {
        int[] mas = mas1.ToArray();
        int comparesCount = 0;
        int swapsCount = 0;

        for (int i = 0; i < mas.Length - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < mas.Length; j++)
            {
                if (mas[j] < mas[minIndex]) //ищет минимальное число и ставит в начало
                {
                    minIndex = j;
                }
                comparesCount++;
            }
            Swap(ref mas[i], ref mas[minIndex]);
            swapsCount++;
        }

        return new SortResult(mas, comparesCount, swapsCount);
    }

    static SortResult BubbleSort(int[] mas1) //Сортировка обменом
    {
        int[] mas = mas1.ToArray();
        int comparesCount = 0;
        int swapsCount = 0;

        if (mas == null || mas.Length < 2) return new SortResult(mas, 0, 0);

        int n = mas.Length;
        for (int i = 0; i < n - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < n - 1 - i; j++) 
            {
                comparesCount++;
                if (mas[j] > mas[j + 1]) //сравнивает со следующим
                {
                    Swap(ref mas[j], ref mas[j + 1]);
                    swapsCount++;
                    swapped = true;
                }
            }

            if (!swapped) break;
        }

        return new SortResult(mas, comparesCount, swapsCount);
    }

    static SortResult QuickSort(int[] mas1, int left, int right) //сортировка разделением, быстрая сортировка
    {
        int[] mas = mas1.ToArray();
        int comparesCount = 0;
        int swapsCount = 0;

        if (left >= right) return new SortResult(mas, 0, 0);

        int len = right - left + 1;

        Swap(ref mas[left + 1], ref mas[left]);
        Swap(ref mas[right - 1], ref mas[right]);
        swapsCount += 2;

        comparesCount++;
        if (mas[left] > mas[right])
        {
            Swap(ref mas[left], ref mas[right]);
            swapsCount++;
        }

        int pivot1 = mas[left];
        int pivot2 = mas[right];

        int less = left + 1;
        int great = right - 1;

        for (int k = less; k <= great; k++)
        {
            comparesCount++;
            if (mas[k] < pivot1)
            {
                Swap(ref mas[k], ref mas[less]);
                swapsCount++;
                less++;
            }
            else
            {
                if (mas[k] > pivot2)
                {
                    while (mas[great] > pivot2 && k < great)
                    {
                        comparesCount++;
                        great--;
                    }
                    comparesCount++;
                    Swap(ref mas[k], ref mas[great]);
                    swapsCount++;
                    great--;
                    comparesCount++;
                    if (mas[k] < pivot1)
                    {
                        Swap(ref mas[k], ref mas[less]);
                        swapsCount++;
                        less++;
                    }
                }
            }
        }

        less--;
        great++;

        Swap(ref mas[left], ref mas[less]);
        Swap(ref mas[right], ref mas[great]);
        swapsCount += 2;

        SortResult res1 = QuickSort(mas, left, less - 1);
        comparesCount += res1.comparesCount;
        swapsCount += res1.swapsCount;

        comparesCount++;
        if (pivot1 < pivot2)
        {
            SortResult res2 = QuickSort(mas, less + 1, great - 1);
            comparesCount += res2.comparesCount;
            swapsCount += res2.swapsCount;
        }

        SortResult res3 = QuickSort(mas, great + 1, right);
        comparesCount += res3.comparesCount;
        swapsCount += res3.swapsCount;

        return new SortResult(mas, comparesCount, swapsCount);
    }

    static SortResult HeapSort(int[] mas1) //пирамидальная сортировка
    {
        int[] mas = mas1.ToArray();
        int comparesCount = 0;
        int swapsCount = 0;

        if (mas == null || mas.Length < 2) return new SortResult(mas, 0, 0);

        int n = mas.Length;

        for (int i = Parent(n - 1); i >= 0; i--)
        {
            comparesCount++;
            (int, int) res1 = SiftDown(mas, i, n - 1);
            comparesCount += res1.Item1;
            swapsCount += res1.Item2;
        }

        for (int end = n - 1; end > 0; end--)
        {
            Swap(ref mas[0], ref mas [end]);
            swapsCount++;
            (int, int) res2 = SiftDown(mas, 0, end - 1);
            comparesCount += res2.Item1;
            swapsCount += res2.Item2;
        }

        return new SortResult(mas, comparesCount, swapsCount);
    }

    static (int, int) SiftDown(int[] mas, int start, int end)
    {
        int root = start;
        int comparesCount = 0;
        int swapsCount = 0;

        while (true)
        {

            int leftChild = 2 * root + 1;
            if (leftChild > end) break;

            int swapIdx = root;

            if (mas[swapIdx] < mas[leftChild])
                swapIdx = leftChild;
            comparesCount++;

            int rightChild = leftChild + 1;
            if (rightChild <= end && mas[swapIdx] < mas[rightChild])
                swapIdx = rightChild;
            comparesCount++;

            if (swapIdx == root)
                break;

            Swap(ref mas[root], ref mas[swapIdx]);
            swapsCount++;
            root = swapIdx;
        }

        return (comparesCount, swapsCount);
    }

    static int Parent(int index) => (index - 1) / 2;

    static void Swap(ref int x, ref int y)
    {
        int buf = x;
        x = y;
        y = buf;
    }
}