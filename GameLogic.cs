using System.Collections.Generic;
using System;
namespace Damka
{
    public class GameLogic
    {
        private Board m_MainBoard;

        private readonly Player r_FirstPlayer;
        private readonly Player r_SecondPlayer;

        private bool m_IsPlayerOneMove;
        
        private Random m_Random;

        public GameLogic(int i_BoardSize, string i_FirstPlayerName, string i_SecondPlayerName)
        {
            m_MainBoard = new Board(i_BoardSize);
            r_FirstPlayer = new Player(i_FirstPlayerName);
            r_SecondPlayer = new Player(i_SecondPlayerName);
            m_IsPlayerOneMove = true;

            // עדכון סטטוס משחק לשחקנים
            r_FirstPlayer.SetPlayerGameStatus(ePlayerGameStatus.Active);
            r_SecondPlayer.SetPlayerGameStatus(ePlayerGameStatus.Waiting);

            if (r_SecondPlayer.GetIsComputerPlayer())
            {
                m_Random = new Random();
            }
        }
        public Player GetPlayerOne()
        {
            return r_FirstPlayer;
        }
        public Player GetPlayerTwo()
        {
            return r_SecondPlayer;
        }
        public bool GetIsPlayerOneMove()
        {
            return m_IsPlayerOneMove;
        }
        public Player GetOtherPlayer()
        {
            return m_IsPlayerOneMove ? r_SecondPlayer : r_FirstPlayer;
        }
        public Board GetMainBoard()
        {
            return m_MainBoard;
        }

        public bool MakeMove(int i_RowCurrentStep, int i_ColCurrentStep, int i_RowNextStep, int i_ColNextStep, ref ePlayerGameStatus i_GameStatus)
        {
            bool v_MakeMove = false;
            i_GameStatus = ePlayerGameStatus.Active;

            int[] currentPosition = convertToPosition(i_RowCurrentStep, i_ColCurrentStep);
            int[] nextStepPosition = convertToPosition(i_RowNextStep, i_ColNextStep);


            if (m_IsPlayerOneMove && i_GameStatus == ePlayerGameStatus.Active)
            {
                if (m_MainBoard.IsCellContainOPiece(currentPosition[0], currentPosition[1]))
                {
                    if (validateRegularMove(ref currentPosition, ref nextStepPosition))
                    {
                        if (checkForMandatoryCaptureOPiece())
                        {
                            i_GameStatus = ePlayerGameStatus.MissedCapture;
                            m_IsPlayerOneMove = true;
                        }
                        else
                        {
                            m_MainBoard.UpdateBoard(ref currentPosition, ref nextStepPosition);
                            m_IsPlayerOneMove = false;
                            v_MakeMove = true;
                        }
                    }
                    else if (isValidCaptureForOPiece(ref currentPosition, ref nextStepPosition))
                    {
                        m_MainBoard.UpdateBoard(ref currentPosition, ref nextStepPosition);
                        if (checkMultipleJumpsForRegularPiece(ref nextStepPosition) || checkMultipleJumpsForKingPiece(ref nextStepPosition))
                        {
                            m_IsPlayerOneMove = true;
                            i_GameStatus = ePlayerGameStatus.ExtraCapture;
                        }
                        else
                        {
                            m_IsPlayerOneMove = false;
                        }

                        v_MakeMove = true;
                    }
                    else
                    {
                        i_GameStatus = ePlayerGameStatus.Error;
                    }
                }
                else if (m_MainBoard.IsCellContainUPiece(currentPosition[0], currentPosition[1]))
                {
                    if (isValidKingMove(ref currentPosition, ref nextStepPosition))
                    {
                        if (checkForMandatoryCaptureOPiece())
                        {
                            i_GameStatus = ePlayerGameStatus.MissedCapture;
                            m_IsPlayerOneMove = true;
                        }
                        else
                        {
                            m_MainBoard.UpdateBoard(ref currentPosition, ref nextStepPosition);
                            m_IsPlayerOneMove = false;
                            v_MakeMove = true;
                        }
                    }
                    else if (isValidCaptureForUPiece(ref currentPosition, ref nextStepPosition))
                    {
                        m_MainBoard.UpdateBoard(ref currentPosition, ref nextStepPosition);
                        if (checkMultipleJumpsForKingPiece(ref nextStepPosition))
                        {
                            m_IsPlayerOneMove = true;
                            i_GameStatus = ePlayerGameStatus.ExtraCapture;
                        }
                        else
                        {
                            m_IsPlayerOneMove = false;
                        }

                        v_MakeMove = true;
                    }
                    else
                    {
                        i_GameStatus = ePlayerGameStatus.Error;
                    }
                }
                else
                {
                    i_GameStatus = ePlayerGameStatus.Error;
                }
            }

            if (!m_IsPlayerOneMove && i_GameStatus == ePlayerGameStatus.Active)
            {
                if (m_MainBoard.IsCellContainXPiece(currentPosition[0], currentPosition[1]))
                {
                    if (validateRegularMove(ref currentPosition, ref nextStepPosition))
                    {
                        if (checkForMandatoryCaptureXPiece())
                        {
                            i_GameStatus = ePlayerGameStatus.MissedCapture;
                            m_IsPlayerOneMove = false;
                        }
                        else
                        {
                            m_MainBoard.UpdateBoard(ref currentPosition, ref nextStepPosition);
                            m_IsPlayerOneMove = true;
                            v_MakeMove = true;
                        }
                    }
                    else if (isValidCaptureForXPiece(ref currentPosition, ref nextStepPosition))
                    {
                        m_MainBoard.UpdateBoard(ref currentPosition, ref nextStepPosition);
                        if (checkMultipleJumpsForRegularPiece(ref nextStepPosition)
                           || checkMultipleJumpsForKingPiece(ref nextStepPosition))
                        {
                            m_IsPlayerOneMove = false;
                            i_GameStatus = ePlayerGameStatus.ExtraCapture;
                        }
                        else
                        {
                            m_IsPlayerOneMove = true;
                        }

                        v_MakeMove = true;

                    }
                    else
                    {
                        i_GameStatus = ePlayerGameStatus.Error;
                    }
                }
                else if (m_MainBoard.IsCellContainKPiece(currentPosition[0], currentPosition[1]))
                {
                    if (isValidKingMove(ref currentPosition, ref nextStepPosition))
                    {
                        if (checkForMandatoryCaptureXPiece())
                        {
                            i_GameStatus = ePlayerGameStatus.MissedCapture;
                            m_IsPlayerOneMove = false;
                        }
                        else
                        {
                            m_MainBoard.UpdateBoard(ref currentPosition, ref nextStepPosition);
                            m_IsPlayerOneMove = true;
                            v_MakeMove = true;
                        }
                    }
                    else if (isValidCaptureForKPiece(ref currentPosition, ref nextStepPosition))
                    {
                        m_MainBoard.UpdateBoard(ref currentPosition, ref nextStepPosition);
                        if (checkMultipleJumpsForKingPiece(ref nextStepPosition))
                        {
                            m_IsPlayerOneMove = false;
                            i_GameStatus = ePlayerGameStatus.ExtraCapture;
                            ;
                        }
                        else
                        {
                            m_IsPlayerOneMove = true;
                        }

                        v_MakeMove = true;
                    }
                    else
                    {
                        i_GameStatus = ePlayerGameStatus.Error;
                    }
                }
                else
                {
                    i_GameStatus = ePlayerGameStatus.Error;
                }
            }

            return v_MakeMove;
        }

        private int[] convertToPosition(int i_Row, int i_Column)
        {
            int[] currentPosition = new int[2];

            currentPosition[0] = i_Row;
            currentPosition[1] = i_Column;

            return currentPosition;
        }
        private bool validateRegularMove(ref int[] i_CurrentPosition, ref int[] i_NextPosition)
        {
            bool validateRegularMove = false;

            if (i_CurrentPosition[0] == i_NextPosition[0] + 1 && IsCellEmpty(i_NextPosition[0], i_NextPosition[1]) && isCellContainXPiece(i_CurrentPosition[0], i_CurrentPosition[1])) // בדיקה שהוא חייב לזוז קדימה כלומר שורה 1 למעלה במטריצה!
            {
                if (i_CurrentPosition[1] == i_NextPosition[1] + 1)
                {
                    validateRegularMove = !validateRegularMove;
                }
                else if (i_CurrentPosition[1] == i_NextPosition[1] - 1)
                {
                    validateRegularMove = !validateRegularMove;
                }
            }

            if (i_CurrentPosition[0] == i_NextPosition[0] - 1 && IsCellEmpty(i_NextPosition[0], i_NextPosition[1]) && isCellContainOPiece(i_CurrentPosition[0], i_CurrentPosition[1])) // בדיקה שהוא חייב לזוז קדימה כלומר שורה 1 למעלה במטריצה!
            {
                if (i_CurrentPosition[1] == i_NextPosition[1] + 1)
                {
                    return validateRegularMove = !validateRegularMove;
                }
                else if (i_CurrentPosition[1] == i_NextPosition[1] - 1)
                {
                    return validateRegularMove = !validateRegularMove;
                }
            }

            return validateRegularMove;
        }
        private bool isValidKingMove(ref int[] i_CurrentPosition, ref int[] i_NextStepPosition)
        {
            bool isValidKingMove = false;

            if (i_CurrentPosition[0] == i_NextStepPosition[0] - 1 && IsCellEmpty(i_NextStepPosition[0], i_NextStepPosition[1]))
            {
                if (i_CurrentPosition[1] == i_NextStepPosition[1] + 1)
                {
                    isValidKingMove = true;
                }
                else if (i_CurrentPosition[1] == i_NextStepPosition[1] - 1)
                {
                    isValidKingMove = true;
                }
            }

            if (i_CurrentPosition[0] == i_NextStepPosition[0] + 1 && IsCellEmpty(i_NextStepPosition[0], i_NextStepPosition[1]))
            {
                if (i_CurrentPosition[1] == i_NextStepPosition[1] + 1)
                {
                    isValidKingMove = true;
                }
                else if (i_CurrentPosition[1] == i_NextStepPosition[1] - 1)
                {
                    isValidKingMove = true;
                }
            }

            return isValidKingMove;
        }
        private bool checkForMandatoryCaptureXPiece()
        {
            bool checkForMandatoryCaptureXPiece = false;

            for (int row = 0; row < m_MainBoard.GetBoardSize(); row++)
            {
                for (int col = 0; col < m_MainBoard.GetBoardSize(); col++)
                {
                    ePieceType e_CurrentPieceType = m_MainBoard.GetPieceAtPosition(row, col);
                    int[] currentPosition = new int[2];

                    currentPosition[0] = row;
                    currentPosition[1] = col;


                    if (e_CurrentPieceType == ePieceType.X)
                    {
                        if (canCaptureXPiece(ref currentPosition))
                        {
                            checkForMandatoryCaptureXPiece = true;
                        }
                    }
                    if (e_CurrentPieceType == ePieceType.K)
                    {
                        if (canCaptureKPiece(ref currentPosition))
                        {
                            checkForMandatoryCaptureXPiece = true;
                        }
                    }

                }
            }

            return checkForMandatoryCaptureXPiece;
        }
        private bool checkMultipleJumpsForRegularPiece(ref int[] i_NextPosition)
        {
            bool checkMultipleJumpsForRegularPiece = false;
            ePieceType currentPiectType = m_MainBoard.GetBoardMatrix()[i_NextPosition[0], i_NextPosition[1]].GetPieceType();

            if (currentPiectType == ePieceType.O)
            {
                if (canCaptureOPiece(ref i_NextPosition))
                {
                    checkMultipleJumpsForRegularPiece = !checkMultipleJumpsForRegularPiece;
                }
            }
            if (currentPiectType == ePieceType.X)
            {
                if (canCaptureXPiece(ref i_NextPosition))
                {
                    checkMultipleJumpsForRegularPiece = !checkMultipleJumpsForRegularPiece;
                }
            }
            return checkMultipleJumpsForRegularPiece;
        }
        private bool checkMultipleJumpsForKingPiece(ref int[] i_NextPosition)
        {
            bool checkMultipleJumpsForKingPiece = false;

            ePieceType currentPiectType = m_MainBoard.GetBoardMatrix()[i_NextPosition[0], i_NextPosition[1]].GetPieceType();

            if (currentPiectType == ePieceType.U)
            {
                if (canCaptureUPiece(ref i_NextPosition))
                {
                    checkMultipleJumpsForKingPiece = !checkMultipleJumpsForKingPiece;
                }
            }
            if (currentPiectType == ePieceType.K)
            {
                if (canCaptureKPiece(ref i_NextPosition))
                {
                    checkMultipleJumpsForKingPiece = true;
                }
            }
            return checkMultipleJumpsForKingPiece;
        }
        private bool canCaptureXPiece(ref int[] i_CurrentPosition)
        {
            bool canCaptureXPiece = false;
            int[] optionToEatFromTopLeft = new int[2];
            int[] optionToEatFromTopRight = new int[2];

            optionToEatFromTopLeft[0] = i_CurrentPosition[0] - 2;
            optionToEatFromTopLeft[1] = i_CurrentPosition[1] - 2;
            optionToEatFromTopRight[0] = i_CurrentPosition[0] - 2;
            optionToEatFromTopRight[1] = i_CurrentPosition[1] + 2;

            if (m_MainBoard.IsValidBoardPosition(optionToEatFromTopLeft[0], optionToEatFromTopLeft[1]))
            {
                if (IsCellEmpty(optionToEatFromTopLeft[0], optionToEatFromTopLeft[1]) &&
                    (isCellContainOPiece(i_CurrentPosition[0] - 1, i_CurrentPosition[1] - 1) || isCellContainUPiece(i_CurrentPosition[0] - 1, i_CurrentPosition[1] - 1)))
                {
                    canCaptureXPiece = !canCaptureXPiece;
                }
            }
            if (m_MainBoard.IsValidBoardPosition(optionToEatFromTopRight[0], optionToEatFromTopRight[1]))
            {
                if (IsCellEmpty(optionToEatFromTopRight[0], optionToEatFromTopRight[1]) &&
                    (isCellContainOPiece(i_CurrentPosition[0] - 1, i_CurrentPosition[1] + 1) || isCellContainUPiece(i_CurrentPosition[0] - 1, i_CurrentPosition[1] + 1)))
                {
                    canCaptureXPiece = !canCaptureXPiece;
                }
            }

            return canCaptureXPiece;

        }
        private bool checkForMandatoryCaptureOPiece()
        {
            bool checkForMandatoryCaptureOPiece = false;

            for (int row = 0; row < m_MainBoard.GetBoardSize(); row++)
            {
                for (int col = 0; col < m_MainBoard.GetBoardSize(); col++)
                {
                    ePieceType e_CurrentPieceType = m_MainBoard.GetPieceAtPosition(row, col);
                    int[] currentPosition = new int[2];

                    currentPosition[0] = row;
                    currentPosition[1] = col;

                    if (e_CurrentPieceType == ePieceType.O)
                    {
                        if (canCaptureOPiece(ref currentPosition))
                        {
                            checkForMandatoryCaptureOPiece = true;
                        }
                    }
                    if (e_CurrentPieceType == ePieceType.U)
                    {
                        if (canCaptureUPiece(ref currentPosition))
                        {
                            checkForMandatoryCaptureOPiece = true;
                        }
                    }
                }
            }

            return checkForMandatoryCaptureOPiece;
        }
        private bool canCaptureUPiece(ref int[] i_NextPosition)
        {
            bool canCaptureUPiece = false;
            int[] optionToEatFromTopLeft = new int[2];
            int[] optionToEatFromTopRight = new int[2];

            optionToEatFromTopLeft[0] = i_NextPosition[0] - 2;
            optionToEatFromTopLeft[1] = i_NextPosition[1] - 2;
            optionToEatFromTopRight[0] = i_NextPosition[0] - 2;
            optionToEatFromTopRight[1] = i_NextPosition[1] + 2;

            if (canCaptureOPiece(ref i_NextPosition))
            {
                canCaptureUPiece = true;
            }

            if (m_MainBoard.IsValidBoardPosition(optionToEatFromTopLeft[0], optionToEatFromTopLeft[1]))
            {
                if (IsCellEmpty(optionToEatFromTopLeft[0], optionToEatFromTopLeft[1]) &&
                    (isCellContainXPiece(i_NextPosition[0] - 1, i_NextPosition[1] - 1) || isCellContainKPiece(i_NextPosition[0] - 1, i_NextPosition[1] - 1)))
                {
                    canCaptureUPiece = !canCaptureUPiece;
                }
            }
            if (m_MainBoard.IsValidBoardPosition(optionToEatFromTopRight[0], optionToEatFromTopRight[1]))
            {
                if (IsCellEmpty(optionToEatFromTopRight[0], optionToEatFromTopRight[1]) &&
                    (isCellContainXPiece(i_NextPosition[0] - 1, i_NextPosition[1] + 1) || isCellContainKPiece(i_NextPosition[0] - 1, i_NextPosition[1] + 1)))
                {
                    canCaptureUPiece = !canCaptureUPiece;
                }
            }
            return canCaptureUPiece;
        }
        private bool canCaptureOPiece(ref int[] i_CurrentPosition)
        {
            bool canCaptureOPiece = false;
            int[] optionToEatFromButtomLeft = new int[2];
            int[] optionToEatFromButtomRight = new int[2];

            optionToEatFromButtomLeft[0] = i_CurrentPosition[0] + 2;
            optionToEatFromButtomLeft[1] = i_CurrentPosition[1] - 2;
            optionToEatFromButtomRight[0] = i_CurrentPosition[0] + 2;
            optionToEatFromButtomRight[1] = i_CurrentPosition[1] + 2;

            if (m_MainBoard.IsValidBoardPosition(optionToEatFromButtomLeft[0], optionToEatFromButtomLeft[1]))
            {
                if (IsCellEmpty(optionToEatFromButtomLeft[0], optionToEatFromButtomLeft[1]) &&
                    (isCellContainXPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] - 1) || isCellContainKPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] - 1)))
                {
                    canCaptureOPiece = true;
                }
            }
            if (m_MainBoard.IsValidBoardPosition(optionToEatFromButtomRight[0], optionToEatFromButtomRight[1]))
            {
                if (IsCellEmpty(optionToEatFromButtomRight[0], optionToEatFromButtomRight[1]) &&
                    (isCellContainXPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] + 1) || isCellContainKPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] + 1)))
                {
                    canCaptureOPiece = true;
                }
            }

            return canCaptureOPiece;
        }
        private bool canCaptureKPiece(ref int[] i_CurrentPosition)
        {
            bool canCaptureKPiece = false;
            int[] optionToEatFromButtomLeft = new int[2];
            int[] optionToEatFromButtomRight = new int[2];

            optionToEatFromButtomLeft[0] = i_CurrentPosition[0] + 2;
            optionToEatFromButtomLeft[1] = i_CurrentPosition[1] - 2;
            optionToEatFromButtomRight[0] = i_CurrentPosition[0] + 2;
            optionToEatFromButtomRight[1] = i_CurrentPosition[1] + 2;

            if (canCaptureXPiece(ref i_CurrentPosition))
            {
                canCaptureKPiece = true;
            }

            if (m_MainBoard.IsValidBoardPosition(optionToEatFromButtomLeft[0], optionToEatFromButtomLeft[1]))
            {
                if (IsCellEmpty(optionToEatFromButtomLeft[0], optionToEatFromButtomLeft[1]) &&
                    (isCellContainOPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] - 1) || isCellContainUPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] - 1)))
                {
                    canCaptureKPiece = true;
                }
            }
            if (m_MainBoard.IsValidBoardPosition(optionToEatFromButtomRight[0], optionToEatFromButtomRight[1]))
            {
                if (IsCellEmpty(optionToEatFromButtomRight[0], optionToEatFromButtomRight[1]) &&
                    (isCellContainOPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] + 1) || isCellContainUPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] + 1)))
                {
                    canCaptureKPiece = true;
                }
            }

            return canCaptureKPiece;
        }
        private bool isValidCaptureForOPiece(ref int[] i_CurrentPosition, ref int[] i_NextPosition)
        {
            bool isValidCaptureForOPiece = false;

            if (i_CurrentPosition[0] == i_NextPosition[0] - 2 && i_CurrentPosition[1] == i_NextPosition[1] - 2)
            {
                if (IsCellEmpty(i_NextPosition[0], i_NextPosition[1]) &&
                    (isCellContainXPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] + 1) || isCellContainKPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] + 1)))
                {
                    isValidCaptureForOPiece = true;
                    m_MainBoard.GetBoardMatrix()[i_CurrentPosition[0] + 1, i_CurrentPosition[1] + 1] = new Piece(ePieceType.E);
                }
            }

            if (i_CurrentPosition[0] == i_NextPosition[0] - 2 && i_CurrentPosition[1] == i_NextPosition[1] + 2)
            {
                if (IsCellEmpty(i_NextPosition[0], i_NextPosition[1]) &&
                    (isCellContainXPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] - 1) || isCellContainKPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] - 1)))
                {
                    isValidCaptureForOPiece = true;
                    m_MainBoard.GetBoardMatrix()[i_CurrentPosition[0] + 1, i_CurrentPosition[1] - 1] = new Piece(ePieceType.E);
                }
            }

            return isValidCaptureForOPiece;
        }
        private bool isValidCaptureForXPiece(ref int[] i_CurrentPosition, ref int[] i_NextPosition)
        {
            bool isValidCaptureForXPiece = false;

            if (i_CurrentPosition[0] == i_NextPosition[0] + 2 && i_CurrentPosition[1] == i_NextPosition[1] - 2)
            {
                if (IsCellEmpty(i_NextPosition[0], i_NextPosition[1]) &&
                    (isCellContainOPiece(i_CurrentPosition[0] - 1, i_CurrentPosition[1] + 1) || isCellContainUPiece(i_CurrentPosition[0] - 1, i_CurrentPosition[1] + 1)))
                {
                    isValidCaptureForXPiece = true;
                    m_MainBoard.GetBoardMatrix()[i_CurrentPosition[0] - 1, i_CurrentPosition[1] + 1] = new Piece(ePieceType.E);
                }
            }

            if (i_CurrentPosition[0] == i_NextPosition[0] + 2 && i_CurrentPosition[1] == i_NextPosition[1] + 2)
            {
                if (IsCellEmpty(i_NextPosition[0], i_NextPosition[1]) &&
                    (isCellContainOPiece(i_CurrentPosition[0] - 1, i_CurrentPosition[1] - 1) || isCellContainUPiece(i_CurrentPosition[0] - 1, i_CurrentPosition[1] - 1)))
                {
                    isValidCaptureForXPiece = true;
                    m_MainBoard.GetBoardMatrix()[i_CurrentPosition[0] - 1, i_CurrentPosition[1] - 1] = new Piece(ePieceType.E);
                }
            }
            return isValidCaptureForXPiece;
        }
        private bool isValidCaptureForKPiece(ref int[] i_CurrentPosition, ref int[] i_NextPosition)
        {
            bool isValidCaptureForKPiece = false;

            if (isValidCaptureForXPiece(ref i_CurrentPosition, ref i_NextPosition))
            {
                isValidCaptureForKPiece = true;
            }

            if (i_CurrentPosition[0] == i_NextPosition[0] - 2 && i_CurrentPosition[1] == i_NextPosition[1] - 2)
            {
                if (IsCellEmpty(i_NextPosition[0], i_NextPosition[1]) &&
                    (isCellContainOPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] + 1) || isCellContainUPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] + 1)))
                {
                    isValidCaptureForKPiece = true;
                    m_MainBoard.GetBoardMatrix()[i_CurrentPosition[0] + 1, i_CurrentPosition[1] + 1] = new Piece(ePieceType.E);
                }
            }

            if (i_CurrentPosition[0] == i_NextPosition[0] - 2 && i_CurrentPosition[1] == i_NextPosition[1] + 2)
            {
                if (IsCellEmpty(i_NextPosition[0], i_NextPosition[1]) &&
                    (isCellContainOPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] - 1) || isCellContainUPiece(i_CurrentPosition[0] + 1, i_CurrentPosition[1] - 1)))
                {
                    isValidCaptureForKPiece = true;
                    m_MainBoard.GetBoardMatrix()[i_CurrentPosition[0] + 1, i_CurrentPosition[1] - 1] = new Piece(ePieceType.E);
                }
            }

            return isValidCaptureForKPiece;
        }
        private bool isValidCaptureForUPiece(ref int[] i_CurrentPosition, ref int[] i_NextPosition)
        {
            bool isValidCaptureForUPiece = false;

            if (isValidCaptureForOPiece(ref i_CurrentPosition, ref i_NextPosition))
            {
                isValidCaptureForUPiece = !isValidCaptureForUPiece;
            }

            if (i_CurrentPosition[0] == i_NextPosition[0] + 2 && i_CurrentPosition[1] == i_NextPosition[1] - 2)
            {
                if (IsCellEmpty(i_NextPosition[0], i_NextPosition[1]) &&
                    (isCellContainXPiece(i_CurrentPosition[0] - 1, i_CurrentPosition[1] + 1) || isCellContainKPiece(i_CurrentPosition[0] - 1, i_CurrentPosition[1] + 1)))
                {
                    isValidCaptureForUPiece = true;
                    m_MainBoard.GetBoardMatrix()[i_CurrentPosition[0] - 1, i_CurrentPosition[1] + 1] = new Piece(ePieceType.E);
                }
            }

            if (i_CurrentPosition[0] == i_NextPosition[0] + 2 && i_CurrentPosition[1] == i_NextPosition[1] + 2)
            {
                if (IsCellEmpty(i_NextPosition[0], i_NextPosition[1]) &&
                    (isCellContainXPiece(i_CurrentPosition[0] - 1, i_CurrentPosition[1] - 1) || isCellContainKPiece(i_CurrentPosition[0] - 1, i_CurrentPosition[1] - 1)))
                {
                    isValidCaptureForUPiece = true;
                    m_MainBoard.GetBoardMatrix()[i_CurrentPosition[0] - 1, i_CurrentPosition[1] - 1] = new Piece(ePieceType.E);
                }
            }

            return isValidCaptureForUPiece;
        }
        public bool IsCellEmpty(int i_Row, int i_Col)
        {
            bool isCellEmpty = false;

            if (m_MainBoard.IsValidBoardPosition(i_Row, i_Col))
            {
                if (m_MainBoard.GetBoardMatrix()[i_Row, i_Col].GetPieceType() == ePieceType.E)
                {
                    isCellEmpty = !isCellEmpty;
                }
            }

            return isCellEmpty;
        }
        private bool isCellContainXPiece(int i_Row, int i_Col)
        {
            bool isCellContainXPiece = false;

            if (m_MainBoard.IsValidBoardPosition(i_Row, i_Col))
            {

                if (m_MainBoard.GetBoardMatrix()[i_Row, i_Col].GetPieceType() == ePieceType.X)
                {
                    isCellContainXPiece = !isCellContainXPiece;
                }
            }

            return isCellContainXPiece;
        }
        private bool isCellContainOPiece(int i_Row, int i_Col)
        {
            bool isCellContainOPiece = false;

            if (m_MainBoard.IsValidBoardPosition(i_Row, i_Col))
            {
                if (m_MainBoard.GetBoardMatrix()[i_Row, i_Col].GetPieceType() == ePieceType.O)
                {
                    isCellContainOPiece = !isCellContainOPiece;
                }
            }

            return isCellContainOPiece;
        }
        private bool isCellContainKPiece(int i_Row, int i_Col)
        {
            bool isCellContainKPiece = false;

            if (m_MainBoard.IsValidBoardPosition(i_Row, i_Col))
            {
                if (m_MainBoard.GetBoardMatrix()[i_Row, i_Col].GetPieceType() == ePieceType.K)
                {
                    isCellContainKPiece = !isCellContainKPiece;
                }
            }

            return isCellContainKPiece;
        }
        private bool isCellContainUPiece(int i_Row, int i_Col)
        {
            bool isCellContainKPiece = false;

            if (m_MainBoard.IsValidBoardPosition(i_Row, i_Col))
            {
                if (m_MainBoard.GetBoardMatrix()[i_Row, i_Col].GetPieceType() == ePieceType.U)
                {
                    isCellContainKPiece = !isCellContainKPiece;
                }
            }

            return isCellContainKPiece;
        }

        public void ComputerGameManager(ref string o_ComputerMoveString)
        {
            if (r_SecondPlayer.GetPlayerGameStatus() == ePlayerGameStatus.ExtraCapture && !(m_IsPlayerOneMove))
            {
                int[] positionArray = r_SecondPlayer.GetPositionForExtraEating();
                extraEatingForComputer(ref positionArray, m_MainBoard.GetBoardMatrix()[positionArray[0], positionArray[1]].GetPieceType(), ref o_ComputerMoveString);
            }
            else
            {
                initComputerGame(ref o_ComputerMoveString);
            }

        }

        private void initComputerGame(ref string o_ComputerMoveString)
        {
            List<(int, int, int, int)> normalMoves = new List<(int, int, int, int)>();
            List<(int, int, int, int)> eatingMoves = new List<(int, int, int, int)>();

            List<(int, int, int, int)> extraEatingMoves = new List<(int, int, int, int)>();

            int isEatingMove = 0;

            for (int rows = 0; rows < m_MainBoard.GetBoardSize(); rows++)
            {
                for (int cols = 0; cols < m_MainBoard.GetBoardSize(); cols++)
                {
                    if (m_MainBoard.GetBoardMatrix()[rows, cols].GetPieceType() == ePieceType.X)
                    {
                        checkIfNextStepValidForComputer(rows, cols, ref normalMoves, ePieceType.X);
                        checkIfEatingValidForComputer(rows, cols, ref eatingMoves, ePieceType.X);
                    }

                    if (m_MainBoard.GetBoardMatrix()[rows, cols].GetPieceType() == ePieceType.K)
                    {
                        checkIfNextStepValidForComputer(rows, cols, ref normalMoves, ePieceType.K);
                        checkIfEatingValidForComputer(rows, cols, ref eatingMoves, ePieceType.K);
                    }
                }
            }

            (int, int, int, int) chosenMove = computerChosenMove(ref normalMoves, ref eatingMoves, ref isEatingMove);

            if (chosenMove != (-1, -1, -1, -1))
            {
                int[] currentPosition = new int[2];
                int[] nextPosition = new int[2];

                currentPosition[0] = chosenMove.Item1;
                currentPosition[1] = chosenMove.Item2;

                nextPosition[0] = chosenMove.Item3;
                nextPosition[1] = chosenMove.Item4;

                if (isEatingMove == 1)
                {
                    int midRow = (chosenMove.Item1 + chosenMove.Item3) / 2;
                    int midCol = (chosenMove.Item2 + chosenMove.Item4) / 2;
                    m_MainBoard.GetBoardMatrix()[midRow, midCol] = new Piece(ePieceType.E);
                    m_MainBoard.UpdateBoard(ref currentPosition, ref nextPosition);
                    convertComputerPlayToString(ref currentPosition, ref nextPosition, ref o_ComputerMoveString);

                    checkIfEatingValidForComputer(nextPosition[0], nextPosition[1],
                                                  ref extraEatingMoves, m_MainBoard.GetBoardMatrix()[nextPosition[0], nextPosition[1]].GetPieceType());

                    if (extraEatingMoves.Count > 0)
                    {
                        m_IsPlayerOneMove = false;
                        r_SecondPlayer.SetPlayerGameStatus(ePlayerGameStatus.ExtraCapture);
                        r_FirstPlayer.SetPlayerGameStatus(ePlayerGameStatus.Waiting);
                        r_SecondPlayer.SetPositionForExtraEating(ref nextPosition);
                    }
                    else
                    {
                        r_SecondPlayer.SetPlayerGameStatus(ePlayerGameStatus.Waiting);
                        r_FirstPlayer.SetPlayerGameStatus(ePlayerGameStatus.Active);
                        m_IsPlayerOneMove = true;
                    }

                }

                else if (isEatingMove == 0)
                {
                    r_SecondPlayer.SetPlayerGameStatus(ePlayerGameStatus.Waiting);
                    r_FirstPlayer.SetPlayerGameStatus(ePlayerGameStatus.Active);
                    m_MainBoard.UpdateBoard(ref currentPosition, ref nextPosition);
                    convertComputerPlayToString(ref currentPosition, ref nextPosition, ref o_ComputerMoveString);
                    m_IsPlayerOneMove = true;

                }
            }
            else
            {
                r_SecondPlayer.SetPlayerGameStatus(ePlayerGameStatus.NoMoreMoves);
                r_FirstPlayer.SetPlayerGameStatus(ePlayerGameStatus.Active);
                m_IsPlayerOneMove = true;
            }

        }

        private void checkIfNextStepValidForComputer(int i_Rows, int i_Cols, ref List<(int, int, int, int)> i_NormalMovesList, ePieceType i_EnumPieceType)
        {

            if (m_MainBoard.IsValidPosition(i_Rows - 1, i_Cols - 1))
            {
                if (m_MainBoard.GetBoardMatrix()[i_Rows - 1, i_Cols - 1].GetPieceType() == ePieceType.E)
                {
                    i_NormalMovesList.Add((i_Rows, i_Cols, i_Rows - 1, i_Cols - 1));
                }
            }

            if (m_MainBoard.IsValidPosition(i_Rows - 1, i_Cols + 1))
            {
                if (m_MainBoard.GetBoardMatrix()[i_Rows - 1, i_Cols + 1].GetPieceType() == ePieceType.E)
                {
                    i_NormalMovesList.Add((i_Rows, i_Cols, i_Rows - 1, i_Cols + 1));
                }
            }


            if (i_EnumPieceType == ePieceType.K)
            {
                if (m_MainBoard.IsValidPosition(i_Rows + 1, i_Cols - 1))
                {
                    if (m_MainBoard.GetBoardMatrix()[i_Rows + 1, i_Cols - 1].GetPieceType() == ePieceType.E)
                    {
                        i_NormalMovesList.Add((i_Rows, i_Cols, i_Rows + 1, i_Cols - 1));
                    }
                }

                if (m_MainBoard.IsValidPosition(i_Rows + 1, i_Cols + 1))
                {
                    if (m_MainBoard.GetBoardMatrix()[i_Rows + 1, i_Cols + 1].GetPieceType() == ePieceType.E)
                    {
                        i_NormalMovesList.Add((i_Rows, i_Cols, i_Rows + 1, i_Cols + 1));
                    }
                }
            }
        }

        private void checkIfEatingValidForComputer(int i_Rows, int i_Cols, ref List<(int, int, int, int)> i_EatingMovesList, ePieceType i_EnumPieceType)
        {

            if (m_MainBoard.IsValidPosition(i_Rows - 2, i_Cols - 2))
            {
                if (m_MainBoard.GetBoardMatrix()[i_Rows - 2, i_Cols - 2].GetPieceType() == ePieceType.E)
                {
                    if (m_MainBoard.GetBoardMatrix()[i_Rows - 1, i_Cols - 1].GetPieceType() == ePieceType.O
                        || m_MainBoard.GetBoardMatrix()[i_Rows - 1, i_Cols - 1].GetPieceType() == ePieceType.U)
                    {
                        i_EatingMovesList.Add((i_Rows, i_Cols, i_Rows - 2, i_Cols - 2));
                    }

                }
            }

            if (m_MainBoard.IsValidPosition(i_Rows - 2, i_Cols + 2))
            {
                if (m_MainBoard.GetBoardMatrix()[i_Rows - 2, i_Cols + 2].GetPieceType() == ePieceType.E)
                {
                    if (m_MainBoard.GetBoardMatrix()[i_Rows - 1, i_Cols + 1].GetPieceType() == ePieceType.O
                        || m_MainBoard.GetBoardMatrix()[i_Rows - 1, i_Cols + 1].GetPieceType() == ePieceType.U)
                    {
                        i_EatingMovesList.Add((i_Rows, i_Cols, i_Rows - 2, i_Cols + 2));
                    }
                }
            }

            if (i_EnumPieceType == ePieceType.K)
            {
                if (m_MainBoard.IsValidPosition(i_Rows + 2, i_Cols - 2))
                {
                    if (m_MainBoard.GetBoardMatrix()[i_Rows + 2, i_Cols - 2].GetPieceType() == ePieceType.E)
                    {
                        if (m_MainBoard.GetBoardMatrix()[i_Rows + 1, i_Cols - 1].GetPieceType() == ePieceType.O
                            || m_MainBoard.GetBoardMatrix()[i_Rows + 1, i_Cols - 1].GetPieceType() == ePieceType.U)
                        {
                            i_EatingMovesList.Add((i_Rows, i_Cols, i_Rows + 2, i_Cols - 2));
                        }
                    }
                }

                if (m_MainBoard.IsValidPosition(i_Rows + 2, i_Cols + 2))
                {
                    if (m_MainBoard.GetBoardMatrix()[i_Rows + 2, i_Cols + 2].GetPieceType() == ePieceType.E)
                    {
                        if (m_MainBoard.GetBoardMatrix()[i_Rows + 1, i_Cols + 1].GetPieceType() == ePieceType.O
                            || m_MainBoard.GetBoardMatrix()[i_Rows + 1, i_Cols + 1].GetPieceType() == ePieceType.U)
                        {
                            i_EatingMovesList.Add((i_Rows, i_Cols, i_Rows + 2, i_Cols + 2));
                        }
                    }
                }
            }
        }

        private (int, int, int, int) computerChosenMove(ref List<(int, int, int, int)> i_NormalMovesList,
                                                       ref List<(int, int, int, int)> i_EatingMovesList, ref int i_IsEatingMove)
        {
            if (i_EatingMovesList.Count > 0)
            {
                i_IsEatingMove = 1;
                return i_EatingMovesList[m_Random.Next(i_EatingMovesList.Count)];
            }
            else if (i_NormalMovesList.Count > 0)
            {
                i_IsEatingMove = 0;
                return i_NormalMovesList[m_Random.Next(i_NormalMovesList.Count)];
            }
            return (-1, -1, -1, -1);
        }

        private void extraEatingForComputer(ref int[] i_PositionForEating, ePieceType i_PieceType, ref string o_ComputerMoveString)
        {
            List<(int, int, int, int)> extraEatingMove = new List<(int, int, int, int)>();

            checkIfEatingValidForComputer(i_PositionForEating[0], i_PositionForEating[1],
                                          ref extraEatingMove, m_MainBoard.GetBoardMatrix()[i_PositionForEating[0], i_PositionForEating[1]].GetPieceType());

            int[] nextPosition = new int[2];
            nextPosition[0] = extraEatingMove[0].Item3;
            nextPosition[1] = extraEatingMove[0].Item4;

            int midRow = (i_PositionForEating[0] + nextPosition[0]) / 2;
            int midCol = (i_PositionForEating[1] + nextPosition[1]) / 2;

            m_MainBoard.GetBoardMatrix()[midRow, midCol] = new Piece(ePieceType.E);
            m_MainBoard.UpdateBoard(ref i_PositionForEating, ref nextPosition);
            convertComputerPlayToString(ref i_PositionForEating, ref nextPosition, ref o_ComputerMoveString);

            extraEatingMove.Clear();

            checkIfEatingValidForComputer(nextPosition[0], nextPosition[1],
                ref extraEatingMove, m_MainBoard.GetBoardMatrix()[nextPosition[0], nextPosition[1]].GetPieceType());

            if (extraEatingMove.Count > 0)
            {
                r_SecondPlayer.SetPlayerGameStatus(ePlayerGameStatus.ExtraCapture);
                r_FirstPlayer.SetPlayerGameStatus(ePlayerGameStatus.Waiting);
                r_SecondPlayer.SetPositionForExtraEating(ref nextPosition);
                m_IsPlayerOneMove = false;
            }
            else
            {
                r_SecondPlayer.SetPlayerGameStatus(ePlayerGameStatus.Waiting);
                r_FirstPlayer.SetPlayerGameStatus(ePlayerGameStatus.Active);
                m_IsPlayerOneMove = true;
            }

        }
        private void convertComputerPlayToString(ref int[] i_CurrentPosition, ref int[] i_NextPosition, ref string o_ComputerMoveString)
        {
            char currentRow = (char)('A' + i_CurrentPosition[0]);
            char nextRow = (char)('A' + i_NextPosition[0]);
            char currentColumn = (char)('a' + i_CurrentPosition[1]);
            char nextColumn = (char)('a' + i_NextPosition[1]);

            o_ComputerMoveString = $"{currentRow}{currentColumn}>{nextRow}{nextColumn}";
        }
        public bool GameOver()
        {
            bool gameOver = false;
            int numXPiece = 0;
            int numOPiece = 0;

            calculateAmountOfPieces(ref numXPiece, ref numOPiece);

            if (numOPiece == 0)
            {
                r_FirstPlayer.SetPlayerGameStatus(ePlayerGameStatus.NoMoreMoves);
                gameOver = true;
                r_SecondPlayer.SetScoreForPlayer(numXPiece);
            }
            else if (numXPiece == 0)
            {
                r_SecondPlayer.SetPlayerGameStatus(ePlayerGameStatus.NoMoreMoves);
                gameOver = true;
                r_FirstPlayer.SetScoreForPlayer(numOPiece);
            }

            return gameOver;
        }
        public bool IsGameOverDueToNoMoves()
        {
            bool isGameOverDueToNoMoves = false;
            int sumOfMovesXPiece = 0;
            int sumOfMovesOPiece = 0;
            int numXPiece = 0;
            int numOPiece = 0;

            for (int row = 0; row < m_MainBoard.GetBoardSize(); row++)
            {
                for (int col = 0; col < m_MainBoard.GetBoardSize(); col++)
                {
                    ePieceType currentPiece = m_MainBoard.GetPieceAtPosition(row, col);
                    int[] currentPosition = new int[2];
                    currentPosition[0] = row;
                    currentPosition[1] = col;

                    if (currentPiece == ePieceType.O)
                    {
                        if (canRegularMoveOPiece(ref currentPosition) || canCaptureOPiece(ref currentPosition))
                        {
                            sumOfMovesOPiece++;
                        }
                    }
                    if (currentPiece == ePieceType.U)
                    {
                        if (canRegularMoveKingPiece(ref currentPosition) || canCaptureUPiece(ref currentPosition))
                        {
                            sumOfMovesOPiece++;
                        }
                    }

                    if (currentPiece == ePieceType.X)
                    {
                        if (canRegularMoveXPiece(ref currentPosition) || canCaptureXPiece(ref currentPosition))
                        {
                            sumOfMovesXPiece++;
                        }
                    }
                    if (currentPiece == ePieceType.K)
                    {
                        if (canRegularMoveKingPiece(ref currentPosition) || canCaptureKPiece(ref currentPosition))
                        {
                            sumOfMovesXPiece++;
                        }
                    }

                }
            }

            if (sumOfMovesOPiece == 0 && sumOfMovesXPiece == 0)
            {
                calculateAmountOfPieces(ref numXPiece, ref numOPiece);
                isGameOverDueToNoMoves = true;
                r_FirstPlayer.SetPlayerGameStatus(ePlayerGameStatus.NoMoreMoves);
                r_SecondPlayer.SetPlayerGameStatus(ePlayerGameStatus.NoMoreMoves);
            }
            else if (sumOfMovesOPiece == 0)
            {
                r_FirstPlayer.SetPlayerGameStatus(ePlayerGameStatus.NoMoreMoves);
                isGameOverDueToNoMoves = true;
                calculateAmountOfPieces(ref numXPiece, ref numOPiece);
                r_SecondPlayer.SetScoreForPlayer(Math.Abs(numXPiece - numOPiece));

            }
            else if (sumOfMovesXPiece == 0)
            {
                r_SecondPlayer.SetPlayerGameStatus(ePlayerGameStatus.NoMoreMoves);
                isGameOverDueToNoMoves = true;
                calculateAmountOfPieces(ref numXPiece, ref numOPiece);
                r_FirstPlayer.SetScoreForPlayer(Math.Abs(numXPiece - numOPiece));
            }

            return isGameOverDueToNoMoves;
        }
        private bool canRegularMoveOPiece(ref int[] i_CurrentPosition)
        {
            bool canRegularMoveOPiece = false;

            int[] optionToMoveButtomLeft = new int[2];
            int[] optionToMoveButtomRight = new int[2];

            optionToMoveButtomLeft[0] = i_CurrentPosition[0] + 1;
            optionToMoveButtomLeft[1] = i_CurrentPosition[1] - 1;
            optionToMoveButtomRight[0] = i_CurrentPosition[0] + 1;
            optionToMoveButtomRight[1] = i_CurrentPosition[1] + 1;

            if (m_MainBoard.IsValidBoardPosition(optionToMoveButtomLeft[0], optionToMoveButtomLeft[1]))
            {
                if (validateRegularMove(ref i_CurrentPosition, ref optionToMoveButtomLeft))
                {
                    canRegularMoveOPiece = true;
                }
            }

            if (m_MainBoard.IsValidBoardPosition(optionToMoveButtomRight[0], optionToMoveButtomRight[1]))
            {
                if (validateRegularMove(ref i_CurrentPosition, ref optionToMoveButtomRight))
                {
                    canRegularMoveOPiece = true;
                }
            }

            return canRegularMoveOPiece;
        }
        private bool canRegularMoveXPiece(ref int[] i_CurrentPosition)
        {
            bool canRegularMoveXPiece = false;

            int[] optionToMoveTopLeft = new int[2];
            int[] optionToMoveTopRight = new int[2];

            optionToMoveTopLeft[0] = i_CurrentPosition[0] - 1;
            optionToMoveTopLeft[1] = i_CurrentPosition[1] - 1;
            optionToMoveTopRight[0] = i_CurrentPosition[0] - 1;
            optionToMoveTopRight[1] = i_CurrentPosition[1] + 1;

            if (m_MainBoard.IsValidBoardPosition(optionToMoveTopLeft[0], optionToMoveTopLeft[1]))
            {
                if (validateRegularMove(ref i_CurrentPosition, ref optionToMoveTopLeft))
                {
                    canRegularMoveXPiece = true;
                }
            }

            if (m_MainBoard.IsValidBoardPosition(optionToMoveTopRight[0], optionToMoveTopRight[1]))
            {
                if (validateRegularMove(ref i_CurrentPosition, ref optionToMoveTopRight))
                {
                    canRegularMoveXPiece = true;
                }
            }

            return canRegularMoveXPiece;

        }
        private bool canRegularMoveKingPiece(ref int[] i_CurrentPosition)
        {
            bool canRegularMoveKingPiece = false;
            int[] optionToMoveTopLeft = new int[2];
            int[] optionToMoveTopRight = new int[2];
            int[] optionToMoveButtomLeft = new int[2];
            int[] optionToMoveButtomRight = new int[2];

            optionToMoveTopLeft[0] = i_CurrentPosition[0] - 1;
            optionToMoveTopLeft[1] = i_CurrentPosition[1] - 1;
            optionToMoveTopRight[0] = i_CurrentPosition[0] - 1;
            optionToMoveTopRight[1] = i_CurrentPosition[1] + 1;


            optionToMoveButtomLeft[0] = i_CurrentPosition[0] + 1;
            optionToMoveButtomLeft[1] = i_CurrentPosition[1] - 1;
            optionToMoveButtomRight[0] = i_CurrentPosition[0] + 1;
            optionToMoveButtomRight[1] = i_CurrentPosition[1] + 1;

            if (m_MainBoard.IsValidBoardPosition(optionToMoveTopLeft[0], optionToMoveTopLeft[1]))
            {
                if (isValidKingMove(ref i_CurrentPosition, ref optionToMoveTopLeft))
                {
                    canRegularMoveKingPiece = true;
                }
            }

            if (m_MainBoard.IsValidBoardPosition(optionToMoveTopRight[0], optionToMoveTopRight[1]))
            {
                if (isValidKingMove(ref i_CurrentPosition, ref optionToMoveTopRight))
                {
                    canRegularMoveKingPiece = true;
                }
            }

            if (m_MainBoard.IsValidBoardPosition(optionToMoveButtomLeft[0], optionToMoveButtomLeft[1]))
            {
                if (isValidKingMove(ref i_CurrentPosition, ref optionToMoveButtomLeft))
                {
                    canRegularMoveKingPiece = true;
                }
            }

            if (m_MainBoard.IsValidBoardPosition(optionToMoveButtomRight[0], optionToMoveButtomRight[1]))
            {
                if (isValidKingMove(ref i_CurrentPosition, ref optionToMoveButtomRight))
                {
                    canRegularMoveKingPiece = true;
                }
            }

            return canRegularMoveKingPiece;
        }

        private void calculateAmountOfPieces(ref int i_NumXPiece, ref int i_NumOPiece)
        {
            for (int row = 0; row < m_MainBoard.GetBoardSize(); row++)
            {
                for (int col = 0; col < m_MainBoard.GetBoardSize(); col++)
                {
                    ePieceType currentPiece = m_MainBoard.GetPieceAtPosition(row, col);

                    if (currentPiece == ePieceType.X)
                    {
                        i_NumXPiece++;
                    }
                    if (currentPiece == ePieceType.K)
                    {
                        i_NumXPiece += 4;
                    }
                    if (currentPiece == ePieceType.O)
                    {
                        i_NumOPiece++;
                    }
                    if (currentPiece == ePieceType.U)
                    {
                        i_NumOPiece += 4;
                    }
                }
            }
        }
    }
}
