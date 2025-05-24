namespace mis_221_pa_5_sydneymarch
{
    public class OrderReport
    {
        private Order[] orders;
        private Pizza[] pizzas;

        public OrderReport(Order[] orders, Pizza[] pizzas)
        {
            this.orders = orders;
            this.pizzas = pizzas;
        }

        public void OrdersByDay()
        {
            if (orders[0] == null)
            {
                Console.WriteLine("No orders found to display.");
                return;
            }
            Sort("date");
            string currentDate = orders[0].GetOrderDate();
            int count = 0;
            int start = 0;

            for (int i = 0; i < orders.Length && orders[i] != null; i++)
            {
                string date = orders[i].GetOrderDate();

                if (date != currentDate || i == GetLastIndex())
                {
                    // Print the group
                    if (i == GetLastIndex()) count++; // include last order
                    Console.WriteLine($"\nTotal orders on {currentDate}: {count}");
                    Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,5}", "Order ID", "Email", "Pizza Name", "Size");
                    Console.WriteLine(new string('-', 65));

                    for (int j = start; j < i; j++)
                    {
                        Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,5}",
                            orders[j].GetOrderID(),
                            orders[j].GetCustomerEmail(),
                            orders[j].ToPizzaName(pizzas),
                            orders[j].GetSize());
                    }

                    if (i == GetLastIndex())
                    {
                        // Print final order if last one is a solo group
                        Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,5}",
                            orders[i].GetOrderID(),
                            orders[i].GetCustomerEmail(),
                            orders[i].ToPizzaName(pizzas),
                            orders[i].GetSize());
                    }

                    Console.WriteLine(new string('-', 65));
                    Console.WriteLine();

                    // reset
                    currentDate = date;
                    start = i;
                    count = 1;
                }
                else
                {
                    count++;
                }
            }
            Sort("orderid");
        }


        public void OrdersInProgress()
        {
            Console.WriteLine("\nOrders In Progress:");
            Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,5}", "Order ID", "Customer", "Pizza", "Size");
            Console.WriteLine(new string('-', 65));

            for (int i = 0; i < orders.Length && orders[i] != null; i++)
            {
                if (orders[i].GetOrderStatus())
                {
                    Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,5}",
                        orders[i].GetOrderID(),
                        orders[i].GetCustomerEmail(),
                        orders[i].ToPizzaName(pizzas),
                        orders[i].GetSize());
                }
            }
        }

        public void Sort(string sortBy)
        {
            for (int i = 0; i < orders.Length - 1 && orders[i] != null; i++)
            {
                int min = i;
                for (int j = i + 1; j < orders.Length && orders[j] != null; j++)
                {
                    bool shouldSwap = false;

                    switch (sortBy.ToLower())
                    {
                        case "date":
                            shouldSwap = orders[j].GetOrderDate().CompareTo(orders[min].GetOrderDate()) < 0;
                            break;
                        case "pizzaid":
                            shouldSwap = orders[j].GetPizzaID() < orders[min].GetPizzaID();
                            break;
                        case "orderid":
                            shouldSwap = orders[j].GetOrderID() < orders[min].GetOrderID();
                            break;
                        default:
                            break;
                    }

                    if (shouldSwap)
                    {
                        Order temp = orders[i];
                        orders[i] = orders[j];
                        orders[j] = temp;
                    }
                }
            }
        }
        private int GetLastIndex()
        {
            for (int i = orders.Length - 1; i >= 0; i--)
            {
                if (orders[i] != null) return i;
            }
            return -1;
        }
        public void Search(string field)
        {
            int count = 0;
            for (int i = 0; i < orders.Length; i++)
            {
                if (orders[i] != null) count++;
            }

            if (count == 0)
            {
                Console.WriteLine("No orders to search.");
                return;
            }

            string[] keys = new string[count];
            int[] indexMap = new int[count];

            for (int i = 0, filled = 0; i < orders.Length && filled < count; i++)
            {
                if (orders[i] == null) continue;

                switch (field.ToLower())
                {
                    case "order id":
                        keys[filled] = orders[i].GetOrderID().ToString();
                        break;
                    case "pizza name":
                        keys[filled] = orders[i].ToPizzaName(pizzas);
                        break;
                    case "customer email":
                        keys[filled] = orders[i].GetCustomerEmail();
                        break;
                    case "order date":
                        keys[filled] = orders[i].GetOrderDate();
                        break;
                    case "order status":
                        keys[filled] = orders[i].GetOrderStatus().ToString();
                        break;
                    case "pizza size":
                        keys[filled] = orders[i].GetSize().ToString();
                        break;
                    default:
                        Console.WriteLine("Unsupported search field.");
                        return;
                }
                indexMap[filled] = i;
                filled++;
            }

            // Selection sort to sync keys and indexMap
            for (int i = 0; i < count - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < count; j++)
                {
                    if (keys[j].CompareTo(keys[min]) < 0)
                    {
                        min = j;
                    }
                }
                if (min != i)
                {
                    string tempKey = keys[i];
                    keys[i] = keys[min];
                    keys[min] = tempKey;

                    int tempIndex = indexMap[i];
                    indexMap[i] = indexMap[min];
                    indexMap[min] = tempIndex;
                }
            }

            string searchVal = MenuUtility.GetStringInput("Enter value to search:");
            if (searchVal == "-1") return;

            int found = FastFind(keys, searchVal, count);
            if (found != -1)
            {
                int orderIndex = indexMap[found];
                Console.WriteLine("Order found:");
                Console.WriteLine(orders[orderIndex].ToString(pizzas));
            }
            else
            {
                Console.WriteLine("No order found matching the given value.");
            }

            MenuUtility.Pause();
        }
        private int FastFind(string[] keys, string searchVal, int count)
        {
            int first = 0;
            int last = count - 1;
            int foundIndex = -1;
            bool found = false;
            int middle;
            while (!found && first <= last)
            {
                middle = (first + last) / 2;
                if (keys[middle].ToUpper() == searchVal.ToUpper())
                {
                    found = true;
                    foundIndex = middle;
                }
                else if (keys[middle].ToUpper().CompareTo(searchVal.ToUpper()) > 0)
                {
                    last = middle - 1;
                }
                else
                {
                    first = middle + 1;
                }
            }
            return foundIndex;
        }
    }
}
