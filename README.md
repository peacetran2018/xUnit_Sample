## 1. Introduction xUnit
    - xUnit is the free, open source unit testing tool for .NET framework
    - Easy and extensible
    - Best to use with a mocking framework called "Moq".
    - There are 3 steps: Arrange, Act, Assert
      - Arrange: means the Declaration of variables and collecting the inputs
      - Act: means calling the method which method you would like to test
      - Assert: means is comparing the expected value with actual value.

### Sample
```C#
    [Fact]
    public void Test1()
    {
        //Arrange
        MyMath myMath = new MyMath();
        int input1 = 10;
        int input2 = 5;
        int expectedValue = 15;
        //Act
        var actualValue = myMath.Add(input1, input2);
        //Assert
        Assert.Equal(expectedValue, actualValue);
    }
```

## 2. Add Country - xUnit Test
    - Add Entities Project with Country Model