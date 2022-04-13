using System;
using Gdk;
using Gtk;
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

        public MainWindow() : base(Config.MainWindowTitle)
        {
            WindowPosition = WindowPosition.CenterAlways;
            DefaultSize = new Size(Config.MainWindowWidth, Config.MainWindowHeight);

            FillMenuBar();
            _mainVbox.PackStart(_menuBar, false, false, 0);
            _mainVbox.PackEnd(_statusBar, false, false, 0);
            _mainVbox.PackEnd(_separator, false, false, 0);

            Add(_mainVbox);
            AddEventHandlers();
            ShowAll();

            _statusBar.Push(0, "Инициализация завершена.");
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
    }
}