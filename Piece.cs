namespace Damka
{
    public class Piece
    {
        private ePieceType m_CurrentType;
        private string m_PieceContext;

        public Piece(ePieceType i_PieceType)
        {
            m_CurrentType = i_PieceType;
            updatePieceContext();
        }

        private void updatePieceContext()
        {
            switch (m_CurrentType)
            {
                case ePieceType.E:
                    m_PieceContext = "   ";
                    break;
                case ePieceType.X:
                    m_PieceContext = " X ";
                    break;
                case ePieceType.O:
                    m_PieceContext = " O ";
                    break;
                case ePieceType.K:
                    m_PieceContext = " K ";
                    break;
                case ePieceType.U:
                    m_PieceContext = " U ";
                    break;
            }
        }

        public ePieceType GetPieceType()
        {
            return m_CurrentType;
        }
    }
}
