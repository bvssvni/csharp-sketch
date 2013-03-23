using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow(): base (Gtk.WindowType.Toplevel)
	{
		Build();

		RefreshFormats();
	}

	private void RefreshFormats() {
		formatComboBox.AppendText("GIF");
	}
	
	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnExportButtonActivated(object sender, EventArgs e)
	{

	}
}
