using PotZH_WinForms.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PotZH_WinForms {
    public partial class View : Form {
        private WhackModel _model;
        private Button[,] _buttonBoard;
        private Timer _timer;
        private int _timeElapsed;
        private Label _timeRem;
        private Label _points;
        private Label _l1;
        private Label _l2;
        public Button[,] ButtonBoard { get => _buttonBoard; set => _buttonBoard = value; }
        public Timer Timer { get => _timer; set => _timer = value; }
        internal WhackModel Model { get => _model; set => _model = value; }
        public Label TimeRem { get => _timeRem; set => _timeRem = value; }
        public Label Points { get => _points; set => _points = value; }
        public Label L1 { get => _l1; set => _l1 = value; }
        public Label L2 { get => _l2; set => _l2 = value; }

        public View() {
            InitializeComponent();
            _model = new WhackModel();
            _model.Refresh += RefreshBoard;
            _model.GameOver += _model_GameOver;
            _model.NewGame(5);
            GenerateBoard();
            RefreshBoard();
            Timer = new Timer();
            Timer.Interval = 1000;
            Timer.Tick += Timer_Tick;
            Timer.Start();
            _timeElapsed = 0;
        }

        private void Timer_Tick(object sender, EventArgs e) {
            _timeElapsed++;
            _model.DecMoleTimer();
            _model.GenerateMoles(30-_timeElapsed);
            /*if(_timeElapsed != 0 && _timeElapsed % 2 == 0) {
                _model.DeleteMoles();
            } else*/
        }

        private void RefreshBoard() {
            for (Int32 i = 0; i < _buttonBoard.GetLength(0); i++) {
                for (Int32 j = 0; j < _buttonBoard.GetLength(1); j++) {
                    if (_model.GameTable.BaseTable[i, j] == 1) {
                        _buttonBoard[i, j].BackColor = Color.Gray;
                        _buttonBoard[i, j].Enabled = true;
                    } else if(_model.GameTable.BaseTable[i,j] == 0) {
                        _buttonBoard[i, j].BackColor = Color.White;
                        _buttonBoard[i, j].Enabled = false;
                    }
                    if(_model.GameTable.Values[i,j] > 0) {
                        _buttonBoard[i, j].BackgroundImage = (System.Drawing.Image)WhackAMoleForms.Properties.Resources.mole;
                        _buttonBoard[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                    } else if(_model.GameTable.Values[i,j] == 0 && _model.GameTable.BaseTable[i, j] == 1) {
                        _buttonBoard[i, j].BackgroundImage = (System.Drawing.Image)WhackAMoleForms.Properties.Resources.hill;
                        _buttonBoard[i, j].BackgroundImageLayout = ImageLayout.Stretch;

                    }
                }
            }
            L1.Text = "Time remaining: ";
            L2.Text = "Points:";
            TimeRem.Text = (30 - _timeElapsed).ToString();
            Points.Text = (_model.Points).ToString();
        }
        private void GenerateBoard() {
            Controls.Clear();
            _buttonBoard = new Button[_model.GameSize, _model.GameSize];
            for (Int32 i = 0; i < _model.GameSize; i++) {
                for (Int32 j = 0; j < _model.GameSize; j++) {
                    _buttonBoard[i, j] = new Button();
                    _buttonBoard[i, j].Location = new Point(5 + 50 * j, 35 + 50 * i);
                    _buttonBoard[i, j].Size = new Size(50, 50);
                    _buttonBoard[i, j].BackColor = Color.Beige;
                    _buttonBoard[i, j].Enabled = true;
                    _buttonBoard[i, j].Tag = new Point(i, j);
                    _buttonBoard[i, j].FlatStyle = FlatStyle.Flat;
                    _buttonBoard[i, j].MouseClick += new MouseEventHandler(ButtonBoard_MouseClick);
                    Controls.Add(_buttonBoard[i, j]);
                }
            }
            TimeRem = new Label();
            L1 = new Label();
            
            L1.Location = new Point(5, 285);
            L2 = new Label();
            L1.Width = 150;
            L2.Width = 90;
            L1.Text = "Time remaining: ";
            L2.Text = "Points:";
            L2.Location = new Point(190, 285);
            Points = new Label();
            Points.Text = (_model.Points).ToString();
            Points.Location = new Point(280, 285);
            TimeRem.Text = (30 - _timeElapsed).ToString();
            TimeRem.Location = new Point(90, 285);
            Controls.Add(TimeRem);
            Controls.Add(Points);
            Controls.Add(L1);
            Controls.Add(L2);
        }
        private void _model_GameOver() {
            _timer.Stop();
            DialogResult res = MessageBox.Show("Game Over\nYour points: " + _model.Points.ToString() + "\nDo you want to try again?", "Whack-A-Mole", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (res == DialogResult.Yes) {
                _model.NewGame(5);
                GenerateBoard();
                RefreshBoard();
                _timeElapsed = 0;
                _timer.Start();
            } else {
                Close();
            }
        }
        private void ButtonBoard_MouseClick(object sender, MouseEventArgs e) {
            Point p = (Point)((sender as Button).Tag);
            int x = p.X;
            int y = p.Y;
            _model.DeleteMole(x,y);
        }

    }
}
