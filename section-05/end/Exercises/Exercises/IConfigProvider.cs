namespace Exercises;

public interface IConfigProvider
{
    string GetOrderPrefix();
    int GetOrderStartSequence();
}