program WeDoScan;

uses
  Vcl.Forms,
  main in 'main.pas' {fmMain},
  devinfo in 'devinfo.pas' {fmDevInfo},
  wclWeDoHub in '..\..\WeDo Framework\wclWeDoHub.pas',
  wclWeDoWatcher in '..\..\WeDo Framework\wclWeDoWatcher.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TfmMain, fmMain);
  Application.Run;
end.
