using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Available player and food strings
string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

// Current player string displayed in the Console
string player = states[0];

// Index of the current food
int food = 0;

InitializeGame();
while (!shouldExit)
{
    Move(ConsumedGoodFood());
    ConsumedBadFood();
    TerminalResizedTerminated();
    ConsumedFood(playerX, playerY, foodX, foodY, food);
}





// Returns true if the Terminal was resized 
bool TerminalResized()
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Displays random food at a random location
void ShowFood()
{
    // Update food to a random index
    food = random.Next(0, foods.Length);

    // Update food position to a random location
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}

// Changes the player to match the food consumed
void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Temporarily stops the player from moving
void FreezePlayer()
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}

// Reads directional input from the Console and moves the player
void Move(int increaseMoveSpeed = 0)
{
    int lastX = playerX;
    int lastY = playerY;

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
            playerY -= 1;
            break;
        case ConsoleKey.DownArrow:
            playerY += 1;
            break;
        case ConsoleKey.LeftArrow:
            playerX -= increaseMoveSpeed + 1;
            break;
        case ConsoleKey.RightArrow:
            playerX += increaseMoveSpeed + 1;
            break;
        case ConsoleKey.Escape:
            shouldExit = true;
            break;
        default:
            TerminateGame();
            break;
    }

    // Clear the characters at the previous position
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++)
    {
        Console.Write(" ");
    }

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Draw the player at the new location
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Clears the console, displays the food and player
void InitializeGame()
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}

void TerminateGame()
{
    Console.Clear();
    shouldExit = true;
}

int ConsumedFood(int playerPosX = 0, int playerPosY = 0, int foodPosX = 0, int foodPosY = 0, int foodType = 0)
{
    if (playerPosX == foodPosX && playerPosY == foodPosY)
    {
        ChangePlayer();
        ShowFood();
        return foodType;
    }
    else
    {
        return 0;
    }
}

void TerminalResizedTerminated()
{
    if (TerminalResized())
    {
        TerminateGame();
        Console.WriteLine("Console was resized. Program exiting.");
    }
}

int ConsumedBadFood()
{
    if (player == states[2])
    {
        FreezePlayer();
        return -3;
    }
    else
    {
        return 0;
    }
}

int ConsumedGoodFood()
{
    if (player == states[1])
    {
        return 3;
    }
    else
    {
        return 0;
    }
}