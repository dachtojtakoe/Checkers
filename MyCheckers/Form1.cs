using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCheckers
{
    public partial class Form1 : Form
    {
        const int mapSize = 8; //Переменная, которая задает размеры доски
        const int cellSize = 80; // Размер одного поля карты

        int currentPlayer; // Текущий игрок

        List<Button> simpleSteps = new List<Button>();

        int countEatSteps = 0; // Количество возможный ходов
        Button prevButton; // Текущая нажатая кнопка
        Button pressedButton; // Предыдущая нажатая кнопка
        bool isContinue = false; // Возможно ли продолжение взятия после первого взятия

        bool isMoving; // находится ли шашка в процессе ходьбы

        int[,] map = new int[mapSize, mapSize]; // массив для создания поля

        Button[,] buttons = new Button[mapSize, mapSize];

        Image whiteFigure;
        Image blackFigure;

        public Form1()
        {
            InitializeComponent();

            // Привязка соответствующих картинок для черных и белых шашек
            whiteFigure = new Bitmap(new Bitmap(Properties.Resources.w), new Size(cellSize - 10, cellSize - 10));
            blackFigure = new Bitmap(new Bitmap(Properties.Resources.b), new Size(cellSize - 10, cellSize - 10));

            this.Text = "Checkers";

            Init();
        }

        public void Init() // Инициализация игры 
        {
            currentPlayer = 2; // Предоставление права первого хода
            isMoving = false;
            prevButton = null;

            map = new int[mapSize, mapSize] { // Создание карты, расстановка шашек
            { 0,1,0,1,0,1,0,1 },
            { 1,0,1,0,1,0,1,0 },
            { 0,1,0,1,0,1,0,1 },
            { 0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0 },
            { 2,0,2,0,2,0,2,0 },
            { 0,2,0,2,0,2,0,2 },
            { 2,0,2,0,2,0,2,0 }
            };

            CreateMap();
        }

        private void Restart(object sender, EventArgs e) // Метод для перезапуска игры 
        {
            this.Controls.Clear();
            this.Init();
        }
        
        private void f_1win() // Метод для вывода формы, если победили белые шашки
        {
            this.Dispose();
            EndGame f_l = new EndGame();
            f_l.ltext = "Белые победили!";
            f_l.aa = new Point(17, 80);
            f_l.ShowDialog();
            f_l.Dispose();
        }

        private void but_1win(object sender, EventArgs e)
        {
            f_1win();
        }

        private void f_2win() // Метод для вывода формы, если победили черные шашки
        {
            this.Dispose();
            EndGame f_l = new EndGame();
            f_l.ltext = "Черные победили!";
            f_l.aa = new Point(3, 80);
            f_l.ShowDialog();
            f_l.Dispose();
        }

        private void but_2win(object sender, EventArgs e)
        {
            f_2win();
        }

        private void f_nowin() // Метод для вывода формы, если случилась ничья
        {
            this.Dispose();
            EndGame f_l = new EndGame();
            f_l.ltext = "Ничья!";
            f_l.aa = new Point(110, 80);
            f_l.ShowDialog();
            f_l.Dispose();
        }
        private void but_nowin(object sender, EventArgs e)
        {
            f_nowin();
        }

        private void but_create(Button b1) // Метод, помогающий настроить схожие кнопки и сократить код
        {
            b1.Size = new Size(130, 80);
            b1.BackColor = Color.LightGray;
            b1.Font = new Font("Times New Roman", 20F);
            this.Controls.Add(b1);
        }

        public void ResetGame() // Метод для проверки надобности перезапуска игры 
        {

            bool player1 = false;
            bool player2 = false;

            // Если на шашечном поле находится еще как минимум одна шашка, игра продолжается
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++) 
                {
                    if (map[i, j] == 1)
                        player1 = true;
                    

                    if (map[i, j] == 2)
                        player2 = true;
                }
            }

            if (!CheckForP1()) // Если у первого игрока все еще есть шашки, но нет возможности для их хода, то игра заканчивается
            {
                player1 = false;
            }

            if (!CheckForP2()) // Если у второго игрока все еще есть шашки, но нет возможности для их хода, то игра заканчивается
            {
                player2 = false;
            }

            if (!DCheck()) // Если у обоих игроков остается по 1 дамке, то игра заканчивается и засчитывается ничейная ситуация 
            {
                player1 = false;
                player2 = false;
            }

            if (!player1 && player2)
            {
                f_1win(); // вывод формы, в случае победы первого игрока
            }

            if (player1 && !player2)
            {
                f_2win(); // вывод формы, в случае победы второго игрока
            }

            if (!player1 && !player2)
            {
                f_nowin(); // вывод формы, в случае ничьей
            }
        }

        public bool DCheck() // Метод для проверки ничейной ситуации с оставшимися двумя дамками на поле
        {
            int count1 = 0;
            int count2 = 0;
            bool flag = true;
            bool flag2 = true;

            for (int i = 0; i < mapSize; i++) // подсчет шашек у игроков
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] == 1)
                        count1++;

                    else if (map[i, j] == 2)
                        count2++;
                }
            }

            if (count1 == 1 && count2 == 1) // Если у обоих игроков осталась одна шашка
                for (int i = 0; i < mapSize; i++)
                {
                    for (int j = 0; j < mapSize; j++)
                    {
                        if (map[i, j] == 1 && buttons[i, j].Text == "D")
                        {
                            flag = false;
                        }

                        else if (map[i, j] == 1 && buttons[i, j].Text != "D")
                        {
                            flag = true;
                        }

                        if (map[i, j] == 2 && buttons[i, j].Text == "D")
                        {
                            flag2 = false;

                        }

                        else if (map[i, j] == 2 && buttons[i, j].Text != "D")
                        {
                            flag2 = true;
                        }
                    }
                }
            if (flag == false && flag2 == false) // Если оставшиеся две на поле шашки являются дамками
            {
                return false;
            }
            else
                return true;
        }

        public bool CheckForP1() // Метод для проверки возникновения ситуации со всеми заблокированными шашками первого игрока
        {
            bool flag = false;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (j == 7 && i != 6 && map[i, j] == 1 && map[i + 1, j - 1] == 2 && map[i + 2, j - 2] == 2) 
                    {

                    }
                    else if (j == 0 && i != 7 && map[i, j] == 1 && map[i + 1, j + 1] == 2 && map[i + 2, j + 2] == 2) 
                    {

                    }
                    else if (map[i, j] == 1)
                    {
                        flag = true;
                    }
                }
            }
            if (flag == true)
                return true;
            else
                return false;
        }
        public bool CheckForP2() // Метод для проверки возникновения ситуации со всеми заблокированными шашками второго игрока
        {
            bool flag = false;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                     if (j == 0 && i != 1 && map[i, j] == 2 && map[i - 1, j + 1] == 1 && map[i - 2, j + 2] == 1)
                    {

                    }

                    else if (j == 7 && i != 0 && map[i, j] == 2 && map[i - 1, j - 1] == 1 && map[i - 2, j - 2] == 1)
                    {

                    }
                    
                    else if (map[i, j] == 2)
                    {
                        flag = true;
                    }
                }
            }
            if (flag == true)
                return true;
            else
                return false;
        }

        public void CreateMap() // создание карты
        {
            this.Width = (mapSize + 2) * cellSize + 28; // Ширина окна
            this.Height = mapSize * cellSize + 39; // Высота окна

            for (int i = 0; i < mapSize; i++) // Создание поля 
            {
                for (int j = 0; j < mapSize; j++)
                {
                    
                    Button button = new Button(); // Созданеи кнопки
                    button.Location = new Point(j * cellSize, i * cellSize); // Позиция кнопки
                    button.Size = new Size(cellSize, cellSize); // Размер кнопки
                    button.Click += new EventHandler(OnFigurePress); // Обработчик события для кнопок
                    if (map[i, j] == 1) // Если элемент карты равен 1, то картинка - черная шашка
                        button.Image = blackFigure;
                    else if (map[i, j] == 2) button.Image = whiteFigure; // Если элемент карты равен 2, то картинка - белая шашка

                    button.BackColor = GetPrevButtonColor(button);
                    button.ForeColor = Color.Lime;

                    buttons[i, j] = button;

                    this.Controls.Add(button);
                }

            }

            //Создание кнопки для вызова досрочной ничьи
            Button nowin = new Button();
            nowin.Location = new Point(660, 280);
            nowin.Text = "Ничья";
            but_create(nowin);
            nowin.Click += new EventHandler(but_nowin);
            //Кнопка предназначенная для намеренного досрочного поражения черных шашек
            Button win_1 = new Button();
            win_1.Location = new Point(660, 200);
            win_1.Text = "Черные сдаются";
            but_create(win_1);
            win_1.Click += new EventHandler(but_1win);
            //Кнопка предназначенная для намеренного досрочного поражения белых шашек
            Button win_2 = new Button();
            win_2.Location = new Point(660, 360);
            win_2.Text = "Белые сдаются";
            but_create(win_2);
            win_2.Click += new EventHandler(but_2win);
            //Кнопка предназначенная для быстрого перезапуска игры, без вывода победителя
            Button endgame = new Button();
            endgame.Location = new Point(660, 560);
            endgame.Text = "Рестарт";
            but_create(endgame);
            endgame.Click += new EventHandler(Restart);

        }

        public void SwitchPlayer() // Функция для смены текущего игрока 
        {
            currentPlayer = currentPlayer == 1 ? 2 : 1;
            ShowPossibleSteps();
            ResetGame();
        }

        public Color GetPrevButtonColor(Button prevButton) // Окрашивание кнопок в предыдущий цвет
        {
            if ((prevButton.Location.Y / cellSize % 2) != 0)
            {
                if ((prevButton.Location.X / cellSize % 2) == 0)
                {
                    return Color.Gray;
                }
            }
            if ((prevButton.Location.Y / cellSize) % 2 == 0)
            {
                if ((prevButton.Location.X / cellSize) % 2 != 0)
                {
                    return Color.Gray;
                }
            }
            return Color.White;
        }

        public void OnFigurePress(object sender, EventArgs e) // обработчик для кнопок
        {
            if (prevButton != null)
                prevButton.BackColor = GetPrevButtonColor(prevButton);

            pressedButton = sender as Button; // Нажатая кнопка

            // Если происходит нажатие на кнопку (шашку), которой сейчас принадлежит ход
            if (map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] != 0 && map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] == currentPlayer)
            {
                CloseSteps();
                pressedButton.BackColor = Color.Red; // Подкрашивание выбранной шашки в красный
                DeactivateAllButtons();
                pressedButton.Enabled = true;
                countEatSteps = 0;
                if (pressedButton.Text == "D")
                    ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, false);
                else ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize);

                if (isMoving)
                {
                    CloseSteps();
                    pressedButton.BackColor = GetPrevButtonColor(pressedButton);
                    ShowPossibleSteps();
                    isMoving = false;
                }
                else
                    isMoving = true;
            }
            else
            {
                if (isMoving)
                {
                    isContinue = false; // 
                    // Если разница в расстоянии была больше единицы - значит был проведен ход взятия 
                    if (Math.Abs(pressedButton.Location.X / cellSize - prevButton.Location.X / cellSize) > 1) 
                    {
                        int pp1 = 0;
                        int pw1 = 0;

                        for (int i = 0; i < mapSize; i++)
                        {
                            for (int j = 0; j < mapSize; j++)
                            {
                                if ((map[i, j] == 1) || (map[i, j] == 2))
                                    pp1 += 1;
                            }
                        }

                        DeleteEaten(pressedButton, prevButton); // Удалить все шашки между начальным и конечным положением ходящей шашки

                        for (int i = 0; i < mapSize; i++)
                        {
                            for (int j = 0; j < mapSize; j++)
                            {
                                if ((map[i, j] == 1) || (map[i, j] == 2))
                                    pw1 += 1;
                            }
                        }

                        if (pw1 < pp1) // Если количество шашек на поле изменилось после хода - шашка взяла вражескую
                        {
                            isContinue = true;
                        }

                    }
                    //Замена (перемещение) шашки
                    int temp = map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize];
                    map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] = map[prevButton.Location.Y / cellSize, prevButton.Location.X / cellSize];
                    map[prevButton.Location.Y / cellSize, prevButton.Location.X / cellSize] = temp;
                    pressedButton.Image = prevButton.Image;
                    prevButton.Image = null;
                    pressedButton.Text = prevButton.Text;
                    prevButton.Text = "";

                    SwitchButtonToCheat(pressedButton); //Проверка на становление шашкой
                    countEatSteps = 0; // Обнуление количество ходов взятия
                    isMoving = false; 
                    CloseSteps();
                    DeactivateAllButtons();

                    if (pressedButton.Text == "D")
                        ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, false);
                    else ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize);
                    if (countEatSteps == 0 || !isContinue)
                    {
                        CloseSteps();
                        SwitchPlayer();
                        isContinue = false;
                    }
                    else if (isContinue)
                    {
                        pressedButton.BackColor = Color.Red;
                        pressedButton.Enabled = true;
                        isMoving = true;
                    }
                }
            }

            prevButton = pressedButton;
        }

        public void ShowPossibleSteps() // Выделение шашек, у которых есть ход взятия 
        {
            bool isOneStep = true; //
            bool isEatStep = false; // Есть ли ходы взятия 
            DeactivateAllButtons(); // Выключение всех кнопок
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] == currentPlayer)
                    {
                        if (buttons[i, j].Text == "D") // Если дамка
                            isOneStep = false;
                        else isOneStep = true;
                        if (IsButtonHasEatStep(i, j, isOneStep, new int[2] { 0, 0 })) // (0,0) - проверка всех направлений
                        {
                            isEatStep = true; // Есть ход взятия
                            buttons[i, j].Enabled = true;
                        }
                    }
                }
            }
            if (!isEatStep) // Если нет ходов взятия 
                ActivateAllButtons(); // Свободный ход
        }

        public void SwitchButtonToCheat(Button button) // Становится ли шашка дамкой 
        {
            // Если игрок первый (верхний), а позиция на карте в нижней горизонтали
            if (map[button.Location.Y / cellSize, button.Location.X / cellSize] == 1 && button.Location.Y / cellSize == mapSize - 1)
            {
                button.Text = "D"; // Шашка становится дамкой
                button.ForeColor = Color.Lime;
            }
            // Если игрок второй (нижний), а позиция на карте в верхней горизонтали
            if (map[button.Location.Y / cellSize, button.Location.X / cellSize] == 2 && button.Location.Y / cellSize == 0)
            {
                button.Text = "D"; // Шашка становится дамкой
                button.ForeColor = Color.Lime;
            }
        }

        public void DeleteEaten(Button endButton, Button startButton) // Удаляет взятые шашки
        {
            // Расстояние между двумя кнопками
            int count = Math.Abs(endButton.Location.Y / cellSize - startButton.Location.Y / cellSize);
            //Вычисление направления
            int startIndexX = endButton.Location.Y / cellSize - startButton.Location.Y / cellSize;
            int startIndexY = endButton.Location.X / cellSize - startButton.Location.X / cellSize;
            startIndexX = startIndexX < 0 ? -1 : 1;
            startIndexY = startIndexY < 0 ? -1 : 1;

            int currCount = 0; // Переменная для цикла 
            int i = startButton.Location.Y / cellSize + startIndexX;
            int j = startButton.Location.X / cellSize + startIndexY;
            while (currCount < count - 1) // Удаление всех кнопок, лежащих в диапозоне 
            {
                map[i, j] = 0;
                buttons[i, j].Image = null;
                buttons[i, j].Text = "";
                i += startIndexX;
                j += startIndexY;
                currCount++;
            }
        }

        public void ShowSteps(int iCurrFigure, int jCurrFigure, bool isOnestep = true) // Расчет шагов, вызываетс при нажатии на кнопку
        {
            simpleSteps.Clear(); // Очистка массива простых шагов, перед последующим вычислением шагов
            ShowDiagonal(iCurrFigure, jCurrFigure, isOnestep);
            if (countEatSteps > 0) // Если есть ходы взятия 
                CloseSimpleSteps(simpleSteps); // Закрываем все шаги кроме взятия 
        }

        public void ShowDiagonal(int IcurrFigure, int JcurrFigure, bool isOneStep = false) // Алгоритм высчитывания шагов на карте
        {
            int flag = 0; // Флаг для последующего запрета хождения обычных шашек в обратном направлении

            int j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if ((currentPlayer == 1 && isOneStep && !isContinue) && (currentPlayer == 2 && isOneStep && !isContinue)) break;
                {
                    flag = 1;
                    if (IsInsideBorders(i, j))
                    {
                        if (!DeterminePath(i, j, flag))
                            break;
                    }
                    if (j < 7)
                        j++;
                    else break;

                    if (isOneStep)
                        break;
                }

            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if ((currentPlayer == 1 && isOneStep && !isContinue) && (currentPlayer == 2 && isOneStep && !isContinue)) break;
                {
                    flag = 2;
                    if (IsInsideBorders(i, j))
                    {
                        if (!DeterminePath(i, j, flag))
                            break;
                    }
                    if (j > 0)
                        j--;
                    else break;

                    if (isOneStep)
                        break;
                }
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if ((currentPlayer == 1 && isOneStep && !isContinue) && (currentPlayer == 2 && isOneStep && !isContinue)) break;
                {
                    flag = 3;
                    if (IsInsideBorders(i, j))
                    {
                        if (!DeterminePath(i, j, flag))
                            break;
                    }
                    if (j > 0)
                        j--;
                    else break;

                    if (isOneStep)
                        break;
                }
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if ((currentPlayer == 1 && isOneStep && !isContinue) && (currentPlayer == 2 && isOneStep && !isContinue)) break;
                {
                    flag = 4;
                    if (IsInsideBorders(i, j))
                    {
                        if (!DeterminePath(i, j, flag))
                            break;
                    }
                    if (j < 7)
                        j++;
                    else break;

                    if (isOneStep)
                        break;
                }
            }
        }

        public bool DeterminePath(int ti, int tj, int flag)
        {
            // Если поле пустое и это не продолжение взятия | Разделение на направления для простых шашек, в зависимости от номера игрока
            if ((map[ti, tj] == 0 && !isContinue) && (currentPlayer == 1 && (flag == 3 || flag == 4)) && (pressedButton.Text != "D"))
            {
                buttons[ti, tj].BackColor = Color.Yellow; 
                buttons[ti, tj].Enabled = true;
                simpleSteps.Add(buttons[ti, tj]); // Добавляется в массив простых шагов
            }
            else if ((map[ti, tj] == 0 && !isContinue) && (currentPlayer == 2 && (flag == 1 || flag == 2)) && (pressedButton.Text != "D"))
            {
                buttons[ti, tj].BackColor = Color.Yellow;
                buttons[ti, tj].Enabled = true;
                simpleSteps.Add(buttons[ti, tj]);
            }
            else if ((map[ti, tj] == 0 && !isContinue) && (pressedButton.Text == "D")) 
            {
                buttons[ti, tj].BackColor = Color.Yellow;
                buttons[ti, tj].Enabled = true;
                simpleSteps.Add(buttons[ti, tj]);
            }
            else
            {
                if (map[ti, tj] != currentPlayer) // Если на поле стоит вражеская шашка
                {
                    if (pressedButton.Text == "D") // Если дамка, то нахождение хода взятия по правилам дамки
                        ShowProceduralEat(ti, tj, false);
                    else ShowProceduralEat(ti, tj);
                }
                return false; // Нашлось возможное взятие
            }
            return true; 
        }

        public void CloseSimpleSteps(List<Button> simpleSteps) // Закрывает все остальные шаги, кроме взятия
        {
            if (simpleSteps.Count > 0)
            {
                for (int i = 0; i < simpleSteps.Count; i++)
                {
                    simpleSteps[i].BackColor = GetPrevButtonColor(simpleSteps[i]);
                    simpleSteps[i].Enabled = false;
                }
            }
        }

        public void ShowProceduralEat(int i, int j, bool isOneStep = true) // Строит следующий ход взятия
        {
            //Направление в котором шашка будет ходить
            int dirX = i - pressedButton.Location.Y / cellSize;
            int dirY = j - pressedButton.Location.X / cellSize;
             
            dirX = dirX < 0 ? -1 : 1;
            dirY = dirY < 0 ? -1 : 1;

            int il = i;
            int jl = j;
            bool isEmpty = true; // Возможно ли построить ход взятия
            while (IsInsideBorders(il, jl)) // пока индексы внутри карты
            {
                if (map[il, jl] != 0 && map[il, jl] != currentPlayer) // проверка на нахождение вражеской шашки
                {
                    isEmpty = false;//нашли шашку ( поле не пустое)
                    break;
                }
                il += dirX;
                jl += dirY;

                if (isOneStep) // Если true (не дамка) - продвигаемся на одну клетку по диагонали
                    break;
            }
            if (isEmpty) //  Если не нашлось возможного взятия, ход заканчивается
                return;
            List<Button> toClose = new List<Button>(); // Кнопки, которые при необходимости нужно выключить
            bool closeSimple = false; // Нужно ли закрыть ходы кроме взятия
            int ik = il + dirX;
            int jk = jl + dirY;
            while (IsInsideBorders(ik, jk)) // Првоерка на вхождение в карту
            {
                if (map[ik, jk] == 0) // Если ячейка пустая
                {
                    if (IsButtonHasEatStep(ik, jk, isOneStep, new int[2] { dirX, dirY })) // Проверка на возможное дальнейшее взятие
                    {
                        closeSimple = true; // Закрываем все ходы кроме взятия
                    }
                    else
                    {
                        toClose.Add(buttons[ik, jk]); // Закрывает, тк не имеет хода взятия
                    }
                    buttons[ik, jk].BackColor = Color.Yellow;
                    buttons[ik, jk].Enabled = true;
                    countEatSteps++; //++ количество шагов
                }
                else break;
                if (isOneStep) // Простая пешка - ход на одну клетку
                    break;
                jk += dirY;
                ik += dirX;
            }
            if (closeSimple && toClose.Count > 0) // Если есть ходы, которые нужно закрыть
            {
                CloseSimpleSteps(toClose);
            }

        }

        // Проверка, есть ли у текущей шашки ход взятия
        public bool IsButtonHasEatStep(int IcurrFigure, int JcurrFigure, bool isOneStep /*true = Шашка, false = Дамка*/ , int[] dir/*Направление движения*/)
        {
            bool eatStep = false;
            
            //Проверка на ходы взятия по верхней правой диагонали
            int j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if ((currentPlayer == 1 && isOneStep && !isContinue) && (currentPlayer == 2 && isOneStep && !isContinue)) break;
                if (dir[0] == 1 && dir[1] == -1 && !isOneStep) break; 
                if (IsInsideBorders(i, j)) // Проверка на вхождение внутрь карты 
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer) //Возможно взятие 
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j + 1)) // Если поле за потенциально съедаемой шашкой не находится внутри карты, то взятие невозможно
                        {
                            eatStep = false;
                            break;
                        }
                        else if (map[i - 1, j + 1] != 0) // Если поле за потенциально съедаемой шашкой не является пустым, то взятие невозможно
                        {
                            eatStep = false;
                            break;
                        }
                        else return eatStep;
                    }
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
            //Проверка на ходы взятия по верхней левой диагонали
            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if ((currentPlayer == 1 && isOneStep && !isContinue) && (currentPlayer == 2 && isOneStep && !isContinue)) break;
                if (dir[0] == 1 && dir[1] == 1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j - 1))
                        {
                            eatStep = false;
                            break;
                        }
                        else if (map[i - 1, j - 1] != 0)
                        {
                            eatStep = false;
                            break;
                        }
                        else return eatStep;
                    }
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }
            //Проверка на ходы взятия по нижней левой диагонали
            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if ((currentPlayer == 1 && isOneStep && !isContinue) && (currentPlayer == 2 && isOneStep && !isContinue))break;
                if (dir[0] == -1 && dir[1] == 1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i + 1, j - 1))
                        {
                            eatStep = false;
                            break;
                        }
                            
                        else if (map[i + 1, j - 1] != 0)
                        {
                            eatStep = false;
                            break;
                        }
                        else return eatStep;
                    }
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }
            //Проверка на ходы взятия по нижней правой диагонали
            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if ((currentPlayer == 1 && isOneStep && !isContinue) && (currentPlayer == 2 && isOneStep && !isContinue)) break;
                if (dir[0] == -1 && dir[1] == -1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i + 1, j + 1))
                        {
                            eatStep = false;
                            break;
                        }
                        else if (map[i + 1, j + 1] != 0)
                        {
                            eatStep = false;
                            break;
                        }
                        else return eatStep; 
                    }
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
            return eatStep;
        }

        public void CloseSteps() // Закрывать все шаги, которые были открыты для текущей шашки
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                }
            }
        }

        public bool IsInsideBorders(int ti, int tj) // Проверка на вхождение индексов внутрь массива
        {
            if (ti >= mapSize || tj >= mapSize || ti < 0 || tj < 0)
            {
                return false;
            }
            return true;
        }

        public void ActivateAllButtons() // Включает все кнопки
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].Enabled = true;
                }
            }
        }

        public void DeactivateAllButtons() // Выключает все кнопки
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].Enabled = false;
                }
            }
        }


    }
}   