using mis_221_pa_5_sydneymarch;

Pizza[] pizzas = new Pizza[100];
Order[] orders = new Order[100];

PizzaFile pizzaFile = new PizzaFile(pizzas);
pizzaFile.GetAllPizzas();

OrderFile orderFile = new OrderFile(orders);
orderFile.GetAllOrders();

Menu menu = new Menu(pizzas, orders, pizzaFile);

