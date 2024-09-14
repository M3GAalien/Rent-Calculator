namespace RentCalculator.Program;

public class Person
{
    public string name;
    public Dictionary<string,int> fees;

    public Person(string name){
        this.name = name;
        fees = new Dictionary<string, int>();
    }
    public Person(string name, int amount, string reason){
        this.name = name;
        fees = new Dictionary<string, int>(){{reason, amount}};
    }

    public Person(string name, Dictionary<string, int> fees){
        this.name = name;
        this.fees = fees;
    }

    public override string ToString() {
        string result = $"\n----{name}----";
        foreach(KeyValuePair<string, int> fee in fees){
            result += $"\n${fee.Value} - {fee.Key}";
        }
        return result;
    }
}