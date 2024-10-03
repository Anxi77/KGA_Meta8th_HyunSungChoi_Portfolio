# 2048 Project

### **2048 게임 규칙**

2048은 표면적으로는 단순해 보이나, 흥미로운 게임 메커니즘을 내포

- n x n (주로 4x4, 5x5, 6x6) 크기의 게임 보드 사용
- 무작위 위치에 2 또는 4가 생성됨
- 사용자가 입력한 방향으로 모든 타일이 이동
- 동일한 숫자의 타일이 만날 시 병합되어 2의 제곱 값으로 증가
- 더 이상 이동이 불가능할 때까지 진행

본 프로젝트에서는 게임 보드를 '그리드'로 지칭. 그리드란 행과 열로 구성된 2차원 배열 형태의 데이터 구조를 의미.

## **핵심 기능 구현**

### **1. 2차원 벡터 그리드 생성**

n x n 그리드의 생성이 첫 번째 과제였다. 확장성과 작업의 효율성을 위해 정적 배열 대신 동적으로 크기를 할당할 수 있는 2차원 벡터를 채택.

```cpp
GridGen::GridGen(int size) : size(size), gen(random_device{}()), isGridFull(false)
{
grid = vector<vector<int>>(size, vector<int>(size, 0));
}
```

이 방식을 통해 4x4, 5x5뿐만 아니라 다양한 크기의 그리드를 용이하게 생성.

### **2. 타일 생성 알고리즘**

초기에는 2차원 좌표를 이용해 빈 칸을 탐색하고 타일을 생성하는 방식을 채택. 그러나 이 방식은 빈 타일이 불균등하게 분포된 경우 효율이 저하되는 문제점 발생.이를 개선하기 위해 1차원 인덱스를 활용한 방식으로 변경

1. 빈 칸의 1차원 인덱스를 벡터에 저장

- 저장된 인덱스 중 하나를 균등 분포로 선택
- 선택된 1차원 인덱스를 2차원 좌표로 변환 (y = index / size, x = index % size)
- 해당 좌표에 90% 확률로 2, 10% 확률로 4를 생성

```cpp
bool GridGen::SpawnRandomNumber()
{
	vector<int> emptyCells;
	for (int i = 0; i < size * size; ++i)
	{
		if (grid[i / size][i % size] == 0)
		{
			emptyCells.push_back(i);
		}
	}
	if (!emptyCells.empty())
	{
		int index = emptyCells[dis(gen) % emptyCells.size()];
		int y = index / size;
		int x = index % size;
		grid[y][x] = (dis(gen) % 10 < 9) ? 2 : 4;
		newSpawn = {y, x};
		return true;
	}
return false;
}
```

### **3. 타일 병합 알고리즘**

타일 이동과 병합은 2048 게임의 핵심 메커니즘. 이를 구현하기 위해 다음과 같은 규칙을 적용

- 타일은 이동 방향에 동일한 숫자가 있을 경우 병합된다. 단, 한 번의 이동에서 각 타일은 한 번만 병합 가능.
- 빈 칸을 만나면 계속 이동.
- 상이한 숫자를 만나거나 그리드 끝에 도달하면 이동을 중지.

이를 구현하기 위해 mergeTarget이라는 변수를 도입. 이 변수는 현재 타일이 이동할 수 있는 최대 위치를 나타냄.

```cpp
bool GridMove::moveGridRight()
{
	bool moved = false;
	int size = gridGen.getSize();
	auto& grid = gridGen.getGrid();
	for (int y = 0; y < size; ++y)
	{
		int mergeTarget = size - 1;
		for (int x = size - 2; x >= 0; --x)
		{
			if (grid[y][x] != 0)
			{
				int currentX = x;
				while (currentX < mergeTarget && grid[y][currentX + 1] == 0)
				{
					grid[y][currentX + 1] = grid[y][currentX];
					grid[y][currentX] = 0;
					currentX++;
					moved = true;
				}
				if (currentX < mergeTarget && grid[y][currentX + 1] == grid[y][currentX])
				{
					grid[y][currentX + 1] *= 2;
					grid[y][currentX] = 0;
					mergeTarget = currentX;
					moved = true;
				}
			}
		}
	}
return moved;
}
```

이 알고리즘을 구현하는 과정에서 방향에 따른 순회 순서의 중요성을 인식. 

### **4. 게임 종료 조건 Validate**

게임의 종료 조건인 "더 이상 이동이 불가능한 상태"를 정확히 구현하는 것이 중요. 초기에는 단순히 새로운 타일을 생성할 수 없을 때 게임이 종료되도록 설계, 이는 그리드가 포화 상태이더라도 여전히 병합이 가능한 경우를 고려하지 못하는 문제점 발생.이를 해결하기 위해 그리드를 순회하며 인접한 타일들을 검사하는 방식으로 개선

```cpp
bool GameManager::GameVali(vector<vector<int>>& grid, int size)
{
	bool Zeros = false;
	int count = 0;
	for (int y = 0; y < size; ++y)
	{
		for (int x = 0; x < size; ++x)
		{
			if (grid[x][y] == 0)
			{
				Zeros = true;
			}
		}
	}
	if (Zeros != true)
	{
		for (int y = 0; y < size; ++y)
		{
			for (int x = 0; x < size; ++x)
			{
				count += checkRow(grid, size, y, x);
				count += checkColumn(grid, size, y, x);
			}
		}
		if (count == 0)
		{
			return false;
		}
	}
	return true;

}
```

## **추가 기능: 세이브/로드**

게임의 진행 상태를 저장하고 불러오는 기능을 구현. fstream 라이브러리를 활용하여 텍스트 파일로 데이터를 저장하고 불러오는 방식을 채택.

```cpp
void GameManager::SaveGame(int topscore, const vector<vector<int>>& grid)
{
	ofstream saveFile(SAVE_FILE);
	if (saveFile.is_open()) 
	{
		saveFile << topscore << endl;
		saveFile << grid.size() << endl;
		for (const auto& row : grid) 
		{
			for (int cell : row) 
			{
				saveFile << cell << " ";
			}
			saveFile << endl;
		}
		saveFile.close();
	}
}

void GameManager::LoadGame(int& topscore, vector<vector<int>>& grid)
{
	ifstream loadFile(SAVE_FILE);
	if (loadFile.is_open()) 
	{
		loadFile >> topscore;
		int size;
		loadFile >> size;
		grid.resize(size, vector<int>(size));
		for (int i = 0; i < size; ++i) 
		{
			for (int j = 0; j < size; ++j) 
			{
				loadFile >> grid[i][j];
			}
		}
		loadFile.close();
	}
}
```

이 기능을 통해 사용자는 게임을 중단하고 후에 재개 가능.