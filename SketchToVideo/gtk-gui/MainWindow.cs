
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VBox vbox1;
	private global::Gtk.FileChooserWidget filechooserwidget1;
	private global::Gtk.HBox hbox1;
	private global::Gtk.Button exportButton;
	
	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.filechooserwidget1 = new global::Gtk.FileChooserWidget (((global::Gtk.FileChooserAction)(0)));
		this.filechooserwidget1.Name = "filechooserwidget1";
		this.vbox1.Add (this.filechooserwidget1);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.filechooserwidget1]));
		w1.Position = 0;
		// Container child vbox1.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox ();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.exportButton = new global::Gtk.Button ();
		this.exportButton.CanFocus = true;
		this.exportButton.Name = "exportButton";
		this.exportButton.UseUnderline = true;
		this.exportButton.Label = global::Mono.Unix.Catalog.GetString ("Start");
		this.hbox1.Add (this.exportButton);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.exportButton]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		this.vbox1.Add (this.hbox1);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox1]));
		w3.Position = 1;
		w3.Expand = false;
		w3.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 400;
		this.DefaultHeight = 300;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.exportButton.Clicked += new global::System.EventHandler (this.OnExportButtonClicked);
	}
}
