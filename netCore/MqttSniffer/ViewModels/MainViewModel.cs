﻿using MqttSniffer.Service;
using Stylet;
using System.Collections.ObjectModel;

namespace MqttSniffer.ViewModels;

internal interface IMainViewModel
{
    string Output { get; set; }
    string Message { get; set; }
    string Topic { get; set; }
    ObservableCollection<MqttMessage> Messages { get; }
    Task SendMessage();
}

internal class MainViewModel : Screen, IMainViewModel
{
    private string console = "";
    private readonly IPublisher publisher;

    public string Output
    {
        get => console; set
        {
            console = value;
            NotifyOfPropertyChange();
        }
    }

    public string Message { get; set; } = "message";
    public string Topic { get; set; } = "FX/FromCSharp";
    public ObservableCollection<MqttMessage> Messages { get; } = [];

    public MainViewModel(IPublisher publisher)
    {
        this.publisher = publisher;
        publisher.SetMessageCallback(ShowMessage);
    }

    private void ShowMessage(MqttMessage message)
    {
        App.Current.Dispatcher.Invoke(() => Messages.Add(message));
    }

    public async Task SendMessage()
    {
        await publisher.SendMessage(Topic, Message);
    }
}
