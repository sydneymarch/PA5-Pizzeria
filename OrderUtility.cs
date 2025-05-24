using System;

namespace mis_221_pa_5_sydneymarch
{
    public class OrderUtility
    {
        private Order[] orders;
        private Pizza[] pizzas;
        private PizzaFile pizzaFile;
        private OrderFile orderFile;

        public OrderUtility(Order[] orders, Pizza[] pizzas, PizzaFile pizzaFile)
        {
            this.orders = orders;
            this.pizzas = pizzas;
            this.pizzaFile = pizzaFile;
            this.orderFile = new OrderFile(orders);
            orderFile.GetAllOrders();
        }

        public void AddOrder()
        {
            MenuUtility.Clear();

            string customerEmail = MenuUtility.LoggedInEmail;
            Console.WriteLine($"Starting new order for: {customerEmail}");

            string[] options = new string[GetPizzaCount()];
            int[] indices = new int[GetPizzaCount()];
            int displayIndex = 0;

            for (int i = 0; i < GetPizzaCount(); i++)
            {
                if (!pizzas[i].GetIsDeleted() && !pizzas[i].GetIsSoldOut())
                {
                    options[displayIndex] = pizzas[i].ToShortString();
                    indices[displayIndex] = i;
                    displayIndex++;
                }
            }
            if (displayIndex == 0)
            {
                Console.WriteLine("No pizzas are currently available to order.");
                MenuUtility.Pause();
                return;
            }
            string[] availPizzaOptions = new string[displayIndex + 1];
            for (int i = 0; i < displayIndex; i++)
            {
                availPizzaOptions[i] = options[i];
            }
            availPizzaOptions[displayIndex] = "Cancel";

            int selected = MenuUtility.SelectionMenu(availPizzaOptions, "Select a pizza to order:");
            if (selected == displayIndex) return;

            int pizzaIndex = indices[selected];
            int pizzaID = pizzas[pizzaIndex].GetID();

            string[] sizeOptions = { "8", "12", "16" };
            int sizeChoice = MenuUtility.SelectionMenu(sizeOptions, "Choose a size:");
            int size = int.Parse(sizeOptions[sizeChoice]);

            int orderID = GetNextOrderID();
            string orderDate = DateTime.Now.ToString("MM/dd/yy");
            bool orderStatus = true;

            Order newOrder = new Order(orderID, customerEmail, pizzaID, orderDate, size, orderStatus);

            for (int i = 0; i < orders.Length; i++)
            {
                if (orders[i] == null)
                {
                    orders[i] = newOrder;
                    break;
                }
            }

            orderFile.SaveAllOrders();
            Console.WriteLine("Order placed successfully!");
            MenuUtility.Pause();
        }

        private int GetNextOrderID()
        {
            int maxID = 0;
            for (int i = 0; i < orders.Length && orders[i] != null; i++)
            {
                if (orders[i].GetOrderID() > maxID)
                {
                    maxID = orders[i].GetOrderID();
                }
            }
            return maxID + 1;
        }

        public void MarkOrderComplete(Pizza[] pizzas)
        {
            MenuUtility.Clear();

            int[] inProgressIndices = GetInProgressOrders();
            if (inProgressIndices.Length == 0)
            {
                Console.WriteLine("No in-progress orders to mark complete.");
                MenuUtility.Pause();
                return;
            }

            int selectedIndex = SelectOrderFromList(inProgressIndices, pizzas);
            if (selectedIndex == -1) return;

            bool confirm = ConfirmOrderCompletion(orders[selectedIndex], pizzas);
            if (confirm)
            {
                orders[selectedIndex].ToggleOrderStatus();
                orderFile.SaveAllOrders();
                Console.WriteLine("Order successfully marked as complete.");
            }
            else
            {
                Console.WriteLine("Operation cancelled.");
            }
        }

        private int[] GetInProgressOrders()
        {
            int[] temp = new int[orders.Length];
            int count = 0;

            for (int i = 0; i < orders.Length && orders[i] != null; i++)
            {
                if (orders[i].GetOrderStatus())
                {
                    temp[count++] = i;
                }
            }

            int[] result = new int[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = temp[i];
            }

            return result;
        }

        private int SelectOrderFromList(int[] inProgressIndices, Pizza[] pizzas)
        {
            int activeCount = inProgressIndices.Length;
            string[] displayOptions = new string[activeCount + 1];
            int[] indexMap = new int[activeCount];

            for (int i = 0; i < activeCount; i++)
            {
                int orderIndex = inProgressIndices[i];
                displayOptions[i] = orders[orderIndex].ToString(pizzas);
                indexMap[i] = orderIndex;
            }

            displayOptions[activeCount] = "Exit";
            int selection = MenuUtility.SelectionMenu(displayOptions, "Select the order to mark as complete:");

            if (selection == activeCount)
            {
                return -1;
            }
            else
            {
                return indexMap[selection];
            }
        }

        private bool ConfirmOrderCompletion(Order order, Pizza[] pizzas)
        {
            Console.WriteLine($"Are you sure you want to mark this order as complete?\n{order.ToString(pizzas)}");
            string[] confirmOptions = { "Yes", "No" };
            int confirmChoice = MenuUtility.SelectionMenu(confirmOptions, "Confirm completion:");
            return confirmChoice == 0;
        }

        public void ViewOrderHistory()
        {
            MenuUtility.Clear();

            string customerEmail = MenuUtility.LoggedInEmail;
            Console.WriteLine($"Viewing order history for: {customerEmail}");;

            bool found = false;

            for (int i = 0; i < orders.Length && orders[i] != null; i++)
            {
                if (orders[i].GetCustomerEmail() == customerEmail)
                {
                    Console.WriteLine(orders[i].ToCustomerString(pizzas));
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No orders found for that email.");
            }
        }
        private int GetPizzaCount()
        {
            int count = 0;
            for (int i = 0; i < pizzas.Length; i++)
            {
                if (pizzas[i] != null) count++;
            }
            return count;
        }
    }
}