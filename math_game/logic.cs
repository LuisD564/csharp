using System.Collections;
using System.Reflection.Emit;

namespace math_game;

public class MathLogic
{
    public List<string> game_history { get; set; } = new List<string>();

    public void menu()
    {
        Console.WriteLine("Please enter an option");
        Console.WriteLine("1. Addition");
        Console.WriteLine("2. Subraction");
        Console.WriteLine("3. Multiplication");
        Console.WriteLine("4. Division");
        Console.WriteLine("5. Show History");
        Console.WriteLine("6. Change Difficulty");
        Console.WriteLine("7. Exit");
    }

    public int math_operation(int first_value, int second_value, char math_operator)
    {
        switch(math_operator)
        {
            case '+':
                game_history.Add($"{first_value} + {second_value} = {first_value + second_value}");
                return first_value + second_value;

            case '-':
                game_history.Add($"{first_value} - {second_value} = {first_value - second_value}");
                return first_value - second_value;

            case '*':
                game_history.Add($"{first_value} * {second_value} = {first_value * second_value}");
                return first_value * second_value;
            
            case '/':
                while(first_value < 0 || first_value > 100)
                {
                    try
                    {
                        Console.WriteLine("Please insert a value between 0 and 100");
                        first_value = Convert.ToInt32(Console.ReadLine());
                    }
                    catch(System.Exception)
                    {
                        Console.WriteLine("Caught an exception");
                    };
                }

                game_history.Add($"{first_value} / {second_value} = {first_value / second_value}");
                return first_value / second_value;
        }
        return 0;
    }
}