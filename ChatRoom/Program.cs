using System;

// README.md를 읽고 아래에 코드를 작성하세요.

ChatRoom chatRoom = new ChatRoom();
ChatLogger logger = new ChatLogger();
NotificationService notificationService = new NotificationService();

chatRoom.MessageReceived += logger.LogMessage;
chatRoom.MessageReceived += notificationService.CheckUrgentMessage;

chatRoom.SendMessage("철수", "안녕하세요");
chatRoom.SendMessage("영희", "긴급 회의가 있습니다");
chatRoom.SendMessage("민수", "점심 뭐 먹을까요?");

