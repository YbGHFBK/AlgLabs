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

        //QuickSort(mas, 0, mas.Length - 1);
        HeapSort(mas);

        Console.WriteLine();
        for (int i = 0; i < mas.Length; i++)
        {
            Console.Write(mas[i] + "  ");
        }
    }

    static int[] InsertionSort(int[] mas) //сортировка включением
    {
        for (int i = 1; i < mas.Length; i++)
            for (int j = i; j > 0 && mas[j - 1] > mas[j]; j--) // пока j>0 и элемент j-1 > j 
                Swap(ref mas[j], ref mas[j-1]);
        return mas;
    }

    static int[] SelectionSort(int[] mas) //Сортировка выбором
    {
        for (int i = 0; i < mas.Length - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < mas.Length; j++)
            {
                if (mas[j] < mas[minIndex])
                {
                    minIndex = j;
                }
            }
            Swap(ref mas[i], ref mas[minIndex]);
        }

        return mas;
    }

    static int[] BubbleSort(int[] mas) //Сортировка обменом
    {
        if (mas == null || mas.Length < 2) return mas;

        int n = mas.Length;
        for (int i = 0; i < n - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < n - 1 - i; j++)
            {
                if (mas[j] > mas[j + 1])
                {
                    Swap(ref mas[j], ref mas[j + 1]);
                    swapped = true;
                }
            }

            if (!swapped) break;
        }

        return mas;
    }

    static int[] QuickSort(int[] mas, int left, int right) //сортировка разделением
    {
        if (left >= right) return mas;

        int len = right - left + 1;

        Swap(ref mas[left + 1], ref mas[left]);
        Swap(ref mas[right - 1], ref mas[right]);

        if (mas[left] > mas[right])
            Swap(ref mas[left], ref mas[right]);

        int pivot1 = mas[left];
        int pivot2 = mas[right];

        int less = left + 1;
        int great = right - 1;

        for (int k = less; k <= great; k++)
        {
            if (mas[k] < pivot1)
            {
                Swap(ref mas[k], ref mas[less]);
                less++;
            }
            else if (mas[k] > pivot2)
            {
                while (mas[great] > pivot2 && k < great)
                    great--;
                Swap(ref mas[k], ref mas[great]);
                great--;
                if (mas[k] < pivot1)
                {
                    Swap(ref mas[k], ref mas[less]);
                    less++;
                }
            }
        }

        less--;
        great++;

        Swap(ref mas[left], ref mas[less]);
        Swap(ref mas[right], ref mas[great]);

        QuickSort(mas, left, less - 1);
        if (pivot1 < pivot2)
            QuickSort(mas, less + 1, great - 1);
        QuickSort(mas, great + 1, right);

        return mas;
    }

    static int[] HeapSort(int[] mas) //пирамидальная сортировка
    {
        if (mas == null || mas.Length < 2) return mas;

        int n = mas.Length;

        // Построение максимум-кучи (heap) — O(n)
        for (int i = Parent(n - 1); i >= 0; i--)
            SiftDown(mas, i, n - 1);

        // Извлекаем максимум по одному и ставим в конец
        for (int end = n - 1; end > 0; end--)
        {
            Swap(ref mas[0], ref mas [end]);        // максимум в конец
            SiftDown(mas, 0, end - 1); // восстанавливаем heap на [0..end-1]
        }

        return mas;
    }

    // SiftDown: просеивание вниз элемента в диапазоне [start..end]
    static void SiftDown(int[] mas, int start, int end)
    {
        int root = start;

        while (true)
        {
            int leftChild = 2 * root + 1;
            if (leftChild > end) break; // выхода из дерева

            int swapIdx = root;

            // если левый ребёнок больше корня
            if (mas[swapIdx] < mas[leftChild])
                swapIdx = leftChild;

            // если есть правый ребёнок и он больше того, что выбран
            int rightChild = leftChild + 1;
            if (rightChild <= end && mas[swapIdx] < mas[rightChild])
                swapIdx = rightChild;

            if (swapIdx == root)
                break; // корень больше обоих детей — порядок восстановлен

            Swap(ref mas[root], ref mas[swapIdx]);
            root = swapIdx; // продолжить просеивание вниз
        }
    }

    static int Parent(int index) => (index - 1) / 2;

    static void Swap(ref int x, ref int y)
    {
        int buf = x;
        x = y;
        y = buf;
    }
}