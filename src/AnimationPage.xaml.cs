namespace MauiRenderDemo;

public partial class AnimationPage : ContentPage
{
    private bool _isAnimating = true;
    public AnimationPage()
	{
		InitializeComponent();
    }

    private void AnimationLoop()
    {
        if (!_isAnimating)
        {
            return;
        }

        if (PlasmaView.IsVisible is false)
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
}