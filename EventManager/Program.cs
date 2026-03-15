using System;

// README.md를 읽고 코드를 작성하세요.

var sound = new SoundSystem();
var score = new ScoreSystem();
var achievement = new AchievementSystem();

EventManager.OnGameEvent += sound.HandleSound;
EventManager.OnGameEvent += score.HandleScore;
EventManager.OnGameEvent += achievement.HandleAchievement;

EventManager.TriggerEvent("ScoreChanged", 100);
EventManager.TriggerEvent("Achievement", "첫 번째 적 처치");
EventManager.TriggerEvent("GameOver");
