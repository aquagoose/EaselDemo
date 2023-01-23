using Easel;
using EaselDemo.Engine;

GameSettings settings = new GameSettings();

using EaselGame game = new EaselGame(settings, new MyScene());
game.Run();