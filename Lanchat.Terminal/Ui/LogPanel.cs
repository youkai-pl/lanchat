using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.Ui
{
	internal class LogPanel : SimpleControl
	{
		private readonly VerticalStackPanel _stackPanel;

		public LogPanel()
		{
			_stackPanel = new VerticalStackPanel();

			Content = _stackPanel;
		}

		public void Add(string message)
		{
			_stackPanel.Add(new WrapPanel
			{
				Content = new HorizontalStackPanel
				{
					Children = new[]
					{
						new TextBlock {Text = message}
					}
				}
			});
		}
	}
}
