using System;
using System.IO;
using Gdk;
using Gtk;
using ProjectsCreator.Table;
using Application = Gtk.Application;
using UI = Gtk.Builder.ObjectAttribute;
using Window = Gtk.Window;

namespace ProjectsCreator
{
    public class MainWindow : Window
    {
        [UI] private readonly VBox _mainVbox = new (false, Config.MainVboxSpacing);
        [UI] private readonly Statusbar _statusBar = new ();
        [UI] private readonly MenuBar _menuBar = new ();
        [UI] private readonly HSeparator _separator = new ();
        [UI] private readonly Paned _paned = new (Orientation.Horizontal);

        private readonly Color _greenColor = new (61, 116, 58);
        private readonly Color _lightGrayColor = new (61, 116, 58);
        private readonly Color _darkGrayColor = new (61, 116, 58);
        private readonly Color _grayColor = new (61, 116, 58);

        public MainWindow() : base(Config.MainWindowTitle)
        {
            WindowPosition = WindowPosition.Center;
            DefaultSize = new Size(Config.MainWindowWidth, Config.MainWindowHeight);

            FillMenuBar();
            FillPaned();
            
            _mainVbox.PackStart(_menuBar, false, false, 0);
            _mainVbox.PackStart(_paned, true, true, 0);
            _mainVbox.PackEnd(_statusBar, false, false, 0);
            _mainVbox.PackEnd(_separator, false, false, 0);
            
            _mainVbox.Name = "green-widget";
            _mainVbox.ModifyFg(StateType.Normal, _lightGrayColor);
            
            Add(_mainVbox);
            AddEventHandlers();
            LoadStyles();
            ShowAll();

            _statusBar.Push(0, "Готов.");
        }

        private void AddEventHandlers()
        {
            DeleteEvent += WindowDeleteEvent;
        }

        private static void WindowDeleteEvent(object sender, DeleteEventArgs a) => Application.Quit();

        private void OnOpenMenuItemClick(object sender, EventArgs a)
        {
            var chooseFolderDialog = new FileChooserDialog("Выбор файла", this, FileChooserAction.Open, "Выбрать",
                    ResponseType.Accept, "Отмена", ResponseType.Cancel);
            
            chooseFolderDialog.ShowAll();
    
            if (chooseFolderDialog.Run() == (int)ResponseType.Accept)
            {
                _statusBar.Push(0, $"Открыт файл: {chooseFolderDialog.Filename}");
            }

            chooseFolderDialog.Destroy();
        }

        private void FillMenuBar()
        {
            var fileMenuItem = new MenuItem("Файл");
            var fileMenu = new Menu();
            var createMenuItem = new MenuItem("Создать");
            var openMenuItem = new MenuItem("Открыть");
            var saveMenuItem = new MenuItem("Сохранить");
            var saveAsMenuItem = new MenuItem("Сохранить как");
            
            openMenuItem.Activated += OnOpenMenuItemClick;
            
            fileMenu.Append(createMenuItem);
            fileMenu.Append(openMenuItem);
            fileMenu.Append(saveMenuItem);
            fileMenu.Append(saveAsMenuItem);
            fileMenuItem.Submenu = fileMenu;
            
            _menuBar.Append(fileMenuItem);
        }

        private static TreeStore GetLeftTableModel()
        {
            var leftTableModel = new TreeStore(typeof(string), typeof(string));

            var root = leftTableModel.AppendValues("Изделие", "ABC.000000.000");

            var item1 = leftTableModel.AppendValues(root, "Сборка 1", "ABC.000000.001");
            var item2 = leftTableModel.AppendValues(root, "Сборка 2", "ABC.000000.002");
            var item3 = leftTableModel.AppendValues(root, "Сборка 3", "ABC.000000.003");
            var item4 = leftTableModel.AppendValues(root, "Сборка 4", "ABC.000000.004");
    
            leftTableModel.AppendValues(item1, "Деталь 1-1", "ABC.000100.001");
            leftTableModel.AppendValues(item1, "Деталь 1-2", "ABC.000100.002");
            leftTableModel.AppendValues(item1, "Деталь 1-3", "ABC.000100.003");
            leftTableModel.AppendValues(item1, "Деталь 1-4", "ABC.000100.004");

            leftTableModel.AppendValues(item2, "Деталь 2-1", "ABC.000200.001");
            leftTableModel.AppendValues(item2, "Деталь 2-2", "ABC.000200.002");
            leftTableModel.AppendValues(item2, "Деталь 2-3", "ABC.000200.003");

            leftTableModel.AppendValues(item3, "Деталь 3-1", "ABC.000300.001");

            leftTableModel.AppendValues(item4, "Деталь 4-1", "ABC.000400.001");
            leftTableModel.AppendValues(item4, "Деталь 4-2", "ABC.000400.002");

            return leftTableModel;
        }

        private void FillPaned()
        {
            var leftScrolledWindow = new ScrolledWindow();
            var leftTable = new TreeView(GetLeftTableModel());
            AddTableColumns(leftTable, new [] { "Наименование", "Обозначение" });
            leftScrolledWindow.WidthRequest = Config.LeftTableWidth;
            leftScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            leftScrolledWindow.Add(leftTable);

            leftTable.Name = "tree-view";
            
            var rightScrolledWindow = new ScrolledWindow();
            var rightTable = new TableDataManager().TreeView;
            AddTableColumns(rightTable, new [] { "Имя", "Код", "Количество" });
            rightScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            rightScrolledWindow.Add(rightTable);
            
            _paned.Add1(leftScrolledWindow);
            _paned.Add2(rightScrolledWindow);
        }
        
        private static void AddTableColumns(in TreeView treeView, in string[] columnNames)
        {
            var i = 0;
            
            foreach (var columnName in columnNames)
            {
                var cellRendererText = new CellRendererText();
                var column = new TreeViewColumn(columnName, cellRendererText, "text", i);
                column.Resizable = true;
                column.SortColumnId = i++;
                column.MinWidth = Config.MinColumnWidth;
                treeView.AppendColumn(column);
            }
        }

        private static void LoadStyles()
        {
            var cssProvider = new CssProvider();
            
            StyleContext.AddProviderForScreen(Screen.Default, cssProvider, StyleProviderPriority.User);
            cssProvider.LoadFromData(File.ReadAllText($"{Config.PathToStylesDirectory}/main.css"));
        }
    }
}