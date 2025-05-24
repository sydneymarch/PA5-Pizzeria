namespace mis_221_pa_5_sydneymarch
{
    public class PizzaReport
    {
        private PizzaFile pizzaFile;
        private Pizza[] pizzas;
        private Order[] orders;
        private OrderReport orderReport;

        public PizzaReport(Pizza[] pizzas, Order[] orders, PizzaFile pizzaFile)
        {
            this.pizzas = pizzas;
            this.orders = orders;
            this.pizzaFile = pizzaFile;
            this.orderReport = new OrderReport(orders, pizzas);
        }

        public void AveragePizzaSizeByCrust()
        {
            int[] sortedOrderIndices = new int[GetOrderCount()];
            for (int i = 0; i < GetOrderCount(); i++)
            {
                sortedOrderIndices[i] = i;
            }

            for (int i = 0; i < GetOrderCount() - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < GetOrderCount(); j++)
                {
                    string crustMin = GetCrustForOrder(sortedOrderIndices[min]);
                    string crustJ = GetCrustForOrder(sortedOrderIndices[j]);

                    if (crustMin.CompareTo(crustJ) > 0)
                    {
                        min = j;
                    }
                }

                int temp = sortedOrderIndices[i];
                sortedOrderIndices[i] = sortedOrderIndices[min];
                sortedOrderIndices[min] = temp;
            }

            int start = 0;
            while (start < GetOrderCount() && GetCrustForOrder(sortedOrderIndices[start]) == "")
            {
                start++;
            }
            if (start >= GetOrderCount()) return;

            string currentCrust = GetCrustForOrder(sortedOrderIndices[start]);
            int sum = orders[sortedOrderIndices[start]].GetSize();
            int count = 1;

            Console.WriteLine("\nAverage Pizza Size by Crust:");
            Console.WriteLine("{0,-20} {1,5}", "Crust Type", "Size");
            Console.WriteLine(new string('-', 27));

            for (int i = start + 1; i < GetOrderCount(); i++)
            {
                int orderIndex = sortedOrderIndices[i];
                string crust = GetCrustForOrder(orderIndex);
                if (crust == "") continue;

                if (crust != currentCrust)
                {
                    Console.WriteLine("{0,-20} {1,5}", currentCrust, sum / count);
                    currentCrust = crust;
                    sum = 0;
                    count = 0;
                }

                sum += orders[orderIndex].GetSize();
                count++;
            }

            if (count > 0)
            {
                Console.WriteLine("{0,-20} {1,5}", currentCrust, sum / count);
            }

            Sort();
        }

        private string GetCrustForOrder(int orderIndex)
        {
            int pizzaID = orders[orderIndex].GetPizzaID();
            for (int i = 0; i < GetPizzaCount(); i++)
            {
                if (pizzas[i].GetID() == pizzaID)
                {
                    return pizzas[i].GetCrustType();
                }
            }
            return "";
        }

        public void Top5PizzasBySales()
        {
            orderReport.Sort("pizzaid");
            if (GetOrderCount() == 0)
            {
                Console.WriteLine("No orders found to generate sales report.");
                return;
            }
            int currID = orders[0].GetPizzaID();
            int currCount = 1;
            int arrayIndex = 0;
            int[][] nameAndCount = new int[GetOrderCount()][];

            for (int i = 1; i < GetOrderCount(); i++)
            {
                if (orders[i].GetPizzaID() == currID)
                {
                    currCount++;
                }
                else
                {
                    ProcessBreak(ref currID, ref currCount, nameAndCount, ref arrayIndex, orders[i].GetPizzaID());
                }
            }

            ProcessBreak(ref currID, ref currCount, nameAndCount, ref arrayIndex, -1);
            Sort(nameAndCount, arrayIndex);

            Console.WriteLine("\nTop 5 Pizzas by Sales:");
            Console.WriteLine("{0,-25} {1,5}", "Pizza Name", "Sales");
            Console.WriteLine(new string('-', 32));

            for (int i = 0; i < 5 && i < arrayIndex; i++)
            {
                int pizzaID = nameAndCount[i][0];
                int count = nameAndCount[i][1];

                for (int j = 0; j < GetPizzaCount(); j++)
                {
                    if (pizzas[j].GetID() == pizzaID)
                    {
                        Console.WriteLine("{0,-25} {1,5}", pizzas[j].GetName(), count);
                        break;
                    }
                }
            }
            Sort();
        }

        private void ProcessBreak(ref int currID, ref int currCount, int[][] nameAndCount, ref int arrayIndex, int nextPizzaID)
        {
            nameAndCount[arrayIndex] = new int[2];
            nameAndCount[arrayIndex][0] = currID;
            nameAndCount[arrayIndex][1] = currCount;
            arrayIndex++;

            if (nextPizzaID != -1)
            {
                currID = nextPizzaID;
                currCount = 1;
            }
        }

        public void SalesByPizza()
        {
            orderReport.Sort("pizzaid");

            if (GetOrderCount() == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }

            int currentPizzaID = orders[0].GetPizzaID();
            int quantity8 = 0, quantity12 = 0, quantity16 = 0;
            double totalRevenue = 0;

            Console.WriteLine("\nSales by Pizza:");
            Console.WriteLine("{0,-25} {1,5} {2,5} {3,5} {4,5} {5,10}", "Pizza Name", "8\"", "12\"", "16\"", "Total", "Revenue");
            Console.WriteLine(new string('-', 65));

            for (int i = 0; i < GetOrderCount(); i++)
            {
                if (orders[i].GetPizzaID() != currentPizzaID)
                {
                    PrintPizzaSales(currentPizzaID, quantity8, quantity12, quantity16, totalRevenue);

                    // reset for next group
                    currentPizzaID = orders[i].GetPizzaID();
                    quantity8 = 0;
                    quantity12 = 0;
                    quantity16 = 0;
                    totalRevenue = 0;
                }

                int size = orders[i].GetSize();
                switch (size)
                {
                    case 8: quantity8++; break;
                    case 12: quantity12++; break;
                    case 16: quantity16++; break;
                }

                double price = GetPizzaPrice(currentPizzaID);
                totalRevenue += price;
            }

            // final group
            PrintPizzaSales(currentPizzaID, quantity8, quantity12, quantity16, totalRevenue);
            Console.WriteLine(new string('-', 65));
        }

        private void PrintPizzaSales(int pizzaID, int qty8, int qty12, int qty16, double revenue)
        {
            string name = "Unknown";
            for (int i = 0; i < GetPizzaCount(); i++)
            {
                if (pizzas[i].GetID() == pizzaID)
                {
                    name = pizzas[i].GetName();
                    break;
                }
            }

            int total = qty8 + qty12 + qty16;
            Console.WriteLine("{0,-25} {1,5} {2,5} {3,5} {4,5} {5,10:C}", name, qty8, qty12, qty16, total, revenue);
        }

        private double GetPizzaPrice(int pizzaID)
        {
            for (int i = 0; i < GetPizzaCount(); i++)
            {
                if (pizzas[i].GetID() == pizzaID)
                {
                    return pizzas[i].GetPrice();
                }
            }
            return 0.0;
        }

        public void DisplayPizzas()
        {
            Console.WriteLine("Available Pizzas:");
            for (int i = 0; i < GetPizzaCount(); i++)
            {
                if (!pizzas[i].GetIsDeleted() && !pizzas[i].GetIsSoldOut())
                {
                    Console.WriteLine(pizzas[i].ToShortString());
                }
            }
        }

        public void DisplayAllPizzas()
        {
            for (int i = 0; i < GetPizzaCount(); i++)
            {
                Console.WriteLine(pizzas[i].ToString());
            }
        }

        public void Sort(string sortBy = "id")
        {
            for (int i = 0; i < GetPizzaCount() - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < GetPizzaCount(); j++)
                {
                    bool shouldSwap = false;

                    switch (sortBy.ToLower())
                    {
                        case "name":
                            shouldSwap = pizzas[min].GetName().CompareTo(pizzas[j].GetName()) > 0;
                            break;
                        case "crust":
                            shouldSwap = pizzas[min].GetCrustType().CompareTo(pizzas[j].GetCrustType()) > 0;
                            break;
                        case "price":
                            shouldSwap = pizzas[min].GetPrice() > pizzas[j].GetPrice();
                            break;
                        case "toppings":
                            shouldSwap = pizzas[min].GetToppingCount() > pizzas[j].GetToppingCount();
                            break;
                        case "id":
                        default:
                            shouldSwap = pizzas[min].GetID() > pizzas[j].GetID();
                            break;
                    }

                    if (shouldSwap)
                    {
                        Swap(min, j);
                    }
                }
            }
        }

        public void Sort(int[][] nameAndCount, int arrayIndex)
        {
            for (int i = 0; i < arrayIndex - 1; i++)
            {
                int max = i;
                for (int j = i + 1; j < arrayIndex; j++)
                {
                    if (nameAndCount[j][1] > nameAndCount[max][1])
                    {
                        max = j;
                    }
                }

                if (max != i)
                {
                    int[] temp = nameAndCount[i];
                    nameAndCount[i] = nameAndCount[max];
                    nameAndCount[max] = temp;
                }
            }
        }

        private void Swap(int x, int y)
        {
            Pizza temp = pizzas[x];
            pizzas[x] = pizzas[y];
            pizzas[y] = temp;
        }
        private int GetPizzaCount()
        {
            return pizzaFile.GetPizzaCount();
        }

        private int GetOrderCount()
        {
            int count = 0;
            for (int i = 0; i < orders.Length; i++)
            {
                if (orders[i] != null) count++;
            }
            return count;
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

        public void Search(string field)
        {
            int count = GetPizzaCount();
            if (count == 0)
            {
                Console.WriteLine("No pizzas available to search.");
                return;
            }

            string[] keys = new string[count];
            int[] indexMap = new int[count];

            for (int i = 0; i < count; i++)
            {
                switch (field.ToLower())
                {
                    case "pizza name":
                        keys[i] = pizzas[i].GetName();
                        break;
                    case "pizza size":
                        keys[i] = pizzas[i].GetToppingCount().ToString(); // change if needed
                        break;
                    case "crust type":
                        keys[i] = pizzas[i].GetCrustType();
                        break;
                    default:
                        Console.WriteLine("Unsupported search field.");
                        return;
                }
                indexMap[i] = i;
            }

            // sort keys and keep indexMap in sync
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
                int pizzaIndex = indexMap[found];
                Console.WriteLine("Pizza found:");
                Console.WriteLine(pizzas[pizzaIndex].ToString());
            }
            else
            {
                Console.WriteLine("No pizza found matching the given value.");
            }

            MenuUtility.Pause();
        }

    }
}
