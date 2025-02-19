using System.Drawing;
using System.Windows.Forms;
using System;

namespace Damka
{
    public class FormGameSettings : Form
    {
        TextBox m_TextBoxPlayerOneName = new TextBox();
        TextBox m_TextBoxPlayerTwoName = new TextBox();

        CheckBox m_CheckBoxPlayerTwo = new CheckBox();

        Label m_LabelBoardSize = new Label();
        Label m_LabelBoardSizeSix = new Label();
        Label m_LabelBoardSizeEight = new Label();
        Label m_LabelBoardSizeTen = new Label();
        Label m_LabelPlayers = new Label();
        Label m_LabelPlayersOne = new Label();
        Label m_LabelPlayersTwo = new Label();

        RadioButton m_RadioButtonBoardSizeSix = new RadioButton();
        RadioButton m_RadioButtonBoardSizeEight = new RadioButton();
        RadioButton m_RadioButtonBoardSizeTen = new RadioButton();

        Button m_ButtonDone = new Button();

        private string m_FirstPlayerName = null;
        private string m_SecondPlayerName = null;
        private int m_BoardSize = 0;


        public FormGameSettings()
        {
            initializeGameSettingForm();
            initializeControls();
        }

        private void initializeGameSettingForm()
        {
            this.Size = new Size(250, 235);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Game Settings";
        }

        public string GetFirstPlayerName()
        {
            return m_FirstPlayerName;
        }

        public string GetSecondPlayerName()
        {
            return m_SecondPlayerName;
        }

        public int GetBoardSize()
        {
            return m_BoardSize;
        }

        private void initializeControls()
        {
            // init Labels btn
            m_LabelBoardSize.Left = 10;
            m_LabelBoardSize.Top = 10;
            m_LabelBoardSize.Text = "Board Size:";
            m_LabelBoardSize.AutoSize = true;
            this.Controls.Add(m_LabelBoardSize);

            m_LabelPlayers.Left = m_LabelBoardSize.Left;
            m_LabelPlayers.Top = m_RadioButtonBoardSizeSix.Bottom + 35;
            m_LabelPlayers.Text = "Players:";
            m_LabelPlayers.AutoSize = true;
            this.Controls.Add(m_LabelPlayers);


            // 6*6
            m_RadioButtonBoardSizeSix.Left = 30;
            m_RadioButtonBoardSizeSix.Top = 30;
            m_RadioButtonBoardSizeSix.AutoSize = true;
            this.Controls.Add(m_RadioButtonBoardSizeSix);

            m_LabelBoardSizeSix.Left = m_RadioButtonBoardSizeSix.Right + 2;
            m_LabelBoardSizeSix.Top = m_RadioButtonBoardSizeSix.Top;
            m_LabelBoardSizeSix.Text = "6x6";
            m_LabelBoardSizeSix.AutoSize = true;
            this.Controls.Add(m_LabelBoardSizeSix);

            // 8*8
            m_RadioButtonBoardSizeEight.Left = m_LabelBoardSizeSix.Right + 10;
            m_RadioButtonBoardSizeEight.Top = m_RadioButtonBoardSizeSix.Top;
            m_RadioButtonBoardSizeEight.AutoSize = true;
            this.Controls.Add(m_RadioButtonBoardSizeEight);

            m_LabelBoardSizeEight.Left = m_RadioButtonBoardSizeEight.Right + 2;
            m_LabelBoardSizeEight.Top = m_RadioButtonBoardSizeSix.Top;
            m_LabelBoardSizeEight.Text = "8x8";
            m_LabelBoardSizeEight.AutoSize = true;
            this.Controls.Add(m_LabelBoardSizeEight);

            // 10*10
            m_RadioButtonBoardSizeTen.Left = m_LabelBoardSizeEight.Right + 10;
            m_RadioButtonBoardSizeTen.Top = m_RadioButtonBoardSizeSix.Top;
            m_RadioButtonBoardSizeTen.AutoSize = true;
            this.Controls.Add(m_RadioButtonBoardSizeTen);

            m_LabelBoardSizeTen.Left = m_RadioButtonBoardSizeTen.Right;
            m_LabelBoardSizeTen.Top = m_LabelBoardSizeSix.Top;
            m_LabelBoardSizeTen.Text = "10x10";
            m_LabelBoardSizeTen.AutoSize = true;
            this.Controls.Add(m_LabelBoardSizeTen);

            m_LabelPlayersOne.Left = m_RadioButtonBoardSizeSix.Left;
            m_LabelPlayersOne.Top = m_LabelPlayers.Bottom + 10;
            m_LabelPlayersOne.Text = "Player 1:";
            m_LabelPlayersOne.AutoSize = true;
            this.Controls.Add(m_LabelPlayersOne);

            m_CheckBoxPlayerTwo.Left = m_RadioButtonBoardSizeSix.Left;
            m_CheckBoxPlayerTwo.Top = m_LabelPlayersOne.Bottom + 15;
            m_CheckBoxPlayerTwo.Checked = false;
            m_CheckBoxPlayerTwo.AutoSize = true;
            this.Controls.Add(m_CheckBoxPlayerTwo);

            m_LabelPlayersTwo.Left = m_CheckBoxPlayerTwo.Right + 5;
            m_LabelPlayersTwo.Top = m_CheckBoxPlayerTwo.Top;
            m_LabelPlayersTwo.Text = "Player 2:";
            m_LabelPlayersTwo.AutoSize = true;
            this.Controls.Add(m_LabelPlayersTwo);

            m_TextBoxPlayerTwoName.Left = m_LabelPlayersTwo.Right + 5;
            m_TextBoxPlayerTwoName.Top = m_CheckBoxPlayerTwo.Top;
            m_TextBoxPlayerTwoName.Text = "[Computer]";
            m_TextBoxPlayerTwoName.Enabled = false;
            this.Controls.Add(m_TextBoxPlayerTwoName);

            m_TextBoxPlayerOneName.Left = m_LabelPlayersTwo.Right + 5;
            m_TextBoxPlayerOneName.Top = m_LabelPlayersOne.Top;
            this.Controls.Add(m_TextBoxPlayerOneName);

            // done btn
            m_ButtonDone.Left = m_TextBoxPlayerTwoName.Left + 25;
            m_ButtonDone.Top = m_TextBoxPlayerTwoName.Bottom + 15;
            m_ButtonDone.Text = "Done";
            this.Controls.Add(m_ButtonDone);

            // event handler
            m_CheckBoxPlayerTwo.CheckedChanged += new EventHandler(checkBoxPlayerTow_CheckedChanged);
            m_ButtonDone.Click += new EventHandler(buttonDone_Click);

        }

        private void buttonDone_Click(object sender, EventArgs e)
        {

            if (!validateInput())
            {
                return;
            }

            m_FirstPlayerName = m_TextBoxPlayerOneName.Text;
            m_SecondPlayerName = m_CheckBoxPlayerTwo.Checked ? m_TextBoxPlayerTwoName.Text : "Computer";

            if (m_RadioButtonBoardSizeSix.Checked)
            {
                m_BoardSize = 6;
            }
            else if (m_RadioButtonBoardSizeEight.Checked)
            {
                m_BoardSize = 8;
            }
            else if (m_RadioButtonBoardSizeTen.Checked)
            {
                m_BoardSize = 10;
            }

            // סגירת הטופס
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool validateInput()
        {
            bool validateInput = true;

            if (string.IsNullOrWhiteSpace(m_TextBoxPlayerOneName.Text))
            {
                MessageBox.Show("Please enter a name for Player 1.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                validateInput = false;
            }
            else if (m_CheckBoxPlayerTwo.Checked && string.IsNullOrWhiteSpace(m_TextBoxPlayerTwoName.Text))
            {
                MessageBox.Show("Please enter a name for Player 2.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                validateInput = false;
            }
            else if (!m_RadioButtonBoardSizeSix.Checked &&
                !m_RadioButtonBoardSizeEight.Checked &&
                !m_RadioButtonBoardSizeTen.Checked)
            {
                MessageBox.Show("Please select a board size.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                validateInput = false;
            }

            return validateInput;
        }

        // פונקציה שבודקת מצב של שחקן שני
        private void checkBoxPlayerTow_CheckedChanged(object sender, EventArgs e)
        {
            if (m_CheckBoxPlayerTwo.Checked)
            {
                m_TextBoxPlayerTwoName.Enabled = true;
                m_TextBoxPlayerTwoName.Text = "";
            }
            else
            {
                m_TextBoxPlayerTwoName.Enabled = false;
                m_TextBoxPlayerTwoName.Text = "[Computer]";
            }
        }
    }
}
