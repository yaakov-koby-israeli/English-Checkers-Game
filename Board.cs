namespace Damka
{
    public class Board
    {
        private Piece[,] m_Board;
        private readonly int r_BoardSize;

        public Board(int i_BoardSize)
        {
            this.r_BoardSize = i_BoardSize;
            this.m_Board = new Piece[i_BoardSize, i_BoardSize];
            initializeBoard();
        }

        public Piece[,] GetBoardMatrix()
        {
            return m_Board;
        }

        public int GetBoardSize()
        {
            return this.r_BoardSize;
        }

        private void initializeBoard()
        {
            int emptyRows = r_BoardSize / 2;

            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    Piece currentPiece = new Piece(ePieceType.E);
                    m_Board[row, col] = currentPiece;
                }
            }

            for (int row = 0; row < emptyRows - 1; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    if (row % 2 == 0 && col % 2 == 1)
                    {
                        Piece currentPiece = new Piece(ePieceType.O);
                        m_Board[row, col] = currentPiece;
                    }

                    if (row % 2 == 1 && col % 2 == 0)
                    {
                        Piece currentPiece = new Piece(ePieceType.O);
                        m_Board[row, col] = currentPiece;
                    }
                }
            }

            for (int row = emptyRows + 1; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    if (row % 2 == 0 && col % 2 == 1)
                    {
                        Piece currentPiece = new Piece(ePieceType.X);
                        m_Board[row, col] = currentPiece;
                    }

                    if (row % 2 == 1 && col % 2 == 0)
                    {
                        Piece currentPiece = new Piece(ePieceType.X);
                        m_Board[row, col] = currentPiece;
                    }
                }
            }
        }

        public bool IsValidBoardPosition(int i_Row, int i_Column)
        {
            bool isValidBoardPosition = true;

            if (i_Row < 0 || i_Row >= r_BoardSize || i_Column < 0 || i_Column >= r_BoardSize)
            {
                isValidBoardPosition = false;
            }

            return isValidBoardPosition;
        }

        public bool IsCellContainOPiece(int i_Row, int i_Col)
        {
            bool isCellContainOPiece = false;

            if (m_Board[i_Row, i_Col].GetPieceType() == ePieceType.O)
            {
                isCellContainOPiece = !isCellContainOPiece;
            }

            return isCellContainOPiece;
        }

        public bool IsCellContainXPiece(int i_Row, int i_Col)
        {
            bool isCellContainXPiece = false;

            if (m_Board[i_Row, i_Col].GetPieceType() == ePieceType.X)
            {
                isCellContainXPiece = true;
            }

            return isCellContainXPiece;
        }

        public bool IsCellContainKPiece(int i_Row, int i_Col)
        {
            bool isCellContainKPiece = false;

            if (m_Board[i_Row, i_Col].GetPieceType() == ePieceType.K)
            {
                isCellContainKPiece = true;
            }

            return isCellContainKPiece;
        }

        public bool IsCellContainUPiece(int i_Row, int i_Col)
        {
            bool isCellContainUPiece = false;

            if (m_Board[i_Row, i_Col].GetPieceType() == ePieceType.U)
            {
                isCellContainUPiece = true;
            }

            return isCellContainUPiece;
        }

        public bool IsValidPosition(int i_Row, int i_Col)
        {
            bool isValidPosition = true;

            if (i_Row < 0 || i_Row >= r_BoardSize || i_Col < 0 || i_Col >= r_BoardSize)
            {
                isValidPosition = false;
            }

            return isValidPosition;
        }

        public ePieceType GetPieceAtPosition(int i_Row, int i_Col)
        {
            return m_Board[i_Row, i_Col].GetPieceType();
        }

        private bool makeKing(int i_Rows, int i_Cols)
        {
            bool v_IsKing = false;

            if (i_Rows == r_BoardSize - 1)
            {
                m_Board[i_Rows, i_Cols] = new Piece(ePieceType.U);
                v_IsKing = true;
            }

            if (i_Rows == 0)
            {
                m_Board[i_Rows, i_Cols] = new Piece(ePieceType.K);
                v_IsKing = true;
            }

            return v_IsKing;
        }

        public void UpdateBoard(ref int[] io_CurrentPosition, ref int[] io_NextPosition)
        {
            int currentRow = io_CurrentPosition[0];
            int currentCol = io_CurrentPosition[1];
            int nextRow = io_NextPosition[0];
            int nextCol = io_NextPosition[1];

            if (m_Board[currentRow, currentCol].GetPieceType() == ePieceType.K
               || m_Board[currentRow, currentCol].GetPieceType() == ePieceType.U)
            {
                ePieceType currentPieceType = GetPieceAtPosition(currentRow, currentCol);
                Piece movingPiece = new Piece(currentPieceType);
                m_Board[nextRow, nextCol] = movingPiece;
            }
            else if (!makeKing(nextRow, nextCol))
            {
                ePieceType currentPieceType = GetPieceAtPosition(currentRow, currentCol);
                Piece movingPiece = new Piece(currentPieceType);
                m_Board[nextRow, nextCol] = movingPiece;
            }

            m_Board[currentRow, currentCol] = new Piece(ePieceType.E);
        }
    }
}
