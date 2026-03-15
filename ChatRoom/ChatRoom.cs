using System;
using System.Collections.Generic;
using System.Text;

class ChatRoom
{
    public event Action<string, string> MessageReceived;

    public void SendMessage(string sender, string message)
    {
        MessageReceived?.Invoke(sender, message);
    }
}

class ChatLogger
{
    public void LogMessage(string sender, string message)
    {
        Console.WriteLine($"[로그] {sender}: {message}");
    }
}

class NotificationService
{
    public void CheckUrgentMessage(string sender, string message)
    {
        if (message.Contains("긴급"))
        {
            Console.WriteLine("[알림] 긴급 메시지 수신!");
        }
    }
}