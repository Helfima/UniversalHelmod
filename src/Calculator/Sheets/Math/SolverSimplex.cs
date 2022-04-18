using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Sheets.Math
{
    class SolverSimplex : Solver
    {
        protected override void Prepare(MatrixValue[] objectives)
        {
            // Ajoute les recettes d'ingredient
            // Recherche les colonnes uniquement ingredient
            List<int> cols = new List<int>();
            for (int icol = 0; icol < matrix.Y; icol++)
            {
                bool isProduct = false;
                for (int irow = 0; irow < matrix.X; irow++)
                {
                    //si une colonne est un produit au moins une fois on l'exclus
                    double value = matrix.Values[irow, icol];
                    if(value > 0)
                    {
                        isProduct = true;
                        break;
                    }
                }
                if (!isProduct)
                {
                    cols.Add(icol);
                }
            }
            // Ajout les rows en column
            for (int irow = 0; irow < matrix.X; irow++)
            {
                var header = matrix.Headers[irow];
                var column = new MatrixHeader($"fake_{header.Type}_{irow}", header.Name);
                int icol = matrix.Columns.Length;
                matrix.AddColumn(column);
                matrix.Values[irow, icol] = 1;
            }
            var columnCoefficient = new MatrixHeader($"fake_Coefficient", "Coefficient");
            matrix.AddColumn(columnCoefficient);
            // ajoute les recettes ingredients
            foreach (int icol in cols) {
                var column = matrix.Columns[icol];
                var header = new MatrixHeader(column.Type, column.Name);
                var row = new MatrixRow($"fake_{column.Type}_{icol}", column.Name);
                var value = new MatrixValue(column.Type, column.Name, 1);
                row.AddValue(value);
                var valueC = new MatrixValue($"fake_Coefficient", "Coefficient", 1e4 * icol);
                row.AddValue(valueC);
                int irow = matrix.Headers.Length;
                matrix.AddRow(header, row);
                matrix.Values[irow, icol] = 1;
            }
            // ajout la row Z
            var zHeader = new MatrixHeader($"fake_Z", "Z");
            var zRow = new MatrixRow(zHeader.Type, zHeader.Name);
            if (objectives != null)
            {
                foreach (MatrixValue objective in objectives)
                {
                    zRow.AddValue(objective);
                }
            }
            matrix.AddRow(zHeader, zRow);
        }
        protected override void Execute()
        {
            int xrow, xcol;
            while(GetPivot(out xrow, out xcol))
            {
                Pivot(xrow, xcol);
            }
        }
        protected override MatrixValue[] BuildResult()
        {
            MatrixValue[] values = new MatrixValue[oriMatrix.Headers.Length];
            for(int icol = 0; icol < oriMatrix.Headers.Length; icol++)
            {
                var header = oriMatrix.Headers[icol];
                var xcol = icol + oriMatrix.Y;
                var zrow = matrix.X - 1;
                var count = matrix.Values[zrow, xcol];
                var value = new MatrixValue(header.Type, header.Name, -count);
                values[icol] = value;
            }
            return values;
        }
        private bool GetPivot(out int xrow, out int xcol)
        {
            double max_z_value = 0;
            xcol = -1;
            double min_ratio_value = 0;
            xrow = -1;
            // boucle sur la derniere ligne nommee Z
            // ne prend pas la colonne coefficient
            for (int icol = 0; icol < matrix.Y - 1; icol++)
            {
                var zRow = matrix.Headers.Length - 1;
                double z_value = matrix.Values[zRow, icol];
                if (z_value > max_z_value)
                {
                    //la valeur repond au critere, la colonne est eligible
                    //on recherche le ligne
                    min_ratio_value = 0;
                    bool first = true;
                    // on ne boucle pas sur la ligne Z
                    for (int irow = 0; irow < matrix.X - 1; irow++)
                    {
                        double x_value = matrix.Values[irow, icol];
                        //seule les cases positives sont prises en compte
                        if (x_value > 0)
                        {
                            //calcul du ratio base / x
                            var coeffCol = matrix.Columns.Length - 1;
                            //valeur coefficent
                            double c_value = matrix.Values[irow, coeffCol];
                            double bx_ratio = c_value / x_value;
                            if (first || bx_ratio < min_ratio_value)
                            {
                                min_ratio_value = bx_ratio;
                                xrow = irow;
                                first = false;
                            }
                        }
                    }
                    if (!first)
                    {
                        //le pivot est possible
                        max_z_value = z_value;
                        xcol = icol;
                    }
                }
            }
            if (max_z_value == 0)
            {
                // il n'y a plus d'amelioration possible fin du programmme
                return false;
            }
            if (xrow == -1 || xcol == -1) return false;
            return true;
        }
        /// <summary>
        /// Calcul pivot de gauss
        /// </summary>
        /// <param name="xrow"></param>
        /// <param name="xcol"></param>
        private void Pivot(int xrow, int xcol)
        {
            var previous = matrix.Clone();
            var pivotValue = previous.Values[xrow, xcol];
            for (int irow = 0; irow < previous.X; irow++)
            {
                for (int icol = 0; icol < previous.Y; icol++)
                {
                    var cellValue = previous.Values[irow, icol];
                    if(irow == xrow)
                    {
                        // Transformation de la ligne pivot : elle est divisee par l'element pivot
                        matrix.Values[irow, icol] = cellValue / pivotValue;
                    }
                    else if (icol == xcol)
                    {
                        // Transformation de la colonne pivot : toutes les cases sauf la case pivot deviennent zero.
                        matrix.Values[irow, icol] = 0;
                    }
                    else
                    {
                        var B = previous.Values[irow, xcol];
                        var D = previous.Values[xrow, icol];
                        matrix.Values[irow, icol] = cellValue - (B * D) / pivotValue;
                    }
                }
            }
        }
    }
}
