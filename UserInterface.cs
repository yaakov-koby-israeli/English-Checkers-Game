using System.Windows.Forms;

namespace Damka
{
    public class UserInterface
    {
        private FormGameSettings m_FormGameSettings;

        private FormGamePlay m_FormGamePlay;

        public void InitGame()
        {
            if (initGameSettings())
            {
                startGame(m_FormGameSettings.GetFirstPlayerName(), m_FormGameSettings.GetSecondPlayerName(), m_FormGameSettings.GetBoardSize());
            }
            else
            {
                MessageBox.Show(
                        $"Game setup was canceled or incomplete.",
                        "Damka",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
            }
        }

        private bool initGameSettings()
        {
            bool callFormFamePlay = false;

            m_FormGameSettings = new FormGameSettings();

            if (m_FormGameSettings.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(
                        $"Game settings saved successfully! Starting game...",
                        "Damka",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                callFormFamePlay = true;
            }

            return callFormFamePlay;
        }
        private void startGame(string i_PlayerOneName, string i_PlayerTowName, int i_BoardSize)
        {
            m_FormGamePlay = new FormGamePlay(i_PlayerOneName, i_PlayerTowName, i_BoardSize);
            m_FormGamePlay.ShowDialog();
        }
    }
}
