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

		var advisor = new FrameAdvisor(m_app);
		advisor.Do(FrameAdvisor.Event.Loading);
	}

	public void Refresh(App.UI ui) {
		switch (ui) {
			case App.UI.Title: RefreshTitle(); break;
			case App.UI.Graphics: sketchcontrol1.RefreshGraphics(); break;
			case App.UI.StartPreview: sketchcontrol1.Preview = true; break;
			case App.UI.StopPreview: sketchcontrol1.Preview = false; break;
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
		var advisor = new DocumentAdvisor(m_app);
		m_app.DeleteEventArgs = a;
		advisor.Do(DocumentAdvisor.Event.Closing);
	}

	protected void OnAddActionActivated (object sender, EventArgs e)
	{
		var advisor = new FrameAdvisor(m_app);
		advisor.Do(FrameAdvisor.Event.AddNewFrame);
	}

	protected void OnGoForwardActionActivated (object sender, EventArgs e)
	{
		var advisor = new FrameAdvisor(m_app);
		advisor.Do(FrameAdvisor.Event.GotoNextFrame);
	}

	protected void OnGoBackActionActivated (object sender, EventArgs e)
	{
		var advisor = new FrameAdvisor(m_app);
		advisor.Do(FrameAdvisor.Event.GotoPreviousFrame);
	}

	protected void OnGotoFirstActionActivated (object sender, EventArgs e)
	{
		var advisor = new FrameAdvisor(m_app);
		advisor.Do(FrameAdvisor.Event.GotoFirstFrame);
	}

	protected void OnGotoLastActionActivated (object sender, EventArgs e)
	{
		var advisor = new FrameAdvisor(m_app);
		advisor.Do(FrameAdvisor.Event.GotoLastFrame);
	}

	protected void OnRemoveActionActivated (object sender, EventArgs e)
	{
		var advisor = new FrameAdvisor(m_app);
		advisor.Do(FrameAdvisor.Event.DeleteSelectedFrame);
	}

	protected void OnMediaPlayActionActivated (object sender, EventArgs e)
	{
		var advisor = new FrameAdvisor(m_app);
		advisor.Do(FrameAdvisor.Event.StartPreview);
	}

	protected void OnMediaStopActionActivated (object sender, EventArgs e)
	{
		var advisor = new FrameAdvisor(m_app);
		advisor.Do(FrameAdvisor.Event.StopPreview);
	}

	protected void OnUndoAction1Activated (object sender, EventArgs e)
	{
		var advisor = new HistoryAdvisor(m_app);
		advisor.Do(HistoryAdvisor.Event.UndoClicked);
	}

	protected void OnRedoAction1Activated (object sender, EventArgs e)
	{
		var advisor = new HistoryAdvisor(m_app);
		advisor.Do(HistoryAdvisor.Event.RedoClicked);
	}

	protected void OnSaveActionActivated (object sender, EventArgs e)
	{
		var advisor = new DocumentAdvisor(m_app);
		m_app.Window = this;
		advisor.Do(DocumentAdvisor.Event.Save);
	}

	protected void OnOpenActionActivated (object sender, EventArgs e)
	{
		var advisor = new DocumentAdvisor(m_app);
		m_app.Window = this;
		advisor.Do(DocumentAdvisor.Event.Open);
	}

	protected void OnNewActionActivated(object sender, EventArgs e)
	{
		var advisor = new DocumentAdvisor(m_app);
		advisor.Do(DocumentAdvisor.Event.New);
	}
	
	protected void OnSaveAsActionActivated(object sender, EventArgs e)
	{
		var advisor = new DocumentAdvisor(m_app);
		advisor.Do(DocumentAdvisor.Event.SaveAs);
	}
}
