using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace juego_de_formar_parejas
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        Label firstClicked = null;
        Label secondClicked = null;
        bool waitingForMismatch = false;

        public Form1()
        {
            InitializeComponent();
            AssignIconsToSquares();
        }

        private void AssignIconsToSquares()
        {
            List<string> icons = new List<string>()
            {
                "p", "p", "Q", "Q", "R", "R", "h", "h",
                "u", "u", "t", "t", "i", "i", "j", "j"
            };

            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    icons.RemoveAt(randomNumber);
                    iconLabel.ForeColor = iconLabel.BackColor;
                    iconLabel.Click += new EventHandler(label_Click);
                }
            }
        }

        private async void label_Click(object sender, EventArgs e)
        {
            if (waitingForMismatch)
                return;

            Label clickedLabel = (Label)sender;

            if (clickedLabel.ForeColor == Color.Black)
                return;

            if (firstClicked == null)
            {
                firstClicked = clickedLabel;
                firstClicked.ForeColor = Color.Black;
                return;
            }

            secondClicked = clickedLabel;
            secondClicked.ForeColor = Color.Black;

            if (firstClicked.Text != secondClicked.Text)
            {
                waitingForMismatch = true;
                await Task.Delay(500);
                firstClicked.ForeColor = firstClicked.BackColor;
                secondClicked.ForeColor = secondClicked.BackColor;
                firstClicked = null;
                secondClicked = null;
                waitingForMismatch = false;
            }
            else
            {
                firstClicked = null;
                secondClicked = null;
                CheckForWinner();
            }
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null && iconLabel.ForeColor != Color.Black)
                    return;
            }

            MessageBox.Show("¡Felicidades! Has encontrado todas las parejas.", "Juego Terminado");
            Close();
        }
    }
}
