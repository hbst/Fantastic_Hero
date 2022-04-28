using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FantasticHero {
    public partial class Form1 : Form {
        
        bool GameIsOn; //Set Gami is Starter 
        int Cur_X, Cur_Y; //Current Position at which to update the game position
        int P_X1, P_Y1; //Current Position P1
        int P_X2, P_Y2; //Current Position P2
        int P_X3, P_Y3; //Current Position P3
        int P_X4, P_Y4; //Current Position P4

        //Max rows \ columns
        int Max_X = 5; 
        int Max_Y = 5;

        //Current Player
        int CurPlayer;
        public Form1() {
            InitializeComponent();
        }

        private void pbCaptainAmerica_Click(object sender, EventArgs e) {
            SetlayerImage(pbCaptainAmerica.Image);
        }

        private void pbDeadPool_Click(object sender, EventArgs e) {
            SetlayerImage(pbDeadPool.Image);
        }

        private void pbHulk_Click(object sender, EventArgs e) {
            SetlayerImage(pbHulk.Image);
        }

        private void pbIronMan_Click(object sender, EventArgs e) {
            SetlayerImage(pbIronMan.Image);
        }

        //assign an image to the player
        private void SetlayerImage(Image Img) {

            if (pbPlayer1Image.Image == null) {
                pbPlayer1Image.Image = Img;
            }
            else {

                if (pbPlayer2Image.Image == null) {
                    if (numNrPlayers.Value > 1) {
                        pbPlayer2Image.Image = Img;
                    }
                }
                else {
                    if (pbPlayer3Image.Image == null) {
                        if (numNrPlayers.Value > 2) {
                            pbPlayer3Image.Image = Img;
                        }
                    }
                    else {
                        if (pbPlayer4Image.Image == null) {
                            if (numNrPlayers.Value > 3) {
                                pbPlayer4Image.Image = Img;
                            }
                        }
                    }
                }
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e) {
            GameIsOn = false;

            flpGame.Visible = false;
            pnPlayersImage.Visible = true;
        }


        //Reset Players Info
        private void resetPlayers() {
            pbPlayer1Image.Image = null;
            lblPlayer1Image.Text = string.Empty;
            pbPlayer2Image.Image = null;
            lblPlayer2Image.Text = string.Empty;
            pbPlayer3Image.Image = null;
            lblPlayer3Image.Text = string.Empty;
            pbPlayer4Image.Image = null;
            lblPlayer4Image.Text = string.Empty;
        }


        //Begin Game 
        private void BeginGame() {
            int Cord_x, Cord_y;
            int Size_Width = 110;
            int Size_Height = 119;
            int PicSpace = 10;
            flpGame.Controls.Clear();
            Cord_y = PicSpace;
            Cord_x = PicSpace;

            //draw the game panel dynamically with the maximum number of rows and columns defined
            for (int y = 1; y <= Max_Y; y++) {
                Cord_x = PicSpace;
                for (int x = 1; x <= Max_X; x++) {
                    //Create object
                    PictureBox PicBox = new PictureBox();
                    PicBox.Size = new Size(Size_Width, Size_Height);
                    PicBox.Location = new Point(Cord_x, Cord_y);
                    PicBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    PicBox.Tag = $"{x}_{y}";
                    PicBox.BackColor = Color.White;
                    PicBox.Name = $"PicBox_{x}_{y}";

                    Cord_x = Cord_x + PicSpace + Size_Width;

                    //Add New PicBox to Panel 
                    flpGame.Controls.Add(PicBox);
                }
                Cord_y = Cord_y + PicSpace + Size_Height;
            }

            flpGame.Size = new Size(Cord_x, Cord_y);
        }

        //Updates the Game Panel with the Positions of Players or monsters
        //note:
        //At this moment, if the player passes over a monster, it will assign the player's image to the monster in order to mark who caught the monster.
        private bool flpGameRefresh(int x, int y, bool Monters = false) {
            bool Result = true;
            PictureBox PicBox;

            // Control[] xx = flpGame.Controls.Find("PicBox_{x}_{y}", true);
            foreach (Control c in flpGame.Controls) {
                if (c.GetType() == typeof(PictureBox)) {
                    PicBox = (PictureBox)c;
                    if (PicBox.Name.ToLower() == $"PicBox_{x}_{y}".ToLower()) {

                        if (Monters) {
                            PicBox.Image = pbMonster.Image;
                            PicBox.Tag = "MONSTER";
                            Cur_X = P_X2;
                            Cur_Y = P_Y2;
                            CurPlayer = 0;
                        }
                        else {
                            switch (CurPlayer) {
                                case 1:
                                    P_X1 = x;
                                    P_Y1 = y;
                                    Cur_X = P_X2;
                                    Cur_Y = P_Y2;
                                    pbCurPlayer.Image = pbPlayer2Image.Image;
                                    PicBox.Image = pbPlayer1Image.Image;
                                    break;
                                case 2:
                                    P_X2 = x;
                                    P_Y2 = y;
                                    Cur_X = P_X3;
                                    Cur_Y = P_Y3;
                                    pbCurPlayer.Image = pbPlayer3Image.Image;
                                    PicBox.Image = pbPlayer2Image.Image;
                                    break;
                                case 3:
                                    P_X3 = x;
                                    P_Y3 = y;
                                    Cur_X = P_X4;
                                    Cur_Y = P_Y4;
                                    pbCurPlayer.Image = pbPlayer4Image.Image;
                                    PicBox.Image = pbPlayer3Image.Image;
                                    break;
                                case 4:
                                    P_X4 = x;
                                    P_Y4 = y;
                                    Cur_X = P_X1;
                                    Cur_Y = P_Y1;
                                    pbCurPlayer.Image = pbPlayer1Image.Image;
                                    PicBox.Image = pbPlayer4Image.Image;
                                    break;
                            }
                        }
                    }
                    else {
                        bool Clean = false;
                        if (PicBox.Tag != "MONSTER") {
                            switch (CurPlayer) {
                                case 1:
                                    if (PicBox.Name.ToLower() != $"PicBox_{P_X2}_{P_Y2}".ToLower()) {
                                        if (PicBox.Name.ToLower() != $"PicBox_{P_X3}_{P_Y3}".ToLower()) {
                                            if (PicBox.Name.ToLower() != $"PicBox_{P_X4}_{P_Y4}".ToLower()) {
                                                Clean = true;
                                            }
                                        }
                                    }
                                    break;
                                case 2:

                                    if (PicBox.Name.ToLower() != $"PicBox_{P_X1}_{P_Y1}".ToLower()) {
                                        if (PicBox.Name.ToLower() != $"PicBox_{P_X3}_{P_Y3}".ToLower()) {
                                            if (PicBox.Name.ToLower() != $"PicBox_{P_X4}_{P_Y4}".ToLower()) {
                                                Clean = true;
                                            }
                                        }
                                    }
                                    break;
                                case 3:
                                    if (PicBox.Name.ToLower() != $"PicBox_{P_X1}_{P_Y1}".ToLower()) {
                                        if (PicBox.Name.ToLower() != $"PicBox_{P_X2}_{P_Y2}".ToLower()) {
                                            if (PicBox.Name.ToLower() != $"PicBox_{P_X4}_{P_Y4}".ToLower()) {
                                                Clean = true;
                                            }
                                        }
                                    }
                                    break;
                                case 4:
                                    if (PicBox.Name.ToLower() != $"PicBox_{P_X1}_{P_Y1}".ToLower()) {
                                        if (PicBox.Name.ToLower() != $"PicBox_{P_X2}_{P_Y2}".ToLower()) {
                                            if (PicBox.Name.ToLower() != $"PicBox_{P_X3}_{P_Y3}".ToLower()) {
                                                Clean = true;
                                            }
                                        }
                                    }
                                    break;
                            }

                            if (Clean) {
                                PicBox.Image = null;
                            }

                        }

                    }

                }
            }


            if (!Monters) {

                if (CurPlayer < numNrPlayers.Value) {
                    CurPlayer++;
                }
                else {
                    CurPlayer = 1;
                }

                if (CurPlayer > numNrPlayers.Value) {
                    Cur_X = P_X1;
                    Cur_Y = P_Y1;
                    pbCurPlayer.Image = pbPlayer1Image.Image;
                }

            }



            return Result;
        }

        private void Form1_Load(object sender, EventArgs e) {
            GameIsOn = false;
            resetPlayers();

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) {
            bool Refresh = false;

            if (GameIsOn) {
                switch (e.KeyData) {
                    case Keys.Up:
                        if (Cur_Y - 1 < 1) {
                            Cur_Y = 1;
                        }
                        else {
                            Cur_Y--;
                            Refresh = true;
                        }

                        break;
                    case Keys.Down:
                        if (Cur_Y + 1 > Max_Y) {
                            Cur_Y = Max_Y;
                        }
                        else {
                            Cur_Y++;
                            Refresh = true;
                        }

                        break;
                    case Keys.Left:
                        if (Cur_X - 1 < 1) {
                            Cur_X = 1;
                        }
                        else {
                            Cur_X--;
                            Refresh = true;
                        }
                        break;
                    case Keys.Right:
                        if (Cur_X + 1 > Max_X) {
                            Cur_X = Max_X;
                        }
                        else {
                            Cur_X++;
                            Refresh = true;
                        }

                        break;
                }

                if (Refresh) {
                    flpGameRefresh(Cur_X, Cur_Y);
                }
            }
        }

        private void pbUp_Click(object sender, EventArgs e) {
            if (GameIsOn) {
                if (Cur_Y - 1 < 1) {
                    Cur_Y = 1;
                }
                else {
                    Cur_Y--;
                    flpGameRefresh(Cur_X, Cur_Y);
                }

            }
        }

        private void pbDown_Click(object sender, EventArgs e) {
            if (GameIsOn) {
                if (Cur_Y + 1 > Max_Y) {
                    Cur_Y = Max_Y;
                }
                else {
                    Cur_Y++;
                    flpGameRefresh(Cur_X, Cur_Y);
                }

            }
        }

        private void pbLeft_Click(object sender, EventArgs e) {
            if (GameIsOn) {
                if (Cur_X - 1 < 1) {
                    Cur_X = 1;
                }
                else {
                    Cur_X--;
                    flpGameRefresh(Cur_X, Cur_Y);
                }
            }
        }

        private void txtP2_TextChanged(object sender, EventArgs e) {
            lblPlayer2Image.Text = txtP2.Text;
        }

        private void txtP3_TextChanged(object sender, EventArgs e) {
            lblPlayer3Image.Text = txtP3.Text;
        }

        private void txtP4_TextChanged(object sender, EventArgs e) {
            lblPlayer4Image.Text = txtP4.Text;
        }

        private void pbRight_Click(object sender, EventArgs e) {
            if (GameIsOn) {
                if (Cur_X + 1 > Max_X) {
                    Cur_X = Max_X;
                }
                else {
                    Cur_X++;
                    flpGameRefresh(Cur_X, Cur_Y);
                }
            }
        }

        private void numNrMonsters_ValueChanged(object sender, EventArgs e) {

        }

        private void BtnPlay_Click(object sender, EventArgs e) {
            StartGame();
            this.Focus();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
            if (!GameIsOn) {

                switch (numNrPlayers.Value) {
                    case 1:
                        txtP1.Visible = true;
                        txtP1.Text = txtP1.Text;
                        txtP2.Visible = false;
                        txtP2.Text = String.Empty;
                        txtP3.Visible = false;
                        txtP3.Text = String.Empty;
                        txtP4.Visible = false;
                        txtP4.Text = String.Empty;
                        break;
                    case 2:
                        txtP1.Visible = true;
                        txtP1.Text = txtP1.Text;
                        txtP2.Visible = true;
                        txtP2.Text = txtP2.Text;
                        txtP3.Visible = false;
                        txtP3.Text = String.Empty;
                        txtP4.Visible = false;
                        txtP4.Text = String.Empty;
                        break;
                    case 3:
                        txtP1.Visible = true;
                        txtP1.Text = txtP1.Text;
                        txtP2.Visible = true;
                        txtP2.Text = txtP2.Text;
                        txtP3.Visible = true;
                        txtP3.Text = txtP3.Text;
                        txtP4.Visible = false;
                        txtP4.Text = String.Empty;
                        break;
                    case 4:
                        txtP1.Visible = true;
                        txtP1.Text = txtP1.Text;
                        txtP2.Visible = true;
                        txtP2.Text = txtP2.Text;
                        txtP3.Visible = true;
                        txtP3.Text = txtP3.Text;
                        txtP4.Visible = true;
                        txtP4.Text = txtP4.Text;
                        break;

                }
            }

        }

        private void txtP1_TextChanged(object sender, EventArgs e) {
            lblPlayer1Image.Text = txtP1.Text;
        }

        private void button2_Click(object sender, EventArgs e) {
            StartGame();
        }

        //Start Game
        private void StartGame() {
            bool stargame = false;

            P_X1 = 0;
            P_Y1 = 0;
            P_X2 = 0;
            P_Y2 = 0;
            P_X3 = 0;
            P_Y3 = 0;
            P_X4 = 0;
            P_Y4 = 0;

            pbCurPlayer.Image = pbPlayer2Image.Image;
            //validate  if all players have name and skin
            switch (numNrPlayers.Value) {
                case 1:
                    if (txtP1.Text.Length > 0 && pbPlayer1Image.Image != null) {
                        stargame = true;
                    }
                    break;
                case 2:
                    if (txtP1.Text.Length > 0 && pbPlayer1Image.Image != null) {
                        if (txtP2.Text.Length > 0 && pbPlayer2Image.Image != null) {
                            stargame = true;
                        }
                    }
                    break;
                case 3:
                    if (txtP1.Text.Length > 0 && pbPlayer1Image.Image != null) {
                        if (txtP2.Text.Length > 0 && pbPlayer2Image.Image != null) {
                            if (txtP3.Text.Length > 0 && pbPlayer3Image.Image != null) {
                                stargame = true;
                            }
                        }
                    }
                    break;
                case 4:
                    if (txtP1.Text.Length > 0 && pbPlayer1Image.Image != null) {
                        if (txtP2.Text.Length > 0 && pbPlayer2Image.Image != null) {
                            if (txtP3.Text.Length > 0 && pbPlayer3Image.Image != null) {
                                if (txtP4.Text.Length > 0 && pbPlayer4Image.Image != null) {
                                    stargame = true;
                                }
                            }
                        }
                    }
                    break;
            }

            if (stargame) {
                pnPlayersImage.Visible = false;
                flpGame.Visible = true;
                BeginGame();
                CurPlayer = 1;
                GameIsOn = true;
                Cur_X = 1;
                Cur_Y = 1;

                int Max_X = 5;
                int Max_Y = 5;


                //Assign starting position to each player
                switch (numNrPlayers.Value) {
                    case 1:
                        P_X1 = 1;
                        P_Y1 = 1;
                        P_X2 = 0;
                        P_Y2 = 0;
                        P_X3 = 0;
                        P_Y3 = 0;
                        P_X4 = 0;
                        P_Y4 = 0;
                        flpGameRefresh(P_X1, P_Y1);
                        break;
                    case 2:
                        P_X1 = 1;
                        P_Y1 = 1;
                        P_X2 = 1;
                        P_Y2 = Max_Y;
                        flpGameRefresh(P_X1, P_Y1);
                        flpGameRefresh(P_X2, P_Y2);
                        break;
                    case 3:
                        P_X1 = 1;
                        P_Y1 = 1;
                        P_X2 = 1;
                        P_Y2 = Max_Y;
                        P_X3 = Max_X;
                        P_Y3 = 1;
                        flpGameRefresh(P_X1, P_Y1);
                        flpGameRefresh(P_X2, P_Y2);
                        flpGameRefresh(P_X3, P_Y3);
                        break;
                    case 4:
                        P_X1 = 1;
                        P_Y1 = 1;
                        P_X2 = 1;
                        P_Y2 = Max_Y;
                        P_X3 = Max_X;
                        P_Y3 = 1;
                        P_X4 = Max_X;
                        P_Y4 = Max_Y;
                        flpGameRefresh(P_X1, P_Y1);
                        flpGameRefresh(P_X2, P_Y2);
                        flpGameRefresh(P_X3, P_Y3);
                        flpGameRefresh(P_X4, P_Y4);
                        break;
                }

                int RandomX = 0;
                int RandomY = 0;
                Random random = new Random();
                bool exitWhile = false;

                //Player 0 = monster
                //Assigns random position to each monster
                CurPlayer = 0;
                for (int i = 0; i < numNrMonsters.Value; i++) {
                    exitWhile = false;
                    while (!exitWhile) {
                        RandomX = random.Next(1, 5);
                        RandomY = random.Next(1, 5);

                        if (RandomX != P_X1 || RandomY != P_Y1) {
                            if (RandomX != P_X2 || RandomY != P_Y2) {
                                if (RandomX != P_X3 || RandomY != P_Y3) {
                                    if (RandomX != P_X4 || RandomY != P_Y4) {
                                        exitWhile = true;
                                    }
                                }
                            }
                        }
                    }

                    flpGameRefresh(RandomX, RandomY, true);

                }
                //Set Current player = 1
                CurPlayer = 1;

            }
            else {
                MessageBox.Show("Select skin and Name");
            }
        }

        private void flpGame_Paint(object sender, PaintEventArgs e) {

        }
    }
}
