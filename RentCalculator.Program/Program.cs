namespace RentCalculator.Program;

public class Application {
    public static void Main() {
        // welcome user
        Console.WriteLine("Welcome to My Rent Calculator!");
        
        #region main loop
        do {
            bool newSave = true;
            int rent = 0;
            Account account = new Account();
            List<Person> listOfPeople = new List<Person>();
            
            #region load save 
            List<Account>? listOfAccounts = account.Load();

            if(listOfAccounts != null){
                //List all accounts or create a new one
                Console.WriteLine("Accounts found");
                for(int i = 0; i < listOfAccounts.Count(); i++){
                    Console.WriteLine($"[{i}] - {listOfAccounts[i]}");
                }
                Console.WriteLine($"[{listOfAccounts.Count()}] - CREATE NEW ACCOUNT");
                
                // Select or create an account
                Console.Write("Select account\n: ");
                int number;
                while (!int.TryParse(Console.ReadLine(), out number) || number > listOfAccounts.Count() || number < 0) {
                    Console.Write("Please try again\n: ");
                };
                if(number < listOfAccounts.Count()){
                    account = listOfAccounts[number];
                    rent = account.rent;
                    listOfPeople = account.listOfPeople;
                    newSave = false;
                }
            }

            if(account.name == ""){
                Console.Write("Account name\n: ");
                account.name = Console.ReadLine() ?? "ERROR";
            }
            #endregion

            // get rent due
            if(newSave){
                Console.Write("How much rent is due?\n: ");
                rent = getNumber();
            }else{
                Console.Write("Has the rent amount changed?\n: ");
                if(getBinaryAnswer()){
                    Console.Write("How much rent is due?\n: ");
                    rent = getNumber();
                }
                Console.Write("Any new people paying for rent?\n: ");
            }

            // get people paying for rent 
            // BUG: accepts  value <= 0
            if(newSave){
                listOfPeople = getPeoplePayingRent(); 
                Console.Write("Are there any individual fees?\n: ");
            }else{
                if(getBinaryAnswer()){
                    listOfPeople = getPeoplePayingRent();
                }
                Console.Write("Any new individual fees?\n: ");
            }

            // get any individual fees
            if(getBinaryAnswer()){
                listOfPeople = getFees(listOfPeople);
            }

            // display result
            displayResults(rent, listOfPeople);
            
            #region save 
            Console.WriteLine("Save Account?");
            if(getBinaryAnswer()){
                account.rent = rent;
                account.listOfPeople = listOfPeople;
                account.Save();
            };
            #endregion

            // prompt to recalculate
            Console.Write("Would you like to use My Rent Calculator again?\n");
        } while (getBinaryAnswer());
        #endregion
        
        // exit program
        Console.WriteLine("Thank you for using My Rent Calculator!");
    }
    
    public static int getNumber() {
        int number;
        while (!int.TryParse(Console.ReadLine(), out number)) {
            Console.Write("Please try again\n: ");
        }
        return number;
    }

    public static bool getBinaryAnswer(){
        int option1 = 1;
        int option2 = 0;
        int choice;
        
        Console.Write($"{option1} - Yes | {option2} - No\n: ",option1,option2);
        do{
            choice = getNumber();
            if(choice != option1 & choice != option2){
                Console.Write("Please respond with a '1' or '0'\n: ");
            }
        }while(choice != option1 & choice != option2);
        return choice == option1;
    }

    public static List<Person> getPeoplePayingRent(){
        int number;
        List<Person> listOfPeople = new List<Person>();

        Console.Write("How many people are paying for rent?\n: ");
        do{
            number = getNumber();
            if(number < 1){
                Console.Write(number < 0 ?  "Number must be positive\n" :
                                        "At least 1 person must pay rent\n");
                Console.Write("Please try again\n: ");
            }
        }while(number < 1);

        if(number > 1){
            for(int i = 0; i < number; i++){
                string ordinal = (i + 1) switch{
                    1 => "1st",
                    2 => "2nd",
                    3 => "3rd",
                    _ => $"{i + 1}th"
                };
                
                Console.Write($"What is the name of the {ordinal} person?\n: ");
                listOfPeople.Add(new Person(Console.ReadLine()));
            }
        }else{
            Console.WriteLine($"What is the name of this person?\n: ");
            listOfPeople.Add(new Person(Console.ReadLine()));
        }
        return listOfPeople;
    }

    public static List<Person> getFees(List<Person> listOfPeople){
        if(listOfPeople.Count() > 1){
            foreach(Person person in listOfPeople){
                Console.Write($"Does {person.name} have any fees they have to pay?\n: ");
                if(!getBinaryAnswer())
                    continue;
                do{
                    Console.Write($"What is the amount of the fee?\n: ");
                    int amount = getNumber();
                    Console.Write($"Why does {person.name} have to pay ${amount}?\n: ");
                    string reason = Console.ReadLine() ?? "No Reason Given";
                    person.fees.Add(reason,amount);
                    Console.Write($"Does {person.name} have any more fees?\n: ");
                }while(getBinaryAnswer());
                
            }
        }else{
            Console.Write($"What is the amount of the fee?\n: ");
            int amount = getNumber();
            Console.Write($"Why does {listOfPeople[0].name} have to pay ${amount}?\n: ");
            string reason = Console.ReadLine() ?? "No Reason Given";
            listOfPeople[0].fees.Add(reason,amount);
        }
        return listOfPeople;
    }

    public static void displayResults(int rent, List<Person> listOfPeople){
        Console.WriteLine("\n\n====================############====================");
        Console.WriteLine($"Rent Total: ${rent}");
        int totalIndividualFees = 0;
        foreach(Person person in listOfPeople){
            Console.WriteLine(person);
            foreach(var fee in person.fees){
                totalIndividualFees += fee.Value;
            }
        }

        Console.WriteLine($"\nIndividual Fees = \t\t\t${totalIndividualFees}");
        int remainingRent = rent - totalIndividualFees;
        Console.WriteLine($"Rent - All Individual Fees = \t\t${remainingRent}");
        int individualRent = remainingRent/listOfPeople.Count;
        Console.WriteLine($"Remaining Rent / People Paying Rent = \t${individualRent}");
        foreach(Person person in listOfPeople){
            Console.Write(person);
            int personalTotal = 0;
            foreach(int fee in person.fees.Values){
                personalTotal += fee;
            }
            Console.WriteLine($"\nPersonal total: ${personalTotal + individualRent}");
        }
        Console.WriteLine("====================############====================\n\n");
    }
}