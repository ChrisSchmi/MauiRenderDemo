using System.Windows.Input;

namespace MauiRenderDemo;

public partial class FadingColorBackgroundPage : ContentPage
{
	public FadingColorBackgroundPage()
	{
        SetBackgroundCommand = new Command<string>(OnSetBackground);
        InitializeComponent();
	}

    public ICommand SetBackgroundCommand { private set; get; }


    private void OnSetBackground(string parameter)
    {
        if (string.IsNullOrEmpty(parameter))
        {
            return;
        }

        var selected = parameter.ToLower();

        switch (selected)
        {
            case "1":
                FadedBackgroundDrawable.ColorsList = new()
                {
                    Color.FromArgb("#00BA00"),
                    Color.FromArgb("#FF0000"),
                    Color.FromArgb("#FF00FF"),
                };
                break;

            case "2":
                FadedBackgroundDrawable.ColorsList = new()
                {
                    Color.FromArgb("#FF0000"),
                    Color.FromArgb("#00FF00"),
                    Color.FromArgb("#0000FF"),
                };
                break;
                
            case "3":
                FadedBackgroundDrawable.ColorsList = new()
                {
                    Color.FromArgb("#FFBA00"),
                    Color.FromArgb("#50FFBA00"),
                    Color.FromArgb("#4000FFFF"),
                    Color.FromArgb("#50FF00DA"),
                    Color.FromArgb("#FF00DA"),
                };
                break;
        }

        Shell.SetBackgroundColor(this, FadedBackgroundDrawable.ColorsList.First());
        this.StatusBarBehavior.StatusBarColor = FadedBackgroundDrawable.ColorsList.First();

        FadingBackgroundView.Invalidate();
    }
}