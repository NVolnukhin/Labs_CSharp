namespace LinearAlgebra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MathVector : IMathVector
{
    private double[] _components;

    public MathVector(params double[] _components)
    {
        if (_components.Length == 0)
        {
            throw new ArgumentException("Вектор должен содержать хотя бы одно значение.");
        }
        this._components = new double[_components.Length];
        Array.Copy(_components, this._components, _components.Length);
    }

    public int Dimensions => _components.Length;

    public double this[int i]
    {
        get 
        {
            if (i < 0 || i >= this.Dimensions)
            {
                throw new ArgumentException("Неверный индекс.");
            }
            else 
            {
                return _components[i];
            }
        }
        set 
        {
            if (i < 0 || i >= this.Dimensions)
            {
                throw new ArgumentException("Неверный индекс.");
            }
            else
            {
                _components[i] = value;
            }
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
        return $"({string.Join(", ", _components)})";
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
        try
        {
            return vector1.Sum(vector2);
        }
        catch (ArgumentException)
        {
            throw;
        }
    }

    public static IMathVector operator -(MathVector vector1, IMathVector vector2)
    {
        try
        {
            return vector1.Sum(vector2.MultiplyNumber(-1));
        }
        catch (ArgumentException)
        {
            throw;
        }
    }

    public static IMathVector operator *(MathVector vector1, IMathVector vector2)
    {
        try
        {
            return vector1.Multiply(vector2);
        }
        catch (ArgumentException)
        {
            throw;
        }
    }
    
    public static IMathVector operator /(MathVector vector1, MathVector vector2)
    {
        try
        {
            return vector1.Division(vector2);
        }
        catch (ArgumentException)
        {
            throw;
        }
    }
}