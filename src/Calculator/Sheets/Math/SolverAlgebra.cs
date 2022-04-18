using System;
using System.Linq;
using System.Text;

namespace Calculator.Sheets.Math
{
    class SolverAlgebra : Solver
    {
        protected override void Prepare(MatrixValue[] objectives)
        {
            this.z = new double[matrix.Columns.Length];
            this.objective = new double[matrix.Columns.Length];
            this.recipeCount = new double[matrix.Headers.Length];
            if (objectives != null)
            {
                foreach (MatrixValue objective in objectives)
                {
                    int index = matrix.GetColumnIndex(objective);
                    if (index != -1)
                    {
                        this.objective[index] = objective.Value;
                        this.z[index] = -objective.Value;
                    }
                }
            }
        }
        protected override void Execute()
        {
            for (int irow = 0; irow < matrix.X; irow++)
            {
                int icol = GetColumn(irow, false);
                RowCompute(irow, icol);
            }
        }
        protected override MatrixValue[] BuildResult()
        {
            MatrixHeader[] headers = this.matrix.Headers;
            MatrixValue[] values = new MatrixValue[headers.Length];
            for (int irow = 0; irow < headers.Length; irow++)
            {
                MatrixHeader header = headers[irow];
                values[irow] = new MatrixValue(header.Type, header.Name, recipeCount[irow]);
            }
            return values;
        }
        private void RowCompute(int xrow, int xcol)
        {
            if(xrow >= 0 && xcol >= 0)
            {
                //valeur demandee Z
                double z = this.z[xcol];
                //valeur produite
                double v = matrix.Values[xrow, xcol];
                //coefficient
                double c = -z / v;
                //% production
                double p = matrix.Headers[xrow].Production;
                //recipe count
                this.recipeCount[xrow] = p * c;
                for (int icol = 0; icol < matrix.Y; icol++)
                {
                    double x = matrix.Values[xrow, icol];
                    this.z[icol] += p * x * c;
                }
            }
        }
        private int GetColumn(int xrow, bool invert)
        {
            double max = 0;
            int xcol = -1;
            //on cherche la plus grande demande
            for(int icol = 0; icol < matrix.Y; icol++)
            {
                double cellValue = matrix.Values[xrow, icol];
                if((!invert && cellValue > 0) || (invert && cellValue < 0))
                {
                    //valeur demandee(objective -Z)
                    double z = this.z[icol] - this.objective[icol];
                    double c = -z / cellValue;
                    if(c > max)
                    {
                        max = c;
                        xcol = icol;
                    }
                }
            }
            return xcol;
        }

        public override string ToString()
        {
            MatrixHeader[] headers = this.matrix.Headers;
            MatrixHeader[] columns = this.matrix.Columns;
            StringBuilder value = new StringBuilder();
            value.AppendLine();
            value.AppendLine("Solver");
            
            value.AppendLine("Objectives");
            for (int irow = 0; irow < columns.Length; irow++)
            {
                value.Append($"|{columns[irow].Type + "." + columns[irow].Name,-30}");
            }
            value.AppendLine();
            for (int irow = 0; irow < columns.Length; irow++)
            {
                value.Append($"|{objective[irow],-30}");
            }

            value.AppendLine();
            value.AppendLine("Resultat");
            for (int irow = 0; irow < headers.Length; irow++)
            {
                value.Append($"|{headers[irow].Type + "." + headers[irow].Name,-30}");
            }
            value.AppendLine();
            for (int irow = 0; irow < headers.Length; irow++)
            {
                value.Append($"|{recipeCount[irow],-30}");
            }
            value.AppendLine();
            return value.ToString();
        }
    }
}
