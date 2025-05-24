namespace mis_221_pa_5_sydneymarch
{
    public class OrderFile
    {
        private Order[] orders;
        private int orderCount;

        public OrderFile(Order[] orders)
        {
            this.orders = orders;
            this.orderCount = 0;
        }

        public void GetAllOrders()
        {
            orderCount = 0;
            StreamReader inFile = new StreamReader("orders.txt");
            string line = inFile.ReadLine();

            while (line != null)
            {
                string[] temp = line.Split('#');
                orders[orderCount] = new Order(
                    int.Parse(temp[0]),
                    temp[1],
                    int.Parse(temp[2]),
                    temp[3],
                    int.Parse(temp[4]),
                    bool.Parse(temp[5])
                );
                orderCount++;

                line = inFile.ReadLine();
            }

            inFile.Close();

            if (orderCount == 0)
            {
                Console.WriteLine("No orders found in the file.");
            }
        }

        public void SaveAllOrders()
        {
            StreamWriter outFile = new StreamWriter("orders.txt");
            for (int i = 0; i < orders.Length && orders[i] != null; i++)
            {
                outFile.WriteLine(orders[i].ToFile());
            }
            outFile.Close();
        }

        public int GetOrderCount()
        {
            return orderCount;
        }
    }
}
