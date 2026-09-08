//#define ANIMATION
#define ANIMATION_OFF

using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics;
using System.Windows.Input;

namespace MauiRenderDemo;

public partial class MainPage : ContentPage
{
    private bool _isAnimating = true;
    public ICommand IconTileCommand { private set; get; }
    public MainPage()
    {
        // vor InitializeComponent - dann klappt die Bindung.
        IconTileCommand = new Command(() =>
        {
            System.Diagnostics.Debug.WriteLine("Y0!");
        });

        InitializeComponent();

#if ANIMATION

        FadeView.IsVisible = false;
        PlasmaView.IsVisible = true;
        // Loop starten
        Dispatcher.DispatchDelayed(TimeSpan.Zero, AnimationLoop);

#else
        FadeView.IsVisible = true;
        PlasmaView.IsVisible = false;
        Dispatcher.Dispatch(DrawFadedBackground);
#endif
    }

    private void DrawFadedBackground()
    {
        FadeView.Invalidate();
    }

    private void AnimationLoop()
    {
        if (!_isAnimating)
        {
            return;
        }

        if(PlasmaView.IsVisible is false)
        {
            return;
        }

        // Signalisiert dem GraphicsView, sich neu zu zeichnen
        PlasmaView.Invalidate();

        // ca. 90 FPS
        Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(12), AnimationLoop);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _isAnimating = false;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _isAnimating = true;
        Dispatcher.DispatchDelayed(TimeSpan.Zero, AnimationLoop);
    }

    private async void OnForgotPasswordClicked(object sender, EventArgs e)
    {
        // Navigiert zur registrierten Route
        await Shell.Current.GoToAsync(nameof(ForgotPasswordPage));
    }
}