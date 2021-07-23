using System;
using System.Collections.Generic;
using System.Text;

namespace PotZH_WinForms.Model {
    class WhackModel {
        private WhackTable _gameTable;
        private Random _rng;
        private int _moleCount;
        private int _points;
        public int GameSize { get { return GameTable.Size; } }
        internal WhackTable GameTable { get => _gameTable; set => _gameTable = value; }
        public int MoleCount { get => _moleCount; set => _moleCount = value; }
        public int Points { get => _points; set => _points = value; }

        public WhackModel() {

        }
        public event Action Refresh;
        public event Action GameOver;
        public void NewGame(int newsize) {
            GameTable = new WhackTable(newsize);
            PrepareBoard();
            _rng = new Random();
            Points = 0;

        }
        public void PrepareBoard() {
            for (int i = 0; i < GameSize; i++) {
                for (int j = 0; j < GameSize; j++) {
                    GameTable.Values[i, j] = 0;
                }
            }
        }
        private Tuple<int,int> GenValidCoords() {
            bool isValid = false;
            int x = _rng.Next(0, GameSize);
            int y = _rng.Next(0, GameSize);
            while (!isValid && MoleCount <= 13) {
                x = _rng.Next(0, GameSize);
                y = _rng.Next(0, GameSize);
                isValid = GameTable.BaseTable[x, y] == 1;
            }

            return new Tuple<int, int>(x, y);

        }
        public void DecMoleTimer() {
            for(int i = 0; i < GameSize; i++) {
                for(int j = 0; j < GameSize; j++) {
                    if (GameTable.Values[i, j] == 2) {
                        GameTable.Values[i, j] = 1;
                    } else if (GameTable.Values[i, j] == 1) {
                        GameTable.Values[i, j] = 0;
                    }
                }
            }
            Refresh?.Invoke();
        }
        public void GenerateMoles(int timerem) {
            int chance = _rng.Next(0, 101);
            if(MoleCount >= 12) {
                return;
            }
            if (chance <= 40) {
                bool moleExists = false;
                Tuple<int, int> moleCoord;
                while (!moleExists) {
                    moleCoord = GenValidCoords();
                    moleExists = GameTable.Values[moleCoord.Item1, moleCoord.Item2] == 0;
                    if(moleExists) {
                        GameTable.Values[moleCoord.Item1, moleCoord.Item2] = 2;
                    }
                }
                moleExists = false;
                while (!moleExists) {
                    moleCoord = GenValidCoords();
                    moleExists = GameTable.Values[moleCoord.Item1, moleCoord.Item2] == 0;
                    if (moleExists) {
                        GameTable.Values[moleCoord.Item1, moleCoord.Item2] = 2;
                    }
                }
            } else if(chance > 40 && chance <= 80) {
                bool moleExists = false;
                Tuple<int, int> moleCoord;
                while (!moleExists) {
                    moleCoord = GenValidCoords();
                    moleExists = GameTable.Values[moleCoord.Item1, moleCoord.Item2] == 0;
                    if (moleExists) {
                        GameTable.Values[moleCoord.Item1, moleCoord.Item2] = 2;
                    }
                }
            }
            Refresh?.Invoke();
            if(timerem == 0) {
                GameOver?.Invoke();
                
            }
        }
        public void DeleteMole(int x, int y) {
            if(GameTable.Values[x,y] > 0) {
                GameTable.Values[x, y] = 0;
                Points++;
            } else if(GameTable.Values[x,y] == 0) {
                Points--;
            }
            Refresh?.Invoke();
        }
    }
}
