using LinearAlgebra;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace MathVectorUnitTests;

public class MathVectorUnitTests
{
    [Fact]
    public void Constructor_ValidInput_CreatesVector()
    {
        var components = new[] { 1.0, 2.0, 3.0 };
        var vector = new MathVector(components);

        Assert.Multiple(() =>
        {
            Assert.That(vector.Dimensions, Is.EqualTo(3));
            Assert.That(vector[0], Is.EqualTo(1.0));
            Assert.That(vector[1], Is.EqualTo(2.0));
            Assert.That(vector[2], Is.EqualTo(3.0));
        });
    }

    [Fact]
    public void Constructor_EmptyArray_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var vector = new MathVector();
        });
    }

    [Fact]
    public void Indexer_ValidIndex_ReturnsCorrectValue()
    {
        var vector = new MathVector(1.0, 2.0, 3.0);
        
        Assert.That(vector[1], Is.EqualTo(2.0));
    }

    [Fact]
    public void Indexer_InvalidIndex_ThrowsArgumentException()
    {
        
        var vector = new MathVector(1.0, 2.0, 3.0);
        
        Assert.Throws<ArgumentException>(() => 
        {
            var d = vector[-1];
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var d = vector[3];
        });
    }

    [Fact]
    public void Length_ValidVector_ReturnsCorrectLength()
    {
        var vector = new MathVector(3.0, 4.0);
        var length = vector.Length;
        
        Assert.That(length, Is.EqualTo(5.0));
    }

    [Fact]
    public void SumNumber_ValidNumber_ReturnsNewVector()
    {
        var vector = new MathVector(1.0, 2.0, 3.0);
        var result = vector.SumNumber(5.0);
        
        Assert.That(result.ToString(), Is.EqualTo(new MathVector(6.0, 7.0, 8.0).ToString()));
    }

    [Fact]
    public void MultiplyNumber_ValidNumber_ReturnsNewVector()
    {
        var vector = new MathVector(1.0, 2.0, 3.0);
        var result = vector.MultiplyNumber(2.0);
        
        Assert.That(result.ToString(), Is.EqualTo(new MathVector(2.0, 4.0, 6.0).ToString()));
    }

    [Fact]
    public void Sum_ValidVectors_ReturnsSumVector()
    {
        var vector1 = new MathVector(1.0, 2.0, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0);
        var result = vector1.Sum(vector2);
        
        Assert.That(result.ToString(), Is.EqualTo(new MathVector(5.0, 7.0, 9.0).ToString()));
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
        var vector1 = new MathVector(1.0, 2.0, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0);
        var result = vector1.ScalarMultiply(vector2);
        
        Assert.That(result, Is.EqualTo(32.0)); // 1*4 + 2*5 + 3*6 = 32
    }

    [Fact]
    public void CalcDistance_ValidVectors_ReturnsCorrectDistance()
    {
        var vector1 = new MathVector(1.0, 2.0, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0);
        var result = vector1.CalcDistance(vector2);
        
        Assert.That(result, Is.EqualTo(Math.Sqrt(27.0))); // sqrt((4-1)^2 + (5-2)^2 + (6-3)^2)
    }

    [Fact]
    public void Operator_Addition_ReturnsSumVector()
    {
        var vector1 = new MathVector(1.0, 2.0, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0);
        var result = vector1 + vector2;
        
        Assert.That(result.ToString(), Is.EqualTo(new MathVector(5.0, 7.0, 9.0).ToString()));
    }

    [Fact]
    public void Operator_Subtraction_ReturnsDifferenceVector()
    {
        var vector1 = new MathVector(4.0, 5.0, 6.0);
        var vector2 = new MathVector(1.0, 2.0, 3.0);
        var result = vector1 - vector2;
        
        Assert.That(result.ToString(), Is.EqualTo(new MathVector(3.0, 3.0, 3.0).ToString()));
    }

    [Fact]
    public void Operator_Multiplication_ReturnsProductVector()
    {
        var vector1 = new MathVector(1.0, 2.0, 3.0);
        var vector2 = new MathVector(4.0, 5.0, 6.0);
        var result = vector1 * vector2;
        
        Assert.That(result.ToString(), Is.EqualTo(new MathVector(4.0, 10.0, 18.0).ToString()));
    }
}