using System;
using Gtk;
using Sketch;

public partial class SketchWindow: Gtk.Window
{	
	public App m_app;

	public SketchWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		m_app = new App();
		m_app.RefreshTitle = this.RefreshTitle;
		m_app.RefreshGraphics = sketchcontrol1.RefreshGraphics;
		m_app.IsBusy = sketchcontrol1.IsBusy;

		sketchcontrol1.App = m_app;

		m_app.RefreshTitle();
	}

	public void RefreshTitle() {
		var filename = m_app.FileName;
		if (filename == null) filename = "untitled";
		else filename = System.IO.Path.GetFileNameWithoutExtension(filename);
		if (m_app.History.Cursor > 0) filename += "*";

		this.Title = "Sketch - " + filename + " (" + m_app.SelectedFrame.ToString() + ")";
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		if (m_app == null) return;
		if (m_app.IsBusy()) {
			a.RetVal = true;
			return;
		}

		Application.Quit ();
	}
	protected void OnAddActionActivated (object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (m_app.IsBusy()) return;
		
		m_app.AddNewFrame();
		
		m_app.RefreshTitle();
		m_app.RefreshGraphics();
	}

	protected void OnGoForwardActionActivated (object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (!m_app.HasNextFrame) return;
		if (m_app.IsBusy()) return;

		m_app.SelectedFrame++;

		m_app.RefreshTitle();
		m_app.RefreshGraphics();
	}

	protected void OnGoBackActionActivated (object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (!m_app.HasPreviousFrame) return;
		if (m_app.IsBusy()) return;

		m_app.SelectedFrame--;

		m_app.RefreshTitle();
		m_app.RefreshGraphics();
	}

	protected void OnGotoFirstActionActivated (object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (m_app.IsBusy()) return;

		m_app.NavigateToFirstFrame();

		m_app.RefreshTitle();
		m_app.RefreshGraphics();
	}

	protected void OnGotoLastActionActivated (object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (m_app.IsBusy()) return;

		m_app.NavigateToLastFrame();

		m_app.RefreshTitle();
		m_app.RefreshGraphics();
	}

	protected void OnRemoveActionActivated (object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (!m_app.HasMoreThanOneFrame) return;
		if (m_app.IsBusy()) return;

		m_app.RemoveSelectedFrame();

		m_app.RefreshTitle();
		m_app.RefreshGraphics();
	}

	protected void OnMediaPlayActionActivated (object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (m_app.IsBusy()) return;

		sketchcontrol1.Preview = true;
	}

	protected void OnMediaStopActionActivated (object sender, EventArgs e)
	{
		sketchcontrol1.Preview = false;
	}

	protected void OnUndoAction1Activated (object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (m_app.IsBusy()) return;

		m_app.History.Undo(m_app);
	}

	protected void OnRedoAction1Activated (object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (m_app.IsBusy()) return;

		m_app.History.Redo(m_app);
	}

	protected void OnSaveActionActivated (object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (m_app.IsBusy()) return;

		var filename = m_app.FileName;
		if (filename == null) filename = Utils.SaveFile.WithFilters(this, "*." + m_app.FileExtension);
		if (filename == null) return;
		if (!filename.EndsWith("." + m_app.FileExtension)) filename += "." + m_app.FileExtension;

		m_app.Save(filename);

		m_app.RefreshTitle();
	}

	protected void OnOpenActionActivated (object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (m_app.IsBusy()) return;

		var filename = Utils.OpenFile.WithFilters(this, "*." + m_app.FileExtension);
		if (filename == null) return;

		m_app.Read(filename);

		m_app.RefreshTitle();
		m_app.RefreshGraphics();
	}

	protected void OnNewActionActivated(object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (m_app.IsBusy()) return;

		// If the program is started by passing file to command line, this might not work.
		System.Diagnostics.Process.Start(Environment.CommandLine);
	}
	
	protected void OnSaveAsActionActivated(object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (m_app.IsBusy()) return;

		var filename = Utils.SaveFile.WithFilters(this, "*." + m_app.FileExtension);
		if (filename == null) return;
		if (!filename.EndsWith("." + m_app.FileExtension)) filename += "." + m_app.FileExtension;
		
		m_app.Save(filename);

		m_app.RefreshTitle();
	}
}
