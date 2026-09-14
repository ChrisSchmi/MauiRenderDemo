using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics;
using System.Windows.Input;

namespace MauiRenderDemo;

public partial class MainPage : ContentPage
{
    private bool _isAnimating = true;

    public MainPage()
    {

        InitializeComponent();

        FadeView.IsVisible = true;

        Dispatcher.Dispatch(DrawFadedBackground);

    }

    private void DrawFadedBackground()
    {
        FadeView.Invalidate();
    }

    private async void OnForgotPasswordClicked(object sender, EventArgs e)
    {
        // Navigiert zur registrierten Route
        await Shell.Current.GoToAsync(nameof(ForgotPasswordPage));
    }
}