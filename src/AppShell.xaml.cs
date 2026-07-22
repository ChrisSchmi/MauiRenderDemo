namespace MauiRenderDemo;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        // Routen für Unterseiten registrieren
        Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
    }
}
