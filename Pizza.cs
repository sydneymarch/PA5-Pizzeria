namespace mis_221_pa_5_sydneymarch
{
    public class Pizza
    {


        private int pizzaID;
        private string pizzaName;
        private int toppingCount;
        private string crustType;
        private double price;
        private bool isSoldOut;
        private bool isDeleted;

        public Pizza() {}

        public Pizza(int pizzaID, string pizzaName, int toppingCount, string crustType, double price, bool isSoldOut, bool isDeleted)
        {
            this.pizzaID = pizzaID;
            this.pizzaName = pizzaName;
            this.toppingCount = toppingCount;
            this.crustType = crustType;
            this.price = price;
            this.isSoldOut = isSoldOut;
            this.isDeleted = isDeleted;
        }

        public int GetID() => pizzaID;
        public void SetID(int pizzaID) => this.pizzaID = pizzaID;

        public string GetName() => pizzaName;
        public void SetName(string pizzaName) => this.pizzaName = pizzaName;

        public int GetToppingCount() => toppingCount;
        public void SetToppingCount(int toppingCount) => this.toppingCount = toppingCount;

        public string GetCrustType() => crustType;
        public void SetCrustType(string crustType) => this.crustType = crustType;

        public double GetPrice() => price;
        public void SetPrice(double price) => this.price = price;

        public bool GetIsSoldOut() => isSoldOut;
        public void ToggleSoldOut() => isSoldOut = !isSoldOut;

        public bool GetIsDeleted() => isDeleted;
        public void ToggleDeleted() => isDeleted = !isDeleted;

        public int CompareTo(int targetID) => pizzaID.CompareTo(targetID);
        public int CompareTo(string targetCrustType) => crustType.CompareTo(targetCrustType);
        public int CompareTo(Pizza pizza) => pizzaID.CompareTo(pizza.GetID());

        public override string ToString() => $"{pizzaID}\t{pizzaName}\t{toppingCount}\t{crustType}\t{price}\t{isSoldOut}\t{isDeleted}";

        public string ToShortString() => $"{pizzaName}";

        public string ToFile() => $"{pizzaID}#{pizzaName}#{toppingCount}#{crustType}#{price}#{isSoldOut}#{isDeleted}";
    }
}
