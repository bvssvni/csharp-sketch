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
		m_app.Refresh = this.Refresh;
		m_app.IsBusy = sketchcontrol1.IsBusy;

		sketchcontrol1.App = m_app;

		m_app.RefreshUI(UI.Title);
	}

	public void Refresh(UI ui) {
		switch (ui) {
			case UI.Title: RefreshTitle(); break;
			case UI.Graphics: sketchcontrol1.RefreshGraphics(); break;
		}
	}

	public void RefreshTitle() {
		var filename = m_app.FileName;
		if (filename == null) filename = "untitled";
		else filename = System.IO.Path.GetFileName(filename);
		if (m_app.History.Cursor > 0) filename += "*";

		this.Title = "Sketch - " + filename + " (" + m_app.SelectedFrame.ToString() + ")";
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		var advisor = new SketchAdvisor(m_app);
		advisor.Closing = true;
		if (!advisor.ShouldClose()) a.RetVal = true;

		m_app.Do(advisor);
	}
	protected void OnAddActionActivated (object sender, EventArgs e)
	{
		var advisor = new SketchAdvisor(m_app);
		advisor.AddNewFrame = true;
		m_app.Do(advisor);
	}

	protected void OnGoForwardActionActivated (object sender, EventArgs e)
	{
		var advisor = new SketchAdvisor(m_app);
		advisor.GotoNextFrame = true;
		m_app.Do(advisor);
	}

	protected void OnGoBackActionActivated (object sender, EventArgs e)
	{
		var advisor = new SketchAdvisor(m_app);
		advisor.GotoPreviousFrame = true;
		m_app.Do(advisor);
	}

	protected void OnGotoFirstActionActivated (object sender, EventArgs e)
	{
		var advisor = new SketchAdvisor(m_app);
		advisor.GotoFirstFrame = true;
		m_app.Do(advisor);
	}

	protected void OnGotoLastActionActivated (object sender, EventArgs e)
	{
		var advisor = new SketchAdvisor(m_app);
		advisor.GotoLastFrame = true;
		m_app.Do(advisor);
	}

	protected void OnRemoveActionActivated (object sender, EventArgs e)
	{
		var advisor = new SketchAdvisor(m_app);
		advisor.DeleteSelectedFrame = true;
		m_app.Do(advisor);
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

		var window = this;
		DocumentModule.Save(window, m_app.FileExtensions, m_app.FileName, m_app);

		m_app.RefreshUI(UI.Title);
	}

	protected void OnOpenActionActivated (object sender, EventArgs e)
	{
		if (m_app == null) return;
		if (m_app.IsBusy()) return;

		var window = this;
		DocumentModule.Open(window, m_app.FileExtensions, m_app);

		m_app.RefreshUI(UI.Title);
		m_app.RefreshUI(UI.Graphics);
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

		var window = this;
		DocumentModule.SaveAs(window, m_app.FileExtensions, m_app);

		m_app.RefreshUI(UI.Title);
	}
}
