using System;

namespace mis_221_pa_5_sydneymarch
{
    public class Menu
    {
        private PizzaUtility pizzaUtility;
        private Pizza[] pizzas;
        private Order[] orders;
        private OrderUtility orderUtility;
        private PizzaFile pizzaFile;
        private PizzaReport pizzaReport;
        private OrderReport orderReport;

        public Menu(Pizza[] pizzas, Order[] orders, PizzaFile pizzaFile)
        {
            this.pizzas = pizzas;
            this.orders = orders;
            this.pizzaFile = pizzaFile;

            pizzaFile.GetAllPizzas();
            this.pizzaUtility = new PizzaUtility(pizzas, pizzaFile);
            this.orderUtility = new OrderUtility(orders, pizzas, pizzaFile);
            this.pizzaReport = new PizzaReport(pizzas, orders, pizzaFile);
            this.orderReport = new OrderReport(orders, pizzas);


            MenuUtility.DisplaySplashScreen();

            string role = "";
            while (role != "exit")
            {
                MenuUtility.Clear();
                role = MenuUtility.Login();

                if (role == "manager")
                {
                    ManagerFlow();
                }
                else if (role == "customer")
                {
                    CustomerFlow();
                }
            }

            DisplayGoodbye();
        }

        public static int GetMainMenuChoice()
        {
            string[] options = { "Manager Menu", "Customer Menu", "Exit" };
            return MenuUtility.SelectionMenu(options);
        }

        public void ProcessMainMenuChoice(int choice)
        {
            switch (choice)
            {
                case 0:
                    ManagerFlow();
                    break;
                case 1:
                    CustomerFlow();
                    break;
                case 2:
                    DisplayGoodbye();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    MenuUtility.Pause();
                    break;
            }
        }

        public void ManagerFlow()
        {
            int choice;
            do
            {
                MenuUtility.Clear();
                choice = GetManagerMenuChoice();
                ProcessManagerMenuChoice(choice);
            } while (choice != 6);
        }

        public static int GetManagerMenuChoice()
        {
            string[] options = {
                "Add a new pizza recipe to the menu",
                "Remove a pizza from the menu",
                "Edit pizza information",
                "Mark a pizza order as complete",
                "Access report menu",
                "Search Pizzas",
                "Exit"
            };
            return MenuUtility.SelectionMenu(options);
        }

        public void ProcessManagerMenuChoice(int choice)
        {
            MenuUtility.Clear();
            switch (choice)
            {
                case 0:
                    pizzaUtility.AddPizza();
                    break;
                case 1:
                    pizzaUtility.DeletePizza();
                    break;
                case 2:
                    pizzaUtility.UpdatePizza();
                    break;
                case 3:
                    orderUtility.MarkOrderComplete(pizzas);
                    break;
                case 4:
                    ReportFlow(); // pauses handled inside
                    break;
                case 5:
                    SearchFlow("manager");
                    break;
                case 6:
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    MenuUtility.Pause();
                    break;
            }
        }

        public void CustomerFlow()
        {
            int choice;
            do
            {
                MenuUtility.Clear();
                choice = GetCustomerMenuChoice();
                ProcessCustomerMenuChoice(choice);
            } while (choice != 4);
        }

        public static int GetCustomerMenuChoice()
        {
            string[] options = {
                "View available pizzas",
                "Place an order",
                "View order history",
                "Search Past Orders",
                "Exit"
            };
            return MenuUtility.SelectionMenu(options);
        }

        public void ProcessCustomerMenuChoice(int choice)
        {
            MenuUtility.Clear();
            switch (choice)
            {
                case 0:
                    pizzaReport.DisplayPizzas();
                    MenuUtility.Pause();
                    break;
                case 1:
                    orderUtility.AddOrder();
                    break;
                case 2:
                    orderUtility.ViewOrderHistory();
                    MenuUtility.Pause();
                    break;
                case 3:
                    SearchFlow("customer");
                    break;
                case 4:
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    MenuUtility.Pause();
                    break;
            }
        }

        public void ReportFlow()
        {
            int choice;
            do
            {
                MenuUtility.Clear();
                choice = GetReportMenuChoice();
                ProcessReportMenuChoice(choice);
            } while (choice != 5);
        }

        public static int GetReportMenuChoice()
        {
            string[] options = {
                "Daily pizza orders report",
                "Orders currently in progress",
                "Average pizza size by crust",
                "Top 5 most popular pizzas",
                "Sales by Pizza Report",
                "Exit"
            };
            return MenuUtility.SelectionMenu(options);
        }

        public void ProcessReportMenuChoice(int choice)
        {
            MenuUtility.Clear();
            switch (choice)
            {
                case 0:
                    orderReport.OrdersByDay();
                    MenuUtility.Pause();
                    break;
                case 1:
                    orderReport.OrdersInProgress();
                    MenuUtility.Pause();
                    break;
                case 2:
                    pizzaReport.AveragePizzaSizeByCrust();
                    MenuUtility.Pause();
                    break;
                case 3:
                    pizzaReport.Top5PizzasBySales();
                    MenuUtility.Pause();
                    break;
                case 4:
                    pizzaReport.SalesByPizza();
                    MenuUtility.Pause();
                    break;
                case 5:
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    MenuUtility.Pause();
                    break;
            }
        }
        public void SearchFlow(string role) {
            int choice;
            do
            {
                MenuUtility.Clear();
                choice = GetSearchMenuChoice(role);
                ProcessSearchMenuChoice(choice, role);
            } while (choice != 5);
        }
        public static int GetSearchMenuChoice(string role)
        {
            string instructions = "Search By: ";
            string[] options;
            if (role == "manager") {
                options = [
                    "Order ID",
                    "Pizza Name",
                    "Customer Email",
                    "Order Date",
                    "Order Status",
                    "Exit"
                ];
            }
            else {
                options = [
                    "Pizza Name",
                    "Pizza Size",
                    "Crust Type",
                    "Order Date",
                    "Order Status",
                    "Exit"
                ];
            }
            return MenuUtility.SelectionMenu(options, instructions);
        }
        public void ProcessSearchMenuChoice(int choice, string role) {
            MenuUtility.Clear();
            switch (choice)
            {
                case 0:
                    if (role == "manager") {
                        orderReport.Search("Order ID");
                    }
                    else {
                        pizzaReport.Search("Pizza Name");
                    }
                    break;
                case 1:
                    if (role == "manager") {
                        orderReport.Search("Pizza Name");
                    }
                    else {
                        orderReport.Search("Pizza Size");
                    }
                    break;
                case 2:
                    if (role == "manager") {
                        orderReport.Search("Customer Email");
                    }
                    else {
                        pizzaReport.Search("Crust Type");
                    }
                    break;
                case 3:
                    if (role == "manager") {
                        orderReport.Search("Order Date");
                    }
                    else {
                        orderReport.Search("Order Date");
                    }
                    break;
                case 4:
                    if (role == "manager") {
                        orderReport.Search("Order Status");
                    }
                    else {
                        orderReport.Search("Order Status");
                    }
                    break;
                case 5:
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    MenuUtility.Pause();
                    break;
            }
        }
        public static void DisplayGoodbye()
        {
            MenuUtility.Clear();
            Console.WriteLine("Thank you for using the Pizza Ordering System. Goodbye!");
        }

    }
}