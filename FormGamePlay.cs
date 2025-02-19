using System.Drawing;
using System.Windows.Forms;
using System;

namespace Damka
{
    public class FormGamePlay : Form
    {
        private PictureBox[,] m_Board;
        private PictureBox m_SelectedPictureBox = null;

        private GameLogic m_GameLogic;

        private Label m_LabelPlayerOne = new Label();
        private Label m_LabelPlayerTwo = new Label();

        private String m_PlayerOneName = null;
        private String m_PlayerTwoName = null;

        private int m_PlayerOneScore = 0;
        private int m_PlayerTwoScore = 0;

        int m_BoardSize = 0;

        private bool isFirstClick = true;
        private bool isFirstPlayerTurn = true;

        private Point firstClickPosition;

        public FormGamePlay(string i_PlayerOneName, string i_PlayerTwoName, int i_BoardSize)
        {
            m_PlayerOneName = i_PlayerOneName;
            m_PlayerTwoName = i_PlayerTwoName;
            m_BoardSize = i_BoardSize;

            m_GameLogic = new GameLogic(i_BoardSize, i_PlayerOneName, i_PlayerTwoName);

            InitializeComponent();
        }

        private void AddLabel()
        {
            /// Label One 
            m_LabelPlayerOne.Text = $"{m_PlayerOneName}: {m_PlayerOneScore}";
            m_LabelPlayerOne.Font = new Font("Arial", 12, FontStyle.Bold);
            m_LabelPlayerOne.AutoSize = true;
            m_LabelPlayerOne.Top = (m_BoardSize * 60);
            m_LabelPlayerOne.Left = this.Width / 4;
            this.Controls.Add(m_LabelPlayerOne);

            /// Label Two
            m_LabelPlayerTwo.Text = $"{m_PlayerTwoName}: {m_PlayerTwoScore}";
            m_LabelPlayerTwo.Font = new Font("Arial", 12, FontStyle.Bold);
            m_LabelPlayerTwo.AutoSize = true;
            m_LabelPlayerTwo.Top = (m_BoardSize * 60) + 25;
            m_LabelPlayerTwo.Left = m_LabelPlayerOne.Left;
            this.Controls.Add(m_LabelPlayerTwo);

            //m_BoardSize * 60 + m_BoardSize
            this.Controls.Add(m_LabelPlayerOne);
            this.Controls.Add(m_LabelPlayerTwo);
        }

        public void InitializeComponent()
        {
            this.ClientSize = new System.Drawing.Size(m_BoardSize * 60, m_BoardSize * 60 + 150);
            this.Name = "FormBoard";
            this.Text = "Damka";
            this.CenterToScreen();
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.ResizeRedraw = false;

            m_Board = new PictureBox[m_BoardSize, m_BoardSize];

            startGame();
        }

        private void startGame()
        {
            createBoard();
            updateBoard();
        }

        private void updateBoard()
        {
            for (int row = 0; row < m_BoardSize; row++)
            {
                for (int col = 0; col < m_BoardSize; col++)
                {
                    if (m_GameLogic.GetMainBoard().IsCellContainOPiece(row, col))
                    {
                        m_Board[row, col].Load("Resource/red-piece.png");
                        m_Board[row, col].SizeMode = PictureBoxSizeMode.StretchImage;

                    }
                    else if (m_GameLogic.GetMainBoard().IsCellContainXPiece(row, col))
                    {
                        m_Board[row, col].Load("Resource/black-piece.png");
                        m_Board[row, col].SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else if (m_GameLogic.IsCellEmpty(row, col) && (row + col) % 2 != 0)
                    {
                        m_Board[row, col].Load("Resource/light-square.png");
                        m_Board[row, col].SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else if (m_GameLogic.GetMainBoard().IsCellContainUPiece(row, col))
                    {
                        m_Board[row, col].Load("Resource/king-red-piece.png");
                        m_Board[row, col].SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else if (m_GameLogic.GetMainBoard().IsCellContainKPiece(row, col))
                    {
                        m_Board[row, col].Load("Resource/king-black-piece.png");
                        m_Board[row, col].SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }

            AddLabel();
        }

        private void createBoard()
        {
            for (int row = 0; row < m_BoardSize; row++)
            {
                for (int col = 0; col < m_BoardSize; col++)
                {
                    PictureBox currentPictureBox = new PictureBox();
                    currentPictureBox.Width = 60;
                    currentPictureBox.Height = 60;
                    currentPictureBox.Location = new Point(col * currentPictureBox.Width, row * currentPictureBox.Height);

                    if ((row + col) % 2 == 0)
                    {
                        currentPictureBox.Load("Resource/dark-square.png");
                        currentPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        currentPictureBox.Enabled = false;
                    }
                    else
                    {
                        currentPictureBox.BackgroundImage = Image.FromFile("Resource/light-square.png");
                        currentPictureBox.Click += pictureBox_Click;
                        currentPictureBox.Tag = new Point(row, col);
                    }

                    m_Board[row, col] = currentPictureBox;
                    this.Controls.Add(currentPictureBox);
                }
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedBox = sender as PictureBox;

            ePlayerGameStatus gameStatus = ePlayerGameStatus.Active;

            if (m_GameLogic.GameOver())
            {
                endGame();
            }

            if (clickedBox != null && clickedBox.Tag != null)
            {
                Point currentPositon = (Point)clickedBox.Tag;

                if (isFirstClick)
                {
                    firstClickPosition = currentPositon;

                    if (isValidPositionForPiece(currentPositon.X, currentPositon.Y))
                    {
                        clickedBox.BorderStyle = BorderStyle.Fixed3D;
                        m_SelectedPictureBox = clickedBox;
                        isFirstClick = false;
                    }
                    else
                    {
                        MessageBox.Show(
                        $"It's Not Your Turn",
                        "Damka",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    }
                }
                else
                {

                    if (clickedBox == m_SelectedPictureBox)
                    {
                        clickedBox.BorderStyle = BorderStyle.None;
                        m_SelectedPictureBox = null;
                    }
                    else
                    {
                        currentPositon = (Point)clickedBox.Tag;

                        int nextRow = currentPositon.X;
                        int nextCol = currentPositon.Y;

                        if (m_GameLogic.MakeMove(firstClickPosition.X, firstClickPosition.Y, nextRow, nextCol, ref gameStatus))
                        {
                            updateBoard();
                            isFirstPlayerTurn = m_GameLogic.GetIsPlayerOneMove();
                        }
                        else
                        {
                            if (gameStatus == ePlayerGameStatus.MissedCapture)
                            {
                                MessageBox.Show(
                                $"Invalid move! A capture is mandatory. Please try again.",
                                "Damka",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );
                            }
                            if (gameStatus == ePlayerGameStatus.ExtraCapture)
                            {
                                MessageBox.Show(
                                $"Another capture is available! Please continue.",
                                "Damka",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );
                            }
                            if (gameStatus == ePlayerGameStatus.Error)
                            {
                                MessageBox.Show(
                                $"Invalid Move",
                                "Damka",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );
                            }
                        }
                    }

                    while (!isFirstPlayerTurn && m_GameLogic.GetPlayerTwo().GetIsComputerPlayer())
                    {
                        string computer = "";
                        m_GameLogic.ComputerGameManager(ref computer);
                        updateBoard();
                        isFirstPlayerTurn = m_GameLogic.GetIsPlayerOneMove();
                    }

                    if (m_SelectedPictureBox != null)
                    {
                        m_SelectedPictureBox.BorderStyle = BorderStyle.None;
                    }

                    m_SelectedPictureBox = null;
                    firstClickPosition = new Point(0, 0);
                    isFirstClick = true;

                    if (m_GameLogic.IsGameOverDueToNoMoves())
                    {
                        endGame();
                    }
                }
            }
        }

        private void endGame()
        {
            m_PlayerOneScore = m_GameLogic.GetPlayerOne().GetPlayerScore();
            m_PlayerTwoScore = m_GameLogic.GetPlayerTwo().GetPlayerScore();

            DialogResult result;

            if (m_GameLogic.GetPlayerOne().GetPlayerGameStatus() == ePlayerGameStatus.NoMoreMoves
                && m_GameLogic.GetPlayerTwo().GetPlayerGameStatus() == ePlayerGameStatus.NoMoreMoves)
            {
                result = MessageBox.Show(
                    $"Tie!!!\nAnother Round?",
                    "Damka",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
            }
            else if (m_GameLogic.GetPlayerOne().GetPlayerGameStatus() == ePlayerGameStatus.NoMoreMoves)
            {
                m_LabelPlayerTwo.Text = $"{m_PlayerTwoName}: {m_PlayerTwoScore}";
                result = MessageBox.Show(
                    $"{m_GameLogic.GetPlayerTwo().GetPlayerName()} Won!\nAnother Round?",
                    "Damka",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
            }
            else
            {
                m_LabelPlayerOne.Text = $"{m_PlayerOneName}: {m_PlayerOneScore}";
                result = MessageBox.Show(
                    $"{m_GameLogic.GetPlayerOne().GetPlayerName()} Won!\nAnother Round?",
                    "Damka",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
            }
            switch (result)
            {
                case DialogResult.Yes:
                    restartGame();
                    break;
                case DialogResult.No:
                    this.Close();
                    break;
            }
        }

        private void restartGame()
        {
            m_GameLogic = new GameLogic(m_BoardSize, m_PlayerOneName, m_PlayerTwoName);

            m_GameLogic.GetPlayerOne().SetScoreForPlayer(m_PlayerOneScore);

            m_GameLogic.GetPlayerTwo().SetScoreForPlayer(m_PlayerTwoScore);

            updateBoard();

            isFirstPlayerTurn = true;
        }

        private bool isValidPositionForPiece(int i_Row, int i_Col)
        {
            bool isValidPositionForPiece = false;

            if (isFirstPlayerTurn)
            {
                if (m_GameLogic.GetMainBoard().IsCellContainOPiece(i_Row, i_Col) || m_GameLogic.GetMainBoard().IsCellContainUPiece(i_Row, i_Col))
                {
                    isValidPositionForPiece = true;
                }
            }
            else
            {
                if (m_GameLogic.GetMainBoard().IsCellContainXPiece(i_Row, i_Col) || m_GameLogic.GetMainBoard().IsCellContainKPiece(i_Row, i_Col))
                {
                    isValidPositionForPiece = true;
                }
            }

            return isValidPositionForPiece;
        }
    }
}
