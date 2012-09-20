
namespace Tiwa
{
    using Atom.Wpf;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;
    using Tiwa.Properties;
    using Forms = System.Windows.Forms;

    public partial class MainWindow : Window
    {
        private enum TiwaState
        {
            Started,
            Paused
        }

        public MainWindow()
        {
            InitializeComponent();
            CreateNotifyIcon();
            
            gameLoop.Stop();
            gameLoop.Updated += OnTimerTicked;

            settings.Load();
            SetTime( TimeSpan.FromMinutes( settings.Time ) );
        }

        private void CreateNotifyIcon()
        {
            notifyIcon = new Forms.NotifyIcon() { 
                BalloonTipText = TiwaResources.BalloonTipText,
                BalloonTipTitle = TiwaResources.ApplicationName,
                Text = TiwaResources.ApplicationName,
                ContextMenu = new Forms.ContextMenu(),
                Icon = LoadIcon()
            };
                                    
            Forms.MenuItem closeMenu = notifyIcon.ContextMenu.MenuItems.Add( "Close" );
            closeMenu.Click += (sender, e) => this.Shutdown();
            notifyIcon.Click += OnNotifyIconClicked;
        }

        private static Icon LoadIcon()
        {
            var uri = new Uri( "pack://application:,,,/Tiwa;component/app-icon.ico" );

            using( Stream stream = Application.GetResourceStream( uri ).Stream )
            {
                return new Icon(stream);
            }
        }

        private void OnTimerTicked( TimeSpan elapsedTime )
        {
            if( elapsedTime < TimeSpan.FromSeconds( 10 ) )
            {
                this.SetTime( time - elapsedTime );
            }

            if( time == TimeSpan.FromSeconds( 0 ) )
            {
                this.OnReachedZero();
            }
        }

        private void OnReachedZero()
        {
            Pause();
            ShowMain();
            Reset();
        }

        private void SetTime( TimeSpan time )
        {
            if( time <= TimeSpan.FromSeconds( 0 ) )
            {
                time = TimeSpan.FromSeconds( 0 );
            }

            this.time = time;
            this.textBlock.Text = string.Format( "{0}:{1}", time.Minutes, time.Seconds );
            this.notifyIcon.Text = TiwaResources.ApplicationName + " " + this.textBlock.Text;
        }

        private void Shutdown()
        {
            App.Current.Shutdown();
        }

        private void ShowMain()
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void CheckTrayIcon()
        {
            ShowTrayIcon( !IsVisible );
        }

        private void ShowTrayIcon( bool show )
        {
            if( notifyIcon != null )
                notifyIcon.Visible = show;
        }

        private void Start()
        {
            gameLoop.Start();
            state = TiwaState.Started;
            buttonStartPause.Content = TiwaResources.Pause;
        }

        private void Pause()
        {
            gameLoop.Stop();
            state = TiwaState.Paused;
            buttonStartPause.Content = TiwaResources.Start;
        }

        private void Minimize()
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Reset()
        {
            this.SetTime( TimeSpan.FromMinutes( settings.Time ) );
        }
        
        private void OnWindowStateChanged( object sender, EventArgs e )
        {
            if( WindowState == WindowState.Minimized )
            {
                Hide();
                if( notifyIcon != null )
                    notifyIcon.ShowBalloonTip( 2000 );
            }
        }

        private void OnWindowIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CheckTrayIcon();
        }

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            notifyIcon.Dispose();
            notifyIcon = null;
        }

        private void OnNotifyIconClicked(object sender, EventArgs e)
        {
            ShowMain();
        }

        private void OnCloseButtonClicked( object sender, RoutedEventArgs e )
        {
            this.Shutdown();
        }

        private void OnMinimizeButtonClicked( object sender, RoutedEventArgs e )
        {
            this.Minimize();
        }

        private void OnStartPauseButtonClicked( object sender, RoutedEventArgs e )
        {
            if( state == TiwaState.Paused )
            {
                Start();
            }
            else
            {
                Pause();
            }

            FocusHelper.Focus( this.buttonClose );

            if( Keyboard.IsKeyDown( Key.LeftShift ) || Keyboard.IsKeyDown( Key.RightShift ) )
            {
                this.Minimize();
            }
        }

        private void OnResetButtonClicked( object sender, RoutedEventArgs e )
        {
            Reset();
        }

        private TimeSpan time;
        private TiwaState state = TiwaState.Paused;

        private Forms.NotifyIcon notifyIcon;
        private readonly GameLoop gameLoop = new GameLoop();
        private readonly TiwaSettings settings = new TiwaSettings();
    }
}

