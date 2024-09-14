namespace RentCalculator.Program;

public class Application {
    public static void Main() {
        
        // welcome user
        Console.WriteLine("Welcome to My Rent Calculator!");
        do {
            // how much rent is due
            Console.Write("How much rent is due?\n: ");
            int rent = getNumber();

            // how many people are paying for rent
            List<Person> listOfPeople = getPeoplePayingRent();
            
            if(listOfPeople.Count > 1){
                // is rent split evenly
                Console.Write("Is rent split evenly?\n: ");
                if(!getBinaryAnswer()){
                    // get any individual fees
                    Console.Write("Are there any individual fees?\n: ");
                    if(getBinaryAnswer()){
                        listOfPeople = getFees(listOfPeople);
                    }
                }
            }
            
            // display result
            Console.WriteLine("\n\n==========###########========");
            Console.WriteLine($"Rent Total: ${rent}");
            foreach(Person person in listOfPeople){
                Console.WriteLine(person);
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
            Console.WriteLine("==========###########========\n\n");
            
            
            // prompt to recalculate
            Console.Write("Would you like to use My Rent Calculator again?\n");
        } while (getBinaryAnswer());
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

        for(int i = 0; i < number; i++){
            string ordinal = (i + 1).ToString();
            switch(i + 1){
                case 1: {
                    ordinal+="st";
                    break;
                }
                case 2:{
                    ordinal+="nd";
                    break;
                }
                case 3:{
                    ordinal+="rd";
                    break;
                }
                default:{
                    ordinal+="th";
                    break;
                }
            }
            Console.Write($"What is the name of the {ordinal} person?\n: ");
            listOfPeople.Add(new Person(Console.ReadLine()));
        }
        return listOfPeople;
    }

    public static List<Person> getFees(List<Person> listOfPeople){
        foreach(Person person in listOfPeople){
            Console.Write($"Does {person.name} have any fees they have to pay?\n: ");
            if(!getBinaryAnswer())
                continue;
            do{
                Console.Write($"What is the amount of the fee?\n: ");
                int amount = getNumber();
                Console.Write($"Why does {person.name} have to pay ${amount}?\n: ");
                string reason = Console.ReadLine();
                person.fees.Add(reason,amount);
                Console.Write($"Does {person.name} have any more fees?\n: ");
            }while(getBinaryAnswer());
            
        }
        return listOfPeople;
    }
}