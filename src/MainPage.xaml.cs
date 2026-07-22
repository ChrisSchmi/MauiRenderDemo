using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics;

namespace MauiRenderDemo;

public partial class MainPage : ContentPage
{
    private bool _isAnimating = true;

    public MainPage()
    {
        InitializeComponent();

        // Loop starten
        Dispatcher.DispatchDelayed(TimeSpan.Zero, AnimationLoop);
    }

    private void AnimationLoop()
    {
        if (!_isAnimating) return;

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