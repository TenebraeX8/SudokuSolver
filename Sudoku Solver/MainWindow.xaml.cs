using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku_Solver
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        protected Label[,] SudokuLabels { get; set; }
        protected SudokuModel Model { get; set; }
        protected TextBox Editor { get; set; }

        public string Input { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            SudokuLabels = new Label[SudokuModel.SIZE, SudokuModel.SIZE];
            this.Editor = new TextBox();
            this.Editor.MaxLength = 1;
            this.Editor.HorizontalAlignment = HorizontalAlignment.Center;
            this.Editor.VerticalAlignment = VerticalAlignment.Center;
            this.Editor.Background = Brushes.Beige;
            this.Editor.MouseDoubleClick += Editor_MouseDoubleClick;
            this.Editor.Visibility = Visibility.Collapsed;
            Model = new SudokuModel();
            InitGrid();
        }

        public void InitGrid()
        {
            for(int row = 0; row < SudokuModel.SIZE; row++)
            {
                for (int col = 0; col < SudokuModel.SIZE; col++)
                {
                    Border border = new Border();
                    border.BorderThickness = new Thickness(2);
                    border.BorderBrush = Brushes.Black; 
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);

                    Label label = new Label();
                    SudokuLabels[row, col] = label;
                    label.Content = " ";
                    label.HorizontalAlignment = HorizontalAlignment.Center;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.FontSize = 14;
                    label.FontWeight = FontWeights.Bold;
                    label.MouseDoubleClick += OnFieldDblClick;
                    border.Child = label;

                    grdSudoku.Children.Add(border);
                }                
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnFieldDblClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label lbl)
            {
                if(this.Editor.Visibility == Visibility.Visible)
                {
                    (this.Editor.Tag as Label).Visibility = Visibility.Visible;
                    this.grdSudoku.Children.Remove(this.Editor);
                }

                Border parent = lbl.Parent as Border;
                Grid.SetRow(this.Editor, Grid.GetRow(parent));
                Grid.SetColumn(this.Editor, Grid.GetColumn(parent));
                this.Editor.Width = parent.ActualWidth;
                this.Editor.Name = "Editor";
                this.Editor.Height = parent.ActualHeight;
                lbl.Visibility = Visibility.Collapsed;
                this.Editor.Visibility = Visibility.Visible;
                this.Editor.Tag = lbl;
                this.grdSudoku.Children.Add(this.Editor);
            }
        }

        private void Editor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox txtbx)
            {
                string content = txtbx.Text;
                int value;
                if (int.TryParse(content, out value))
                {
                    (int, int) index = (Grid.GetRow(txtbx), Grid.GetColumn(txtbx));
                    if(Model.SetIfValid(index, value)) ApplyModel();
                    else
                    {
                        Label corresponding = txtbx.Tag as Label;
                        (corresponding.Parent as Border).Background = Brushes.Red;
                    }
                }
                Editor.Visibility = Visibility.Collapsed;
                (txtbx.Tag as Label).Visibility = Visibility.Visible;
                Editor.Tag = null;
                Editor.Text = "";
                this.grdSudoku.Children.Remove(this.Editor);
            }
        }

        private void ApplyModel()
        {
            for(int row = 0; row < SudokuModel.SIZE; row++)
            {
                for(int col = 0; col < SudokuModel.SIZE; col++)
                {
                    if (this.SudokuLabels[row, col] != null)
                    {
                        var label = this.SudokuLabels[row, col];
                        var value = this.Model[(row, col)].ToString();
                        if (label.Content.ToString() != value && value != "0")
                        {
                            (label.Parent as Border).Background = Brushes.ForestGreen;
                        }
                        else (label.Parent as Border).Background = Brushes.White;
                        label.Content = value != "0" ? value : " ";
                        
                    }
                }
            }
            Console.WriteLine(this.Model.ToString());
            Console.WriteLine();
            
        }

        private void Parse(IEnumerable<string> lines)
        {
            this.Model.Reset();

            if (lines != null)
            {
                int row = 0;
                foreach (string line in lines)
                {
                    if (row >= 9) break;
                    int col = 0;
                    foreach (char c in line)
                    {
                        if (col >= 9) break;
                        int value = 0;
                        if (int.TryParse(c.ToString(), out value))
                        {
                            if (value > 0) Model.SetIfValid((row, col), value);
                        }
                        col++;
                    }
                    row++;
                }
            }
            this.ApplyModel();
        }

        private void btnParse_Click(object sender, RoutedEventArgs e)
        {
            Parse(this.Input?.Split(new char[] { '\n' }));
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            if(!SudokuSolver.Solve(this.Model)) MessageBox.Show("Unable to solve!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);         
            ApplyModel();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            SudokuGenerator.Generate(this.Model);
            ApplyModel();
        }
    }
}
