namespace Exercises;

public interface IOrderRepository
{
    void SaveOrder(Order order);
    Order GetOrder(int id);
}