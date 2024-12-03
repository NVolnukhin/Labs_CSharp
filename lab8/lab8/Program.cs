using LinearAlgebra;

namespace lab8;

class Program
{
    static void Main()
    {
        MathVector v1 = new MathVector(3, 8);
        MathVector v2 = new MathVector(1, 2);
        MathVector v3 = new MathVector(1, 2, 3);      
        MathVector infVec = new MathVector(double.PositiveInfinity, double.NegativeInfinity);      

        Console.WriteLine("\nВектор 1: " + v1);
        Console.WriteLine("Вектор 2: " + v2);
        Console.WriteLine("Вектор 3: " + v3);
        Console.WriteLine("Вектор бесконечности: " + infVec);

        IMathVector v1Mod = v1 + 70;
        Console.WriteLine("\n" + v1 + " + 70 = " + v1Mod);

        IMathVector v2Mod = v2 * 7;
        Console.WriteLine("\n" + v2 + " * 7 = " + v2Mod);

        Console.WriteLine("\n Попытка сложить вектора 1 и 3...");
        try
        {
            IMathVector sum = v1 + v3;
            Console.WriteLine("Сумма векторов 1 и 3: " + sum);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }

        Console.WriteLine("\n Попытка перемножить вектора 1 и 2...");
        try
        {
            IMathVector multiply = v1 * v2;
            Console.WriteLine($"Произведение векторов 1 и 2: {multiply}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
        
        Console.WriteLine("\n Попытка перемножить вектора 1 и бесконечный...");
        try
        {
            IMathVector multiply = v1 * infVec;
            Console.WriteLine($"Произведение векторов 1 и inf: {multiply}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }

        Console.WriteLine("\n Попытка найти евклидово расстояние между векторами 1 и 2...");
        try
        {
            double distance = v1.CalcDistance(v2);
            Console.WriteLine("Евклидово расстояние между векторами 1 и 2: " + distance);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
        
        Console.WriteLine("\n Попытка найти скалярное произведение векторов 1 и 2...");
        try
        {
            double scalar = v1.ScalarMultiply(v2);
            Console.WriteLine("Скалярное произведение векторов 1 и 2: " + scalar);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
        
        Console.WriteLine("\n 0ой элемент вектора 3: " + v3[0]);

        Console.WriteLine("\n Длина вектора 3: " + v3.Length);

        Console.WriteLine("\nПрисвоим 1ому элементу вектора 3 значение 88, а 10ому - 77");
        try
        {
            v3[1] = 88;
            v3[10] = 77;
        }
        catch (IndexOutOfRangeException ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
        Console.WriteLine("Вектор 3 после изменения: " + v3);
    }
}