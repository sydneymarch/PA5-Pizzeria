using System;

namespace mis_221_pa_5_sydneymarch
{
    public class PizzaUtility
    {
        private Pizza[] pizzas;
        private PizzaFile pizzaFile;

        public PizzaUtility(Pizza[] pizzas, PizzaFile pizzaFile)
        {
            this.pizzas = pizzas;
            this.pizzaFile = pizzaFile;
        }

        public void AddPizza()
        {
            MenuUtility.Clear();

            string name = MenuUtility.GetStringInput("Enter the name of the pizza");
            if (name == "-1") return;

            MenuUtility.Clear();
            int toppingCount = MenuUtility.GetIntInput($"Enter the topping count of {name}");
            if (toppingCount == -1) return;

            string[] crustOptions = { "Thin", "Thick", "Stuffed", "Gluten-Free" };
            int crustChoice = MenuUtility.SelectionMenu(crustOptions, $"Select the crust type for {name}:");
            string crustType = crustOptions[crustChoice];

            MenuUtility.Clear();
            double price = MenuUtility.GetDoubleInput($"Enter the price of {name}");
            if (price == -1) return;

            string[] soldOutOptions = { "True", "False" };
            int soldOutChoice = MenuUtility.SelectionMenu(soldOutOptions, $"Is {name} sold out? (True/False):");
            bool isSoldOut = bool.Parse(soldOutOptions[soldOutChoice]);

            int pizzaID = pizzaFile.GetMaxPizzaID() + 1;
            Pizza newPizza = new Pizza(pizzaID, name, toppingCount, crustType, price, isSoldOut, false);

            pizzas[pizzaFile.GetPizzaCount()] = newPizza;
            pizzaFile.IncrementPizzaCount();
            pizzaFile.UpdateMaxPizzaID(pizzaID);

            pizzaFile.SaveAllPizzas();
            Console.WriteLine($"Pizza {name} added successfully with ID {pizzaID}.");
        }

        public void DeletePizza()
        {
            MenuUtility.Clear();

            int choice = GetPizzaChoice("Select the pizza you would like to delete");
            if (choice == -1) return;

            Pizza pizzaToDelete = pizzas[choice];

            Console.WriteLine($"Are you sure you want to delete {pizzaToDelete.GetName()}?");
            string[] confirmOptions = { "Yes", "No" };
            int confirm = MenuUtility.SelectionMenu(confirmOptions, "Confirm deletion:");

            if (confirm == 0)
            {
                pizzaToDelete.ToggleDeleted();
                pizzaFile.SaveAllPizzas();
                Console.WriteLine($"{pizzaToDelete.GetName()} was successfully deleted.");
            }
            else
            {
                Console.WriteLine("Deletion canceled.");
            }
        }

        public void UpdatePizza()
        {
            MenuUtility.Clear();

            int choice;
            do
            {
                choice = GetPizzaChoice("Select the pizza you want to update:");
                if (choice >= 0 && choice < pizzaFile.GetPizzaCount())
                {
                    ProcessPizzaUpdate(pizzas[choice]);
                }
            } while (choice != -1);
        }

        public int GetPizzaChoice(string instructions)
        {
            int count = pizzaFile.GetPizzaCount();
            int activeCount = 0;

            for (int i = 0; i < count; i++)
            {
                if (!pizzas[i].GetIsDeleted())
                {
                    activeCount++;
                }
            }

            string[] displayOptions = new string[activeCount + 1];
            int[] indexMap = new int[activeCount];

            int displayIndex = 0;
            for (int i = 0; i < count; i++)
            {
                if (!pizzas[i].GetIsDeleted())
                {
                    displayOptions[displayIndex] = pizzas[i].ToShortString();
                    indexMap[displayIndex] = i;
                    displayIndex++;
                }
            }

            displayOptions[activeCount] = "Exit";
            int selection = MenuUtility.SelectionMenu(displayOptions, instructions);

            if (selection == activeCount)
            {
                return -1;
            }
            else
            {
                return indexMap[selection];
            }
        }

        public void ProcessPizzaUpdate(Pizza pizza)
        {
            bool done = false;
            while (!done)
            {
                MenuUtility.Clear();
                Console.WriteLine("Updating Pizza:");
                Console.WriteLine(pizza.ToString());
                Console.WriteLine();

                string[] updateOptions = {
                    $"Name: {pizza.GetName()}",
                    $"Topping Count: {pizza.GetToppingCount()}",
                    $"Crust Type: {pizza.GetCrustType()}",
                    $"Price: {pizza.GetPrice()}",
                    $"Sold Out Status: {pizza.GetIsSoldOut()}",
                    "Exit"
                };

                int choice = MenuUtility.SelectionMenu(updateOptions, "Select the field you want to update:");
                MenuUtility.Clear();
                switch (choice)
                {
                    case 0:
                        string newName = MenuUtility.GetStringInput("Enter new name");
                        if (newName != "-1") pizza.SetName(newName);
                        break;
                    case 1:
                        int newToppingCount = MenuUtility.GetIntInput("Enter new topping count");
                        if (newToppingCount != -1) pizza.SetToppingCount(newToppingCount);
                        break;
                    case 2:
                        string[] crustOptions = { "Thin", "Thick", "Stuffed", "Gluten-Free" };
                        int crustChoice = MenuUtility.SelectionMenu(crustOptions, "Select new crust type");
                        pizza.SetCrustType(crustOptions[crustChoice]);
                        break;
                    case 3:
                        double newPrice = MenuUtility.GetDoubleInput("Enter new price");
                        if (newPrice != -1) pizza.SetPrice(newPrice);
                        break;
                    case 4:
                        string[] soldOutOptions = { "True", "False" };
                        int soldOutChoice = MenuUtility.SelectionMenu(soldOutOptions, "Is this pizza sold out?");
                        bool isSoldOut = bool.Parse(soldOutOptions[soldOutChoice]);
                        pizza.ToggleSoldOut();
                        if (pizza.GetIsSoldOut() != isSoldOut)
                        {
                            pizza.ToggleSoldOut();
                        }
                        break;
                    case 5:
                        done = true;
                        break;
                }
            }

            pizzaFile.SaveAllPizzas();
            Console.WriteLine("Pizza updated successfully.");
            MenuUtility.Pause();
        }
    }
}