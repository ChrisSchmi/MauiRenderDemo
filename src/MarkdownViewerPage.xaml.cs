using Org.Apache.Http.Client;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiRenderDemo;

public partial class MarkdownViewerPage : ContentPage
{
    // because IsBusy will be removed in net11
    private bool _isLoading;
    public new bool IsLoading
    {
        get 
        {
            return _isLoading;
        }
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }
    private string _markdownContent = "Lade Markdown...";
    private readonly HttpClient _httpClient;
    public string MarkdownContent
    {
        get => _markdownContent;
        set
        {
            if (_markdownContent != value)
            {
                _markdownContent = value;
                OnPropertyChanged();
            }
        }
    }
    public MarkdownViewerPage()
	{
		InitializeComponent();
        _httpClient = new();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        string targetUrl = "https://raw.githubusercontent.com/ChrisSchmi/MauiRenderDemo/refs/heads/main/README.md";

        await LoadMarkdownFromUrlAsync(targetUrl);
    }

    protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        MarkdownContent = string.Empty;
    }

    // Diese Methode wird aufgerufen, wenn die Seite betreten/navigiert wird
    public async Task LoadMarkdownFromUrlAsync(string url)
    {
        try
        {
            IsLoading = true;

            await Task.Delay(2_000);
            // Lädt den Markdown-Text direkt von der URL herunter
            MarkdownContent = await _httpClient.GetStringAsync(url);
        }
        catch (Exception ex)
        {
            MarkdownContent = $"# Fehler beim Laden\n\nKonnte Inhalt nicht abrufen: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }


}