using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Sheets.Math
{
    abstract class Solver
    {
        protected double[] z;
        protected double[] objective;
        protected double[] recipeCount;
        protected Matrix oriMatrix;
        protected Matrix matrix;

        public MatrixValue[] Solve(Matrix oriMatrix, MatrixValue[] objectives)
        {
            if (oriMatrix != null)
            {
                this.oriMatrix = oriMatrix;
                this.matrix = oriMatrix.Clone();
                Prepare(objectives);
                Execute();
                return BuildResult();
            }
            return null;
        }
        protected abstract void Prepare(MatrixValue[] objectives);
        protected abstract void Execute();
        protected abstract MatrixValue[] BuildResult();
        
    }
}
