using System.Diagnostics;
using System;
using math_game;

MathLogic math_logic = new MathLogic();
Random random = new Random();

int first_value;
int second_value;
int score = 0;
bool game_over = false;
DifficultyLevel difficulty_level = DifficultyLevel.easy;

while(!game_over)
{
    int user_menu_selection = get_user_menu_selection(math_logic);
    first_value = random.Next(1, 101);
    second_value = random.Next(1, 101);

    switch(user_menu_selection)
    {
        case 1:
            score += await perform_operation(math_logic, first_value, second_value, '+', score, difficulty_level);
            break;

        case 2:
            score += await perform_operation(math_logic, first_value, second_value, '-', score, difficulty_level);
            break;

        case 3:
            score += await perform_operation(math_logic, first_value, second_value, '*', score, difficulty_level);
            break;

        case 4:
            while(first_value % second_value != 0)
            {
                first_value = random.Next(1, 101);
                second_value = random.Next(1, 101);
            }
            score += await perform_operation(math_logic, first_value, second_value, '/', score, difficulty_level);
            break;

        case 5:
            Console.WriteLine("GAME HISTORY: \n");
            foreach (string operation in math_logic.game_history)
            {
                Console.WriteLine($"{operation}");
            }
            break;

        case 6:
            difficulty_level = select_difficulty();
            DifficultyLevel difficultyEnum = (DifficultyLevel)difficulty_level;
            Enum.IsDefined(typeof(DifficultyLevel), difficultyEnum);
            Console.WriteLine($"Your new difficulty level: {difficulty_level}");
            break;
        
        case 7:
            game_over = true;
            Console.WriteLine("Game over!!!");
            break;
    }
}

static DifficultyLevel select_difficulty()
{
    Console.WriteLine("Select the difficulty level");
    Console.WriteLine("1. Easy");
    Console.WriteLine("2. Medium");
    Console.WriteLine("3. Hard");

    try
    {
        int selected_difficulty_level = Convert.ToInt32(Console.ReadLine());
        
        switch(selected_difficulty_level)
        {
            case 1:
                return DifficultyLevel.easy;

            case 2:
                return DifficultyLevel.medium;

            case 3:
                return DifficultyLevel.hard;
        }

    } 
    catch(System.Exception)
    {
        Console.WriteLine("Caught an exception");
    }

    return 0;
}

static void display_question(int first_value, int second_value, char math_operator)
{
    Console.WriteLine($"{first_value} {math_operator} {second_value} = ?");
}

static int get_user_menu_selection(MathLogic math_logic)
{
    int selection = -1;
    math_logic.menu();

    while(selection < 1 || selection > 8)
    {
        try
        {
            if (selection < 1 || selection > 8)
            {
                Console.WriteLine("Enter a number between 1 and 8");
                selection = Convert.ToInt32(Console.ReadLine());
            }
        }
        catch(System.Exception)
        {
            Console.WriteLine("Caught an exception");
        }
    }

    return selection;
}

static async Task<int?> get_user_response(DifficultyLevel difficulty)
{
    int response = 0;
    int timeout = (int)difficulty;

    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    Task<string?> getUserInputTask = Task.Run(() => Console.ReadLine());

    try
    {
        string? result = await Task.WhenAny(getUserInputTask, Task.Delay(timeout * 1000)) == getUserInputTask ? getUserInputTask.Result : null;

        stopwatch.Stop();

        if (result != null && int.TryParse(result, out response))
        {
            //Console.WriteLine($"Time taken to answer: {stopwatch.Elapsed.ToString(@"m\::ss\.fff")}");
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed.TotalSeconds} seconds");
            return response;
        }

        else
        {
            throw new OperationCanceledException();
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Time is up!");
        return null;
    }
}

static int validate_result(int result, int? user_response, int score)
{
    if (result == user_response)
    {
        Console.WriteLine("You answer correctly; You earned 5 point");
        score += 5;
    }
    else
    {
        Console.WriteLine("Try again!");
        Console.WriteLine($"Correct answer is: {result}");
    }
    return score;
}

static async Task<int> perform_operation(MathLogic math_logic, int first_value, int second_value, char math_operator, int score, DifficultyLevel difficulty)
{
    int result;
    int? user_response;

    display_question(first_value, second_value, math_operator);
    result = math_logic.math_operation(first_value, second_value, math_operator);
    user_response = await get_user_response(difficulty);
    score = validate_result(result, user_response, score);
    return score;
}

public enum DifficultyLevel
{
    easy = 45,
    medium = 30,
    hard = 15
} 