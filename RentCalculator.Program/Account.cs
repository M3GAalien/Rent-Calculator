namespace RentCalculator.Program;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Account {
    [JsonInclude]
    private static int id = 1;
    [JsonInclude]
    public int AccountID;
    [JsonInclude]
    public string name;
    [JsonInclude]
    public int rent;
    [JsonInclude]
    public List<Person> listOfPeople;
    
    private const string path = @"./Savefile.txt";
    
    public Account(){
        AccountID = id++;
        name = "";
        rent = 0;
        listOfPeople = new List<Person>();
    }

    public void Save(){
        Console.WriteLine("Saving Account....");
        List<Account> listOfAccounts = Load() ?? new List<Account>();
        
        if(listOfAccounts.Any(x => x.AccountID == AccountID)){
            Console.WriteLine("FOUND EXISTING ACCOUNT");
            int index = listOfAccounts.FindIndex(x => x.AccountID == AccountID);
            listOfAccounts[index] = this;
        }else{
            listOfAccounts.Add(this);
        }
        
        string text = JsonSerializer.Serialize(listOfAccounts);
        File.WriteAllText(path, text);
        Console.WriteLine("Account Saved");
    }

    public List<Account>? Load(){
        Console.WriteLine("Loading Accounts....");
        if(File.Exists(path)){
            string text = File.ReadAllText(path);
            List<Account>? listOfAccounts = JsonSerializer.Deserialize<List<Account>>(text);
            if(listOfAccounts == null){ // should never happen
                Console.WriteLine("ERROR OCCURED: null list found");
            }
            
            return listOfAccounts;
        }else{
            Console.WriteLine("No Accounts found");
            return null;
        }
    }

    public override string ToString() {
        string result = "\t________________________\n" +
                       $"\t|ID:\t{AccountID.ToString("D3")}\n"+
                       $"\t|Name:\t{name}\n" + 
                       $"\t|Rent:\t{rent}\n" +
                       $"\t|____ [{listOfPeople.Count().ToString("D3")}] Tenants ____\n";
        foreach(Person p in listOfPeople){
            result += $"\t| - {p.name}\n";
        }
        result += "\t------------------------\n";
        return result;
    }
}