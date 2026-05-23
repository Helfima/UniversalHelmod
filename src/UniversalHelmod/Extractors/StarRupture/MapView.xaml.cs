using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using UniversalHelmod.Classes;
using UniversalHelmod.Databases.Models;
using UniversalHelmod.Extractors.StarRupture.Models;
using UniversalHelmod.Sheets.Models;

namespace UniversalHelmod.Extractors.StarRupture
{
    public partial class MapView : Window
    {
        private Point _lastMousePos;
        private bool _isDragging;
        private Dictionary<string, SREntity> _entities;
        private double _canvasSize = 2000*10.0;

        public MapView(Dictionary<string, SREntity> entities)
        {
            InitializeComponent();
            _entities = entities;
            DrawEntities(_canvasSize);
        }

        private void DrawEntities(double spacing)
        {
            MapCanvas.Children.Clear();

            if (_entities == null || _entities.Count == 0) return;

            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;

            foreach (var kvp in _entities)
            {
                var t = kvp.Value.SpawnData?.Transform?.Translation;
                if (t == null) continue;
                minX = Math.Min(minX, t.X);
                maxX = Math.Max(maxX, t.X);
                minY = Math.Min(minY, t.Y);
                maxY = Math.Max(maxY, t.Y);
            }

            double centerX = (minX + maxX) / 2;
            double centerY = (minY + maxY) / 2;
            double rangeX = maxX - minX;
            double rangeY = maxY - minY;
            double range = Math.Max(rangeX, rangeY);
            if (range == 0) range = 1;

            double scale = _canvasSize / range;
            

            foreach (var kvp in _entities)
            {
                if (kvp.Value.SpawnData.EntityConfigDataPath.Contains("Modular")) continue;
                if (kvp.Value.SpawnData.EntityConfigDataPath.Contains("DroneConnections")) continue;
                var t = kvp.Value.SpawnData?.Transform?.Translation;
                if (t == null) continue;
                var entity = kvp.Value;
                // Appliquer l'espacement depuis le centre
                double sx = centerX + (t.X - centerX) * spacing;
                double sy = centerY + (t.Y - centerY) * spacing;

                double cx = (sx - (centerX - range / 2 * spacing)) * scale / spacing;
                double cy = (sy - (centerY - range / 2 * spacing)) * scale / spacing;

                var configPath = kvp.Value.SpawnData?.EntityConfigDataPath ?? "";
                var tooltip = $"{kvp.Key}\n{configPath}\nX={t.X:F0} Y={t.Y:F0} Z={t.Z:F0}\n{entity.RecipePath}";
                UIElement link = null;
                UIElement icon = null;
                double dotSize = 6;
                Grid shape = new Grid
                {
                    ToolTip = tooltip,
                    DataContext = entity
                };
                switch (entity.Type)
                {
                    case EntityType.BaseCore:
                        {
                            dotSize *= 2;
                            var color = Brushes.Magenta;
                            shape.Children.Add(new Ellipse
                            {
                                Width = dotSize,
                                Height = dotSize,
                                Stroke = color,
                                StrokeThickness = 2
                            });
                        }
                        break;
                    case EntityType.Receiver:
                        {
                            dotSize *= 2;
                            var color = Brushes.CornflowerBlue;
                            shape.Children.Add(new Rectangle
                            {
                                Width = dotSize,
                                Height = dotSize,
                                Fill = color
                            });
                            if(entity.Inventory.Count > 0)
                            {
                                var slot = entity.Inventory[0];
                                shape.ToolTip += $"\n{slot.ItemPath}";
                                if (slot.Item != null)
                                {
                                    shape.Children.Add(new Image
                                    {
                                        Width = dotSize,
                                        Height = dotSize,
                                        Source = slot.Item.Icon
                                    });
                                }
                            }
                        }
                        break;
                    case EntityType.Sender:
                        {
                            dotSize *= 2;
                            var color = Brushes.CornflowerBlue;
                            shape.Children.Add(new Rectangle
                            {
                                Width = dotSize,
                                Height = dotSize,
                                Fill = color
                            });
                            if (entity.Inventory.Count > 0)
                            {
                                var slot = entity.Inventory[0];
                                shape.ToolTip += $"\n{slot.ItemPath}";
                                if (slot.Item != null)
                                {
                                    shape.Children.Add(new Image
                                    {
                                        Width = dotSize,
                                        Height = dotSize,
                                        Source = slot.Item.Icon
                                    });
                                }
                            }
                        }
                        break;
                    case EntityType.Exporter:
                        {
                            dotSize *= 2;
                            var color = Brushes.CornflowerBlue;
                            shape.Children.Add(new Ellipse
                            {
                                Width = dotSize,
                                Height = dotSize,
                                Stroke = color,
                                StrokeThickness = 2
                            });
                        }
                        break;
                    case EntityType.Extractor:
                        {
                            dotSize *= 2;
                            var color = Brushes.Orange;
                            shape.Children.Add(new Rectangle
                            {
                                Width = dotSize,
                                Height = dotSize,
                                Fill = color
                            });
                            if (entity.Recipe != null)
                            {
                                shape.Children.Add(new Image
                                {
                                    Width = dotSize,
                                    Height = dotSize,
                                    Source = entity.Recipe.Icon
                                });
                            }
                        }
                        break;
                    case EntityType.Turret:
                        {
                            var color = Brushes.Red;
                            shape.Children.Add(new Rectangle
                            {
                                Width = dotSize,
                                Height = dotSize,
                                Fill = color
                            });
                        }
                        break;
                    case EntityType.Fabricator:
                        {
                            dotSize *= 2;
                            var color = Brushes.Green;
                            shape.Children.Add(new Rectangle
                            {
                                Width = dotSize,
                                Height = dotSize,
                                Fill = color
                            });
                            if (entity.Recipe != null)
                            {
                                shape.Children.Add(new Image
                                {
                                    Width = dotSize,
                                    Height = dotSize,
                                    Source = entity.Recipe.Icon
                                });
                            }
                        }
                        break;
                    case EntityType.Zipline:
                        {
                            var color = Brushes.AntiqueWhite;
                            shape.Children.Add(new Ellipse
                            {
                                Width = dotSize,
                                Height = dotSize,
                                Fill = color,
                                ToolTip = tooltip,
                                DataContext = entity
                            });
                            if (entity.Link.Count > 0)
                            {
                                double sx1 = centerX + (entity.Link[0].X - centerX) * spacing;
                                double sy1 = centerY + (entity.Link[0].Y - centerY) * spacing;
                                double sx2 = centerX + (entity.Link[1].X - centerX) * spacing;
                                double sy2 = centerY + (entity.Link[1].Y - centerY) * spacing;
                                link = new Line()
                                {
                                    Stroke = color,
                                    StrokeThickness = 1,
                                    X1 = (sx1 - (centerX - range / 2 * spacing)) * scale / spacing,
                                    Y1 = (sy1 - (centerY - range / 2 * spacing)) * scale / spacing,
                                    X2 = (sx2 - (centerX - range / 2 * spacing)) * scale / spacing,
                                    Y2 = (sy2 - (centerY - range / 2 * spacing)) * scale / spacing
                                };
                            }
                        }
                        break;
                    case EntityType.Generator:
                        {
                            dotSize *= 2;
                            var color = Brushes.Yellow;
                            shape.Children.Add(new Rectangle
                            {
                                Width = dotSize,
                                Height = dotSize,
                                Fill = color
                            });
                        }
                        break;
                    default:
                        {
                            var color = Brushes.Gray;
                            shape.Children.Add(new Ellipse
                            {
                                Width = dotSize,
                                Height = dotSize,
                                Fill = color,
                                ToolTip = tooltip,
                                DataContext = entity
                            });
                        }
                        break;
                }
                
                if (shape != null)
                {
                    Canvas.SetLeft(shape, cx - dotSize / 2);
                    Canvas.SetTop(shape, cy - dotSize / 2);
                    shape.MouseUp += Shape_MouseUp;
                    MapCanvas.Children.Add(shape);
                }
                if (icon != null)
                {
                    Canvas.SetLeft(icon, cx - dotSize / 2);
                    Canvas.SetTop(icon, cy - dotSize / 2);
                    MapCanvas.Children.Add(icon);
                }
                if (link != null)
                {
                    MapCanvas.Children.Add(link);
                }
            }

            Loaded += (s, e) => FitView();
        }

        private void Shape_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var source = (dynamic)e.Source;
                var entity = source.DataContext as SREntity;
                if(entity != null)
                {
                    System.Windows.Clipboard.SetText(entity.RecipePath);
                }
            } 
            catch(Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private Recipe FindIcon(SREntity entity)
        {
            var database = Workspaces.Models.WorkspacesModel.Intance.Current.Database;
            return database.SelectRecipeByTag(entity.RecipePath);
        }

        private Recipe FindIcon2(SREntity entity)
        {
            var database = Workspaces.Models.WorkspacesModel.Intance.Current.Database;
            var pattern = @"CR_([^_.]*)[_]?([^_]*?)$";
            var match = Regex.Match(entity.RecipePath, pattern);
            if (match.Success)
            {
                var recipe_name = match.Groups[1].Value;
                var recipe = database.SelectRecipe(recipe_name);
                if (recipe != null) return recipe;
            }
            return null;
        }
        private void FitView()
        {
            double scaleX = ActualWidth / _canvasSize;
            double scaleY = ActualHeight / _canvasSize;
            double fitScale = Math.Min(scaleX, scaleY) * 2;
            ScaleTransform.ScaleX = fitScale;
            ScaleTransform.ScaleY = fitScale;
            TranslateTransform.X = (ActualWidth - _canvasSize * fitScale) / 2;
            TranslateTransform.Y = (ActualHeight - _canvasSize * fitScale) / 2;
        }

        private void SpacingSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if (SpacingLabel == null) return;
            //SpacingLabel.Content = $"x{e.NewValue:F1}";

            //_canvasSize = 2000 * e.NewValue;
            //DrawEntities(e.NewValue);
            //FitView();
        }
        private void MapCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double factor = e.Delta > 0 ? 1.2 : 1 / 1.2;
            var pos = e.GetPosition(this);
            TranslateTransform.X = pos.X - (pos.X - TranslateTransform.X) * factor;
            TranslateTransform.Y = pos.Y - (pos.Y - TranslateTransform.Y) * factor;
            ScaleTransform.ScaleX *= factor;
            ScaleTransform.ScaleY *= factor;
        }

        private void MapCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _lastMousePos = e.GetPosition(this);
            MapCanvas.CaptureMouse();
        }

        private void MapCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            MapCanvas.ReleaseMouseCapture();
        }

        private void MapCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;
            var pos = e.GetPosition(this);
            TranslateTransform.X += pos.X - _lastMousePos.X;
            TranslateTransform.Y += pos.Y - _lastMousePos.Y;
            _lastMousePos = pos;
        }
    }
}
