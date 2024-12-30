using LinearAlgebra;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace MathVectorUnitTests;

public class MathVectorUnitTests
{
    [Fact]
    public void Constructor_ValidArrayInput_CreatesVector()
    {
        var components = new[] { 1.0, 2.0, 3.0 };
        var vector = new MathVector(components);

        Assert.Multiple(() =>
        {
            Assert.That(vector.Dimensions, Is.EqualTo(3));
            
            for (int i = 0; i < components.Length; i++)
            {
                Assert.That(vector[i], Is.EqualTo(components[i]));
            }
        });
    }

    [Fact]
    public void Constructor_EmptyArray_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new MathVector();
        });
    }

    [Fact]
    public void Constructor_ValidListInput_CreatesVector()
    {
        var components = new List<double> { 1.0, 2.0, 3.0 };
        var vector = new MathVector(components);

        Assert.Multiple(() =>
        {
            Assert.That(vector.Dimensions, Is.EqualTo(3));
            
            for (int i = 0; i < components.Count; i++)
            {
                Assert.That(vector[i], Is.EqualTo(components[i]));
            }
        });
    }
    
    [Fact]
    public void Constructor_EmptyListInput_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new MathVector(new List<double>());
        });
    }

    [Fact]
    public void TestDimensions_GetVectorDimensions_ReturnsCorrectValue()
    {
        IMathVector vector = new MathVector(1.0, 2.0, 3.0);
        
        var dimensions = vector.Dimensions;
        
        Assert.That(dimensions, Is.EqualTo(3));
    }
    
    [Fact]
    public void Indexer_ValidIndex_ReturnsCorrectValue()
    {
        var vector = new MathVector(1.0, 2.0, 3.0);
        
        Assert.That(vector[1], Is.EqualTo(2.0));
    }
    
    [Fact]
    public void Indexer_ValidIndex_SetsCorrectValue()
    {
        IMathVector vector = new MathVector(1.0, 2.0, 3.0);
        
        vector[0] = 6;
        
        Assert.That(vector[0], Is.EqualTo(6.0));
    }
    
    [Fact]
    public void Indexer_GetValueInvalidIndex_ThrowsArgumentException()
    {
        IMathVector vector = new MathVector(1.0, 2.0, 3.0);
        Assert.Throws<ArgumentException>(() =>
        {
            _ = vector[vector.Dimensions];
        });
    }
    
    [Fact]
    public void Indexer_SetValueInvalidIndex_ThrowsArgumentException()
    {
        IMathVector vector = new MathVector(1.0, 2.0, 3.0);
        Assert.Throws<ArgumentException>(() =>
        {
            vector[vector.Dimensions] = 5;
        });
    }

    [Fact]
    public void Indexer_SetNegativeIndex_ThrowsArgumentException()
    {
        var vector = new MathVector(1.0, 2.0, 3.0);
        
        Assert.Throws<ArgumentException>(() =>
        {
            vector[-3] = 3.0;
        });
    }
    
    [Fact]
    public void Indexer_GetNegativeIndex_ThrowsArgumentException()
    {
        var vector = new MathVector(1.0, 2.0, 3.0);
        
        Assert.Throws<ArgumentException>(() => 
        {
            _ = vector[-1];
        });
    }

    [Fact]
    public void Length_ValidVector_ReturnsCorrectLengthPositiveNumbers()
    {
        var vector = new MathVector(3.0, 4.0);
        var length = vector.Length;
        
        Assert.That(length, Is.EqualTo(5.0));
    }
    
    [Fact]
    public void Length_ValidVector_ReturnsCorrectLengthNegativeNumbers()
    {
        var vector = new MathVector(-3.0, -4.0);
        var length = vector.Length;
        
        Assert.That(length, Is.EqualTo(5.0));
    }
    
    [Fact]
    public void Length_ValidVector_ReturnsCorrectLengthMixedNumbers()
    {
        var vector = new MathVector(3.0, -4.0);
        var length = vector.Length;
        
        Assert.That(length, Is.EqualTo(5.0));
    }

    [Fact]
    public void SumPositiveNumber_ValidNumber_ReturnsNewVector()
    {
        var vector = new MathVector(1.0, 2.0, 3.0);
        var result = vector.SumNumber(5.5);
        
        Assert.That(result, Is.EqualTo(new MathVector(6.5, 7.5, 8.5)));
    }
    
    [Fact]
    public void SumNegativeNumber_ValidNumber_ReturnsNewVector()
    {
        var vector = new MathVector(1.0, 2.0, 3.0);
        var result = vector.SumNumber(-0.5);
        
        Assert.That(result, Is.EqualTo(new MathVector(0.5, 1.5, 2.5)));
    }
    
    [Fact]
    public void SumInfVector_ValidNumber_ReturnsNewVector()
    {
        var vector = new MathVector(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        var result = vector.SumNumber(4534);
        
        Assert.That(result, Is.EqualTo(new MathVector(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity)));
    }

    [Fact]
    public void MultiplyPositiveNumber_ValidNumber_ReturnsNewVector()
    {
        var vector = new MathVector(1, 2.47, 3.3);
        var result = vector.MultiplyNumber(2);
        
        Assert.That(result, Is.EqualTo(new MathVector(2, 4.94, 6.6)));
    }
    
    [Fact]
    public void MultiplyNegativeNumber_ValidNumber_ReturnsNewVector()
    {
        var vector = new MathVector(1.0, 2.47, 3.3);
        var result = vector.MultiplyNumber(-2);
        
        Assert.That(result, Is.EqualTo(new MathVector(-2, -4.94, -6.6)));
    }
    
    [Fact]
    public void MultiplyZero_ValidNumber_ReturnsNewVector()
    {
        var vector = new MathVector(1.0, 2.0, 3.0);
        var result = vector.MultiplyNumber(0);
        
        Assert.That(result, Is.EqualTo(new MathVector(0.0, 0.0, 0.0)));
    }
    
    [Fact]
    public void MultiplyInfVector_ValidNumber_ReturnsNewVector()
    {
        var vector = new MathVector(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        var result = vector.MultiplyNumber(4534);
        
        Assert.That(result, Is.EqualTo(new MathVector(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity)));
    }
    
    [Fact]
    public void Sum_ValidVectors_ReturnsSumVector()
    {
        var vector1 = new MathVector(1.0, 2.0, 3.0);
        var vector2 = new MathVector(4.0, -5.0, 6.0);
        var result = vector1.Sum(vector2);
        
        Assert.That(result, Is.EqualTo(new MathVector(5.0, -3.0, 9.0)));
    }

    [Fact]
    public void Sum_DifferentDimensions_ThrowsArgumentException()
    {
        var vector1 = new MathVector(1.0, 2.0);
        var vector2 = new MathVector(3.0);
        
        Assert.Throws<ArgumentException>(() => vector1.Sum(vector2));
    }

    
    
    [Fact]
    public void ScalarMultiply_ValidVectors_ReturnsCorrectResult()
    {
        var vector1 = new MathVector(1.0, 2.2, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0);
        var result = vector1.ScalarMultiply(vector2);
        
        Assert.That(result, Is.EqualTo(33.0)); // 1*4 + 2.2*5 + 3*6 = 33
    }
    
    [Fact]
    public void ScalarMultiply_DifferentDimensions_ThrowsArgumentException()
    {
        var vector1 = new MathVector(1.0, 2.2, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0, 9.0);
        
        Assert.Throws<ArgumentException>(() => vector1.ScalarMultiply(vector2));
    }

    [Fact]
    public void CalcDistance_ValidVectors_ReturnsCorrectDistance()
    {
        var vector1 = new MathVector(1.0, 2.5, 3.0);
        var vector2 = new MathVector(4.0, 5.5, 6.0);
        var result = vector1.CalcDistance(vector2);
        
        Assert.That(result, Is.EqualTo(Math.Sqrt(27.0))); // sqrt((4-1)^2 + (5.5-2.5)^2 + (6-3)^2)
    }
    
    [Fact]
    public void CalcDistance_DifferentDimensions_ThrowsArgumentException()
    {
        var vector1 = new MathVector(1.0, 2.2, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0, 9.0);
        
        Assert.Throws<ArgumentException>(() => vector1.CalcDistance(vector2));
    }

    [Fact]
    public void Operator_AdditionVectors_ReturnsSumVector()
    {
        var vector1 = new MathVector(1.0, 2.0, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0);
        var result = vector1 + vector2;
        
        Assert.That(result, Is.EqualTo(new MathVector(5.0, 7.0, 9.0)));
    }
    
    [Fact]
    public void Operator_AdditionInfVector_ReturnsProductVector()
    {
        var vector1 = new MathVector(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        var vector2 = new MathVector(4.0, 5.0, 6.0);
        var result = vector1 + vector2;
        
        Assert.That(result, Is.EqualTo(new MathVector(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity)));
    }
    
    [Fact]
    public void Operator_AdditionVectorsWithDifferentDimensions_ThrowsArgumentException()
    {
        var vector1 = new MathVector(1.0, 2.2, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0, 9.0);

        Assert.Throws<ArgumentException>(() => _ = vector1 + vector2);
    }

    [Fact]
    public void Operator_SubtractionVectors_ReturnsDifferenceVector()
    {
        var vector1 = new MathVector(4.0, 5.5, 6.0);
        var vector2 = new MathVector(1.0, 2.2, 3.0);
        var result = vector1 - vector2;
        
        Assert.That(result, Is.EqualTo(new MathVector(3.0, 3.3, 3.0)));
    }
    
    [Fact]
    public void Operator_SubtractionVectorsWithDifferentDimensions_ThrowsArgumentException()
    {
        var vector1 = new MathVector(1.0, 2.2, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0, 9.0);

        Assert.Throws<ArgumentException>(() => _ = vector1 - vector2);
    }
    
    [Fact]
    public void Operator_SubtractionInfVector_ReturnsProductVector()
    {
        var vector1 = new MathVector(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        var vector2 = new MathVector(4.0, 5.0, 6.0);
        var result = vector1 - vector2;
        
        Assert.That(result, Is.EqualTo(new MathVector(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity)));
    }

    [Fact]
    public void Operator_MultiplicationVectors_ReturnsProductVector()
    {
        var vector1 = new MathVector(1.0, 2.0, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0);
        var result = vector1 * vector2;
        
        Assert.That(result, Is.EqualTo(new MathVector(4.0, 10.0, 18.0)));
    }
    
    [Fact]
    public void Operator_MultiplicationInfVector_ReturnsProductVector()
    {
        var vector1 = new MathVector(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        var vector2 = new MathVector(4.0, 5.0, 6.0);
        var result = vector1 * vector2;
        
        Assert.That(result, Is.EqualTo(new MathVector(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity)));
    }

    
    [Fact]
    public void Operator_MultiplicationVectorsWithDifferentDimensions_ThrowsArgumentException()
    {
        var vector1 = new MathVector(1.0, 2.2, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0, 9.0);

        Assert.Throws<ArgumentException>(() => _ = vector1 * vector2);
    }
    
    [Fact]
    public void Operator_DivisionVectors_ReturnsProductVector()
    {
        var vector1 = new MathVector(4.0, 4.0, 4.5);
        var vector2 = new MathVector(1.0, 2.0, 4.5);
        var result = vector1 / vector2;
        
        Assert.That(result, Is.EqualTo(new MathVector(4.0, 2.0, 1.0)));
    }
    
    [Fact]
    public void Operator_DivisionInfVector_ReturnsProductVector()
    {
        var vector1 = new MathVector(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        var vector2 = new MathVector(4.0, 5.0, 6.0);
        var result = vector1 / vector2;
        
        Assert.That(result, Is.EqualTo(new MathVector(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity)));
    }
    
    [Fact]
    public void Operator_DivisionVectorsWithDifferentDimensions_ThrowsArgumentException()
    {
        var vector1 = new MathVector(1.0, 2.2, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0, 9.0);

        Assert.Throws<ArgumentException>(() => _ = vector1 / vector2);
    }
    
    [Fact]
    public void Operator_DivisionVectorsWithZeroArgument_ThrowsArgumentException()
    {
        var vector1 = new MathVector(1.0, 2.2, 3.0);
        var vector2 = new MathVector(4.0, 0.0, 6.0);

        Assert.Throws<ArgumentException>(() => _ = vector1 / vector2);
    }
    
    [Fact]
    public void Operator_ToString_ReturnsCorrectValue()
    {
        var vector = new MathVector(1.1, -2.2, 3);
        
        Assert.That(vector.ToString(), Is.EqualTo("(1.1, -2.2, 3)"));
    }
}