namespace mis_221_pa_5_sydneymarch
{
    public class PizzaFile
    {
        private Pizza[] pizzas; // holds all pizzas (including deleted)
        private int pizzaCount;
        private int maxPizzaID;

        public PizzaFile(Pizza[] pizzas)
        {
            this.pizzas = pizzas;
            this.pizzaCount = 0;
            this.maxPizzaID = 0;
        }

        public void GetAllPizzas()
        {
            pizzaCount = 0;
            maxPizzaID = 0;

            StreamReader inFile = new StreamReader("pizza-menu.txt");
            string line = inFile.ReadLine();

            while (line != null)
            {
                string[] temp = line.Split('#');
                Pizza pizza = new Pizza(
                    int.Parse(temp[0]),
                    temp[1],
                    int.Parse(temp[2]),
                    temp[3],
                    double.Parse(temp[4]),
                    bool.Parse(temp[5]),
                    bool.Parse(temp[6])
                );

                pizzas[pizzaCount] = pizza;
                IncrementPizzaCount();

                if (pizza.GetID() > maxPizzaID)
                {
                    maxPizzaID = pizza.GetID();
                }

                line = inFile.ReadLine();
            }

            inFile.Close();

            if (pizzaCount == 0)
            {
                Console.WriteLine("No pizzas found in the file.");
            }
        }

        public void SaveAllPizzas()
        {
            StreamWriter outFile = new StreamWriter("pizza-menu.txt");
            for (int i = 0; i < pizzaCount; i++)
            {
                outFile.WriteLine(pizzas[i].ToFile());
            }
            outFile.Close();
        }

        public int GetPizzaCount()
        {
            return pizzaCount;
        }

        public int GetMaxPizzaID()
        {
            return maxPizzaID;
        }

        public void IncrementPizzaCount()
        {
            pizzaCount++;
        }

        public void UpdateMaxPizzaID(int newID)
        {
            if (newID > maxPizzaID)
            {
                maxPizzaID = newID;
            }
        }
    }
}
