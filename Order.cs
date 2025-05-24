using System.Security.Cryptography;

namespace mis_221_pa_5_sydneymarch
{
    public class Order
    {
        private int orderID;
        private string customerEmail;
        private int pizzaID;
        private string orderDate;
        private int size;
        private bool orderStatus;
        public Order(int orderID, string customerEmail, int pizzaID, string orderDate, int size, bool orderStatus)
        {
            this.orderID = orderID;
            this.customerEmail = customerEmail;
            this.pizzaID = pizzaID;
            this.orderDate = orderDate;
            this.size = size;
            this.orderStatus = orderStatus;
        }

        public int GetOrderID() => orderID;
        public void SetOrderID(int orderID) => this.orderID = orderID;

        public string GetCustomerEmail() => customerEmail;
        public void SetCustomerEmail(string customerEmail) => this.customerEmail = customerEmail;

        public int GetPizzaID() => pizzaID;
        public void SetPizzaID(int pizzaID) => this.pizzaID = pizzaID;

        public string GetOrderDate() => orderDate;
        public void SetOrderDate(string orderDate) => this.orderDate = orderDate;

        public int GetSize() => size;
        public void SetSize(int size) => this.size = size;

        public bool GetOrderStatus() => orderStatus;
        public void SetOrderStatus(bool orderStatus) => this.orderStatus = orderStatus;
        public void ToggleOrderStatus() => orderStatus = !orderStatus;

        public int CompareTo(int targetID) => orderID.CompareTo(targetID);
        public int CompareTo(Order order) => orderID.CompareTo(order.GetOrderID());

        public string ToFile() =>  $"{orderID}#{customerEmail}#{pizzaID}#{orderDate}#{size}#{orderStatus}";
        public override string ToString() => $"{orderID}\t{customerEmail}\t{pizzaID}\t{orderDate}\t{size}\t{orderStatus}";

        public string ToString(Pizza[] pizzas)
        {
            string pizzaName = "Unknown Pizza";

            for (int i = 0; i < pizzas.Length && pizzas[i] != null; i++)
            {
                if (pizzas[i].GetID() == pizzaID)
                {
                    pizzaName = pizzas[i].GetName();
                    break;
                }
            }

            return $"Order #{orderID}: {pizzaName} ({size}\"), Order Date: {orderDate}, In Progress: {orderStatus}";
        }
        public string ToPizzaName(Pizza[] pizzas){
            string pizzaName = "Unknown Pizza";

            for (int i = 0; i < pizzas.Length && pizzas[i] != null; i++)
            {
                if (pizzas[i].GetID() == pizzaID)
                {
                    pizzaName = pizzas[i].GetName();
                    break;
                }
            }

            return $"{pizzaName}";
        }
        public string ToCustomerString(Pizza[] pizzas) {
            string pizzaName = "Unknown Pizza";

            for (int i = 0; i < pizzas.Length && pizzas[i] != null; i++)
            {
                if (pizzas[i].GetID() == pizzaID)
                {
                    pizzaName = pizzas[i].GetName();
                    break;
                }
            }

            return $"Order #{orderID}: {pizzaName} ({size}\"), Order Date: {orderDate}";
        }
    }
}