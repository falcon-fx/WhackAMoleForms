using System;
using System.Collections.Generic;
using System.Text;

namespace PotZH_WinForms.Model {
    class WhackTable {
        private int[,] _values;
        private int[,] _baseTable;
        private int size;

        public int[,] Values { get => _values; set => _values = value; }
        public int Size { get => size; set => size = value; }
        public int[,] BaseTable { get => _baseTable; set => _baseTable = value; }

        public WhackTable(int size) {
            Size = size;
            Values = new int[Size, Size];
            BaseTable = new int[Size, Size];
            for(int i = 0; i < Size; i++) {
                for(int j = 0; j < Size; j++) {
                    if (i % 2 == 0 && j % 2 == 0) {
                        BaseTable[i, j] = 1;
                    } else if (i % 2 == 1 && j % 2 == 1) {
                        BaseTable[i, j] = 1;
                    } else {
                        BaseTable[i, j] = 0;
                    }
                }
            }
        }
        public void SetValue(int x, int y, int value) {
            Values[x, y] = value;
        }
        public int GetValue(int x, int y) {
            return Values[x, y];
        }
    }
}
