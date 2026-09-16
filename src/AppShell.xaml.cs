using Android.Media;

namespace MauiRenderDemo;

public partial class AppShell : Shell
{
    public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();
    public AppShell()
	{
		InitializeComponent();
        RegisterRoutes();
        BindingContext = this;
    }

    void RegisterRoutes()
    {
        Routes.Add(nameof(MainPage), typeof(MainPage));
        Routes.Add(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
        Routes.Add(nameof(ButtonPage), typeof(ButtonPage));
        Routes.Add(nameof(AnimationPage), typeof(AnimationPage));
        Routes.Add(nameof(FadingBackgroundPage), typeof(FadingBackgroundPage));
        Routes.Add(nameof(BottomSheetPage), typeof(BottomSheetPage));
        Routes.Add(nameof(MarkdownViewerPage), typeof(MarkdownViewerPage));

        foreach (var item in Routes)
        {
            Routing.RegisterRoute(item.Key, item.Value);
        }
    }
}
