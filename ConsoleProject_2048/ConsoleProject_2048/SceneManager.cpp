#include "SceneManager.h"


void SceneManager::GameOverScene()
{
    system("cls");
    Borders();
    GameOverScreen::PrintGameover();
}

void SceneManager::StageScene(int size, int topscore, int currentscore, vector<vector<int>> grid, pair<int,int> newSpawn)
{
    Borders();
    StageScreen::Instructions();
    StageScreen::GridDisplay(size);
    StageScreen::TileDisplay(size, grid, newSpawn);
    StageScreen::Scores(topscore, currentscore);
}

void SceneManager::LobbyScene()
{
    LobbyScreen::PrintLogo();
}

SceneManager::SceneManager()
{
    SetConsoleCursorPosition(0, 0);
    ConsoleUtils::SetConsoleFontSize(61, 60);
    ConsoleUtils::SetConsoleBufferSize(82, 24);
    ConsoleUtils::SetConsoleWindowSize(82, 24);
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    CONSOLE_SCREEN_BUFFER_INFO csbi;
    GetConsoleScreenBufferInfo(hConsole, &csbi);
    csbi.dwSize.X = 82;
    csbi.dwSize.Y = 24;
    SetConsoleScreenBufferSize(hConsole, csbi.dwSize);
    HWND consoleWindow = GetConsoleWindow();
    RECT r;
    GetWindowRect(consoleWindow, &r);
    MoveWindow(consoleWindow, r.left, r.top, (r.right - r.left) + 2, (r.bottom - r.top) + 2, TRUE);
    ShowWindow(GetConsoleWindow(), SW_MAXIMIZE);
    ConsoleUtils::CursorView();
}