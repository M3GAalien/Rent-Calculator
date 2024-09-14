using System.IO.Pipes;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RentCalculator.Program;
using System.Collections.Generic;

namespace RentCalculator.UnitTests;

public class UnitTest1
{
    // [Fact]
    // public void MainTest1()
    // {
    //     // Arrange
    //     StringReader input = new StringReader("1\n2\n1\n1\n0\n");
    //     Console.SetIn(input);
    //     StringWriter actualOutput = new StringWriter();
    //     Console.SetOut(actualOutput);
    //     string expectedOutput = "Welcome to My Rent Calculator!\r\n" + 
    //                             "How much rent is due?\n" +
    //                             ": " +
    //                             "Would you like to use My Rent Calculator again?\n" +
    //                             "1 - Yes | 0 - No\n" +
    //                             ": " +
    //                             "Please respond with a '1' or '0'\r\n" +
    //                             "Would you like to use My Rent Calculator again?\n" +
    //                             "1 - Yes | 0 - No\n" +
    //                             ": " +
    //                             "How much rent is due?\n" +
    //                             ": " +
    //                             "Would you like to use My Rent Calculator again?\n" +
    //                             "1 - Yes | 0 - No\n" +
    //                             ": " +
    //                             "Thank you for using My Rent Calculator!\r\n";

    //     // Act
    //     Application.Main();

    //     // Assert
    //     Assert.Equal(expectedOutput, actualOutput.ToString());
    // }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1.00003)]
    public void getNumberTest1(int expected)
    {
        // Arrange
        StringReader input = new StringReader(expected.ToString());
        Console.SetIn(input);
        // Act
        int actual = Application.getNumber();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("f")]
    [InlineData("d")]
    [InlineData("G")]
    [InlineData("-")]
    [InlineData(".")]
    public void getNumberTest2(string number)
    {
        // Arrange
        StringReader input = new StringReader(number + "\n1\n");
        Console.SetIn(input);
        StringWriter actualOutput = new StringWriter();
        Console.SetOut(actualOutput);
        string expectedOutput = "Please try again\n: ";

        // Act
        Application.getNumber();

        // Assert
        Assert.Equal(expectedOutput, actualOutput.ToString());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(2)]
    [InlineData(10)]
    public void getBinaryAnswerTest1(int number)
    {
        // Arrange
        StringWriter actualMessage = new StringWriter();
        Console.SetOut(actualMessage);
        StringReader input = new StringReader(number.ToString());
        bool expectedOutput = number == 1;
        if (number != 1 & number != 0)
        {
            input = new StringReader(number.ToString() + "\r\n1");
            expectedOutput = true;
        }
        Console.SetIn(input);
        string expectedMessage = "1 - Yes | 0 - No\n: ";
        if (!(number == 1 || number == 0))
        {
            expectedMessage += "Please respond with a '1' or '0'\n: ";
        }
        // Act
        bool actualOutput = Application.getBinaryAnswer();

        // Assert
        Assert.Equal(expectedMessage, actualMessage.ToString());
        Assert.Equal(expectedOutput, actualOutput);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(10)]
    public void peoplePayingRentTest1(int number)
    {
        // Arrange
        int DEFAULT_VALUE = 1;
        string input = number.ToString();
        if (number < 1)
        {
            input += $"\r\n{DEFAULT_VALUE}";
        }
        StringReader stringReader = new StringReader(input);
        Console.SetIn(stringReader);

        // Act
        List<Person> actualOutput = Application.getPeoplePayingRent();

        // Assert
        Assert.Equal((number < DEFAULT_VALUE) ? DEFAULT_VALUE : number, actualOutput.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(50)]
    public void getPeoplePayingRentTest2(int number)
    {
        // Arrange
        List<Person> expectedList = new List<Person>();
        List<Person> actualList;
        string input = number.ToString() + "\r\n";
        foreach (int person in Enumerable.Range(0, number))
        {
            input += "generic name\r\n";
            expectedList.Add(new Person("generic name"));
        }
        StringReader stringReader = new StringReader(input);
        Console.SetIn(stringReader);

        // Act
        actualList = Application.getPeoplePayingRent();

        // Assert
        Assert.Equivalent(expectedList, actualList);
    }

    [Fact]
    public void getFeesTest1()
    {
        // Arrange
        List<Person> expectedList = [
            new Person("person 1"),
            new Person("person 2", 10, "reason 1"),
            new Person("person 3", new Dictionary<string, int>{
                {"reason 2",10},
                {"reason 3",20}
            })
        ];

        string input = "0\r\n" +
                        "1\r\n10\r\nreason 1\r\n" +
                        "0\r\n" +
                        "1\r\n10\r\nreason 2\r\n" +
                        "1\r\n20\r\nreason 3\r\n" +
                        "0\r\n";
        StringReader stringReader = new StringReader(input);
        Console.SetIn(stringReader);
        List<Person> defaulList = [
            new Person("person 1"),
            new Person("person 2"),
            new Person("person 3")
        ];

        // Act
        List<Person> actualList = Application.getFees(defaulList);

        // Assert
        Assert.Equivalent(expectedList, actualList);
    }

    [Fact]
    public void personToStringTest1()
    {
        // Arrange
        List<Person> list = [
            new Person("person 1"),
            new Person("person 2", 10, "reason 1"),
            new Person("person 3", new Dictionary<string, int>{
                {"reason 2",10},
                {"reason 3",20}
            })
        ];

        string expected = "\n----person 1----" +
                            "\n----person 2----" +
                            "\n$10 - reason 1" +
                            "\n----person 3----" +
                            "\n$10 - reason 2" +
                            "\n$20 - reason 3";

        // Act
        string actual = "";
        foreach (Person p in list)
        {
            actual += p.ToString();
        }

        // Assert
        Assert.Equal(expected, actual);
    }


}