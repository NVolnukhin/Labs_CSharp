using System.Globalization;

namespace LinearAlgebra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MathVector : IMathVector
{
    private readonly double[] _components;

    public MathVector(params object[] components)
    {
        if (components is [IEnumerable enumerable])
        {
            // Если передан список, массив или любая другая коллекция
            _components = enumerable.Cast<object>()
                .Select(Convert.ToDouble)
                .ToArray();
        }
        else
        {
            // Если переданы отдельные значения
            _components = components.Select(Convert.ToDouble).ToArray();
        }

        if (_components.Length == 0)
        {
            throw new ArgumentException("Вектор должен содержать хотя бы одно значение.");
        }
    }

    public int Dimensions => _components.Length;

    public double this[int i]
    {
        get 
        {
            if (i < 0 || i >= Dimensions)
            {
                throw new ArgumentException("Неверный индекс.");
            }
            
            return _components[i];
        }
        set 
        {
            if (i < 0 || i >= Dimensions)
            {
                throw new ArgumentException("Неверный индекс.");
            }
            
            _components[i] = value;
            
        }
    }

    public double Length => Math.Sqrt(_components.Sum(c => c * c));

    public IMathVector SumNumber(double number)
    {
        return new MathVector(_components.Select(c => c + number).ToArray());
    }

    public IMathVector MultiplyNumber(double number)
    {
        return new MathVector(_components.Select(c => c * number).ToArray());
    }

    public IMathVector Sum(IMathVector vector)
    {
        if (Dimensions != vector.Dimensions)
        {
            throw new ArgumentException("Размерности векторов не совпадают.");
        }
        return new MathVector(_components.Select((c, i) => c + vector[i]).ToArray());
    }

    public IMathVector Multiply(IMathVector vector)
    {
        if (Dimensions != vector.Dimensions)
        {
            throw new ArgumentException("Размерности векторов не совпадают.");
        }
        return new MathVector(_components.Select((c, i) => c * vector[i]).ToArray());
    }
    
    public IMathVector Division(IMathVector vector)
    {
        if (Dimensions != vector.Dimensions)
        {
            throw new ArgumentException("Размерности векторов не совпадают.");
        }
        
        for (int i = 0; i < vector.Dimensions; i++)
        {
            if (vector[i] == 0.0)
            {
                throw new ArgumentException("Деление на ноль");
            }
        }
        
        return new MathVector(_components.Select((c, i) => c / vector[i]).ToArray());
    }

    public double ScalarMultiply(IMathVector vector)
    {
        if (Dimensions != vector.Dimensions)
        {
            throw new ArgumentException("Размерности векторов не совпадают.");
        }
        return _components.Select((c, i) => c * vector[i]).Sum();
    }

    public double CalcDistance(IMathVector vector)
    {
        if (Dimensions != vector.Dimensions)
        {
            throw new ArgumentException("Размерности векторов не совпадают.");
        }
        return Math.Sqrt(_components.Select((c, i) => Math.Pow(c - vector[i], 2)).Sum());
    }

    public IEnumerator<double> GetEnumerator()
    {
        return ((IEnumerable<double>)_components).GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        var cultureInfo = new CultureInfo("us-US"); // или любая локаль, использующая запятую

        return $"({string.Join(", ", _components.Select(c => c.ToString(cultureInfo)))})";
    }

    public static IMathVector operator /(MathVector vector, double number)
    {
        if (number == 0.0)
        {
            throw new ArgumentException("деление на ноль");
        }
        return vector.SumNumber(1/number);
    }
    
    public static IMathVector operator +(MathVector vector, double number)
    {
        return vector.SumNumber(number);
    }

    public static IMathVector operator -(MathVector vector, double number)
    {
        return vector.SumNumber(-number);
    }

    public static IMathVector operator *(MathVector vector, double number)
    {
        return vector.MultiplyNumber(number);
    }

    public static IMathVector operator +(MathVector vector1, IMathVector vector2)
    { 
        return vector1.Sum(vector2);
    }

    public static IMathVector operator -(MathVector vector1, IMathVector vector2)
    {
        return vector1.Sum(vector2.MultiplyNumber(-1));
    }

    public static IMathVector operator *(MathVector vector1, IMathVector vector2)
    { 
        return vector1.Multiply(vector2);
    }
    
    public static IMathVector operator /(MathVector vector1, MathVector vector2)
    {
        return vector1.Division(vector2);
    }
}